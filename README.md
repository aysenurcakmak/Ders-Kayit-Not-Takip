# Ders Kayıt ve Not Takip Sistemi

Veritabanı Yönetim Sistemleri laboratuvar dersi kapsamında geliştirdiğim, ASP.NET Core MVC mimarisine sahip web tabanlı otomasyon projesidir. Proje, üniversite süreçlerindeki ders seçimi, notlandırma ve kullanıcı yönetimini dijitalleştirmeyi amaçlar.

## Proje Hakkında

Bu sistemde Admin, Öğretim Üyesi ve Öğrenci olmak üzere üç farklı kullanıcı rolü bulunmaktadır. "Code First" yaklaşımı kullanılarak veritabanı bağlantısı sağlanmış ve tüm CRUD (Ekleme, Silme, Güncelleme, Listeleme) işlemleri arayüz üzerinden gerçekleştirilebilir hale getirilmiştir.

### Kullanılan Teknolojiler

* **Backend:** C#, ASP.NET Core 8.0
* **Veritabanı:** MS SQL Server, Entity Framework Core
* **Frontend:** HTML, CSS, Bootstrap, JavaScript
* **IDE:** Visual Studio 2022
  <img width="1906" height="1033" alt="image" src="https://github.com/user-attachments/assets/52d78af7-4c6c-47e5-a139-fa5ff9f93d81" />

### Temel Özellikler

**Admin Modülü:**
* Öğrenci, Öğretim Üyesi ve Bölüm ekleme/silme/güncelleme işlemleri.
* Ders atamaları ve kullanıcı yetkilendirmeleri.
  <img width="1900" height="1020" alt="image" src="https://github.com/user-attachments/assets/449dabe6-e2a8-445b-b764-4b896a31198c" />


**Öğretim Üyesi Modülü:**
* Verilen derslerin listelenmesi.
* Öğrencilere vize ve final notu girişi yapılması.
  <img width="1907" height="1002" alt="image" src="https://github.com/user-attachments/assets/c07b00fc-91e6-45e4-acf7-8fdd28072b66" />


**Öğrenci Modülü:**
* Aktif dönem derslerinin seçilmesi (Ders Kayıt).
* Sınav notlarının ve başarı durumunun görüntülenmesi.
  <img width="1900" height="1041" alt="image" src="https://github.com/user-attachments/assets/93764496-5ba2-4eae-8c76-caee9c4b42e3" />


### Kurulum

Projeyi kendi bilgisayarınızda çalıştırmak için:

1.  Projeyi klonlayın veya zip olarak indirin.
2.  `appsettings.json` dosyasındaki "ConnectionStrings" alanını kendi yerel SQL sunucunuza göre düzenleyin.
3.  Package Manager Console üzerinden `Update-Database` komutunu çalıştırarak veritabanını oluşturun.
4.  Projeyi çalıştırın.
