using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OnePage.Data;
using OnePage.Service.Interface;

namespace OnePage.Framework.View
{
    /// <summary>
    /// 語系輔助類別。
    /// </summary>
    public class GlobalizationHelper
    {
        private readonly ILanguageService languageService;
        private readonly ILocaleStringResourceService localeStringResourceService;

        public GlobalizationHelper(ViewContext viewContext, IViewDataContainer viewDataContainer)
        {
            languageService = DependencyResolver.Current.GetService<ILanguageService>();
            localeStringResourceService = DependencyResolver.Current.GetService<ILocaleStringResourceService>();
        }

        /// <summary>
        /// 目前頁面文化特性。
        /// </summary>
        public string RouteCultureValue
        {
            get
            {
                return GetRouteData("culture");
            }
        }

        /// <summary>
        /// 目前控制器名稱。
        /// </summary>
        private string CurrentController
        {
            get
            {
                return GetRouteData("controller");
            }
        }

        /// <summary>
        /// 目前動作名稱。
        /// </summary>
        private string CurrentAction
        {
            get
            {
                return GetRouteData("action");
            }
        }

        /// <summary>
        /// 取得路由值。
        /// </summary>
        /// <param name="key">路由值名稱。</param>
        /// <returns>路由值。</returns>
        private string GetRouteData(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return string.Empty;

            return Convert.ToString(HttpContext.Current.Request.RequestContext.RouteData.Values[key]);
        }

        /// <summary>
        /// 取得語系化文字。
        /// </summary>
        /// <param name="resourceName">ResourceName.</param>
        /// <returns>內容。</returns>
        public string ResourceString(string resourceName)
        {
            Language language = languageService.GetByCultureOrDefault(RouteCultureValue);

            if (language == null)
                return resourceName;

            return localeStringResourceService.GetByLanguageIdAndName(language.Id, resourceName);
        }

        /// <summary>
        /// 取得語系化文字。
        /// </summary>
        /// <param name="resourceName">ResourceName.</param>
        /// <param name="culture">Culture.</param>
        /// <returns>內容。</returns>
        public string ResourceString(string resourceName, string culture)
        {
            Language language = languageService.GetByCultureOrDefault(culture);

            if (language == null)
                return resourceName;

            return localeStringResourceService.GetByLanguageIdAndName(language.Id, resourceName);
        }

        /// <summary>
        /// 語言清單項目。
        /// </summary>
        /// <returns>語言清單項目。</returns>
        public IEnumerable<SelectListItem> LanguageItems()
        {
            return languageService.GetSelectOptions().Select(o => new SelectListItem()
            {
                Text = o.DisplayName,
                Value = RouteTable.Routes.GetVirtualPathForArea(HttpContext.Current.Request.RequestContext, new RouteValueDictionary(new { action = CurrentAction, controller = CurrentController, culture = o.Value })).VirtualPath,
                Selected = o.Value == RouteCultureValue
            });
        }
    }
}
