namespace ItemsSummary.Common.Extensions
{
    /// <summary>
    /// DateTimeの拡張メソッド
    /// </summary>
    public static class DateTimeExtesions
    {
        public static IEnumerable<string> GetLastDaysStrings(this DateTime currentDate, int days)
        {
            for (int i = 0; i < days; i++)
            {
                yield return currentDate.AddDays(-i).ToString("yyyyMMdd");
            }
        }
    }
}
