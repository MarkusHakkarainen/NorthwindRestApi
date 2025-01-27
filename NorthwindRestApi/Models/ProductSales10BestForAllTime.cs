using System;
using System.Collections.Generic;

namespace NorthwindRestApi.Models;

public partial class ProductSales10BestForAllTime
{
    public long Rowid { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal? ProductSales { get; set; }
}
