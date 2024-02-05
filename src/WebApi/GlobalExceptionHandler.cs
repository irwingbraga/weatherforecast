using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApi
{
	internal class GlobalExceptionHandler : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			var details = new ProblemDetails()
			{
				Instance = httpContext.Request.Path
			};

			if (exception is ValidationException validationException)
			{
				details.Status = (int)HttpStatusCode.BadRequest;
				details.Title = "Operation Failed";
				details.Detail = "Error details provided below for diagnosis.";
				var errors = validationException.Errors.GroupBy(
					x => x.PropertyName,
					x => x.ErrorMessage,
					(propertyName, errorMessages) => new
					{
						Key = propertyName,
						Values = errorMessages.Distinct().ToArray()
					})
				.ToDictionary(x => x.Key, x => x.Values);

				details.Extensions = new Dictionary<string, object?>()
				{
					{ "errors", errors.Select(p => new {Property = p.Key, Messages = p.Value }) }
				};

				httpContext.Response.StatusCode = details.Status.Value;
			}
			else
			{
				details.Status = httpContext.Response.StatusCode;
				details.Title = "Unexpected Error Occurred";
				details.Detail = exception.Message;
			}

			await httpContext.Response.WriteAsJsonAsync(details, cancellationToken: cancellationToken);

			return true;
		}
	}
}
