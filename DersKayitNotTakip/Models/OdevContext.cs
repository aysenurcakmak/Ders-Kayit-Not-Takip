using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Models;

public partial class OdevContext : DbContext
{
    public OdevContext()
    {
    }

    public OdevContext(DbContextOptions<OdevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bolum> Bolums { get; set; }

    public virtual DbSet<Der> Ders { get; set; }

    public virtual DbSet<DersAcma> DersAcmas { get; set; }

    public virtual DbSet<DersKayit> DersKayits { get; set; }

    public virtual DbSet<Donem> Donems { get; set; }

    public virtual DbSet<Kullanici> Kullanicis { get; set; }

    public virtual DbSet<Notlar> Notlars { get; set; }

    public virtual DbSet<Ogrenci> Ogrencis { get; set; }

    public virtual DbSet<OgretimUyesi> OgretimUyesis { get; set; }

    public virtual DbSet<Yetki> Yetkis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=ODEV;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bolum>(entity =>
        {
            entity.HasKey(e => e.BolumId).HasName("PK__Bolum__7BAD4B5CF6795448");
        });

        modelBuilder.Entity<Der>(entity =>
        {
            entity.HasKey(e => e.DersId).HasName("PK__Ders__E8B3DE7180C563EA");

            entity.HasOne(d => d.Bolum).WithMany(p => p.Ders).HasConstraintName("FK__Ders__BolumID__5441852A");
        });

        modelBuilder.Entity<DersAcma>(entity =>
        {
            entity.HasKey(e => e.DersAcmaId).HasName("PK__DersAcma__63FB021B448D08CA");

            entity.Property(e => e.Kontenjan).HasDefaultValue(50);

            entity.HasOne(d => d.Ders).WithMany(p => p.DersAcmas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DersAcma__DersID__5812160E");

            entity.HasOne(d => d.Donem).WithMany(p => p.DersAcmas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DersAcma__DonemI__59FA5E80");

            entity.HasOne(d => d.OgretimUyesi).WithMany(p => p.DersAcmas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DersAcma__Ogreti__59063A47");
        });

        modelBuilder.Entity<DersKayit>(entity =>
        {
            entity.HasKey(e => e.DersKayitId).HasName("PK__DersKayi__9B0B7C566ECA2C73");

            entity.Property(e => e.Durum).HasDefaultValue("Kayitli");
            entity.Property(e => e.KayitTarihi).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.DersAcma).WithMany(p => p.DersKayits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DersKayit__DersA__60A75C0F");

            entity.HasOne(d => d.Ogrenci).WithMany(p => p.DersKayits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DersKayit__Ogren__5FB337D6");
        });

        modelBuilder.Entity<Donem>(entity =>
        {
            entity.HasKey(e => e.DonemId).HasName("PK__Donem__886960298767FA50");
        });

        modelBuilder.Entity<Kullanici>(entity =>
        {
            entity.HasKey(e => e.KullaniciId).HasName("PK__Kullanic__E011F09B14E2EC5C");

            entity.HasOne(d => d.Ogrenci).WithMany(p => p.Kullanicis).HasConstraintName("FK__Kullanici__Ogren__6C190EBB");

            entity.HasOne(d => d.OgretimUyesi).WithMany(p => p.Kullanicis).HasConstraintName("FK__Kullanici__Ogret__6D0D32F4");

            entity.HasOne(d => d.Yetki).WithMany(p => p.Kullanicis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Kullanici__Yetki__6E01572D");
        });

        modelBuilder.Entity<Notlar>(entity =>
        {
            entity.HasKey(e => e.NotId).HasName("PK__Notlar__4FB200AAF602E52F");

            entity.Property(e => e.HarfNotu).HasComputedColumnSql("(case when isnull([Butunleme],[Vize]*(0.4)+[Final]*(0.6))>=(85) then 'AA' when isnull([Butunleme],[Vize]*(0.4)+[Final]*(0.6))>=(70) then 'BA' when isnull([Butunleme],[Vize]*(0.4)+[Final]*(0.6))>=(60) then 'BB' when isnull([Butunleme],[Vize]*(0.4)+[Final]*(0.6))>=(50) then 'CB' when isnull([Butunleme],[Vize]*(0.4)+[Final]*(0.6))>=(40) then 'CC' else 'FF' end)", true);
            entity.Property(e => e.Ortalama).HasComputedColumnSql("(isnull([Butunleme],[Vize]*(0.4)+[Final]*(0.6)))", true);

            entity.HasOne(d => d.DersKayit).WithMany(p => p.Notlars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notlar__DersKayi__66603565");
        });

        modelBuilder.Entity<Ogrenci>(entity =>
        {
            entity.HasKey(e => e.OgrenciId).HasName("PK__Ogrenci__E497E6D4B2D819E6");

            entity.HasOne(d => d.Bolum).WithMany(p => p.Ogrencis).HasConstraintName("FK__Ogrenci__BolumID__4BAC3F29");
        });

        modelBuilder.Entity<OgretimUyesi>(entity =>
        {
            entity.HasKey(e => e.OgretimUyesiId).HasName("PK__OgretimU__DCA3E5F2C851276A");

            entity.HasOne(d => d.Bolum).WithMany(p => p.OgretimUyesis).HasConstraintName("FK__OgretimUy__Bolum__4E88ABD4");
        });

        modelBuilder.Entity<Yetki>(entity =>
        {
            entity.HasKey(e => e.YetkiId).HasName("PK__Yetki__3496140694473334");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
