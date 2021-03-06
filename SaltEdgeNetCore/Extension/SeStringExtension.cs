using System;
using System.Linq;
using System.Text;

namespace SaltEdgeNetCore.Extension
{
    public static class SeStringExtension
    {
        public static string ToString(this DateTime? dt, string format)
            => dt == null ? "" : ((DateTime) dt).ToString(format);
    }
}