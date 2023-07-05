using System.Reflection;
using System.Web.Mvc;

namespace OnePage.Framework.Attribute.Mvc
{
    /// <summary>
    /// 只允許AJAX請求。
    /// </summary>
    public class ActionAjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}
