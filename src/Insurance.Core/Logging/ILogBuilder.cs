using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Core.Logging
{
    public interface ILogBuilder
    {
        string BuildLog(MethodBase methodBase, string details, dynamic criticalInput = null);
    }
}
