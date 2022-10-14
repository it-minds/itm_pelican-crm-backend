namespace Pelican.Infrastructure.Google.Authentication.Models;

public struct EmployeeClaimData
{
	public string EmployeeId;
	public string EmployeeName;
	public string Company;
	public IEnumerable<string> UserGroups;
};
