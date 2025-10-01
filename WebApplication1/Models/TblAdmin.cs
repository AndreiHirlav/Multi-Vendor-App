using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public partial class TblAdmin
{
    public int AdId { get; set; }
    [Required(ErrorMessage = "Username field is required")]

    public string AdUsername { get; set; } = null!;
    [Required(ErrorMessage = "Password field is required")]

    public string AdPassword { get; set; } = null!;
}
