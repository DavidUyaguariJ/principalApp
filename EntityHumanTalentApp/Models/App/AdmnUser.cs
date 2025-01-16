using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsHumanTalentApp.Models.App;

[Table("ADMN_USERS")]
public partial class AdmnUser
{
    [Key]
    [Column("IDE_USER")]
    public Guid IdeUser { get; set; }

    [Column("IDE_ROLE")]
    public Guid IdeRole { get; set; }

    [Column("DNI")]
    [StringLength(50)]
    [Unicode(false)]
    public string Dni { get; set; } = null!;

    [Column("USER_NAME")]
    [StringLength(100)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [Column("FIRST_NAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("LAST_NAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("EMAIL")]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("PASSWORD")]
    [StringLength(300)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [ForeignKey("IdeRole")]
    [InverseProperty("AdmnUsers")]
    [JsonIgnore]
    public virtual TAdmnRole? IdeRoleNavigation { get; set; }
}
