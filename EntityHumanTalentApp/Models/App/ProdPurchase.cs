using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsPrincipalApp.Models.App;

[Table("PROD_PURCHASES")]
public partial class ProdPurchase
{
    [Key]
    [Column("IDE_PURCHASE")]
    [JsonPropertyName("idePurchase")]
    public long? IdePurchase { get; set; }

    [Column("IDE_SUPPLIER")]
    [JsonPropertyName("ideSupplier")]
    public Guid? IdeSupplier { get; set; }

    [Column("IDE_PRODUCT")]
    [JsonPropertyName("ideProduct")]
    public Guid? IdeProduct { get; set; }

    [Column("QUANTITY")]
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
