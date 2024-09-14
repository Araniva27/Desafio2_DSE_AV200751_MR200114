using APIUsuarios.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIUsuarios.Test2
{
    public static class Setup
    {
        public static UsuariosDBContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<UsuariosDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new UsuariosDBContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
