using FidenzProjectMVC.Repository;

namespace FidenzProjectMVC.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
    }
}
