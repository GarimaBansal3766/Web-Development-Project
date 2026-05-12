using Microsoft.AspNetCore.Mvc;
using Online_Shoppng.Models.Entity;
using Online_Shoppng.Models.Services;

namespace Online_Shoppng.Controllers
{
    public class LoginController : Controller
    {
        private Iflogin _iflogin;

        public LoginController(Iflogin _iflg)
        {
            _iflogin = _iflg;
        }
        public IActionResult CreateAccount()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Signup(login lg)
        {
            // 1. Check if required fields are provided
            if (string.IsNullOrEmpty(lg.Username) ||
                string.IsNullOrEmpty(lg.Password) ||
                string.IsNullOrEmpty(lg.PhoneNumber) ||
                string.IsNullOrEmpty(lg.Email))
            {
                return Json(new { success = false, message = "All fields are required." });
            }

            // 2. Validate password length (must be between 6 and 8 characters)
            if (lg.Password.Length < 6 || lg.Password.Length > 8)
            {
                return Json(new { success = false, message = "Password must be between 6 to 8 characters." });
            }

            // 3. Validate phone number (must be exactly 10 digits)
            if (lg.PhoneNumber.Length != 10 || !lg.PhoneNumber.All(char.IsDigit))
            {
                return Json(new { success = false, message = "Phone number must be exactly 10 digits." });
            }

            // All validations passed – create account

            lg.Role = "User";
            lg.ProfileImage = null;

            _iflogin.CreateAccount(lg);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Login(login lg)
        {
            login lgobj = _iflogin.LoginAccount(lg);

            if (lgobj != null)
            {
                HttpContext.Session.SetString("Username", lgobj.Username);
                HttpContext.Session.SetString("Role", lgobj.Role);

                    if (lgobj.Role == "User")
                {
                    return Json(new { success = true, role = "User" });
                }
                else if (lgobj.Role == "Admin")
                {
                    return Json(new { success = false, role = "Admin", message = "Redirecting to admin..." });
                }
            }

            return Json(new { success = false, message = "Invalid credentials." });
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();   // 🔥 Clear all session data
            return Ok();
        }


    }
}
