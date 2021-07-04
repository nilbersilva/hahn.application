using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Interfaces
{
    public interface IAudit
    {
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset ModifiedAt { get; set; }

        public string CreatByIPAddress { get; set; }

        public string ModifiedByIPAddress { get; set; }

        public byte[] VersionStamp { get; set; }
    }
}
