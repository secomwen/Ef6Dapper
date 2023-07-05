using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace OnePage.Data
{
    /// <summary>
    /// 訂單主檔
    /// </summary>
    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderRows = new HashSet<OrderRow>();
        }

        /// <summary>
        /// 序號
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 賣家編號.參考Adver.Id, AspNetUsers.AdverId
        /// </summary>
        public int AdverId { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [StringLength(32)]
        public string OrderId { get; set; }

        /// <summary>
        /// 訂單狀態:0:新訂單,1:處理中,2:結束,3:取消
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 貨款
        /// </summary>
        public decimal? ItemAmount { get; set; }

        /// <summary>
        /// 運費
        /// </summary>
        public decimal? ShippingFee { get; set; }

        /// <summary>
        /// 訂單折扣. 參考 Campaign.Discount, 與 CouponCode 的優惠價無關
        /// </summary>
        public decimal? OrderDiscount { get; set; }

        /// <summary>
        /// 訂單總額
        /// </summary>
        public decimal? OrderAmount { get; set; }

        /// <summary>
        /// 訂單應稅總額
        /// </summary>
        public decimal? OrderWithTaxAmount { get; set; }

        /// <summary>
        /// 訂單零稅總額
        /// </summary>
        public decimal? OrderZeroTaxAmount { get; set; }

        /// <summary>
        /// 訂單免稅總額
        /// </summary>
        public decimal? OrderFreeTaxAmount { get; set; }

        /// <summary>
        /// 金流支付單位回傳的實付金額
        /// </summary>
        public decimal? PaidAmount { get; set; }
        /// <summary>
        /// 付款時間(UTC)
        /// </summary>
        public DateTime? PaidTimeUtc { get; set; }

        /// <summary>
        /// 傳給金流單位的訂單編號。有些金流單位要求每次傳送時必須給唯一的訂單編號. 若直接用訂單編號, 在付款失敗後將無法重新付款. 因此設計此欄位做為唯一對應值。
        /// </summary>
        [StringLength(40)]
        public string PaymentOrderId { get; set; }

        /// <summary>
        /// 金流單位回傳的付款交易序號
        /// </summary>
        [StringLength(40)]
        public string PaymentTransaction { get; set; }

        #region patch for i519. 2019-01-07

        /// <summary>
        /// 傳送交易訂單至i519時, 經手人在i519的使用者帳號。未指定時用AdverPayment設定值的管理帳號
        /// HttpPost 傳送到 https://cloud.i519.com.tw/payment/api/details 時 body 的 username 欄位
        /// </summary>
        [StringLength(50)]
        public string PaymentRequestUserName { get; set; }

        /// <summary>
        /// 消費者輸入的自訂結帳欄位資料值. JSON陣列.
        /// 格式 [{"Name":"", "Value":""}]
        /// </summary>
        public string CheckoutAttributeValue { get; set; }

        /*
        IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'PaymentRequestUserName' AND Object_ID = Object_ID(N'Order'))
        BEGIN
            ALTER TABLE [Order] ADD [PaymentRequestUserName] NVARCHAR(50);
            EXEC sp_addextendedproperty N'MS_Description', N'傳送交易訂單至i519時, 經手人在i519的使用者帳號。未指定時用AdverPayment設定值的管理帳號', N'user', N'dbo', N'table', N'Order', N'column', N'PaymentRequestUserName'
        END
        IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'CheckoutAttributeValue' AND Object_ID = Object_ID(N'Order'))
        BEGIN
            ALTER TABLE [Order] ADD [CheckoutAttributeValue] NVARCHAR(MAX);
            EXEC sp_addextendedproperty N'MS_Description', N'消費者輸入的自訂結帳欄位資料值. JSON陣列格式 [{"Name":"", "Value":""}]', N'user', N'dbo', N'table', N'Order', N'column', N'CheckoutAttributeValue'
        END
        */

        #endregion patch for i519. 2019-01-07

        /// <summary>
        /// 金流單位回傳的付款交易錯誤代碼
        /// </summary>
        public string PaymentErrorCode { get; set; }

        /// <summary>
        /// 付款狀態: 0:待付款, 1:已付款, 2:已退款, 3:未知
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// 客戶姓名
        /// </summary>
        [Required]
        [StringLength(64)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 客戶eMail
        /// </summary>
        [Required]
        [StringLength(64)]
        public string CustomerEmail { get; set; }

        /// <summary>
        /// 客戶電話
        /// </summary>
        [Required]
        [StringLength(64)]
        public string CustomerPhone { get; set; }

        /// <summary>
        /// 客戶貨物收件地址
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [StringLength(100)]
        public string CustomerAddress { get; set; }

        /// <summary>
        /// 配送狀態: 0:新訂單, 1:已出貨, 2:退貨
        /// </summary>
        public ShippingStatus ShippingStatus { get; set; }

        /// <summary>
        /// 出貨時間
        /// </summary>
        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// 物流商名稱
        /// </summary>
        [StringLength(40)]
        public string LspName { get; set; }

        /// <summary>
        /// 物流追蹤碼/貨運單號
        /// </summary>
        [StringLength(40)]
        public string ShippingTrackCode { get; set; }

        /// <summary>
        /// 發票類型: 0=2聯式發票, 1=3聯式發票, 2=收據, 3=無
        /// </summary>
        [StringLength(20)]
        public string ReceiptType { get; set; }

        /// <summary>
        /// 發票號碼
        /// </summary>
        [StringLength(16)]
        public string ReceiptNumber { get; set; }

        /// <summary>
        /// 發票開立時間
        /// </summary>
        public DateTime? ReceiptTime { get; set; }

        /// <summary>
        /// 買方統一編號
        /// </summary>
        [StringLength(10)]
        public string TaxNumber { get; set; }

        /// <summary>
        /// 備註/客戶給賣家的留言
        /// </summary>
        public string OrderNote { get; set; }

        /// <summary>
        /// 客戶訂購時上網裝置的IP(從網頁下單才有用)
        /// </summary>
        [StringLength(40)]
        public string CustomerIp { get; set; }

        /// <summary>
        /// Google Analytics Client Id
        /// </summary>
        public string GoogleAnalysticTrackers { get; set; }

        /// <summary>
        /// 訂單開立時間
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// 第三方支付外掛程式編號
        /// </summary>
        public int? PaymentPluginId { get; set; }

        /// <summary>
        /// 付款方式. Credit, CreditCard, CCARD, WebATM, ATM, BARCODE, CVS, ExpressCheckout, CVSPickUp, EzShipType1, None, UserHandle, GooglePay, ApplePay, ICO, TopUp, Offline, CSTORE_IBON, CSTORE_FAMIPORT,...
        /// 參考 enum PaymentMethod
        /// </summary>
        [StringLength(100)]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// 支付點. 便利商店 IBON/FAMIPORT/LIFEET, 或虛擬帳號轉帳的銀行名稱
        /// </summary>
        [StringLength(100)]
        public string SubPaymentMethod { get; set; }

        /// <summary>
        /// 信用卡前6碼
        /// </summary>
        [StringLength(6)]
        public string Card6no { get; set; }

        /// <summary>
        /// 信用卡後4碼
        /// </summary>
        [StringLength(4)]
        public string Card4no { get; set; }

        /// <summary>
        /// ATM轉帳銀行代碼
        /// </summary>
        [StringLength(10)]
        public string BankId { get; set; }

        /// <summary>
        /// ATM轉帳銀行名稱
        /// </summary>
        [StringLength(10)]
        public string BankName { get; set; }

        /// <summary>
        /// ATM轉帳銀行帳號
        /// </summary>
        [StringLength(10)]
        public string BankAccNo { get; set; }

        /// <summary>
        /// 虛擬帳號
        /// </summary>
        [StringLength(20)]
        public string VAccount { get; set; }

        /// <summary>
        /// 超商Kiosk(ibon/FamiPort/OK-Go/Hi-Life)付款條碼
        /// </summary>
        [StringLength(14)]
        public string PaymentNo { get; set; }

        /// <summary>
        /// 付款截止時間
        /// </summary>
        public DateTime? ExpireDateUtc { get; set; }

        /// <summary>
        /// 超商付款商店
        /// </summary>
        [StringLength(10)]
        public string PayFrom { get; set; }

        /// <summary>
        /// 超商條碼繳費第一段條碼
        /// </summary>
        [StringLength(20)]
        public string Barcode1 { get; set; }

        /// <summary>
        /// 超商條碼繳費第二段條碼
        /// </summary>
        [StringLength(20)]
        public string Barcode2 { get; set; }

        /// <summary>
        /// 超商條碼繳費第三段條碼
        /// </summary>
        [StringLength(20)]
        public string Barcode3 { get; set; }

        /// <summary>
        /// 發票title
        /// </summary>
        [StringLength(100)]
        public string ReceiptBuyer { get; set; }

        /// <summary>
        /// 發票寄送地址
        /// </summary>
        [StringLength(100)]
        public string ReceiptAddress { get; set; }

        /// <summary>
        /// 訂購時用的計價幣別
        /// </summary>
        [StringLength(5)]
        public string Currency { get; set; }

        /// <summary>
        /// 超商取貨付款店名
        /// </summary>
        [StringLength(20)]
        public string PayInStoreCvs { get; set; }

        /// <summary>
        /// 客戶的UserId,參考AspNetUsers.Id
        /// </summary>
        [StringLength(128)]
        public string UserId { get; set; }

        /// <summary>
        /// 付款時的基準貨幣代碼
        /// </summary>
        [StringLength(5)]
        public string BaseCurrencyCode { get; set; }

        /// <summary>
        /// 付款時訂單金額對基準貨幣的匯率
        /// </summary>
        public decimal? CurrencyExchangeRate { get; set; }

        /// <summary>
        /// 訂單來源網頁網址
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// 付款時選用的幣別. 可能與購物時的計價幣別不同
        /// </summary>
        [StringLength(5)]
        public string PaymentCurrency { get; set; }

        /// <summary>
        /// 訂單取消時間(UTC)
        /// </summary>
        public DateTime? CanceledOnUtc { get; set; }

        /// <summary>
        /// 取消訂單者. A=賣家,C=消費者
        /// </summary>
        [StringLength(8)]
        public string CanceledBy { get; set; }

        /// <summary>
        /// 消費者可獲得的購物紅利
        /// 購物紅利設定, 請參考 Setting 資料表 Reward.Type 及 Reward.GrantEvent 設定值
        /// </summary>
        public int Reward { get; set; }

        /// <summary>
        /// 是否已發放購物紅利給消費者?
        /// 購物紅利設定, 請參考 Setting 資料表 Reward.Type 及 Reward.GrantEvent 設定值
        /// </summary>
        public bool RewardGranted { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ApplicationUser User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderRow> OrderRows { get; set; }

        /// <summary>
        /// 其它額外參數宣告,以JSON格式存 2022-01-03 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public string Claims { get; set; }

        /// <summary>
        /// 取餐時間 2022-01-03 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public DateTime? PickupTime { get; set; }

        /// <summary>
        /// 商品附加項(口味、加料)合計金額
        /// </summary>
        public decimal? VariantsAmount { get; set; }

        /// <summary>
        /// 訂單類型
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// 傳送給電子發票加值中心的交易單號(不可重複) 2022-10-13 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public string EInvoiceOrderId { get; set; }

        /// <summary>
        /// 開立發票時的電子發票加值中心開立序號 2022-10-13 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public string EInvTransaction { get; set; }
        /// <summary>
        /// 電子發票外掛程式編號 2022-10-13 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public int EInvoicePluginId { get; set; }

        /// <summary>
        /// 電子發票消費者載具類型
        /// </summary>
        public CarrierType CarrierType { get; set; }

        /// <summary>
        /// 電子發票消費者載具號碼
        /// </summary>
        public string CarrierNum { get; set; }

        /// <summary>
        /// 電子發票消費者捐贈碼
        /// </summary>
        public string LoveCode { get; set; }

        public decimal? OrderServiceFee { get; set; }
        /// <summary>
        /// 開立電子發票方式(觸發時機點)
        /// 1=即時開立發票,0=等待觸發開立發票,3=預約自動開立發票
        /// </summary>
        public string EInvTriggMethod { get; set; } = "1";
        /// <summary>
        /// 預約自動開立發票
        /// </summary>
        public DateTime? EInvTriggDate { get; set; }
    }


    /// <summary>
    /// 進階訂單搜尋用的資料容器
    /// </summary>
    public class OrderExt
    {
        public int Id { get; set; }

        public int AdverId { get; set; }

        public string OrderId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string OrderType { get; set; }

        public decimal? ItemAmount { get; set; }

        public decimal? ShippingFee { get; set; }

        public decimal? OrderDiscount { get; set; }

        public decimal? OrderAmount { get; set; }

        public decimal? PaidAmount { get; set; }

        public DateTime? PaidTimeUtc { get; set; }

        public string PaymentOrderId { get; set; }

        public string PaymentTransaction { get; set; }

        public string PaymentRequestUserName { get; set; }

        public string CheckoutAttributeValue { get; set; }

        public string PaymentErrorCode { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }

        public string CustomerAddress { get; set; }

        public ShippingStatus ShippingStatus { get; set; }

        public DateTime? DeliveryTime { get; set; }

        public string LspName { get; set; }

        public string ShippingTrackCode { get; set; }

        public string ReceiptType { get; set; }

        public string ReceiptNumber { get; set; }

        public DateTime? ReceiptTime { get; set; }

        public string TaxNumber { get; set; }

        public string OrderNote { get; set; }

        public string CustomerIp { get; set; }

        public string GoogleAnalysticTrackers { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int? PaymentPluginId { get; set; }

        public string PaymentMethod { get; set; }

        public string SubPaymentMethod { get; set; }

        public string Card6no { get; set; }

        public string Card4no { get; set; }

        public string BankId { get; set; }

        public string BankName { get; set; }

        public string BankAccNo { get; set; }

        public string VAccount { get; set; }

        public string PaymentNo { get; set; }

        public DateTime? ExpireDateUtc { get; set; }

        public string PayFrom { get; set; }

        public string Barcode1 { get; set; }

        public string Barcode2 { get; set; }

        public string Barcode3 { get; set; }

        public string ReceiptBuyer { get; set; }

        public string ReceiptAddress { get; set; }

        public string Currency { get; set; }

        public string PayInStoreCvs { get; set; }

        public string UserId { get; set; }

        public string BaseCurrencyCode { get; set; }

        public decimal? CurrencyExchangeRate { get; set; }

        public string SourceUrl { get; set; }

        public string PaymentCurrency { get; set; }

        public DateTime? CanceledOnUtc { get; set; }

        public string CanceledBy { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<OrderRow> OrderRows { get; set; }

        public int CampaignId { get; set; }

        public string CampaignName { get; set; }

        public string AdverRealName { get; set; }

        /// <summary>
        /// 消費者可獲得的購物紅利
        /// 購物紅利設定, 請參考 Setting 資料表 Reward.Type 及 Reward.GrantEvent 設定值
        /// </summary>
        public int Reward { get; set; }

        /// <summary>
        /// 是否已發放購物紅利給消費者?
        /// 購物紅利設定, 請參考 Setting 資料表 Reward.Type 及 Reward.GrantEvent 設定值
        /// </summary>
        public bool RewardGranted { get; set; }

        /// <summary>
        /// 其它額外參數宣告,以JSON格式存 2022-01-03 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public string Claims { get; set; }

        /// <summary>
        /// 取餐時間 2022-01-03 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public DateTime? PickupTime { get; set; }

        /// <summary>
        /// 商品附加項(口味、加料)合計金額
        /// </summary>
        public decimal? VariantsAmount { get; set; }

        /// <summary>
        /// 已開立的電子發票狀態 2022-10-13 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public int? EInvoiceStatus { get; set; } 

        public DateTime? EInvoiceCreateDate { get; set; }
        /// <summary>
        /// 傳送給電子發票加值中心的交易單號(不可重複) 2022-10-13 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        //public string EInvoiceOrderId { get; set; }

    }



    public enum PaymentStatus
    {
        New = 0, Paid = 1, Refund = 2
    }

    public enum OrderStatus
    {
        New = 0, Processing = 1, Closed = 2, Canceled = 3
    }

    public enum ShippingStatus
    {
        New = 0, Delivered = 1, Returned = 2, Arrived = 3
    }

    public enum PaymentMethod
    {
        Credit, CreditCard, CCARD, WebATM, ATM, BARCODE, CVS, ExpressCheckout, CVSPickUp, EzShipType1, None, UserHandle, GooglePay, ApplePay, ICO, TopUp, Offline, CSTORE_IBON, CSTORE_FAMIPORT,LINEPAY,
        TanwanPay,CREDIT
    }
    public enum EInvoiceStatus
    {
        New = 1, voided = 2, None = 99
    }
    public enum CarrierType //自然人憑證(Citizen Digital Certificate)
    {
        None = 99 ,PhoneBarcode = 0, CDC =1 ,EzPay =2
    }
}