using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TrainingProject.Models
{
    public class TrainingProjectContext : DbContext
    {
        public TrainingProjectContext (DbContextOptions<TrainingProjectContext> options)
            : base(options)
        {
        }

        public DbSet<TrainingProject.Models.GasStation> GasStation { get; set; }
    }
}
