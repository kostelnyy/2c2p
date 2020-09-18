using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace CCPP.WebApi.Validation
{
    public class MaxSizeFilterAttribute : Attribute, IResourceFilter
    {
        private readonly int _maxSizeBytes;

        public MaxSizeFilterAttribute(int maxSizeBytes)
        {
            _maxSizeBytes = maxSizeBytes;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //nothing here
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (context.HttpContext.Request.ContentLength > _maxSizeBytes)
            {
                context.ModelState.AddModelError("FileSize", $"Request body size should not exceed {_maxSizeBytes} bytes.");
            }

        }
    }
}
