using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace OnePage.Data
{
    /// <summary>
    /// 訂單明細
    /// </summary>
    [Table("OrderRow")]
    public partial class OrderRow
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int CampaignItemId { get; set; }

        public int DisplayOrder { get; set; }

        public string ImageUrl { get; set; }

        public string ItemName { get; set; }

        public string ItemUrl { get; set; }

        public string Specification { get; set; }

        public decimal? OriginalPrice { get; set; } //patch 2018-06-06 added. 記錄未折扣前的價格

        public decimal? Price { get; set; }

        public int Qty { get; set; }

        public decimal? SubTotal { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        public string SKU { get; set; }

        public int ProductId { get; set; }

        public int CampaignId { get; set; }

        public decimal? SalesCost { get; set; }

        /// <summary>
        /// 是否已從CampaignItem扣除庫存量?
        /// </summary>
        public bool StockDeducted { get; set; } //patch 2019-02-16

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOnUtc { get; set; }

        #region customization for i519. 2019-07-24
        /// <summary>
        /// 經由團購主銷售時, 傳給金流單位的拆帳比例(0~100). 產生訂單時從Campaign複製, 不可修改
        /// </summary>
        public decimal? GroupBuyShareRate { get; set; }

        /// <summary>
        /// 經由團購主銷售時, 傳給金流單位的拆帳通路代號(由金流單位定義). 產生訂單時從Campaign複製, 不可修改
        /// </summary>
        [StringLength(10)]
        public string GroupBuyChannelId { get; set; }
        #endregion

        /// <summary>
        /// 訂購此商品後贈送給消費者的紅利點數
        /// 購物紅利設定, 請參考 Setting 資料表 Reward.Type 及 Reward.GrantEvent 設定值
        /// </summary>
        public int Reward { get; set; }


        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Order Order { get; set; }

        /// <summary>
        /// 口味加料尺寸顏色碼等附加屬性 2022-01-03 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public string Variants { get; set; }

        /// <summary>
        /// 其它額外參數宣告,以JSON格式存 2022-01-03 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public string Claims { get; set; }

        /// <summary>
        /// 商品附加項(口味、加料)小計金額
        /// </summary>
        public decimal? VariantsSubTotal { get; set; }

        public OrderRowStatus OrderRowStatus { get; set; }
    }
    public enum OrderRowStatus
    {
        Normal = 1, Returned = 2, Allowance = 3
    }
}
