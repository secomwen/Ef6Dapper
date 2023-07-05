using System;

namespace OnePage.Framework.Xsrf
{
    /// <summary>
    /// 動作或控制器標註此屬性將忽略檢查AJAX防範偽造要求。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowAjaxValidateAntiForgeryTokenAttribute : System.Attribute { }
}
