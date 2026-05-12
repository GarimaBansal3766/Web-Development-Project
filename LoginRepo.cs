using Microsoft.EntityFrameworkCore;
using Online_Shoppng.Models.Entity;
using Online_Shoppng.Models.Services;

namespace Online_Shoppng.Models.Repository
{
    public class LoginRepo : Iflogin
    {
        private OnlineShoppingDBcontext _dbcontext;
        public LoginRepo(OnlineShoppingDBcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public int CreateAccount(login lg)
        {
            _dbcontext.loginDetails.Add(lg);
            int i = _dbcontext.SaveChanges();
            return i;
        }

        public login LoginAccount(login lg)
        {
            return _dbcontext.loginDetails.FirstOrDefault(u=>u.Username == lg.Username && u.Password==lg.Password);
        }
        public login GetUserByUsername(string username)
        {
            return _dbcontext.loginDetails.FirstOrDefault(x => x.Username == username);
        }

        public void UpdateProfileImage(string username, string imagePath)
        {
            var user = _dbcontext.loginDetails.FirstOrDefault(x => x.Username == username);
            if (user != null)
            {
                user.ProfileImage = imagePath;
                _dbcontext.SaveChanges();
            }
        }

    }
}
