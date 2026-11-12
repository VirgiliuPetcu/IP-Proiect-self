using saxafon.Data;
using saxafon.Models;

namespace saxafon.Services
{


    public interface IUserService { 
    
    
    }
    public class UserService : IUserService
    {
        private readonly AppDbContext context;


        public UserService(AppDbContext _context) {
            context = _context;
        }

        public async Task<IEnumerable<User>> GetAllUsers() {
            return await context.Users.Select(usr => new User())
        }



    }



}
