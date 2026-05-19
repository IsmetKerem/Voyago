# 🌍 Voyago — Travel Intelligence Platform

Booking.com mantığında otel arama, AI travel asistanı ve 8 farklı canlı seyahat verisini tek bir dashboard'da birleştiren, **uçtan uca Full Stack .NET 8** projesi.

<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 42 27" src="https://github.com/user-attachments/assets/902a5e83-20d4-47a7-8dd1-ea7995a8e4b4" />

---

## 🎯 Proje Hakkında

Voyago, bir seyahatçinin trip planning sürecinde ihtiyaç duyacağı tüm bilgileri **tek bir platformda** toplayan modern bir travel intelligence platform'dur. **9 farklı 3rd party API + OpenAI** entegrasyonu ile kullanıcıya zenginleştirilmiş bir deneyim sunar.

**Hikaye:** Sıradan bir API tüketimi değil; canlı verileri (hava, döviz, haber, hotel, AI tavsiye) tek bir dashboard'da toplamak ve gerçek bir Booking.com benzeri user flow inşa etmek.

---

## ✨ Özellikler

### 🏨 Hotel Search & Booking (Booking.com clone)
- **Destinasyon araması:** Şehir bazlı otel araması (Booking-com15 API)
- **Akıllı filtreleme:** Fiyat aralığı, yıldız sayısı, review score
- **Defensive client-side filter:** API filter sonuçlarını biz de doğruluyoruz
- **Hotel Detail Page:** Foto galeri, facilities, açıklama, harita linki, sticky booking sidebar
- **Affiliate model:** Detay info bizden, transaction Booking.com'da

<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 40 06" src="https://github.com/user-attachments/assets/d0f71e40-f2b3-4482-b1c9-0976f9d63698" />

