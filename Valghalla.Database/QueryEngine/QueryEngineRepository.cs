using AutoMapper;
using Valghalla.Application.Exceptions;
using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;
using Valghalla.Database.QueryEngine.Builders;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;

namespace Valghalla.Database.QueryEngine
{
    // TQueryForm: query data type
    // TQueryResultItem: mapping data type from entity
    // TQueryFormParameters: parameters to get filters
    // TEntity: entity class
    public abstract class QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> : IQueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters>
        where TEntity : class
        where TQueryForm : QueryForm<TQueryResultItem, TQueryFormParameters>
        where TQueryFormParameters : QueryFormParameters
    {
        private readonly Dictionary<string, QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>> Builders = new();
        private readonly Dictionary<string, GroupQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>> GroupBuilders = new();

        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        private IQueryable<TEntity> queryable = null!;
        private Func<IQueryable<TEntity>, IQueryable<TEntity>>? initialQueryFunc;
        private Func<IQueryable<TEntity>, Order, IQueryable<TEntity>>? orderFunc;

        public QueryEngineRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public abstract Task<QueryResult<TQueryResultItem>> ExecuteQuery(TQueryForm form, CancellationToken cancellationToken);

        public IQueryable<TEntity> ApplyOrder(IQueryable<TEntity> queryable, TQueryForm queryForm)
        {
            if (queryForm.Order != null && orderFunc != null)
            {
                return orderFunc(queryable, queryForm.Order);
            }

            return queryable;
        }

        protected QueryEngineExecutor<TQueryResultItem, TQueryFormParameters, TEntity> ApplyQuery(TQueryForm queryForm)
        {
            var queryFormType = typeof(TQueryForm);

            queryable = dataContext.Set<TEntity>().AsNoTracking();

            if (initialQueryFunc != null)
            {
                queryable = initialQueryFunc(queryable);
            }    

            var properties = queryFormType.GetProperties();
            var calledQueryForProperties = new List<string>();

            foreach (var property in properties)
            {
                var queryPropertyName = property.Name;
                var queryValue = property.GetValue(queryForm);
                if (queryValue == null) continue;

                if (GroupBuilders.TryGetValue(queryPropertyName, out var groupBuilder))
                {
                    var alreadyCalled = groupBuilder.PropertyNames.Any(propertyName => calledQueryForProperties.Any(p => p == propertyName));
                    
                    if (alreadyCalled)
                    {
                        calledQueryForProperties.Add(queryPropertyName);
                        continue;
                    }    

                    queryable = CallQuery(queryable, queryForm, queryValue, groupBuilder);

                    calledQueryForProperties.Add(queryPropertyName);
                }
                else if (Builders.TryGetValue(queryPropertyName, out var builder))
                {
                    queryable = CallQuery(queryable, queryForm, queryValue, builder);
                }
            }

            queryable = ApplyOrder(queryable, queryForm);  

            return new QueryEngineExecutor<TQueryResultItem, TQueryFormParameters, TEntity>(queryable, queryForm, mapper);
        }

        public async Task<QueryFormInfo> GetQueryFormInfo(TQueryFormParameters @params, CancellationToken cancellationToken)
        {
            var queryFormType = typeof(TQueryForm);

            var optionsDictionary = new Dictionary<string, object>();
            var propertyTypeMappings = new Dictionary<string, QueryFormPropertyType>();

            var properties = queryFormType.GetProperties();

            foreach (var property in properties)
            {
                var queryPropertyName = property.Name;
                var queryType = property.PropertyType;

                if (!Builders.TryGetValue(queryPropertyName, out var builder))
                {
                    continue;
                }

                var queryFormPropertyType = GetQueryFormPropertyType(builder);
                propertyTypeMappings.Add(queryPropertyName, queryFormPropertyType);

                var options = await CallOptions(@params, queryType, builder);

                if (options != null)
                {
                    optionsDictionary.Add(queryPropertyName, options);
                }
            }

            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            jsonOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            var jsonPropertyTypeMappings = JsonSerializer.Serialize(propertyTypeMappings, jsonOptions);
            var jsonOptionsDictionary = JsonSerializer.Serialize(optionsDictionary, jsonOptions);

            return new QueryFormInfo(jsonPropertyTypeMappings, jsonOptionsDictionary);
        }

        public QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> statement)
        {
            initialQueryFunc = statement;
            return this;
        }

        public QueryEngineRepository<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> Order(Func<IQueryable<TEntity>, Order, IQueryable<TEntity>> statement)
        {
            orderFunc = statement;
            return this;
        }

        public GroupQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> QueryGroup()
        {
            var builder = new GroupQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>(this, queryable);
            return builder;
        }

