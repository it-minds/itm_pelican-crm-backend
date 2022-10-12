using Pelican.Domain.Enums;

namespace Pelican.Infrastructure.Google.Authentication.Models;

public struct EmployeeClaimData
{
	public string EmployeeId;
	public string EmployeeName;
	public Company Company;
	public IEnumerable<string> UserGroups;
};
