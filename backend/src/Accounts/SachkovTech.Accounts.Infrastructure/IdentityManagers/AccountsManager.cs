using SachkovTech.Accounts.Domain;

namespace SachkovTech.Accounts.Infrastructure.IdentityManagers;

public class AccountsManager(AccountsWriteDbContext accountsWriteContext)
{
    public async Task CreateAdminAccount(AdminAccount adminAccount)
    {
        await accountsWriteContext.AdminAccounts.AddAsync(adminAccount);
        await accountsWriteContext.SaveChangesAsync();
    }
}