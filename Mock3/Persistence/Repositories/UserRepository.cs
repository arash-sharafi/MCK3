using Mock3.Core.Models;
using Mock3.Core.Repositories;

namespace Mock3.Persistence.Repositories
{
    class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public ApplicationUser GetUserById(string userId)
        {
            return _context.Users.Find(userId);
        }
    }
}