using App_Core.Dal.Repostories.Interfaces;
using App_Core.Data;
using App_Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace App_Core.Dal.Repostories
{
    public class AuditRepository : Repository<Audit>, IAuditRepository
    {

        public AuditRepository(TodoContext context) : base(context)
        {
        }

    }
}
