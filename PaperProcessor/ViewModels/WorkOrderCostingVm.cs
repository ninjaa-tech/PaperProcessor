namespace PaperProcessor.ViewModels
{
    public class WorkOrderCostingVm
    {
        public int WorkOrderId { get; set; }
        public string WorkOrderNo { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string ProductName { get; set; } = "";

        public int QuantityOrdered { get; set; }
        public decimal UnitSellPrice { get; set; }

        public decimal Revenue => QuantityOrdered * UnitSellPrice;

        public decimal MaterialCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal TotalCost => MaterialCost + LaborCost;

        public decimal Profit => Revenue - TotalCost;
    }
}
