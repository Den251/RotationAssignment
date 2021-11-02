using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotationAssignment.Models
{
    public class RotationContext : DbContext
    {
        public RotationContext(DbContextOptions<RotationContext> options)
            : base(options)
        {
        }

        public DbSet<Cargo> Cargoes { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
    }
}
