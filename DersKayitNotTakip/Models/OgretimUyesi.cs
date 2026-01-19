using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Models;

[Table("OgretimUyesi")]
public partial class OgretimUyesi
{
    [Key]
    [Column("OgretimUyesiID")]
    public int OgretimUyesiId { get; set; }

    [StringLength(50)]
    public string Ad { get; set; } = null!;

    [StringLength(50)]
    public string Soyad { get; set; } = null!;

    [Column("BolumID")]
    public int? BolumId { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Unvan { get; set; }

    [ForeignKey("BolumId")]
    [InverseProperty("OgretimUyesis")]
    public virtual Bolum? Bolum { get; set; }

    [InverseProperty("OgretimUyesi")]
    public virtual ICollection<DersAcma> DersAcmas { get; set; } = new List<DersAcma>();

    [InverseProperty("OgretimUyesi")]
    public virtual ICollection<Kullanici> Kullanicis { get; set; } = new List<Kullanici>();
}
