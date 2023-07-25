using Microsoft.AspNetCore.Identity;

namespace WebBlog.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
