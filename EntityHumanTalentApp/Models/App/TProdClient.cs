using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsPrincipalApp.Models.App;

[Table("T_PROD_CLIENTS")]
public partial class TProdClient
{
    [Key]
    [Column("IDE_CLIENT")]
    public Guid IdeClient { get; set; }

    [Column("NOMBRE")]
    [StringLength(255)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Column("STATUS")]
    public bool? Status { get; set; }

    [Column("CHAT_ID")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ChatId { get; set; }
}
