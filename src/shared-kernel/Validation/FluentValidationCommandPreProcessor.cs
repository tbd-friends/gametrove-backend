using Ardalis.Result;
using FluentValidation;
using FluentValidation.Results;
using Mediator;

namespace shared_kernel.Validation;

/// <summary>
/// FluentValidation based pre-processor with Result response instead of throwing a validation response
/// </summary>
/// <param name="validator"></param>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class FluentValidationCommandPreProcessor<TCommand, TResponse>(IValidator<TCommand> validator)
    : IPipelineBehavior<TCommand, TResponse>
    where TCommand : ICommand<TResponse>, IValidate
    where TResponse : Result
{
    public ValueTask<TResponse> Handle(TCommand message, MessageHandlerDelegate<TCommand, TResponse> next, CancellationToken cancellationToken)
    {
        var task = validator.ValidateAsync(message, cancellationToken);

        return task.IsCompletedSuccessfully
            ? next(message, cancellationToken)
            : HandleInternal(task, message, cancellationToken, next);

        static async ValueTask<TResponse> HandleInternal(
            Task<ValidationResult> task,
            TCommand message,
            CancellationToken cancellationToken,
            MessageHandlerDelegate<TCommand, TResponse> next
        )
        {
            var result = await task;

            if (result.IsValid)
            {
                return await next(message, cancellationToken);
            }

            return (TResponse)Result.Error(new ErrorList(result.Errors.Select(x => x.ErrorMessage)));
        }
    }
}