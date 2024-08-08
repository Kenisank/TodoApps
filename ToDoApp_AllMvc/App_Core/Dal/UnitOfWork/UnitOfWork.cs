
using App_Core.Dal.Repostories.Interfaces;
using App_Core.Dal.Repostories;
using App_Core.Data;

namespace App_Core.Dal.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoContext _context;
        private bool disposed = false;
        private ITodoRepository _todoItemRepository;
        private IAuditRepository _auditRepository;

        public UnitOfWork(TodoContext context) { _context = context; }

        public ITodoRepository TodoLists
        {
            get
            {

                return _todoItemRepository = this._todoItemRepository == null ? new TodoRepository(_context) : _todoItemRepository;
            }
        }

        public IAuditRepository Audits
        {
            get
            {

                return _auditRepository = this._auditRepository == null ? new AuditRepository(_context) : _auditRepository;
            }
        }


        public void SaveAsync()
        {
            _context.SaveChangesAsync();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
