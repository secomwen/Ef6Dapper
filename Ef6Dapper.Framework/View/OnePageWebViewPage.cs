using System.Web.Mvc;
using OnePage.Service.Interface;

namespace OnePage.Framework.View
{
    public abstract class OnePageWebViewPage<TModel> : WebViewPage<TModel>
    {
        private ISettingService settingService;

        /// <summary>
        /// 語系輔助類別。
        /// </summary>
        public GlobalizationHelper Globalization { get; private set; }

        /// <summary>
        /// 取得設定值字串。
        /// </summary>
        /// <param name="settingName">SettingName.</param>
        /// <returns>設定值字串。</returns>
        public string SettingValue(string settingName)
        {
            return settingService.GetBySettingName(settingName);
        }

        public override void InitHelpers()
        {
            settingService = DependencyResolver.Current.GetService<ISettingService>();

            base.InitHelpers();
            Globalization = new GlobalizationHelper(ViewContext, this);
        }
    }
}
