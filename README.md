# LookTwiice

.NET 10 + PostgreSQL + Identity tabanlı, yeniden kullanılabilir proje şablonu.

## Özellikler
- ASP.NET Core Identity (özel ApplicationUser: Name, Surname, PhoneNumber, ProfileImageUrl)
- Rol tabanlı yetkilendirme (Admin/User)
- Areas mimarisi (Admin paneli ayrı bölüm)
- Çok dilli destek (TR/EN/DE)
- Email doğrulama + şifre sıfırlama (MailKit + Gmail SMTP)
- Özel 404/500 hata sayfaları
- BaseEntity (Id, CreatedAt, UpdatedAt) — yeni modeller için hazır altyapı
- Servis katmanı (Interfaces/Implementations ayrımı)

## Kurulum

### 1. PostgreSQL
Local'de PostgreSQL kurulu ve çalışır olmalı. Bir database oluştur (örn. `MVCTemplateDb`).

### 2. User Secrets
Visual Studio'da proje adına sağ tık → **Manage User Secrets**, şunu ekle:

\`\`\`json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=MVCTemplateDb;Username=postgres;Password=GERÇEK_ŞİFREN"
  },
  "EmailSettings": {
    "SenderEmail": "gerçek-gmailin@gmail.com",
    "SenderPassword": "16-haneli-Gmail-uygulama-şifresi"
  },
  "SeedAdmin": {
    "Email": "ilk-admin-olacak-email@gmail.com"
  }
}
\`\`\`

### 3. Gmail Uygulama Şifresi (email gönderimi için)
1. [myaccount.google.com/security](https://myaccount.google.com/security) → 2 Adımlı Doğrulama'yı aç
2. "Uygulama Şifreleri" oluştur, 16 haneli kodu `secrets.json`'a yapıştır

### 4. Çalıştır
\`\`\`bash
dotnet run
\`\`\`
Migration'lar otomatik uygulanır (`Database.MigrateAsync()`), Admin/User rolleri otomatik oluşturulur, elle bir şey yapmana gerek yok.

### 5. İlk Admin
`SeedAdmin:Email` alanına yazdığın email ile **Register** ol, uygulamayı yeniden başlat — o kullanıcı otomatik Admin rolüne atanır.

## Notlar
- Admin paneli: `/Admin/Admin`
- Kullanıcı yönetimi: `/Admin/Users`
- Profil sayfası: `/Identity/Account/Manage`