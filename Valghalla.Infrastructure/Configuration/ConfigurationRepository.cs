using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Valghalla.Application.Configuration.Interfaces;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Infrastructure.Configuration
{
    internal class ConfigurationRepository : IConfigurationRepository
    {
        private readonly IQueryable<ConfigurationEntity> configurations;

        public ConfigurationRepository(DataContext dataContext)
        {
            configurations = dataContext.Set<ConfigurationEntity>().AsNoTracking();
        }

        public async Task<T> GetConfigurationAsync<T>(CancellationToken cancellationToken) where T : IConfiguration
        {
            var type = typeof(T);
            var configuration = await GetConfigurationAsync(type, cancellationToken);
            return (T)configuration;
        }

        public async Task<object> GetConfigurationAsync(Type type, CancellationToken cancellationToken)
        {
            var configuration = Activator.CreateInstance(type)!;

            var keys = type.GetProperties().Select(prop => type.Name + "." + prop.Name).ToList();

            var configurationDict = await GetConfigurationByKeysAsync(keys, cancellationToken);

            foreach (var prop in type.GetProperties())
            {
                var key = (type.Name + "." + prop.Name).ToLowerInvariant();

                if (configurationDict.TryGetValue(key, out var setting))
                {
                    if (prop.PropertyType.IsListType())
                    {
                        var items = setting.Split(',').ToList();
                        prop.SetValue(configuration, items, null);
                    }
                    else
                    {
                        if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                            continue;

                        if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                            continue;

                        var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);
                        prop.SetValue(configuration, value, null);
                    }
                }
            }

            return configuration;
        }

        private async Task<Dictionary<string, string>> GetConfigurationByKeysAsync(IEnumerable<string> keys, CancellationToken cancellationToken)
        {
            var lowerCasedKeys = keys.Select(key => key.ToLower());
            var settings = await configurations.Where(x => lowerCasedKeys.Contains(x.Key.ToLower())).ToArrayAsync(cancellationToken);
            var dictionary = new Dictionary<string, string>();

            foreach (var s in settings)
            {
                var resourceName = s.Key.ToLower();
                dictionary.Add(resourceName, s.Value);
            }

            foreach (var key in lowerCasedKeys)
            {
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, string.Empty);
                }
            }

            return dictionary;
        }
    }
}
