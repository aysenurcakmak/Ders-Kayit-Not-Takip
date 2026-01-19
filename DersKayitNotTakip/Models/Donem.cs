using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Models;

[Table("Donem")]
public partial class Donem
{
    [Key]
    [Column("DonemID")]
    public int DonemId { get; set; }

    public int Yil { get; set; }

    [StringLength(10)]
    public string Yariyil { get; set; } = null!;

    [InverseProperty("Donem")]
    public virtual ICollection<DersAcma> DersAcmas { get; set; } = new List<DersAcma>();
}
