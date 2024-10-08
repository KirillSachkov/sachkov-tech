using Microsoft.AspNetCore.Identity;

namespace SachkovTech.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private User()
    {
    }

    private List<Role> _roles = [];

    public IReadOnlyList<Role> Roles => _roles;

    public static User CreateAdmin(string userName, string email, Role role)
    {
        return new User
        {
            UserName = userName,
            Email = email,
            _roles = [role]
        };
    }
}