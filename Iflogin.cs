using Online_Shoppng.Models.Entity;

namespace Online_Shoppng.Models.Services
{
    public interface Iflogin
    {
        int CreateAccount(login lg);
        login LoginAccount(login lg);
        login GetUserByUsername(string username);
        void UpdateProfileImage(string username, string imagePath);

    }
}
