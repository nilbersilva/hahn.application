using Hahn.ApplicatonProcess.February2021.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _dbContext;

        public UnitOfWork(DataContext context)
        {
            _dbContext = context;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
