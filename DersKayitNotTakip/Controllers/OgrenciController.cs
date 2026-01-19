using DersKayitNotTakip.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly OdevContext _context;

        public OgrenciController(OdevContext context)
        {
            _context = context;
        }

        public IActionResult Panel()
        {
            int? kullaniciId = HttpContext.Session.GetInt32("KullaniciID");

            if (kullaniciId == null)
                return RedirectToAction("Login", "Account");

            var kullanici = _context.Kullanicis
                .FirstOrDefault(x => x.KullaniciId == kullaniciId);

            if (kullanici == null || kullanici.OgrenciId == null)
                return RedirectToAction("Login", "Account");

            int ogrenciId = kullanici.OgrenciId.Value;

            var dersler = _context.DersKayits
                .Where(dk => dk.OgrenciId == ogrenciId)
                .Select(dk => new
                {
                    DersAdi = dk.DersAcma.Ders.DersAdi,
                    Vize = dk.Notlars.FirstOrDefault() != null ? dk.Notlars.First().Vize : null,
                    Final = dk.Notlars.FirstOrDefault() != null ? dk.Notlars.First().Final : null,
                    Ortalama = dk.Notlars.FirstOrDefault() != null ? dk.Notlars.First().Ortalama : null,
                    HarfNotu = dk.Notlars.FirstOrDefault() != null ? dk.Notlars.First().HarfNotu : "-"
                })
                .ToList();

            return View(dersler);
        }

        public IActionResult DersSec()
        {
            int? kullaniciId = HttpContext.Session.GetInt32("KullaniciID");

            if (kullaniciId == null)
                return RedirectToAction("Login", "Account");

            var ogrenciId = _context.Kullanicis
                .Where(k => k.KullaniciId == kullaniciId)
                .Select(k => k.OgrenciId)
                .FirstOrDefault();

            var dersler = _context.DersAcmas
                .Include(d => d.Ders)
                .Include(d => d.OgretimUyesi)
                .ToList();

            ViewBag.OgrenciId = ogrenciId;
            return View(dersler);
        }

        public IActionResult DersKayit(int id)
        {
            int? kullaniciId = HttpContext.Session.GetInt32("KullaniciID");
            if (kullaniciId == null)
                return RedirectToAction("Login", "Account");

            var ogrenciId = _context.Kullanicis
                .Where(k => k.KullaniciId == kullaniciId)
                .Select(k => k.OgrenciId)
                .FirstOrDefault();

            if (ogrenciId == null)
                return RedirectToAction("Login", "Account");

            var ogrenci = _context.Ogrencis
                .FirstOrDefault(o => o.OgrenciId == ogrenciId);

            var dersAcma = _context.DersAcmas
                .Include(d => d.Ders)
                .FirstOrDefault(d => d.DersAcmaId == id);

            if (dersAcma == null)
            {
                TempData["Hata"] = "Ders bulunamadı.";
                return RedirectToAction("DersSec");
            }

            // ❌ aynı ders açma
            bool ayniDersAcma = _context.DersKayits.Any(dk =>
                dk.OgrenciId == ogrenciId &&
                dk.DersAcmaId == id);

            if (ayniDersAcma)
            {
                TempData["Hata"] = "Bu dersi zaten aldınız.";
                return RedirectToAction("DersSec");
            }

            // ❌ aynı ders daha önce alınmış mı
            bool ayniDersOnce = _context.DersKayits
                .Include(dk => dk.DersAcma)
                .Any(dk =>
                    dk.OgrenciId == ogrenciId &&
                    dk.DersAcma.DersId == dersAcma.DersId);

            if (ayniDersOnce)
            {
                TempData["Hata"] = "Bu dersi daha önce aldığınız için tekrar alamazsınız.";
                return RedirectToAction("DersSec");
            }

            // ❌ bölüm kontrolü
            if (ogrenci.BolumId != dersAcma.Ders.BolumId)
            {
                TempData["Hata"] = "Kendi bölümünüz dışındaki dersleri alamazsınız.";
                return RedirectToAction("DersSec");
            }

            // ✅ kayıt
            var kayit = new DersKayit
            {
                OgrenciId = ogrenci.OgrenciId,
                DersAcmaId = id
            };

            _context.DersKayits.Add(kayit);
            _context.SaveChanges();

            TempData["Basarili"] = "Ders başarıyla eklendi.";
            return RedirectToAction("Panel");
        }
    }
}
