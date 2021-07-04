using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Utils
{
    public static class Extensions
    {
        public static string GetIPAddress(this IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                string remoteIpAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    remoteIpAddress = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];

                return remoteIpAddress;
            }
            catch (Exception)
            {}
            return null;
        }
    }
}