### 🤖 AI Concierge (OpenAI Integration)
- **GPT-4o-mini** ile travel-focused asistan
- **System prompt engineering** ile davranış sınırı
- **Modal popup UI** + AJAX proxy pattern (API key UI'ya sızmıyor)
- **Defensive token limit** (max 500 karakter input)
- **Multi-tier error handling** (HTTP, JSON, network)

<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 43 15" src="https://github.com/user-attachments/assets/75360848-765c-45c3-aee7-b6b4c24ae49b" />

### 📊 Travel Intelligence Dashboard
Tek bir sayfada 9 farklı canlı veri:

<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 42 35" src="https://github.com/user-attachments/assets/c3c0db93-1a98-480c-8264-6a13e9e82878" />


| # | Kart | API | Veri |
|---|---|---|---|
| 1 | Weather | Open-Meteo | Anlık hava durumu |
| 2 | Currency | Currency Exchange | USD bazlı döviz kurları |
| 3 | Crypto | Coinranking | Top kripto fiyatları |
| 4 | News | Real-Time News | Güncel başlıklar |
| 5 | Movies | IMDb | Top 250 filmler |
| 6 | Quotes | Quotes API | Günün ilham sözleri |
| 7 | Air Quality | API Ninjas | EPA standartlarında AQI |
| 8 | Football | Live Football | Avrupa liglerinden maçlar |
| 9 | AI Concierge | OpenAI | Travel asistan |

<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 42 42" src="https://github.com/user-attachments/assets/355294b1-32b0-4f07-b1b9-6056068b81ba" />

### 🏨 Hotel Detail Page

Booking-com15 `getHotelDetails` endpoint'i ile zenginleştirilmiş detay sayfası:

<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 40 21" src="https://github.com/user-attachments/assets/2235ab66-b864-44f6-b36b-5699f1c1b2e9" />

- Foto galeri (Booking CDN'den high-res)
- Facilities (Wi-Fi, parking, fitness, vs.)
- Full adres + Google Maps linki
- Sticky booking sidebar
- Booking.com'a yönlendirme (search context korunarak)

### 🌍 Zenginleştirilmiş Anasayfa

<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 39 48" src="https://github.com/user-attachments/assets/e07aec1c-8c66-4866-ad3c-5306bec283bc" />

<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 40 06" src="https://github.com/user-attachments/assets/a0ea1936-cdeb-438f-902b-2a341c6b0b05" />

8 ayrı section ile Booking.com-style modern landing page:
- Hero + Search Bar
- Travel Intelligence Dashboard (9 kart)
- Popular Destinations (6 şehir)
- Featured Hotels (4 lüks otel)
- Why Voyago (4 value prop)
- Travel Stats (4 sayı)
- Trip Inspiration (4 kategori)
- Property Types (4 tip)
- Testimonials (3 müşteri yorumu)

---

## 🏗 Mimari

```
┌─────────────────────┐         ┌─────────────────────┐
│   Voyago.WebUI      │  HTTP   │   Voyago.WebAPI     │
│   (MVC + Razor)     │ ──────▶ │   (REST API)        │
│   Port: 5002        │         │   Port: 5001        │
└─────────────────────┘         └──────────┬──────────┘
                                           │
                          ┌────────────────┼────────────────┐
                          ▼                ▼                ▼
                    Booking.com         OpenAI         7 RapidAPI
                   (Hotels + Detail)   (AI Chat)      services
```

### Neden 2 ayrı proje?
- **Separation of concerns:** UI ve API tamamen ayrı
- **API key isolation:** Hiçbir 3rd party key UI'ya sızmıyor
- **Mobile-ready:** Yarın native app eklensem, aynı API'yi tüketirim
- **Microservice-friendly:** WebAPI tek başına deploy edilebilir

### Proje yapısı

```
Voyago/
├── Voyago.WebAPI/
│   ├── Controllers/       # 10 endpoint controller
│   ├── Services/          # I{X}Service + {X}Service her API için
│   ├── Dtos/              # Rapid{X}Dtos (dış) + {X}Dto (iç)
│   ├── Program.cs         # DI registration + HttpClient yapılandırma
│   └── appsettings.json   # API keys (.gitignore'da)
│
└── Voyago.WebUI/
    ├── Controllers/       # Home, Hotels, AI
    ├── ViewComponents/    # 9 dashboard kartı için
    ├── Services/          # VoyagoApiClient (tek WebAPI proxy)
    ├── Views/             # Razor pages + partials
    ├── Dtos/              # WebUI tarafı DTO'ları
    └── wwwroot/           # CSS, JS, static assets
```

---

## 🚀 Teknik Çözümler

### 📌 9 ViewComponent Pattern
Dashboard kartlarının her biri **ayrı async ViewComponent**. Sayfa yüklenmesi paralel, biri hata verirse diğerleri etkilenmiyor.

### 📌 Defensive Design
- **3rd party API filter sonuçlarını client-side doğrulama**
- **Null-safe DTO mapping** — Booking response'ları tutarsız, her field nullable
- **Multi-tier error handling** — HTTP, JSON, network, generic

### 📌 IMemoryCache Stratejisi
Hotel detail için key bazlı cache: `hotel:{id}:{date}:{guests}`. Free tier rate limit koruması + UX hızlandırma. **30 dakika TTL.**

### 📌 Prompt Engineering (OpenAI)
System prompt ile:
- Travel-focused davranış
- No markdown output (UI render kolaylığı)
- Off-topic'e kibar handling
- Honesty rule ("bilmiyorsam söylerim")

### 📌 URL State Management
Hotel filtreleri URL'de tutulur (`?minPrice=100&starRatings=5`). Bookmarkable + shareable + back button works.

### 📌 Affiliate Model
Hotel detail sayfası bizim, ama transaction Booking.com'da. Sticky booking sidebar, search context (tarih + kişi) Booking.com'a parametre olarak iletiliyor.

### 📌 Photo URL Optimization
Booking CDN URL pattern reverse-engineering: `/square60/` → `/max1024x768/`. **0 ekstra API request** ile high-res foto.

---

## 💡 Sayılarla Voyago

- **2 ayrı proje** (WebAPI + WebUI), N-Tier mimari
- **9 farklı 3rd party API** + OpenAI = 10 entegrasyon
- **9 dashboard kartı**, hepsi canlı veriyle
- **8 section'lı** Booking-style anasayfa
- **3 sayfa:** Home, Hotels listing, Hotel detail
- **Hotel arama:** Filter (price, stars, review) + pagination
- **AI Concierge:** Modal UI + AJAX + ESC/Cmd+Enter shortcuts

---

## 🛠 Kullanılan Teknolojiler

**Backend:**
- .NET 8
- ASP.NET Core MVC + Web API
- HttpClient Factory + Typed clients
- IMemoryCache
- System.Text.Json

**Frontend:**
- Razor Views + ViewComponents
- Vanilla JavaScript (no framework)
- Custom CSS (no framework — design system)
- Inline SVG (Lucide-style icons)

**External APIs:**
- Booking-com15 (RapidAPI)
- OpenAI (gpt-4o-mini)
- Open-Meteo
- Currency Exchange (RapidAPI)
- Coinranking (RapidAPI)
- Real-Time News (RapidAPI)
- IMDb (RapidAPI)
- API Ninjas Air Quality (RapidAPI)
- Free API Live Football (RapidAPI)

---

## 📸 Ekran Görüntüleri

### Hero & Search
<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 42 27" src="https://github.com/user-attachments/assets/4430d232-9f78-4fa4-9f0e-c9d96d8c0113" />

### Travel Intelligence Dashboard
<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 42 42" src="https://github.com/user-attachments/assets/0a632143-6f76-4355-9e92-eab8490cac3a" />
<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 42 35" src="https://github.com/user-attachments/assets/8b64d11c-a0b1-407d-8437-229d70329ec6" />


### Popular Destinations
<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 44 07" src="https://github.com/user-attachments/assets/3d416ea3-6941-45be-a04a-a74eda682e47" />

### Featured Hotels
<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 39 48" src="https://github.com/user-attachments/assets/9b8992d8-d2f1-4b79-8401-b71070efe7c9" />

### Hotel Search & Filters
<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 40 06" src="https://github.com/user-attachments/assets/124f38a0-cda4-4a69-bcaf-3c5dd5056580" />

### Hotel Detail Page
<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 40 21" src="https://github.com/user-attachments/assets/64105125-4c6d-4dd4-b998-43fe39f748f3" />

### AI Concierge
<img width="1494" height="836" alt="Screenshot 2026-05-19 at 14 43 15" src="https://github.com/user-attachments/assets/6f036cef-1cf3-4688-87c7-b00c7085117f" />

---

## ⚙️ Kurulum

### Gereksinimler
- .NET 8 SDK
- RapidAPI hesabı (free tier yeterli)
- OpenAI API key

### Adımlar

```bash
# 1. Clone
git clone https://github.com/ismetkerem/voyago.git
cd voyago

# 2. API keys'i yapılandır
# Voyago.WebAPI/appsettings.json oluştur (appsettings.example.json'a bak)

# 3. WebAPI başlat
cd Voyago.WebAPI
dotnet run
# Port: http://localhost:5001

# 4. WebUI başlat (yeni terminal)
cd Voyago.WebUI
dotnet run
# Port: http://localhost:5002

# 5. Tarayıcıda aç
# http://localhost:5002
```

### appsettings.json örneği

```json
{
  "RapidApi": { "Key": "YOUR_RAPIDAPI_KEY" },
  "OpenAi": { "ApiKey": "YOUR_OPENAI_KEY" }
}
```

### RapidAPI Subscriptions (free tier)
Aşağıdaki API'lere subscribe ol (hepsi ücretsiz plan ile):

- [Booking-com15](https://rapidapi.com/DataCrawler/api/booking-com15)
- [Currency Conversion](https://rapidapi.com/natkapral/api/currency-conversion-and-exchange-rates)
- [Coinranking1](https://rapidapi.com/Coinranking/api/coinranking1)
- [Real-Time News](https://rapidapi.com/letscrape-6bRBa3QguO5/api/real-time-news-data)
- [IMDb236](https://rapidapi.com/Glavier/api/imdb236)
- [Air Quality](https://rapidapi.com/apininjas/api/air-quality-by-api-ninjas)
- [Free API Live Football](https://rapidapi.com/letscrape-6bRBa3QguO5/api/free-api-live-football-data)

---

## 🎓 Öğrenilenler

Bu projede üzerinde durduğum konular:

- **3rd party API entegrasyonu** — 9 farklı API'nin tamamen farklı response yapısı ile çalışma
- **Defensive design** — null safety, graceful degradation, multi-tier error handling
- **Prompt engineering** — LLM davranışını system prompt ile şekillendirme
- **Cache strategy** — IMemoryCache ile rate limit yönetimi
- **API key isolation** — server-side proxy pattern ile credential güvenliği
- **UX patterns** — Modal, sticky sidebar, URL state, defensive UI
- **CDN URL reverse engineering** — Photo URL pattern manipülasyonu ile network optimization

---

## 🙏 Teşekkürler

Bu süreçteki vizyoner rehberliği için değerli hocam **[Murat Yücedağ](https://www.linkedin.com/in/muratyucedag/)**'a sonsuz teşekkürler.

[M&Y Yazılım Eğitim Akademi Danışmanlık](https://www.linkedin.com/company/m-y-yaz%C4%B1l%C4%B1m-e%C4%9Fitim-akademi-dan%C4%B1%C5%9Fmanl%C4%B1k/) bünyesinde tamamladığım **6. Full Stack .NET** projesidir.

---

## 📄 Lisans

MIT License — özgürce kullanabilirsiniz.

---

⭐ Beğendiyseniz star vermeyi unutmayın!
