namespace Pelican.Application.RazorEmails.Interfaces;
public interface IRazorViewToStringRenderer
{
	Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
}
