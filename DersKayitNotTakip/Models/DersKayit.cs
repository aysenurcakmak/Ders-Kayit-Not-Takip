using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Models;

[Table("DersKayit")]
[Index("OgrenciId", "DersAcmaId", Name = "UK_Ogrenci_DersAcma", IsUnique = true)]
public partial class DersKayit
{
    [Key]
    [Column("DersKayitID")]
    public int DersKayitId { get; set; }

    [Column("OgrenciID")]
    public int OgrenciId { get; set; }

    [Column("DersAcmaID")]
    public int DersAcmaId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? KayitTarihi { get; set; }

    [StringLength(20)]
    public string? Durum { get; set; }

    [ForeignKey("DersAcmaId")]
    [InverseProperty("DersKayits")]
    public virtual DersAcma DersAcma { get; set; } = null!;

    [InverseProperty("DersKayit")]
    public virtual ICollection<Notlar> Notlars { get; set; } = new List<Notlar>();

    [ForeignKey("OgrenciId")]
    [InverseProperty("DersKayits")]
    public virtual Ogrenci Ogrenci { get; set; } = null!;
}
