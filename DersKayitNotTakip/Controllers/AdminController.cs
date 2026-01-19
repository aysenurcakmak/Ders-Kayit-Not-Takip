using Microsoft.AspNetCore.Mvc;
using DersKayitNotTakip.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DersKayitNotTakip.Controllers
{
    public class AdminController : Controller
    {
        private readonly OdevContext _context;

        public AdminController(OdevContext context)
        {
            _context = context;
        }

        // 🔐 Admin kontrol
        private bool AdminMi()
        {
            return HttpContext.Session.GetInt32("YetkiID") == 1;
        }

        // =========================
        // 📋 ÖĞRENCİ İŞLEMLERİ
        // =========================
        public IActionResult Panel()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            return View();
        }

        public IActionResult OgrenciListe()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            var ogrenciler = _context.Ogrencis
                .Include(o => o.Bolum)
                .ToList();

            return View(ogrenciler);
        }

        public IActionResult OgrenciEkle()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            ViewBag.Bolumler = _context.Bolums.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult OgrenciEkle(Ogrenci ogrenci, string KullaniciAdi, string Sifre)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            bool kullaniciVarMi = _context.Kullanicis
                .Any(k => k.KullaniciAdi == KullaniciAdi);

            if (kullaniciVarMi)
            {
                ViewBag.Hata = "Bu kullanıcı adı zaten kullanılıyor!";
                ViewBag.Bolumler = _context.Bolums.ToList();
                return View();
            }

            _context.Ogrencis.Add(ogrenci);
            _context.SaveChanges();

            var kullanici = new Kullanici
            {
                KullaniciAdi = KullaniciAdi,
                Sifre = Sifre,
                OgrenciId = ogrenci.OgrenciId,
                YetkiId = 2
            };

            _context.Kullanicis.Add(kullanici);
            _context.SaveChanges();

            TempData["Basarili"] = "Öğrenci başarıyla kaydedildi.";
            return RedirectToAction("OgrenciListe");
        }

        public IActionResult OgrenciSil(int id)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            var dersKayitlar = _context.DersKayits
                .Where(dk => dk.OgrenciId == id)
                .ToList();

            foreach (var dk in dersKayitlar)
            {
                var notlar = _context.Notlars
                    .Where(n => n.DersKayitId == dk.DersKayitId)
                    .ToList();

                _context.Notlars.RemoveRange(notlar);
            }

            _context.DersKayits.RemoveRange(dersKayitlar);

            var kullanici = _context.Kullanicis
                .FirstOrDefault(k => k.OgrenciId == id);

            if (kullanici != null)
                _context.Kullanicis.Remove(kullanici);

            var ogrenci = _context.Ogrencis.Find(id);
            if (ogrenci != null)
                _context.Ogrencis.Remove(ogrenci);

            _context.SaveChanges();

            TempData["Basarili"] = "Öğrenci ve ilgili kayıtları silindi.";
            return RedirectToAction("OgrenciListe");
        }

        // ✏️ ÖĞRENCİ DÜZENLEME (YENİ EKLENDİ)
        [HttpGet]
        public IActionResult OgrenciDuzenle(int id)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Account");

            var ogrenci = _context.Ogrencis.Find(id);
            if (ogrenci == null) return NotFound();

            // Öğrencinin giriş bilgilerini de (Kullanıcı Adı/Şifre) bulalım
            var kullanici = _context.Kullanicis.FirstOrDefault(k => k.OgrenciId == id);

            ViewBag.KullaniciAdi = kullanici?.KullaniciAdi;
            ViewBag.Sifre = kullanici?.Sifre;
            ViewBag.Bolumler = _context.Bolums.ToList();

            return View(ogrenci);
        }

        [HttpPost]
        public IActionResult OgrenciDuzenle(Ogrenci ogrenci, string KullaniciAdi, string Sifre)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Account");

            var mevcutOgrenci = _context.Ogrencis.Find(ogrenci.OgrenciId);
            if (mevcutOgrenci == null) return NotFound();

            // Öğrenci Bilgilerini Güncelle
            mevcutOgrenci.Ad = ogrenci.Ad;
            mevcutOgrenci.Soyad = ogrenci.Soyad;
            mevcutOgrenci.Email = ogrenci.Email;
            mevcutOgrenci.DogumTarihi = ogrenci.DogumTarihi;
            mevcutOgrenci.BolumId = ogrenci.BolumId;

            // Kullanıcı Giriş Bilgilerini Güncelle
            var kullanici = _context.Kullanicis.FirstOrDefault(k => k.OgrenciId == ogrenci.OgrenciId);
            if (kullanici != null)
            {
                kullanici.KullaniciAdi = KullaniciAdi;
                kullanici.Sifre = Sifre;
            }

            _context.SaveChanges();
            TempData["Basarili"] = "Öğrenci bilgileri güncellendi.";

            return RedirectToAction("OgrenciListe");
        }


        // =========================
        // 🏫 BÖLÜM İŞLEMLERİ
        // =========================

        // 📋 Bölüm Listesi
        public IActionResult BolumListe()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            return View(_context.Bolums.ToList());
        }

        // ➕ Bölüm Ekle (GET)
        public IActionResult BolumEkle()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // ➕ Bölüm Ekle (POST)
        [HttpPost]
        public IActionResult BolumEkle(Bolum bolum)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            _context.Bolums.Add(bolum);
            _context.SaveChanges();
            TempData["Basarili"] = "Bölüm başarıyla eklendi.";

            return RedirectToAction("BolumListe");
        }

        // 🗑️ BÖLÜM SİLME İŞLEMİ
        [HttpGet]
        public IActionResult BolumSil(int id)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Account");

            var bolum = _context.Bolums.Find(id);
            if (bolum == null) return NotFound();

            return View(bolum);
        }

        [HttpPost, ActionName("BolumSil")]
        public IActionResult BolumSilOnay(int id)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Account");

            try
            {
                var bolum = _context.Bolums.Find(id);
                if (bolum != null)
                {
                    _context.Bolums.Remove(bolum);
                    _context.SaveChanges();
                    TempData["Basarili"] = "Bölüm başarıyla silindi.";
                }
            }
            catch (Exception)
            {
                TempData["Hata"] = "Bu bölümde kayıtlı öğrenci, hoca veya ders olduğu için silinemiyor!";
            }

            return RedirectToAction("BolumListe");
        }


        // =========================
        // 📚 DERS İŞLEMLERİ
        // =========================

        // 📋 Ders Listesi
        public IActionResult DersListe()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            var dersler = _context.Ders
                .Include(d => d.Bolum)
                .ToList();

            return View(dersler);
        }

        // ➕ Ders Ekle (GET)
        public IActionResult DersEkle()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            ViewBag.Bolumler = _context.Bolums.ToList();
            return View();
        }

        // ➕ Ders Ekle (POST)
        [HttpPost]
        public IActionResult DersEkle(Der ders)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            bool varMi = _context.Ders.Any(d => d.DersKodu == ders.DersKodu);

            if (varMi)
            {
                ViewBag.Hata = "Bu ders kodu zaten kayıtlı!";
                ViewBag.Bolumler = _context.Bolums.ToList();
                return View();
            }

            _context.Ders.Add(ders);
            _context.SaveChanges();

            TempData["Basarili"] = "Ders başarıyla kaydedildi.";
            return RedirectToAction("DersListe");
        }

        // 🗑️ DERS SİLME İŞLEMLERİ
        [HttpGet]
        public IActionResult DersSil(int id)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            // Silinecek dersi bul ve onay sayfasına gönder
            var ders = _context.Ders.Include(d => d.Bolum).FirstOrDefault(d => d.DersId == id);

            if (ders == null)
            {
                return NotFound();
            }
            return View(ders);
        }

        [HttpPost, ActionName("DersSil")]
        public IActionResult DersSilOnay(int id)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            try
            {
                var ders = _context.Ders.Find(id);
                if (ders != null)
                {
                    _context.Ders.Remove(ders);
                    _context.SaveChanges();
                    TempData["Basarili"] = "Ders başarıyla silindi.";
                }
            }
            catch (Exception)
            {
                // Eğer derse kayıtlı öğrenci varsa veritabanı silmeye izin vermeyebilir
                TempData["Hata"] = "Bu derse ait kayıtlı veriler olduğu için silinemiyor! Önce ilişkili kayıtları silmelisiniz.";
            }

            return RedirectToAction("DersListe");
        }


        // 📋 Açılan Dersler
        public IActionResult DersAcmaListe()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            var liste = _context.DersAcmas
                .Include(d => d.Ders)
                .Include(d => d.OgretimUyesi)
                .ToList();

            return View(liste);
        }

        // ➕ Ders Aç (GET)
        public IActionResult DersAc()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            ViewBag.Dersler = _context.Ders.ToList();
            ViewBag.Hocalar = _context.OgretimUyesis.ToList();
            return View();
        }

        // ➕ Ders Aç (POST)
        [HttpPost]
        public IActionResult DersAc(DersAcma da)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            _context.DersAcmas.Add(da);
            _context.SaveChanges();

            ViewBag.Mesaj = "Ders başarıyla açıldı.";
            ViewBag.Dersler = _context.Ders.ToList();
            ViewBag.Hocalar = _context.OgretimUyesis.ToList();

            return View();
        }


        // =========================
        // 👨‍🏫 ÖĞRETİM ÜYESİ İŞLEMLERİ
        // =========================

        public IActionResult OgretimUyesiListe()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            var uyeler = _context.OgretimUyesis
                .Include(o => o.Bolum)
                .ToList();

            return View(uyeler);
        }

        public IActionResult OgretimUyesiEkle()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            ViewBag.Bolumler = _context.Bolums.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult OgretimUyesiEkle(
            OgretimUyesi ogretimUyesi,
            string KullaniciAdi,
            string Sifre)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Account");

            // 1. Kullanıcı adı kontrolü
            bool kullaniciVarMi = _context.Kullanicis
                .Any(k => k.KullaniciAdi == KullaniciAdi);

            if (kullaniciVarMi)
            {
                ViewBag.Hata = "Bu kullanıcı adı zaten kullanılıyor!";
                ViewBag.Bolumler = _context.Bolums.ToList();
                return View();
            }

            // 2. Hoca Ekleme
            _context.OgretimUyesis.Add(ogretimUyesi);
            _context.SaveChanges();

            // 3. Kullanıcı (Giriş) Hesabı Oluşturma
            var kullanici = new Kullanici
            {
                KullaniciAdi = KullaniciAdi,
                Sifre = Sifre,
                OgretimUyesiId = ogretimUyesi.OgretimUyesiId,
                YetkiId = 3 // 3 = Akademisyen
            };

            _context.Kullanicis.Add(kullanici);
            _context.SaveChanges();

            // ✅ Başarı mesajı eklendi
            TempData["Basarili"] = "Öğretim üyesi başarıyla kaydedildi.";

            return RedirectToAction("OgretimUyesiListe");
        }

        // 🗑️ ÖĞRETİM ÜYESİ SİLME İŞLEMİ
        [HttpGet]
        public IActionResult OgretimUyesiSil(int id)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Account");

            var hoca = _context.OgretimUyesis.Include(x => x.Bolum).FirstOrDefault(x => x.OgretimUyesiId == id);
            if (hoca == null) return NotFound();

            return View(hoca);
        }

        [HttpPost, ActionName("OgretimUyesiSil")]
        public IActionResult OgretimUyesiSilOnay(int id)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Account");

            try
            {
                // Önce hocanın Kullanıcı hesabını silelim (Varsa)
                var kullanici = _context.Kullanicis.FirstOrDefault(k => k.OgretimUyesiId == id);
                if (kullanici != null) _context.Kullanicis.Remove(kullanici);

                // Sonra hocayı silelim
                var hoca = _context.OgretimUyesis.Find(id);
                if (hoca != null) _context.OgretimUyesis.Remove(hoca);

                _context.SaveChanges();
                TempData["Basarili"] = "Öğretim üyesi ve giriş hesabı silindi.";
            }
            catch (Exception)
            {
                TempData["Hata"] = "Bu hocanın üzerinde aktif dersler veya notlar olduğu için silinemiyor.";
            }

            return RedirectToAction("OgretimUyesiListe");
        }

        // ✏️ ÖĞRETİM ÜYESİ DÜZENLEME (YENİ EKLENDİ)
        [HttpGet]
        public IActionResult OgretimUyesiDuzenle(int id)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Account");

            var hoca = _context.OgretimUyesis.Find(id);
            if (hoca == null) return NotFound();

            // Hocanın giriş bilgilerini bulalım
            var kullanici = _context.Kullanicis.FirstOrDefault(k => k.OgretimUyesiId == id);

            ViewBag.KullaniciAdi = kullanici?.KullaniciAdi;
            ViewBag.Sifre = kullanici?.Sifre;
            ViewBag.Bolumler = _context.Bolums.ToList();

            return View(hoca);
        }

        [HttpPost]
        public IActionResult OgretimUyesiDuzenle(OgretimUyesi hoca, string KullaniciAdi, string Sifre)
        {
            if (!AdminMi()) return RedirectToAction("Login", "Account");

            var mevcutHoca = _context.OgretimUyesis.Find(hoca.OgretimUyesiId);
            if (mevcutHoca == null) return NotFound();

            // Hoca Bilgilerini Güncelle
            mevcutHoca.Unvan = hoca.Unvan;
            mevcutHoca.Ad = hoca.Ad;
            mevcutHoca.Soyad = hoca.Soyad;
            mevcutHoca.Email = hoca.Email;
            mevcutHoca.BolumId = hoca.BolumId;

            // Kullanıcı Giriş Bilgilerini Güncelle
            var kullanici = _context.Kullanicis.FirstOrDefault(k => k.OgretimUyesiId == hoca.OgretimUyesiId);
            if (kullanici != null)
            {
                kullanici.KullaniciAdi = KullaniciAdi;
                kullanici.Sifre = Sifre;
            }

            _context.SaveChanges();
            TempData["Basarili"] = "Öğretim üyesi bilgileri güncellendi.";

            return RedirectToAction("OgretimUyesiListe");
        }
    }
}