using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnePage.Data;

namespace OnePage.WebAPI.Models
{
    public class GetOrderListResponse : CollectionResponseBase
    {
        public GetOrderListResponse(int totalCount, int pageSize) : base(totalCount, pageSize) {
        }

        public IList<OrderSummaryResponse> Orders { get; set; }
    }


    public class OrderSummaryResponse
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

        public PaymentStatus PaymentStatus { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }

        public string CustomerAddress { get; set; }

        public ShippingStatus ShippingStatus { get; set; }

        public DateTime? DeliveryTime { get; set; }

        public string OrderNote { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int? PaymentPluginId { get; set; }

        public string PaymentMethod { get; set; }

        public string SubPaymentMethod { get; set; }

        public string Currency { get; set; }

        public string UserId { get; set; }

        public string BaseCurrencyCode { get; set; }

        public decimal? CurrencyExchangeRate { get; set; }

        public string SourceUrl { get; set; }

        public string PaymentCurrency { get; set; }

        public DateTime? CanceledOnUtc { get; set; }

        public DateTime? PaymentExpireOnUtc { get; set; }

        public string CanceledBy { get; set; }

        public string CouponCode { get; set; }

        public int? CouponOrderId { get; set; }

        public int CampaignId { get; set; }

        public string CampaignName { get; set; }

        public int? PuberId { get; set; }

        public string PuberName { get; set; }

        /// <summary>
        /// 消費者可獲得的購物紅利
        /// 購物紅利設定, 請參考 Setting 資料表 Reward.Type 及 Reward.GrantEvent 設定值
        /// </summary>
        public int Reward { get; set; }

        public bool RewardGranted { get; set; }

        public string ReceiptType { get; set; }
        public string ReceiptBuyer { get; set; }
        public string ReceiptAddress { get; set; }

        public string ReceiptNumber { get; set; }

        public DateTime? ReceiptTime { get; set; }

        public int? EInvoiceStatus { get; set; }

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
        /// 消費者輸入的自訂結帳欄位資料值. JSON陣列.
        /// 格式 [{"Name":"", "Value":""}]
        /// </summary>
        public string CheckoutAttributeValue { get; set; }
    }
}