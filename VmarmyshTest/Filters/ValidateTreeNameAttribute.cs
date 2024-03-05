using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace VmarmyshTest.Filters
{
    //public class ErrorHandlingFilter : ExceptionFilterAttribute
    //{
    //    public override void OnException(ExceptionContext context)
    //    {
    //        var exception = context.Exception;
    //        Console.WriteLine(exception);
    //        ////log your exception here
    //        //context.ExceptionHandled = true; //optional 
    //    }
    //}

    public class ValidateTreeNameAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("treeName") || context.ActionArguments["treeName"] == null || string.IsNullOrWhiteSpace(context.ActionArguments["treeName"].ToString()))
            {
                context.Result = new StatusCodeResult(500); // Bad request
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
