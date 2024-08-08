

using App_Core.Dal.Repostories.Interfaces;

namespace App_Core.Dal.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        ITodoRepository TodoLists { get; }
        IAuditRepository Audits { get; }
        void SaveAsync();
    }
}
