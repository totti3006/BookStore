using BookStore.Application.Models.User;
using System.Linq.Expressions;

namespace BookStore.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> AddUser(CreateUserDto createUserDto);
        Task<UserDto?> AuthorizeUser(string email, string passwordRaw);
        Task<bool> IsExistUser(string email);
        Task<bool> ChangePassword(ResetPasswordDto resetPasswordDto);
    }
}
