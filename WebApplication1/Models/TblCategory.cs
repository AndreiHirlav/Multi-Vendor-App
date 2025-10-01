using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TblCategory
{
    public int CatId { get; set; }

    public string CatName { get; set; } = null!;

    public string CatImage { get; set; } = null!;

    public int CatStatus { get; set; }

    public int? CatFkAd { get; set; }
}
