using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Models;

[Table("DersAcma")]
public partial class DersAcma
{
    [Key]
    [Column("DersAcmaID")]
    public int DersAcmaId { get; set; }

    [Column("DersID")]
    public int DersId { get; set; }

    [Column("OgretimUyesiID")]
    public int OgretimUyesiId { get; set; }

    [Column("DonemID")]
    public int DonemId { get; set; }

    public int? Kontenjan { get; set; }

    [StringLength(50)]
    public string? Salon { get; set; }

    [StringLength(50)]
    public string? GunSaat { get; set; }

    [ForeignKey("DersId")]
    [InverseProperty("DersAcmas")]
    public virtual Der Ders { get; set; } = null!;

    [InverseProperty("DersAcma")]
    public virtual ICollection<DersKayit> DersKayits { get; set; } = new List<DersKayit>();

    [ForeignKey("DonemId")]
    [InverseProperty("DersAcmas")]
    public virtual Donem Donem { get; set; } = null!;

    [ForeignKey("OgretimUyesiId")]
    [InverseProperty("DersAcmas")]
    public virtual OgretimUyesi OgretimUyesi { get; set; } = null!;
}
