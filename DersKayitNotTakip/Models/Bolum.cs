using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Models;

[Table("Bolum")]
public partial class Bolum
{
    [Key]
    [Column("BolumID")]
    public int BolumId { get; set; }

    [StringLength(100)]
    public string BolumAdi { get; set; } = null!;

    [InverseProperty("Bolum")]
    public virtual ICollection<Der> Ders { get; set; } = new List<Der>();

    [InverseProperty("Bolum")]
    public virtual ICollection<Ogrenci> Ogrencis { get; set; } = new List<Ogrenci>();

    [InverseProperty("Bolum")]
    public virtual ICollection<OgretimUyesi> OgretimUyesis { get; set; } = new List<OgretimUyesi>();
}
