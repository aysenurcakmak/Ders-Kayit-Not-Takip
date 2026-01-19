using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Models;

[Index("DersKodu", Name = "UQ__Ders__9DCB30EF55A68D3F", IsUnique = true)]
public partial class Der
{
    [Key]
    [Column("DersID")]
    public int DersId { get; set; }

    [StringLength(20)]
    public string? DersKodu { get; set; } = null!;

    [StringLength(200)]
    public string DersAdi { get; set; } = null!;

    public int? Kredi { get; set; }

    [Column("BolumID")]
    public int? BolumId { get; set; }

    [StringLength(500)]
    public string? Aciklama { get; set; }

    [ForeignKey("BolumId")]
    [InverseProperty("Ders")]
    public virtual Bolum? Bolum { get; set; }

    [InverseProperty("Ders")]
    public virtual ICollection<DersAcma> DersAcmas { get; set; } = new List<DersAcma>();
}
