using SachkovTech.Accounts.Domain;

namespace SachkovTech.Accounts.Infrastructure.IdentityManagers;

public class AccountsManager(AccountsDbContext accountsContext)
{
    public async Task CreateAdminAccount(AdminAccount adminAccount)
    {
        await accountsContext.AdminAccounts.AddAsync(adminAccount);
        await accountsContext.SaveChangesAsync();
    }
}