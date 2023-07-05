namespace OnePage.Data.Repository
{
    /// <summary>
    /// DbContext工廠。
    /// </summary>
    public interface IDbContextFactory
    {
        /// <summary>
        /// 取得DbContext。
        /// </summary>
        /// <returns>DbContext.</returns>
        OnePageDbContext GetDbContext();
    }
}
