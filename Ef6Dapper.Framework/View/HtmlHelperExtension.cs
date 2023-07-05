using OnePage.Utility;
using System;
using System.Web.Mvc;

namespace OnePage.Framework.View
{
    public static class HtmlHelperExtension
    {
        /// <summary>
        /// 產生Google Recaptcha。
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper.</param>                
        /// <param name="languageCulture">語系。</param>
        /// <param name="submitFunctionName">送出表單函式名稱。</param>        
        /// <returns>Google Recaptcha.</returns>
        public static MvcHtmlString GoogleRecaptcha(this HtmlHelper htmlHelper, string languageCulture, string submitFunctionName)
        {
            //載入完成執行函式名稱。
            string callback = string.Format("{0}{1}{2}", "gRecaptcha", DateTime.UtcNow.ToString("yyyyMMddHHmmss"), SecurityUtility.GetRandomString(SecurityUtility.RandomStringType.Alphabet, 5));

            //外層容器。
            TagBuilder container = new TagBuilder("div");
            container.MergeAttribute("style", "width: 256px;margin: 15px auto;");
            container.MergeAttribute("id", callback + "cnr");

            //Recaptcha script.
            TagBuilder script = new TagBuilder("script");
            script.MergeAttribute("type", "text/javascript");
            script.MergeAttribute("src", string.Format("{0}?onload={1}&render={2}&hl={3}", GoogleRecaptchaInfo.JsUrl, callback, "explicit", languageCulture));

            //載入完成執行函式。
            TagBuilder callbackScript = new TagBuilder("script");
            callbackScript.MergeAttribute("type", "text/javascript");

            string callbackScripts = string.Format(@"
            (function(w,d,$){{                
                w['{0}']=function(){{
                   var validate=function(form){{
                        if($ && $(form).valid())
                            grecaptcha.execute();
                        }};
                   grecaptcha.render('{1}', {{'sitekey': '{2}','callback': '{3}','badge': 'inline','size': 'invisible'}});
                   var recaptchaForm=d.querySelectorAll('#{1} [name=g-recaptcha-response]')[0].form;
                   var submitBtn=recaptchaForm.querySelectorAll('button[type=submit]')[0];
                   submitBtn.onclick=function(e){{e.preventDefault();validate(recaptchaForm);}};
    
                   w['{3}']=function(){{ w['{4}'](recaptchaForm); }};
                }};
            }})(window,document,jQuery)", callback, container.Attributes["id"], GoogleRecaptchaInfo.SiteKey, string.Concat(callback, "sm"), submitFunctionName);

            callbackScript.InnerHtml = callbackScripts;

            return new MvcHtmlString(string.Concat(container.ToString(), callbackScript.ToString(), script.ToString()));
        }
    }
}