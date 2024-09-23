using ApiProject.Model;
using ApiProject.Model.DogModel;
using ApiProject.Model.UserModel;
using System.Security.Claims;

namespace ApiProject.Services
{
    public interface IUserService
    {
        Task<User> GetByName(string name);
        Task<bool> IsValidUserAsync(LoginUser users);

        UserRefreshTokens AddUserRefreshTokens(UserRefreshTokens user);

        UserRefreshTokens GetSavedRefreshTokens(int id, string username, string refreshtoken);

        void DeleteUserRefreshTokens(int id, string username, string refreshToken);
        

    }
}
