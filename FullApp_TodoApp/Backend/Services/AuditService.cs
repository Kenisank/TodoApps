using Backend.Models;

namespace Backend.Services
{
    public class AuditService
    {
        private readonly TodoDbContext _dbContext;

        public AuditService(TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogActionAsync(string action, string user, string details)
        {
            var audit = new Audit
            {
                Action = action,
                Timestamp = DateTime.UtcNow,
                User = user,
                Details = details
            };

            _dbContext.Audits.Add(audit);
            await _dbContext.SaveChangesAsync();
        }
    }

}
