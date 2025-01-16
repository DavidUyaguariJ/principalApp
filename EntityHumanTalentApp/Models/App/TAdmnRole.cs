using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsHumanTalentApp.Models.App;

[Table("T_ADMN_ROLES")]
public partial class TAdmnRole
{
    [Key]
    [Column("IDE_ROLE")]
    public Guid IdeRole { get; set; }

    [Column("NAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("CODE")]
    [StringLength(4)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [InverseProperty("IdeRoleNavigation")]
    public virtual ICollection<AdmnUser> AdmnUsers { get; set; } = new List<AdmnUser>();
}
