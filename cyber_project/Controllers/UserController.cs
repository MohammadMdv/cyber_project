using System.Web.Mvc;
using cyber_project.App_Data;
using Microsoft.AspNetCore.Mvc;
using cyber_project.Models;

namespace cyber_project.Controllers
{

    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext; // Replace YourDbContext with your actual database context class
        
        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        // GET: /User/Register
        public IActionResult Register()
        {
            return View();
        }
        
        // POST: /User/Register
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the username is unique (you may need additional validation)
                if (_dbContext.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(model);
                }
        
                // Register the user
                var newUser = User.RegisterUser(model.Username, model.Password);
        
                // Save the new user to the database
                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();
        
                // Redirect to a success page or login page
                return RedirectToAction("RegistrationSuccess");
            }
        
            // If the model is not valid, return to the registration page
            return View(model);
        }
        
        // GET: /User/RegistrationSuccess
        public IActionResult RegistrationSuccess()
        {
            return View();
        }
    }

    public class RegisterViewModel
    {
    }
}