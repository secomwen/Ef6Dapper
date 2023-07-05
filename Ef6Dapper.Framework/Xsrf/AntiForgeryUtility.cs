using System;
using System.Web;
using System.Web.Helpers;

namespace OnePage.Framework.Xsrf
{
    /// <summary>
    /// 惡意指令碼安全輔助類別。
    /// </summary>
    public static class AntiForgeryUtility
    {
        public readonly static string XsrfHeaderKey = "X-XSRF-TOKEN";
        public readonly static string XsrfCookieName = "XSRF-TOKEN";

        /// <summary>
        /// 產生搜尋語彙基元。
        /// </summary>
        /// <returns>搜尋語彙基元。</returns>
        public static string GenerateToken()
        {
            string cookieToken, formToken;

            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return string.Concat(cookieToken, ":", formToken);
        }

        /// <summary>
        /// 產生至Cookie。
        /// </summary>
        public static void GenerateToCookie()
        {
            if (HttpContext.Current == null)
                throw new Exception("HttpContext");

            HttpCookie httpCookie = new HttpCookie(XsrfCookieName, GenerateToken());

#if (!DEBUG)
                        httpCookie.Secure = true;                        
#endif

            HttpContext.Current.Response.SetCookie(httpCookie);
        }

        /// <summary>
        /// 驗證。
        /// </summary>
        /// <param name="token">搜尋語彙基元。</param>
        /// <returns>驗證結果。</returns>
        public static bool ValidateToken(string token)
        {
            try
            {
                string cookieToken = string.Empty;
                string formToken = string.Empty;

                if (!string.IsNullOrEmpty(token))
                {
                    string[] tokens = token.Split(':');
                    if (tokens.Length == 2)
                    {
                        cookieToken = tokens[0].Trim();
                        formToken = tokens[1].Trim();
                    }
                }

                AntiForgery.Validate(cookieToken, formToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
