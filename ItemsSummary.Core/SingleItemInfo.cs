namespace ItemsSummary.Core
{
    /// <summary>
    /// 単独商品のModel
    /// </summary>
    public class SingleItemInfo
    {
        public required string Pid { get; set; }
        public required int Quantity { get; set; }
        public required int Set { get; set; }
    }
}
