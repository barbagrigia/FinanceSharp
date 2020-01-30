namespace FinanceSharp.Examples.Data {
    public class BinanceTick {
        public long Id { get; set; }
        public double time { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public bool IsBuyerMaker { get; set; }
        public long BuyerOrderId { get; set; }
        public long SellerOrderId { get; set; }
        public bool IsBestPriceMatch { get; set; }
    }
}