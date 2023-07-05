using System.Linq;
using System.Net;
using System.Web.Mvc;
using OnePage.Data;
using OnePage.Service.Interface;

namespace OnePage.Framework.Attribute.Mvc
{
    /// <summary>
    /// 模型繫結驗證。
    /// </summary>
    public class ValidationModelFilter : ActionFilterAttribute
    {
        private readonly ILogService logService;

        public ValidationModelFilter()
        {
            logService = DependencyResolver.Current.GetService<ILogService>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ModelStateDictionary modelState = filterContext.Controller.ViewData.ModelState;

            if (!modelState.IsValid)
            {
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string actionName = filterContext.ActionDescriptor.ActionName;

                string errorString = string.Join(";", modelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.ToArray()
                    ).Select(d => d.Key + "=[" + string.Join(",", d.Value.Select(v => v.ErrorMessage + (v.Exception == null ? string.Empty : v.Exception.ToString()))) + "]"));

                string logString = string.Format("Controller:{0} Action:{1} Message:{2}", controllerName, actionName, errorString);

                logService.Create(LogType.ModelStateInvalid, logString);

                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                filterContext.Result = new EmptyResult();
            }
        }
    }
}
