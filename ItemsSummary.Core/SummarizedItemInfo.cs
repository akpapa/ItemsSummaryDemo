namespace ItemsSummary.Core
{
    /// <summary>
    /// 数量合算された商品情報Model
    /// </summary>
    public record class SummarizedItemInfo
    {
        public required string Pid { get; set; }
        public required int Quantity { get; set; }
        public required int Set { get; set; }
        public required IEnumerable<string> Pos { get; set; }
    }
}
