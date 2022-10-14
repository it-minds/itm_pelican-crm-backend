using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using Pelican.Application.Auth;
using Pelican.Infrastructure.Google.Settings;

namespace Pelican.Infrastructure.Google.Authentication.Services;
///<inheritdoc cref="IGroupService"/>
public class GoogleGroupService : IGroupService
{
	//private readonly ICertificateService _certService;
	private readonly IOptionsMonitor<DirectorySettings> _directorySettingsMonitor;

	public GoogleGroupService(
		//ICertificateService certService, 
		IOptionsMonitor<DirectorySettings> _directorySettingsMonitor)
	{
		//_certService = certService;
		this._directorySettingsMonitor = _directorySettingsMonitor;
	}

	private async Task<DirectoryService> GetDirectoryService()
	{
		//var cert = await _certService.GetGoogleCertificate();
		ServiceAccountCredential credential = new ServiceAccountCredential(
			new ServiceAccountCredential.Initializer(_directorySettingsMonitor.CurrentValue.ServiceAccountEmail)
			{
				Scopes = new[] { DirectoryService.Scope.AdminDirectoryGroupReadonly },
				User = _directorySettingsMonitor.CurrentValue.AdminEmail
			});
		//.FromCertificate(cert));

		return new DirectoryService(
			new BaseClientService.Initializer
			{
				ApplicationName = _directorySettingsMonitor.CurrentValue.ApplicationName,
				HttpClientInitializer = credential,
			});
	}

	public async Task<IEnumerable<string>> GetGroupsForUser(string userKey)
	{
		var directoryService = await GetDirectoryService();
		var request = directoryService.Groups.List();
		request.UserKey = userKey;

		var result = await request.ExecuteAsync();
		return result.GroupsValue.Select(gv => gv.Email);
	}
}
