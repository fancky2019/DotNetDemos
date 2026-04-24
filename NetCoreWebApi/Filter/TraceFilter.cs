using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System.Threading;
using System;

namespace NetCoreWebApi.Filter
{
    public class TraceFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("  MappedDiagnosticsLogicalContext.Set(\"uuid\", Guid.NewGuid());");
            MappedDiagnosticsLogicalContext.Set("uuid", Guid.NewGuid());
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine(" MappedDiagnosticsLogicalContext.Clear();");
            MappedDiagnosticsLogicalContext.Clear();
        }
    }
}
