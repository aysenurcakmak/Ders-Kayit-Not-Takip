using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Models;

[Table("Notlar")]
public partial class Notlar
{
    [Key]
    [Column("NotID")]
    public int NotId { get; set; }

    [Column("DersKayitID")]
    public int DersKayitId { get; set; }

    public double? Vize { get; set; }

    public double? Final { get; set; }

    public double? Butunleme { get; set; }

    public double? Ortalama { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string HarfNotu { get; set; } = null!;

    [ForeignKey("DersKayitId")]
    [InverseProperty("Notlars")]
    public virtual DersKayit DersKayit { get; set; } = null!;
}
