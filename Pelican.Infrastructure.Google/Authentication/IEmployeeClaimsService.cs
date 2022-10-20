using Pelican.Infrastructure.Authentication.Authentication.Models;

namespace Pelican.Infrastructure.Authentication.Authentication;

public interface IEmployeeClaimsService
{
	Task<EmployeeClaimData> GetEmployeeClaimDataAsync(string email, CancellationToken cancellationToken = default);
}
