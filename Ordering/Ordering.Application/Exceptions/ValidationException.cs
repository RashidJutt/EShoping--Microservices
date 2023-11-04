using FluentValidation.Results;

namespace Ordering.Application.Exceptions;

public class ValidationException : ApplicationException
{
    public IDictionary<string, string[]> Errors { get; set; }
    public ValidationException() : base("One or more error(s) occurred")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
    {
        Errors = failures
            .GroupBy(x => x.PropertyName, x => x.ErrorMessage) // Second argument indicated property that needs to be grouped.
            .ToDictionary(x => x.Key, x => x.ToArray());
    }
}
