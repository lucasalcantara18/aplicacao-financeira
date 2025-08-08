
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Text;

namespace WebAppCubos.Controllers.Login
{
    public class EncryptPasswordAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var arg in context.ActionArguments.Values)
            {
                if (arg == null) continue;

                var props = arg.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(p => p.Name.Equals("Password", StringComparison.OrdinalIgnoreCase));

                foreach (var prop in props)
                {
                    var value = prop.GetValue(arg) as string;
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var encrypted = value.CriptografarSenha();
                        prop.SetValue(arg, encrypted);
                    }
                }
            }
        }
    }
}