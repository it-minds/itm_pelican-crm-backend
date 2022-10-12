using System.Security.Claims;
using LazyCache;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Hosting;
using Pelican.Infrastructure.Google.Authentication.Interfaces;


namespace Pelican.Infrastructure.Google.Authentication;
public class GoogleClaimsTransformation : IClaimsTransformation
{
	private readonly IEmployeeClaimsService _employeeClaimsService;
	private readonly IHostEnvironment _environment;
	private readonly IAppCache _appCache;
	public GoogleClaimsTransformation(IEmployeeClaimsService employeeClaimsService, IHostEnvironment environment, IAppCache appCache)
	{
		_employeeClaimsService = employeeClaimsService;
		_environment = environment;
		_appCache = appCache;
	}

	public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
	{
		ClaimsIdentity claimsIdentity = (ClaimsIdentity)principal.Identity;
		var userEmail = principal.Claims.FirstOrDefault(c => c.Type == GoogleClaims.Email).Value;
		var employeeClaimsData = await _employeeClaimsService.GetEmployeeClaimDataAsync(userEmail, CancellationToken.None);
		if (employeeClaimsData.EmployeeId == null || employeeClaimsData.EmployeeId == String.Empty)
		{
			claimsIdentity.AddClaim(new Claim(CustomClaims.EmployeeId, employeeClaimsData.EmployeeId));
		}
		if (employeeClaimsData.EmployeeName == null || employeeClaimsData.EmployeeName == String.Empty)
		{
			claimsIdentity.AddClaim(new Claim(CustomClaims.EmployeeName, employeeClaimsData.EmployeeName));
		}
		claimsIdentity.AddClaim(new Claim(CustomClaims.Company, employeeClaimsData.Company.ToString()));
		var userGroups = employeeClaimsData.UserGroups;

		if (IsAccountManager(userGroups))
		{
			//Add accesspolicies when these are created
			AddPolicyClaims(claimsIdentity);
		}
		if (IsDirector(userGroups))
		{
			//Add accesspolicies when these are created
			AddPolicyClaims(claimsIdentity);
		}
		if (IsSalesManager(userGroups))
		{
			//Add accesspolicies when these are created
			AddPolicyClaims(claimsIdentity);
		}
		return principal;
	}
	private ClaimsIdentity AddPolicyClaims(ClaimsIdentity claimsIdentity, params AccessPolicies[] policyNames)
	{
		foreach (var policyName in policyNames)
		{
			if (claimsIdentity.Claims.Any(c => c.Type == policyName.ToString())) continue;

			claimsIdentity.AddClaim(new Claim(policyName.ToString(), bool.TrueString));
		}

		return claimsIdentity;
	}
	private bool IsAccountManager(IEnumerable<string> groups)
	{
		return groups.Contains(GoogleGroups.SalesAalborg) || groups.Contains(GoogleGroups.SalesAarhus) || groups.Contains(GoogleGroups.SalesCph) || groups.Contains(GoogleGroups.SalesNorway);
	}
	private bool IsSalesManager(IEnumerable<string> groups)
	{
		return groups.Contains(GoogleGroups.SalesManager) || groups.Contains(GoogleGroups.SalesManagerNo);
	}
	private bool IsDirector(IEnumerable<string> groups)
	{
		return groups.Contains(GoogleGroups.Directors) || groups.Contains(GoogleGroups.DirectorsNo);
	}
}