        public GenericQueryBuilder<TQueryForm, TQueryResultItem, TQueryValue, TQueryFormParameters, TEntity> QueryFor<TQueryValue>(Expression<Func<TQueryForm, TQueryValue>> expression)
        {
            var builder = new GenericQueryBuilder<TQueryForm, TQueryResultItem, TQueryValue, TQueryFormParameters, TEntity>(this, queryable);
            CacheBuilder(expression, builder);
            return builder;
        }

        public FreeTextQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> QueryFor(Expression<Func<TQueryForm, FreeTextSearchValue?>> expression)
        {
            var builder = new FreeTextQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>(this, queryable);
            CacheBuilder(expression, builder);
            return builder;
        }

        public BooleanQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> QueryFor(Expression<Func<TQueryForm, BooleanFilterValue?>> expression)
        {
            var builder = new BooleanQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>(this, queryable);
            CacheBuilder(expression, builder);
            return builder;
        }

        public DateTimeQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> QueryFor(Expression<Func<TQueryForm, DateTimeFilterValue?>> expression)
        {
            var builder = new DateTimeQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>(this, queryable);
            CacheBuilder(expression, builder);
            return builder;
        }

        public SingleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity, TValue> QueryFor<TValue>(Expression<Func<TQueryForm, SingleSelectionFilterValue<TValue>?>> expression)
        {
            var builder = new SingleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity, TValue>(this, queryable);
            CacheBuilder(expression, builder);
            return builder;
        }

        public MultipleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity, TValue> QueryFor<TValue>(Expression<Func<TQueryForm, MultipleSelectionFilterValue<TValue>?>> expression)
        {
            var builder = new MultipleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity, TValue>(this, queryable);
            CacheBuilder(expression, builder);
            return builder;
        }

        public void CacheGroupBuilder(GroupQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> builder)
        {
            var duplicatedProperties = builder.PropertyNames.Where(GroupBuilders.ContainsKey);

            if (duplicatedProperties.Any())
            {
                var text = string.Join(", ", duplicatedProperties);
                throw new QueryEngineException("These properties are already defined in another group query: " + text);
            }

            foreach (var name in builder.PropertyNames)
            {
                GroupBuilders.Add(name, builder);
            }
        }

        private void CacheBuilder<TValue>(Expression<Func<TQueryForm, TValue>> expression, QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> builder)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                var name = memberExpression.Member.Name;

                if (!Builders.ContainsKey(name))
                {
                    Builders.Add(name, builder);
                }

