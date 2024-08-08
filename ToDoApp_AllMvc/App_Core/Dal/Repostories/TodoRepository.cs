using App_Core.Dal.Repostories.Interfaces;
using App_Core.Data;
using App_Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace App_Core.Dal.Repostories
{
    public class TodoRepository : Repository<TodoItem>, ITodoRepository
    {

        public TodoRepository(TodoContext context) : base(context)
        {
        }

    }
}
