using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Utils
{
    public class WithMemoryCache<T> where T : new()
    {
        public IMemoryCache? MemoryCache { get; set; }
        public T Instance { get; set; }

        public WithMemoryCache()
        {
            Instance = new T();
        }   
    }
}
