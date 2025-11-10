using Newtonsoft.Json;

namespace WbApiData
{
    public class FinancialReport
    {
        [JsonProperty("realizationreport_id")] public int RealizationreportId { get; init; }
        [JsonProperty("date_from")] public required string DateFrom { get; init; }
        [JsonProperty("date_to")] public required string DateTo { get; init; }
        [JsonProperty("create_dt")] public required string CreateDt { get; init; }
        [JsonProperty("currency_name")] public required string CurrencyName { get; init; }
        [JsonProperty("gi_id")] public int GiId { get; init; }
        [JsonProperty("dlv_prc")] public double DlvPrc { get; init; }
        [JsonProperty("subject_name")] public required string SubjectName { get; init; }
        [JsonProperty("nm_id")] public int NmId { get; init; }
        [JsonProperty("brand_name")] public required string BrandName { get; init; }
        [JsonProperty("sa_name")] public required string SaName { get; init; }
        [JsonProperty("ts_name")] public required string TsName { get; init; }
        [JsonProperty("barcode")] public required string Barcode { get; init; }
        [JsonProperty("doc_type_name")] public required string DocTypeName { get; init; }
        [JsonProperty("quantity")] public int Quantity { get; init; }
        [JsonProperty("retail_price")] public double RetailPrice { get; init; }
        [JsonProperty("retail_amount")] public double RetailAmount { get; init; }
        [JsonProperty("sale_percent")] public int SalePercent { get; init; }
        [JsonProperty("commission_percent")] public double CommissionPercent { get; init; }
        [JsonProperty("office_name")] public required string OfficeName { get; init; }
        [JsonProperty("order_dt")] public required string OrderDt { get; init; }
        [JsonProperty("sale_dt")] public required string SaleDt { get; init; }
        [JsonProperty("rr_dt")] public required string RrDt { get; init; }
        [JsonProperty("shk_id")] public long ShkId { get; init; }
        [JsonProperty("retail_price_withdisc_rub")] public double RetailPriceWithdiscRub { get; init; }
        [JsonProperty("delivery_amount")] public int DeliveryAmount { get; init; }
        [JsonProperty("return_amount")] public int ReturnAmount { get; init; }
        [JsonProperty("delivery_rub")] public double DeliveryRub { get; init; }
        [JsonProperty("gi_box_type_name")] public required string GiBoxTypeName { get; init; }
        [JsonProperty("product_discount_for_report")] public double ProductDiscountForReport { get; init; }
        [JsonProperty("supplier_promo")] public double SupplierPromo { get; init; }
        [JsonProperty("ppvz_spp_prc")] public double PpvzSppPrc { get; init; }
        [JsonProperty("ppvz_kvw_prc_base")] public double PpvzKvwPrcBase { get; init; }
        [JsonProperty("ppvz_kvw_prc")] public double PpvzKvwPrc { get; init; }
        [JsonProperty("sup_rating_prc_up")] public double SupRatingPrcUp { get; init; }
        [JsonProperty("is_kgvp_v2")] public double IsKgvpV2 { get; init; }
        [JsonProperty("ppvz_sales_commission")] public double PpvzSalesCommission { get; init; }
        [JsonProperty("ppvz_for_pay")] public double PpvzForPay { get; init; }
        [JsonProperty("ppvz_reward")] public double PpvzReward { get; init; }
        [JsonProperty("acquiring_fee")] public double AcquiringFee { get; init; }
        [JsonProperty("acquiring_percent")] public double AcquiringPercent { get; init; }
        [JsonProperty("payment_processing")] public required string PaymentProcessing { get; init; }
        [JsonProperty("acquiring_bank")] public required string AcquiringBank { get; init; }
        [JsonProperty("ppvz_vw")] public double PpvzVw { get; init; }
        [JsonProperty("ppvz_vw_nds")] public double PpvzVwNds { get; init; }
        [JsonProperty("ppvz_office_name")] public required string PpvzOfficeName { get; init; }
        [JsonProperty("ppvz_office_id")] public int PpvzOfficeId { get; init; }
        [JsonProperty("ppvz_supplier_id")] public int PpvzSupplierId { get; init; }
        [JsonProperty("ppvz_supplier_name")] public required string PpvzSupplierName { get; init; }
        [JsonProperty("ppvz_inn")] public required string PpvzInn { get; init; }
        [JsonProperty("declaration_number")] public required string DeclarationNumber { get; init; }
        [JsonProperty("bonus_type_name")] public required string BonusTypeName { get; init; }
        [JsonProperty("sticker_id")] public required string StickerId { get; init; }
        [JsonProperty("site_country")] public required string SiteCountry { get; init; }
        [JsonProperty("srv_dbs")] public bool SrvDbs { get; init; }
        [JsonProperty("penalty")] public double Penalty { get; init; }
        [JsonProperty("additional_payment")] public double AdditionalPayment { get; init; }
        [JsonProperty("rebill_logistic_cost")] public double RebillLogisticCost { get; init; }
        [JsonProperty("rebill_logistic_org")] public required string RebillLogisticOrg { get; init; }
        [JsonProperty("storage_fee")] public double StorageFee { get; init; }
        [JsonProperty("deduction")] public double Deduction { get; init; }
        [JsonProperty("acceptance")] public double Acceptance { get; init; }
        [JsonProperty("assembly_id")] public long AssemblyId { get; init; }
        [JsonProperty("kiz")] public required string Kiz { get; init; }
        [JsonProperty("srid")] public required string Srid { get; init; }
        [JsonProperty("report_type")] public int ReportType { get; init; }
        [JsonProperty("is_legal_entity")] public bool IsLegalEntity { get; init; }
        [JsonProperty("trbx_id")] public required string TrbxId { get; init; }
        [JsonProperty("installment_cofinancing_amount")] public double InstallmentCofinancingAmount { get; init; }
        [JsonProperty("wibes_wb_discount_percent")] public double WibesWbDiscountPercent { get; init; }
        [JsonProperty("cashback_amount")] public double CashbackAmount { get; init; }
        [JsonProperty("cashback_discount")] public double CashbackDiscount { get; init; }
        [JsonProperty("cashback_commission_change")] public double CashbackCommissionChange { get; init; }
        [JsonProperty("order_uid")] public required string OrderUid { get; init; }
    }
}