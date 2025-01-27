using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsPrincipalApp.Models.App;

[Table("T_PROD_SUPPLIERS")]
public partial class TProdSupplier
{
    [Key]
    [Column("IDE_SUPPLIER")]
    public Guid IdeSupplier { get; set; }

    [Column("NAME")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("STATUS")]
    public bool? Status { get; set; }

    [Column("CHAT_ID")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ChatId { get; set; }
}
