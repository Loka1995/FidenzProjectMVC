using FidenzProjectMVC.Data;
using FidenzProjectMVC.Models.Dto;
using FidenzProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FidenzProjectMVC.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private const double EarthRadiusKm = 6371.0;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string word)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(word))
            {
                query = query.Where(u => u.Name.Contains(word) || u.EyeColor.Contains(word) || u.Email.Contains(word));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UsersByZipCodeDto>> GetUsersGroupedByZipCodeAsync()
        {
            var groupedUsers = await _context.Users
                .GroupBy(u => u.Address.ZipCode)
                .ToListAsync();

            return groupedUsers.Select(group => new UsersByZipCodeDto
            {
                ZipCode = group.Key,
                Users = group.ToList(),
                // UserNames = group.Select(u => u.Name).ToList()
            });
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public async Task<double> CalculateDistanceAsync(string userId, double latitude, double longitude)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            double lat1 = user.Latitude * (Math.PI / 180.0);
            double lon1 = user.Longitude * (Math.PI / 180.0);
            double lat2 = latitude * (Math.PI / 180.0);
            double lon2 = longitude * (Math.PI / 180.0);

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }
    }
}
