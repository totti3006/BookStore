using BookStore.Application.Extensions;
using BookStore.Application.Interfaces.Repositories;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.User;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;

namespace BookStore.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddUser(CreateUserDto createUserDto)
        {
            try
            {
                bool isEmailDuplicate = await _unitOfWork.UserRepository.Any(u => u.Email == createUserDto.Email);

                if (isEmailDuplicate)
                {
                    return false;
                }

                User user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = createUserDto.Name,
                    Email = createUserDto.Email,
                    PasswordHash = createUserDto.PasswordRaw!.HashWithSHA256(createUserDto.CreatedDate.ToString()),
                    Role = Role.AuthorizedUser,
                    CreatedDate = createUserDto.CreatedDate,
                    LastModifiedDate = createUserDto.CreatedDate,
                };

                await _unitOfWork.UserRepository.Add(user);
                await _unitOfWork.SaveChanges();

                return true;
            } 
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto?> AuthorizeUser(string email, string passwordRaw)
        {
            try
            {
                User? user = await _unitOfWork.UserRepository.SingleOrDefault(u => u.Email == email);
                
                if (user is null)
                {
                    return null;
                }

                if (user.PasswordHash != passwordRaw.HashWithSHA256(user.CreatedDate.ToString()))
                {
                    return null;
                }

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Role = user.Role == Role.AuthorizedUser ? "user" : "admin"
                };

                return userDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ChangePassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                User? user = await _unitOfWork.UserRepository.SingleOrDefault(u => u.Email == resetPasswordDto.Email);

                if (user is null)
                {
                    return false;
                }

                Otp? otp = await _unitOfWork.OtpRepository.GetNonExpiredLastestOtp(user.Id);

                if (otp is null)
                {
                    return false;
                }

                otp.IsVerified = true;

                user.PasswordHash = resetPasswordDto.NewPasswordRaw!.HashWithSHA256(user.CreatedDate.ToString());

                await _unitOfWork.OtpRepository.Update(otp);

                await _unitOfWork.UserRepository.Update(user);

                await _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExistUser(string email)
        {
            try
            {
                return await _unitOfWork.UserRepository.Any(u => u.Email == email);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
