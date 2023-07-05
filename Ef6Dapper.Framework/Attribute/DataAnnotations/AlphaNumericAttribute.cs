using System.ComponentModel.DataAnnotations;

namespace OnePage.Framework.Attribute.DataAnnotations
{
    public class AlphaNumericAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 大小寫字母/數字驗證。
        /// </summary>
        public AlphaNumericAttribute() : base(@"^[a-zA-Z0-9_]*$") { }
    }
}
