using Mock3.Core.Models;

namespace Mock3.Core.Repositories
{
    public interface IUserRepository
    {
        ApplicationUser GetUserById(string userId);
    }
}