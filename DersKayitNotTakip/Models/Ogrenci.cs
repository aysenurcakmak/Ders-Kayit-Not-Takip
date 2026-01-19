using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Models;

[Table("Ogrenci")]
public partial class Ogrenci
{
    [Key]
    [Column("OgrenciID")]
    public int OgrenciId { get; set; }

    [StringLength(50)]
    public string Ad { get; set; } = null!;

    [StringLength(50)]
    public string Soyad { get; set; } = null!;

    [Column("BolumID")]
    public int? BolumId { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    public DateOnly? DogumTarihi { get; set; }

    [ForeignKey("BolumId")]
    [InverseProperty("Ogrencis")]
    public virtual Bolum? Bolum { get; set; }

    [InverseProperty("Ogrenci")]
    public virtual ICollection<DersKayit> DersKayits { get; set; } = new List<DersKayit>();

    [InverseProperty("Ogrenci")]
    public virtual ICollection<Kullanici> Kullanicis { get; set; } = new List<Kullanici>();
}
