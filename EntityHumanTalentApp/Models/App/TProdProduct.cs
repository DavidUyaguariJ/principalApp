using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsPrincipalApp.Models.App;

[Table("T_PROD_PRODUCTS")]
public partial class TProdProduct
{
    [Key]
    [Column("IDE_PRODUCT")]
    public Guid IdeProduct { get; set; }

    [Column("NAME")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("STATUS")]
    public bool? Status { get; set; }
}
