using System.Text;

namespace Pelican.Presentation.GraphQL.Errors;
public class PelicanErrorFilter : IErrorFilter
{
	public IError OnError(IError error)
	{
		if (error.Exception != null)
		{
			var exceptionType = error.Exception.GetType().Name;
			var messageBuilder = new StringBuilder($"[{exceptionType}] ");
			messageBuilder.Append(error.Exception.Message);

			var inner = error.Exception.InnerException;
			while (inner != null)
			{
				messageBuilder.AppendLine(inner.Message);
				inner = inner.InnerException;
			}

			error = error.WithMessage(messageBuilder.ToString());
			error = error.SetExtension("exceptionType", exceptionType);
		}

		return error;
	}
}
