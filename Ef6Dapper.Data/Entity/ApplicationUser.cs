using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OnePage.Data
{
    // 您可以在 ApplicationUser 類別新增更多屬性，為使用者新增設定檔資料，請造訪 http://go.microsoft.com/fwlink/?LinkID=317594 以深入了解。
    public class ApplicationUser : IdentityUser
    {
        public int LanguageId { get; set; }

        public int UtcTimeOffset { get; set; }

        public bool Deleted { get; set; }

        [StringLength(5)]
        public string Currency { get; set; }

        [StringLength(64)]
        public string RealName { get; set; }

        public bool Subscription { get; set; }

        public string IntroducerFacebook { get; set; }

        public string IntroducerId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOnUtc { get; set; }

        public string CreatorUserId { get; set; }

        /// <summary>
        /// 用戶的紅利積點餘額(系統自用)
        /// </summary>
        public int RewardPoint { get; set; }

        /// <summary>
        /// 標簽 
        /// 2022-01-03 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public string Tags { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            //注意 authenticationType 必須符合 CookieAuthenticationOptions.AuthenticationType 中定義的項目。
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            //在這裡新增自訂使用者宣告。
            return userIdentity;
        }

    }

}