                return;
            }

            throw new QueryEngineUnableToAnalyzeExpressionException();
        }

        private static IQueryable<TEntity> CallQuery(IQueryable<TEntity> queryable, TQueryForm queryForm, object queryValue, QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> builder)
        {
            if (builder is FreeTextQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> freeTextBuilder)
            {
                return freeTextBuilder.QueryFunc(queryable, (FreeTextSearchValue)queryValue, queryForm);
            }
            else if (builder is BooleanQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> booleanBuilder)
            {
                return booleanBuilder.QueryFunc(queryable, (BooleanFilterValue)queryValue, queryForm);
            }
            else if (builder is DateTimeQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> dateTimeBuilder)
            {
                return dateTimeBuilder.QueryFunc(queryable, (DateTimeFilterValue)queryValue, queryForm);
            }
            else if (builder is SingleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>)
            {
                var valueType = queryValue.GetType().GetGenericArguments().First();
                var singleSelectionValueType = typeof(SingleSelectionFilterValue<>).MakeGenericType(valueType);

                if (queryValue.GetType().IsAssignableFrom(singleSelectionValueType))
                {
                    var builderType = typeof(SingleSelectionQueryBuilder<,,,,>)
                        .MakeGenericType(typeof(TQueryForm), typeof(TQueryResultItem), typeof(TQueryFormParameters), typeof(TEntity), valueType);

                    if (DoesBuilderHaveQuery(builderType, builder))
                    {
                        return queryable;
                    }

                    return InvokeQueryFunc(queryable, queryForm, queryValue, builderType, builder);
                }
            }
            else if (builder is MultipleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>)
            {
                var valueType = queryValue.GetType().GetGenericArguments().First();
                var multipleSelectionValueType = typeof(MultipleSelectionFilterValue<>).MakeGenericType(valueType);

                if (queryValue.GetType().IsAssignableFrom(multipleSelectionValueType))
                {
                    var builderType = typeof(MultipleSelectionQueryBuilder<,,,,>)
                        .MakeGenericType(typeof(TQueryForm), typeof(TQueryResultItem), typeof(TQueryFormParameters), typeof(TEntity), valueType);

                    if (DoesBuilderHaveQuery(builderType, builder))
                    {
                        return queryable;
                    }

                    return InvokeQueryFunc(queryable, queryForm, queryValue, builderType, builder);
                }
            }
            else if (builder is GenericQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>)
            {
                var valueType = queryValue.GetType();
                var builderType = typeof(GenericQueryBuilder<,,,,>)
                    .MakeGenericType(typeof(TQueryForm), typeof(TQueryResultItem), valueType, typeof(TQueryFormParameters), typeof(TEntity));

                return InvokeQueryFunc(queryable, queryForm, queryValue, builderType, builder);
            }
            else if (builder is GroupQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>)
            {
                var builderType = typeof(GroupQueryBuilder<,,,>)
                    .MakeGenericType(typeof(TQueryForm), typeof(TQueryResultItem), typeof(TQueryFormParameters), typeof(TEntity));

                return InvokeQueryGroupFunc(queryable, queryForm, builderType, builder);
            }

            throw new QueryEngineUnhandledDynamicCallException();
        }

        private static QueryFormPropertyType GetQueryFormPropertyType(QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> builder)
        {
            if (builder is FreeTextQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>)
            {
                return QueryFormPropertyType.FreeText;
            }
            else if (builder is BooleanQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>)
            {
                return QueryFormPropertyType.Boolean;
            }
            else if (builder is SingleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>)
            {
                return QueryFormPropertyType.SingleSelection;
            }
            else if (builder is MultipleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>)
            {
                return QueryFormPropertyType.MutipleSelection;
            }
            else if (builder is DateTimeQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>)
            {
                return QueryFormPropertyType.DateTime;
            }

            return QueryFormPropertyType.Generic;
        }

        private static async Task<object?> CallOptions(TQueryFormParameters @params, Type queryValueType, QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> builder)
        {
            if (builder is SingleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>)
            {
                var valueType = queryValueType.GetGenericArguments().First();
                var singleSelectionValueType = typeof(SingleSelectionFilterValue<>).MakeGenericType(valueType);

                if (queryValueType.IsAssignableFrom(singleSelectionValueType))
                {
                    var builderType = typeof(SingleSelectionQueryBuilder<,,,,>)
                        .MakeGenericType(typeof(TQueryForm), typeof(TQueryResultItem), typeof(TQueryFormParameters), typeof(TEntity), valueType);

                    return await InvokeOptionsFunc(@params, builderType, builder);
                }
            }
            else if (builder is MultipleSelectionQueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity>)
            {
                var valueType = queryValueType.GetGenericArguments().First();
                var multipleSelectionValueType = typeof(MultipleSelectionFilterValue<>).MakeGenericType(valueType);

                if (queryValueType.IsAssignableFrom(multipleSelectionValueType))
                {
                    var builderType = typeof(MultipleSelectionQueryBuilder<,,,,>)
                        .MakeGenericType(typeof(TQueryForm), typeof(TQueryResultItem), typeof(TQueryFormParameters), typeof(TEntity), valueType);

                    return await InvokeOptionsFunc(@params, builderType, builder);
                }
            }

            await Task.CompletedTask;
            return null;
        }

        private static bool DoesBuilderHaveQuery(Type builderType, QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> builder)
        {
            return (bool)builderType.GetProperty("NoQuery")!.GetValue(builder)!;
        }    

        private static IQueryable<TEntity> InvokeQueryFunc(IQueryable<TEntity> queryable, TQueryForm queryForm, object queryValue, Type builderType, QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> builder)
        {
            var getter = builderType.GetProperty("QueryFunc")!.GetMethod!.Invoke(builder, null)!;
            var invoker = getter.GetType().GetMethod("Invoke")!;
            return (IQueryable<TEntity>)invoker.Invoke(getter, new object[] { queryable, queryValue, queryForm })!;
        }

        private static IQueryable<TEntity> InvokeQueryGroupFunc(IQueryable<TEntity> queryable, TQueryForm queryForm, Type builderType, QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> builder)
        {
            var getter = builderType.GetProperty("QueryFunc")!.GetMethod!.Invoke(builder, null)!;
            var invoker = getter.GetType().GetMethod("Invoke")!;
            return (IQueryable<TEntity>)invoker.Invoke(getter, new object[] { queryable, queryForm })!;
        }

        private static async Task<object?> InvokeOptionsFunc(TQueryFormParameters @params, Type builderType, QueryBuilder<TQueryForm, TQueryResultItem, TQueryFormParameters, TEntity> builder)
        {
            var getter = builderType.GetProperty("OptionsFunc")!.GetMethod!.Invoke(builder, null)!;
            var invoker = getter.GetType().GetMethod("Invoke")!;
            var task = (Task)invoker.Invoke(getter, new object[] { @params })!;
            await task;
            return task.GetType().GetProperty("Result")!.GetValue(task);
        }
    }
}
