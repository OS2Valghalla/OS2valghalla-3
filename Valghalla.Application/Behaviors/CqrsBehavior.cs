using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;

namespace Valghalla.Application.Behaviors
{
    public sealed class CqrsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : Response
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;
        private readonly IServiceProvider serviceProvider;

        public CqrsBehavior(IEnumerable<IValidator<TRequest>> validators, IServiceProvider serviceProvider)
        {
            this.validators = validators;
            this.serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var errorsDictionary = validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (errorsDictionary.Any())
            {
                await Task.CompletedTask;

                var response = Activator.CreateInstance<TResponse>();
                response.IsSuccess = false;
                response.Message = "shared.error.message";
                response.Errors = new Dictionary<string, string[]>
                {
                    { "validation", errorsDictionary.Select(i => i.ErrorMessage).ToArray() }
                };

                return response;
            }

            if (request is ConfirmationCommand command && !command.Confirmed)
            {
                var confirmator = serviceProvider.GetService<IConfirmator<TRequest>>();

                if (confirmator != null)
                {
                    await Task.CompletedTask;

                    var response = Activator.CreateInstance<TResponse>();
                    response.IsSuccess = true;

                    var title = confirmator.Title;
                    var messages = new List<string>()
                    {
                        confirmator.Message
                    };

                    foreach (var rule in confirmator.Rules)
                    {
                        var result = await rule.Predicate(request, cancellationToken);

                        if (!result) continue;

                        if (!string.IsNullOrEmpty(rule.Message))
                        {
                            if (confirmator.MultipleMessageEnabled)
                            {
                                messages.Add(rule.Message);
                                continue;
                            }
                            else
                            {
                                messages = new List<string>()
                                {
                                    rule.Message
                                };
                            }
                        }
                        else if (rule.MessageProvider != null)
                        {
                            if (confirmator.MultipleMessageEnabled)
                            {
                                var ruleMessages = await rule.MessageProvider(request, cancellationToken);
                                messages.AddRange(ruleMessages);
                                continue;
                            }
                            else
                            {
                                var ruleMessages = await rule.MessageProvider(request, cancellationToken);
                                messages = ruleMessages.ToList();
                            }
                        }

                        response.Confirmation = new()
                        {
                            Title = title,
                            Messages = messages.ToArray(),
                        };

                        return response;
                    }

                    if (!confirmator.BypassIfNoRuleTriggers)
                    {
                        response.Confirmation = new()
                        {
                            Title = title,
                            Messages = messages.ToArray(),
                        };

                        return response;
                    }
                }
            }

            return await next();
        }
    }
}
