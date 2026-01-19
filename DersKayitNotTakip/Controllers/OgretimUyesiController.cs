using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DersKayitNotTakip.Models;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Controllers
{
    public class OgretimUyesiController : Controller
    {
        private readonly OdevContext _context;

        public OgretimUyesiController(OdevContext context)
        {
            _context = context;
        }

        // 🔹 Öğretim Üyesi Paneli
        public IActionResult Panel()
        {
            int? kullaniciId = HttpContext.Session.GetInt32("KullaniciID");
            if (kullaniciId == null)
                return RedirectToAction("Login", "Account");

            // öğretim üyesi
            var ogretimUyesi = _context.Kullanicis
                .Include(k => k.OgretimUyesi)
                .FirstOrDefault(k => k.KullaniciId == kullaniciId)
                ?.OgretimUyesi;

            if (ogretimUyesi == null)
                return RedirectToAction("Login", "Account");

            int? bolumId = ogretimUyesi.BolumId;

            var ogrenciler = _context.DersKayits
                .Include(dk => dk.Ogrenci)
                .Include(dk => dk.DersAcma)
                    .ThenInclude(da => da.Ders)
                .Where(dk => dk.Ogrenci.BolumId == bolumId)
                .Select(dk => new
                {
                    dk.DersKayitId,
                    Ad = dk.Ogrenci.Ad,
                    Soyad = dk.Ogrenci.Soyad,
                    dk.Ogrenci.Email,
                    DersAdi = dk.DersAcma.Ders.DersAdi
                })
                .ToList();

            return View(ogrenciler);
        }


        // 🔹 Not Gir (GET)
        public IActionResult NotGir(int id)
        {
            ViewBag.DersKayitId = id;
            return View();
        }

        // 🔹 Not Gir (POST)
        [HttpPost]
        public IActionResult NotGir(int dersKayitId, double vize, double final)
        {
            var not = _context.Notlars
                .FirstOrDefault(n => n.DersKayitId == dersKayitId);

            if (not == null)
            {
                not = new Notlar
                {
                    DersKayitId = dersKayitId,
                    Vize = vize,
                    Final = final
                };
                _context.Notlars.Add(not);
            }
            else
            {
                not.Vize = vize;
                not.Final = final;
            }

            _context.SaveChanges();
            return RedirectToAction("Panel");
        }
    }
}
