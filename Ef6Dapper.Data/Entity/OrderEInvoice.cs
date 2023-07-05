using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace OnePage.Data
{
    /// <summary>
    /// 訂單發票檔
    /// </summary>
    [Table("OrderEInvoice")]
    public class OrderEInvoice
    {
        public int Id { get; set; }
        public int AdverId { get; set; }
        public int OrderId { get; set; }

        /// <summary>
        /// 發票類別: 07=一般稅額, 08=特種稅額
        /// </summary>
        public string InvoiceType { get; set; } = "07";

        /// <summary>
        /// 列印(申報)格式 一般稅額計算之電子發票：23（進貨退出或折讓證明單）、25（進項）
        /// 33（銷貨退回或折讓證明單）、35（銷項）
        /// 特種稅額計算之電子發票：37（銷項）、38（銷貨退回或折讓證明單）
        /// </summary>
        public string InvoiceFormat { get; set; } = "25";


        /// <summary>
        /// 發票狀態  New = 1, voided = 2, None = 99
        /// </summary>
        public int? EInvoiceStatus { get; set; }

        /// <summary>
        /// 發票類型: 0=2聯式發票, 1=3聯式發票, 2=收據, 3=無
        /// </summary>
        public string ReceiptType { get; set; }
        public string YearMonth { get; set; }
        public string InvoiceNumber { get; set; }
        public string RandomNum { get; set; }
        public string MerchantOrderNo { get; set; }

        /// <summary>
        /// 開立發票時的電子發票加值中心開立序號 2022-10-13 Wen patch(sql file 20211209_patch.sql)
        /// </summary>
        public string EInvTransaction { get; set; }
        public string BuyerName { get; set; }
        public string BuyerUBN { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerEmail { get; set; }
        public CarrierType? CarrierType { get; set; }
        public string CarrierNum { get; set; }
        public string LoveCode { get; set; }
        public string PrintFlag { get; set; }
        public string KioskPrintFlag { get; set; }
        public string TaxType { get; set; }
        public decimal TaxRate { get; set; }
        public string CustomsClearance { get; set; }
        public decimal Amt { get; set; }
        public decimal AmtSales { get; set; }
        public decimal AmtZero { get; set; }
        public decimal AmtFree { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal TotalAmt { get; set; }
        public string ItemName { get; set; }
        public string ItemCount { get; set; }
        public string ItemUnit { get; set; }
        public string ItemPrice { get; set; }
        public string ItemAmt { get; set; }
        public string ItemTaxType { get; set; }
        public string Comment { get; set; }
        public string BarCode { get; set; }
        public string QRcodeL { get; set; }
        public string QRcodeR { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string InvalidReason { get; set; }
        public DateTime? InvalidOnUtc { get; set; }
    }
}
