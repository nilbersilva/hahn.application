using Hahn.ApplicatonProcess.February2021.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Infrastructure
{
    public class DataContext : DbContext
    {
        public DbSet<Asset> Assets { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {}
    }
}
