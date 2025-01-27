using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsPrincipalApp.Models.App;

[Table("PROD_SALES")]
public partial class ProdSale
{
    [Key]
    [Column("IDE_SALE")]
    public long IdeSale { get; set; }

    [Column("IDE_CLIENT")]
    public Guid? IdeClient { get; set; }

    [Column("IDE_PRODUCT")]
    public Guid? IdeProduct { get; set; }

    [Column("QUANTITY")]
    public int Quantity { get; set; }
}
