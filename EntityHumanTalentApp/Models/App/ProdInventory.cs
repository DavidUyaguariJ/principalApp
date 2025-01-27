using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsPrincipalApp.Models.App;

[Table("PROD_INVENTORY")]
public partial class ProdInventory
{
    [Key]
    [Column("IDE_INVENTORY")]
    public long IdeInventory { get; set; }

    [Column("IDE_PRODUCT")]
    public Guid? IdeProduct { get; set; }

    [Column("QUANTITY")]
    public int Quantity { get; set; }
}
