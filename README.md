# Ders Kayıt ve Not Takip Sistemi

Veritabanı Yönetim Sistemleri laboratuvar dersi kapsamında geliştirdiğim, ASP.NET Core MVC mimarisine sahip web tabanlı otomasyon projesidir. Proje, üniversite süreçlerindeki ders seçimi, notlandırma ve kullanıcı yönetimini dijitalleştirmeyi amaçlar.

## Proje Hakkında

Bu sistemde Admin, Öğretim Üyesi ve Öğrenci olmak üzere üç farklı kullanıcı rolü bulunmaktadır. "Code First" yaklaşımı kullanılarak veritabanı bağlantısı sağlanmış ve tüm CRUD (Ekleme, Silme, Güncelleme, Listeleme) işlemleri arayüz üzerinden gerçekleştirilebilir hale getirilmiştir.

### Kullanılan Teknolojiler

* **Backend:** C#, ASP.NET Core 8.0
* **Veritabanı:** MS SQL Server, Entity Framework Core
* **Frontend:** HTML, CSS, Bootstrap, JavaScript
* **IDE:** Visual Studio 2022

### Temel Özellikler

**Admin Modülü:**
* Öğrenci, Öğretim Üyesi ve Bölüm ekleme/silme/güncelleme işlemleri.
* Ders atamaları ve kullanıcı yetkilendirmeleri.

**Öğretim Üyesi Modülü:**
* Verilen derslerin listelenmesi.
* Öğrencilere vize ve final notu girişi yapılması.

**Öğrenci Modülü:**
* Aktif dönem derslerinin seçilmesi (Ders Kayıt).
* Sınav notlarının ve başarı durumunun görüntülenmesi.

### Kurulum

Projeyi kendi bilgisayarınızda çalıştırmak için:

1.  Projeyi klonlayın veya zip olarak indirin.
2.  `appsettings.json` dosyasındaki "ConnectionStrings" alanını kendi yerel SQL sunucunuza göre düzenleyin.
3.  Package Manager Console üzerinden `Update-Database` komutunu çalıştırarak veritabanını oluşturun.
4.  Projeyi çalıştırın.
