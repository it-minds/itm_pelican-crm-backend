namespace Pelican.Presentation.Api;

public class StartUp
{
	public IConfiguration Configuration { get; set; }
	public IWebHostEnvironment Environment { get; set; }
	public StartUp(IConfiguration configuration, IWebHostEnvironment environment)
	{
		Configuration = configuration;
		Environment = environment;
	}

}
