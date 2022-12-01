using Microsoft.Extensions.Options;
using Moq;
using Pelican.Application.RestSharp;
using Pelican.Domain.Settings;

namespace Pelican.Application.Test.RestSharp;

public class RestSharpClientTests
{
	private readonly RestSharpClient<BaseSettings> _uut;
	private readonly Mock<IOptions<BaseSettings>> _settingsMock = new();

	public RestSharpClientTests()
	{
		_uut = new(_settingsMock.Object);
	}
}
