using DersKayitNotTakip.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DersKayitNotTakip.Controllers
{
    public class AccountController : Controller
    {
        private readonly OdevContext _context;

        public AccountController(OdevContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string kullaniciAdi, string sifre)
        {
            // 1. Kullanıcıyı buluyoruz
            var kullanici = _context.Kullanicis
                .FirstOrDefault(x => x.KullaniciAdi == kullaniciAdi && x.Sifre == sifre);

            if (kullanici == null)
            {
                ViewBag.Hata = "Kullanıcı adı veya şifre hatalı";
                return View();
            }

            // --- SESSION AYARLARI (BURAYI GÜNCELLEDİM) ---

            // Temel bilgileri Session'a atıyoruz
            HttpContext.Session.SetInt32("KullaniciID", kullanici.KullaniciId);
            HttpContext.Session.SetInt32("YetkiID", kullanici.YetkiId);

            // Ad Soyad bilgisini YetkiID'ye göre ilgili tablodan çekiyoruz
            string adSoyad = "Kullanıcı";

            if (kullanici.YetkiId == 1) // Admin
            {
                adSoyad = "Yönetici (Admin)";
            }
            else if (kullanici.YetkiId == 2) // Öğrenci
            {
                // Öğrenci tablosundan ismini bulalım
                var ogrenci = _context.Ogrencis.FirstOrDefault(o => o.OgrenciId == kullanici.OgrenciId);
                if (ogrenci != null)
                {
                    adSoyad = ogrenci.Ad + " " + ogrenci.Soyad;
                }
            }
            else if (kullanici.YetkiId == 3) // Öğretim Üyesi
            {
                // Hoca tablosundan ismini bulalım
                var hoca = _context.OgretimUyesis.FirstOrDefault(h => h.OgretimUyesiId == kullanici.OgretimUyesiId);
                if (hoca != null)
                {
                    adSoyad = hoca.Unvan + " " + hoca.Ad + " " + hoca.Soyad;
                }
            }

            // Bulduğumuz ismi Session'a kaydediyoruz (Layout'ta göstermek için)
            HttpContext.Session.SetString("AdSoyad", adSoyad);

            // ----------------------------------------------

            // Yönlendirmeler
            if (kullanici.YetkiId == 1)
                return RedirectToAction("Panel", "Admin");

            if (kullanici.YetkiId == 2)
                return RedirectToAction("Panel", "Ogrenci");

            if (kullanici.YetkiId == 3)
                return RedirectToAction("Panel", "OgretimUyesi");

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Tüm oturum bilgilerini temizle
            return RedirectToAction("Login");
        }
    }
}