using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsHumanTalentApp.Models.App;

[Table("T_AUDT_GOVERMENT_OBJETIVES")]
public partial class TAudtGovermentObjetive
{
    [Key]
    [Column("IDE_GUVERMENT_OBJETIVE")]
    public Guid IdeGuvermentObjetive { get; set; }

    [Column("NAME")]
    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("CODE")]
    [StringLength(4)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [Column("IS_PRINCIPAL")]
    public bool IsPrincipal { get; set; }

    [Column("STATUS")]
    public bool Status { get; set; }
}
