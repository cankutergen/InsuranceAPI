using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Core.Logging
{
    public class LogBuilder : ILogBuilder
    {
        public string BuildLog(MethodBase method, string details, dynamic input = null)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Business Unit: {method.ReflectedType.FullName}");
            builder.AppendLine($"Details: {details}");
            builder.AppendLine($"Input: {JsonConvert.SerializeObject(input)}");

            return builder.ToString();
        }
    }
}
