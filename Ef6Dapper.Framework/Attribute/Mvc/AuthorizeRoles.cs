using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OnePage.Data;

namespace OnePage.Framework.Attribute.Mvc
{
    /// <summary>
    /// 指定僅限符合授權需求的使用者才能存取控制器或動作方法。
    /// </summary>
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute()
        {
            Order = 1;
        }

        public AuthorizeRolesAttribute(params ApplicationRole[] allowedRoles) : this()
        {
            IEnumerable<string> allowedRolesAsStrings = allowedRoles.Select(x => Enum.GetName(typeof(ApplicationRole), x));
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}
