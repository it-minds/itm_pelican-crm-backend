namespace Pelican.Application.Auth;
public interface IGroupService
{
	/// <summary>
	/// Retrieve all groups of a domain or of a user given a userKey (paginated)
	/// </summary>
	/// <param name="userKey">Email or immutable ID of the user if only those groups are to be listed, the given user is a member of. If it's an ID, it should match with the ID of the user object.</param>
	/// <returns>List of group emails</returns>
	Task<IEnumerable<string>> GetGroupsForUser(string userKey);
}
