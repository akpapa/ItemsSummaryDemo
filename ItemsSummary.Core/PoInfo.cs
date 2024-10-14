namespace ItemsSummary.Core
{
    /// <summary>
    /// 注文情報Model
    /// </summary>
    public class PoInfo
    {
        public required string PoName { get; set; }
        public string Summary
        {
            get
            {
                if (Items == null || Items.Count() == 0) return string.Empty;
                int kind = Items.Count();
                int sum = Items.Sum(i => i.Quantity);
                return $"商品種類数：{kind}, 商品数量:{sum}";
            }
        }
        public required IEnumerable<SingleItemInfo> Items { get; set; }

    }
}
