# Canlıya Alma (Deploy) Rehberi

Sunucu: Plesk (Windows / IIS), site kökü `httpdocs`. Uygulama .NET 8 runtime ile çalışır (framework-dependent).

## Altın kurallar

1. **Sunucudaki dosyaları asla toplu silme.** `wwwroot/images`, `wwwroot/videos` ve `wwwroot/certificates` canlı admin panelinden yüklenen dosyaları içerir; silinirlerse veritabanı olmayan dosyalara işaret eder (kırık görseller, açılmayan PDF'ler). Deploy = **üzerine yazmak**, silmek değil.
2. **Önce veritabanı, sonra kod.** Yeni kod yeni tabloları/kolonları bekler; migration uygulanmadan yüklenirse site açılışta hata verir.
3. **Her deploy'dan önce yedek al.**

## Her deploy'da

### 1. Paketi hazırla (kendi bilgisayarında)
```bash
dotnet publish MyPortfolio -c Release -o publish
```
`publish` klasörünün içindekileri zip'le. `appsettings.Development.json` pakete otomatik olarak girmez.

### 2. Migration betiğini üret (kendi bilgisayarında)
```bash
dotnet ef migrations script --idempotent --project MyPortfolio.DataAccessLayer --startup-project MyPortfolio --output migrate.sql
```
Betik **idempotent**: her migration için "zaten uygulanmış mı?" diye bakar, sadece eksikleri uygular. Yarıda kalırsa ya da yanlışlıkla iki kez çalıştırılırsa zarar vermez.

### 3. Yedek al (Plesk)
- **Web siteleri ve alan adları → Yedekleme Yöneticisi → Yedekle** (dosyalar + veritabanı), ya da en azından **Veritabanları → (veritabanın) → Yedekle**.

### 4. Bakım moduna al (Plesk Dosya Yöneticisi)
- Repodaki `app_offline.example.htm` dosyasını `httpdocs` içine **`app_offline.htm`** adıyla yükle.
- IIS uygulamayı durdurur, ziyaretçiler bakım sayfasını görür, DLL kilitleri açılır.

### 5. Veritabanını güncelle (SSMS)
- SQL Server Management Studio → **Connect**:
  - Server name: sunucu IP'si veya alan adı
  - Authentication: **SQL Server Authentication**
  - Login / Password: canlı bağlantı cümlesindeki (connection string) kullanıcı adı ve şifre
- Canlı veritabanını seç → `migrate.sql`'i aç → **Execute (F5)**.
- Sonuç **"Commands completed successfully"** olmalı. Hata çıkarsa dur, sonraki adımlara geçme.

### 6. Dosyaları yükle (Plesk Dosya Yöneticisi)
- Zip'i `httpdocs` içine yükle → **Çıkar (Extract)** → **mevcut dosyaların üzerine yaz** seçeneğiyle.
- Hiçbir klasörü silme.

### 7. Ayarları kontrol et
- Canlı ayarlarda gerekli anahtarlar tanımlı mı (bkz. README → *Configure*). Yeni bir ayar eklendiyse (örn. `AutoMapper:LicenseKey`) canlı ayar dosyasına eklendiğinden emin ol.
- `wwwroot/certificates` klasörü yoksa boş olarak oluştur (ilk sertifika yüklemesinde klasör izni sorunu çıkmasın).

### 8. Yayına aç
- `httpdocs/app_offline.htm` dosyasını sil. Site kendiliğinden açılır.

### 9. Duman testi
- Ana sayfa, `/Certificate/Index`, bir proje detay sayfası açılıyor mu
- Admin girişi (`/Login/Index?key=...`) çalışıyor mu
- Tarayıcı konsolunda (F12) kırmızı hata var mı

## Bir şey ters giderse (geri alma)

1. `app_offline.htm`'i tekrar koy.
2. Adım 3'te aldığın yedekten **dosyaları** geri yükle.
3. Migration uygulandıysa ve eski kod yeni şemayla çalışmıyorsa **veritabanı yedeğini** de geri yükle.
4. `app_offline.htm`'i sil.
