using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebBlog.Hubs;
using WebBlog.Models.ViewModels;
using WebBlog.Repositories;

namespace WebBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IHubContext<NotificationHub> notificationHub;

        public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager, IHubContext<NotificationHub> notificationHub)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.notificationHub = notificationHub;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await userRepository.GetAll();

            var userViewModel = new UserViewModel();
            userViewModel.Users = new List<User>();

            foreach (var user in users)
            {
                userViewModel.Users.Add(
                    new Models.ViewModels.User
                    {
                        Id = Guid.Parse(user.Id),
                        Username = user.UserName,
                        EmailAddress = user.Email
                    });
            }

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
            };

            var identityResult = await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult is not null)
            {
                if (identityResult.Succeeded)
                {
                    var roles = new List<string> { "User"};
                    if (request.AdminRoleCheckbox) {
                        roles.Add("Admin");
                    }

                    identityResult= await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult is not null && identityResult.Succeeded) 
                    {
                        return RedirectToAction("List","AdminUsers");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id) 
        {
            var user= await userManager.FindByIdAsync(id.ToString());

            if (user is not null)
            {
                var identityResult = await userManager.DeleteAsync(user);
                if (identityResult is not null && identityResult.Succeeded)
                {
                    return RedirectToAction("List","AdminUsers");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendSignal(string message) 
        {
            await notificationHub.Clients.All.SendAsync("ReceiveMsg",message);
            return RedirectToAction("List", "AdminUsers");
        }
    }
}
