using FidenzProjectMVC.Common.Interfaces;
using FidenzProjectMVC.Data;

namespace FidenzProjectMVC.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository User { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            User = new UserRepository(_context);
        }
    }
}
