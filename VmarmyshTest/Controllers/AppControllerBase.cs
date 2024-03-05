using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using VmarmyshTest.Models;

namespace VmarmyshTest.Controllers
{
    public static class ControllerExtensions
    {

        public static void CheckRequiredFields(this AppControllerBase controller, MethodBase? currentMethod)
        {
            var parameters = currentMethod!.GetParameters();
            foreach (var item in parameters)
            {
                if (item.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(BindRequiredAttribute)) != null)
                {
                    var value = controller.Request.Query.FirstOrDefault(x => x.Key == item.Name).Value;
                    if (string.IsNullOrEmpty(value.ToString().Trim()))
                    {
                        throw new SecureException($"The {item.Name} field is required.");
                    }
                }
            }
        }
    }

    public class AppControllerBase : ControllerBase
    {
    }
}