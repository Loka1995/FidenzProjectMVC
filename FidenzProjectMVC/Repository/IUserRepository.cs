using FidenzProjectMVC.Models.Dto;
using FidenzProjectMVC.Models;

namespace FidenzProjectMVC.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> SearchUsersAsync(string word);
        Task<IEnumerable<UsersByZipCodeDto>> GetUsersGroupedByZipCodeAsync();
        void UpdateUser(User user);
        Task<double> CalculateDistanceAsync(string userId, double latitude, double longitude);
    }
}
