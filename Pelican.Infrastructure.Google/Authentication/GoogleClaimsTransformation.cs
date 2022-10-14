using System.Security.Claims;
using LazyCache;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Hosting;
using Pelican.Application.Auth;
using Pelican.Infrastructure.Google.Authentication.Claims;
using Pelican.Infrastructure.Google.Authentication.Interfaces;


namespace Pelican.Infrastructure.Google.Authentication;
public class GoogleClaimsTransformation : IClaimsTransformation
{
	private readonly IEmployeeClaimsService _employeeClaimsService;
	private readonly IHostEnvironment _environment;
	private readonly IAppCache _appCache;
	private readonly IGroupService _groupService;
	public GoogleClaimsTransformation(IEmployeeClaimsService employeeClaimsService, IHostEnvironment environment, IAppCache appCache, IGroupService groupService)
	{
		_employeeClaimsService = employeeClaimsService;
		_environment = environment;
		_appCache = appCache;
		_groupService = groupService;
	}
	//This funtion gets the claims of the currently loggedin user.
	public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
	{
		ClaimsIdentity claimsIdentity = (ClaimsIdentity)principal.Identity;
		var userEmail = principal.Claims.FirstOrDefault(c => c.Type == GoogleClaims.Email).Value;
		var employeeClaimsData = await _employeeClaimsService.GetEmployeeClaimDataAsync(userEmail, CancellationToken.None);
		if (employeeClaimsData.EmployeeId != null || employeeClaimsData.EmployeeId == String.Empty)
		{
			claimsIdentity.AddClaim(new Claim(CustomClaims.EmployeeId, employeeClaimsData.EmployeeId));
		}
		if (employeeClaimsData.EmployeeName != null || employeeClaimsData.EmployeeName == String.Empty)
		{
			claimsIdentity.AddClaim(new Claim(CustomClaims.EmployeeName, employeeClaimsData.EmployeeName));
		}
		if (employeeClaimsData.Company != null || employeeClaimsData.Company == String.Empty)
		{
			claimsIdentity.AddClaim(new Claim(CustomClaims.Company, employeeClaimsData.Company.ToString()));
		}
		var groupCacheKey = GoogleGroups.GetGroupCacheStringForUser(userEmail);
		////The Code below is commented out as of 14/10/2022 this code gets the google groups that a user is a part of from google
		////Currently this has not been implemented but will be shortly.
		//Func<Task<IEnumerable<string>>> getUserGroups = () => _groupService.GetGroupsForUser(userEmail);
		//var userGroups = await _appCache.GetOrAddAsync(groupCacheKey, getUserGroups);
		//if (IsAccountManager(userGroups))
		//{
		//	//Add accesspolicies when these are created
		//	AddPolicyClaims(claimsIdentity);
		//}
		//if (IsDirector(userGroups))
		//{
		//	//Add accesspolicies when these are created
		//	AddPolicyClaims(claimsIdentity);
		//}
		//if (IsSalesManager(userGroups))
		//{
		//	//Add accesspolicies when these are created
		//	AddPolicyClaims(claimsIdentity);
		//}
		return principal;
	}
	//This function will in the future be used to add claims
	private ClaimsIdentity AddPolicyClaims(ClaimsIdentity claimsIdentity, params AccessPolicies[] policyNames)
	{
		foreach (var policyName in policyNames)
		{
			if (claimsIdentity.Claims.Any(c => c.Type == policyName.ToString())) continue;

			claimsIdentity.AddClaim(new Claim(policyName.ToString(), bool.TrueString));
		}

		return claimsIdentity;
	}
	//This function checks if the the current user loggedin is part of any group that would make them an AccountManager
	private bool IsAccountManager(IEnumerable<string> groups)
	{
		return groups.Contains(GoogleGroups.SalesAalborg) || groups.Contains(GoogleGroups.SalesAarhus) || groups.Contains(GoogleGroups.SalesCph) || groups.Contains(GoogleGroups.SalesNorway);
	}
	//This function checks if the the current user loggedin is part of any group that would make them a SaleManager
	private bool IsSalesManager(IEnumerable<string> groups)
	{
		return groups.Contains(GoogleGroups.SalesManager) || groups.Contains(GoogleGroups.SalesManagerNo);
	}
	//This function checks if the the current user loggedin is part of any group that would make them a Direvtor
	private bool IsDirector(IEnumerable<string> groups)
	{
		return groups.Contains(GoogleGroups.Directors) || groups.Contains(GoogleGroups.DirectorsNo);
	}
}
