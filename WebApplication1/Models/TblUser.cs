using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public partial class TblUser
{
    public int UId { get; set; }

    public string UName { get; set; } = null!;

    [Required(ErrorMessage ="Email is required")]
    public string UEmail { get; set; } = null!;
    [Required(ErrorMessage = "Password is required")]
    public string UPassword { get; set; } = null!;

    public string UImage { get; set; } = null!;

    public string UContact { get; set; } = null!;

}
