using Microsoft.AspNetCore.Mvc;
using OnlineVoteApplication.Models;

namespace OnlineVoteApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly OnlineVoteApplicationContext _context;

        public AccountController(OnlineVoteApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                if (loginModel != null)
                {
                    var checkUser = _context.Voters.Where(x => x.EmailId == loginModel.Username && x.Password == loginModel.Password && x.IsApprovedUser == true).FirstOrDefault();
                    var checkAdmin = _context.AdminUsers.Where(x => x.EmailID == loginModel.Username && x.Password == loginModel.Password).FirstOrDefault();
                    var checkVoting = _context.VotingSystem.Where(x => x.VoterEmail == loginModel.Username).FirstOrDefault();
                    if (checkUser != null && checkVoting == null)
                    {
                        ModelState.Clear();
                        TempData["LoginUserID"] = checkUser.Id;
                        return RedirectToAction("Index", "Voters");
                    }
                    else if (checkAdmin != null)
                    {
                        ModelState.Clear();
                        TempData["LoginUserID"] = checkAdmin.Id;
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (checkVoting != null)
                    {
                        ViewBag.AlreadyExist = "Your already done with your voting.";
                    }
                    else
                    {
                        ViewBag.InvalidMessage = "Username or Password is Invalid/Election commision admin not verified your profile.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View();
        }

    }
}
