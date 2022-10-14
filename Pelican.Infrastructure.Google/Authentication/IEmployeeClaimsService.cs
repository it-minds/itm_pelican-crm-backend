using Pelican.Infrastructure.Google.Authentication.Models;

namespace Pelican.Infrastructure.Google.Authentication.Interfaces;

public interface IEmployeeClaimsService
{
	Task<EmployeeClaimData> GetEmployeeClaimDataAsync(string email, CancellationToken cancellationToken = default);
}
