using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsPrincipalApp.Models.App;

[Table("PROD_SALES")]
public partial class ProdSale
{
    [Key]
    [Column("IDE_SALE")]
    [JsonPropertyName("ideSale")]
    public long? IdeSale { get; set; }

    [Column("IDE_CLIENT")]
    [JsonPropertyName("ideClient")]
    public Guid? IdeClient { get; set; }

    [Column("IDE_PRODUCT")]
    [JsonPropertyName("ideProduct")]
    public Guid? IdeProduct { get; set; }

    [Column("QUANTITY")]
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
