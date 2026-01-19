using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Models;

[Table("Yetki")]
public partial class Yetki
{
    [Key]
    [Column("YetkiID")]
    public int YetkiId { get; set; }

    [StringLength(50)]
    public string YetkiAdi { get; set; } = null!;

    [InverseProperty("Yetki")]
    public virtual ICollection<Kullanici> Kullanicis { get; set; } = new List<Kullanici>();
}
