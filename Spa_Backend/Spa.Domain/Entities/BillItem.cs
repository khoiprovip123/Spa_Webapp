namespace Spa.Domain.Entities
{
    public class BillItem
    {
        public long BillItemID { get; set; }
        public long BillID { get; set; }
        public long ServiceID { get; set; }
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; } // Tính tổng giá dựa trên số lượng và đơn giá

        public double? AmountDiscount { get; set; } = 0;
        public string? KindofDiscount { get; set; }

        public string? Note { get; set; }

        public Bill? Bill { get; set; }
    }
}