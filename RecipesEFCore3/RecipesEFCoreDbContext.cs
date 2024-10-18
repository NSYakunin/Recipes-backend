using Microsoft.EntityFrameworkCore;
using RecipesEFCore3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesEFCore.DataAccess.SQLServer
{
    public class RecipesEFCoreDbContext : DbContext
    {
        public RecipesEFCoreDbContext(DbContextOptions<RecipesEFCoreDbContext> options)
            : base(options) { }
        public DbSet<Recipe> Recipes { get; set; }
    }
}
