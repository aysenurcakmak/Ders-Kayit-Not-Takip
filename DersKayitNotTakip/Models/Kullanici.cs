using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Models;

[Table("Kullanici")]
[Index("KullaniciAdi", Name = "UQ__Kullanic__5BAE6A7568351142", IsUnique = true)]
public partial class Kullanici
{
    [Key]
    [Column("KullaniciID")]
    public int KullaniciId { get; set; }

    [StringLength(50)]
    public string KullaniciAdi { get; set; } = null!;

    [StringLength(200)]
    public string Sifre { get; set; } = null!;

    [Column("OgrenciID")]
    public int? OgrenciId { get; set; }

    [Column("OgretimUyesiID")]
    public int? OgretimUyesiId { get; set; }

    [Column("YetkiID")]
    public int YetkiId { get; set; }

    [ForeignKey("OgrenciId")]
    [InverseProperty("Kullanicis")]
    public virtual Ogrenci? Ogrenci { get; set; }

    [ForeignKey("OgretimUyesiId")]
    [InverseProperty("Kullanicis")]
    public virtual OgretimUyesi? OgretimUyesi { get; set; }

    [ForeignKey("YetkiId")]
    [InverseProperty("Kullanicis")]
    public virtual Yetki Yetki { get; set; } = null!;
}
