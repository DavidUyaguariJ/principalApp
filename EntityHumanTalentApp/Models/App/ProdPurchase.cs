using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsPrincipalApp.Models.App;

[Table("PROD_PURCHASES")]
public partial class ProdPurchase
{
    [Key]
    [Column("IDE_PURCHASE")]
    public long? IdePurchase { get; set; }

    [Column("IDE_SUPPLIER")]
    public Guid? IdeSupplier { get; set; }

    [Column("IDE_PRODUCT")]
    public Guid? IdeProduct { get; set; }

    [Column("QUANTITY")]
    public int Quantity { get; set; }
}
