using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public partial class TblProduct
{
    public int ProId { get; set; }

    public string ProName { get; set; } = null!;

    public string ProImage { get; set; } = null!;

    public int? ProPrice { get; set; }

    public string ProDes { get; set; } = null!;

    public int ProFkCat { get; set; }

    public int ProFkUser { get; set; }

    [ForeignKey(nameof(ProFkCat))]
    public virtual TblCategory TblCategory { get; set; }
    [ForeignKey(nameof(ProFkUser))]
    public virtual TblUser TblUser { get; set; }
}
