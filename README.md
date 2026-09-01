
  Zindan Gezgini, C# dili kullanılarak geliştirilmiş, terminal üzerinden oynanan, prosedürel olarak üretilen haritalara
  sahip bir Rogue-like konsol oyunudur. Oyuncular, rastgele oluşturulan tehlikeli bir zindanda hayatta kalmaya
  çalışırken düşmanlarla savaşır ve çıkış kapısını aramaktadır.

  🚀 Özellikler

  - Prosedürel Harita Üretimi: Her yeni oyunda duvarların ve düşmanların yerleri rastgele belirlenir, böylece her
    deneyim farklıdır.
  - Savaş Sistemi: Düşmanlarla karşılaşıldığında otomatik olarak devreye giren, can (HP) ve saldırı gücü tabanlı bir
    savaş mekaniği.
  - Gelişim ve Seviye Sistemi: Yendiğiniz her düşman size tecrübe puanı (TP) kazandırır. Yeterli TP topladığınızda
    seviye atlar, saldırı gücünüz artar ve canınız yenilenir.
  - Envanter Yönetimi: Hayatta kalmak için sınırlı sayıdaki iyileştirme iksirlerini stratejik olarak kullanma
    zorunluluğu.

  🎮 Nasıl Oynanır?

  Kontroller

  ┌─────┬────────────────────────────────────────┐
  │ Tuş │                 Eylem                  │
  ├─────┼────────────────────────────────────────┤
  │ W   │ Yukarı Hareket Et                      │
  ├─────┼────────────────────────────────────────┤
  │ A   │ Sola Hareket Et                        │
  ├─────┼────────────────────────────────────────┤
  │ S   │ Aşağı Hareket Et                       │
  ├─────┼────────────────────────────────────────┤
  │ D   │ Sağa Hareket Et                        │
  ├─────┼────────────────────────────────────────┤
  │ H   │ İksir Kullan (Can Yenile)              │
  ├─────┼────────────────────────────────────────┤
  │ E   │ Çıkış Kapısından Ayrıl (Kazanmak için) │
  └─────┴────────────────────────────────────────┘

  Harita Sembolleri

  - @ (Mavi) : Siz (Kahraman)
  - G (Kırmızı) : Goblin (Düşman)
  - # : Duvar (Geçilemez Engel)
  - . : Boş Yol
  - E : Çıkış Kapısı (Exit)

  🛠️ Kurulum ve Çalıştırma

  Bu projeyi çalıştırmak için bilgisayarınızda .NET SDK yüklü olmalıdır.

  1. Projeyi Klonlayın veya Dosyaları Alın:
  git clone https://github.com/kullaniciadin/zindan-gezgini.git
  cd zindan-gezgini
  2. Projeyi Derleyin ve Çalıştırın:
  dotnet run

  💻 Teknik Detaylar

  - Dil: C#
  - Platform: .NET Core / .NET 5+
  - Yazılım Desenleri: Kalıtım (Inheritance) kullanılarak Entity sınıfı üzerinden Player ve Enemy sınıfları
    türetilmiştir.
  - Algoritmalar: Rastgele sayı üretimi (Random class) ile dinamik harita ve düşman yerleştirme algoritması
    uygulanmıştır.

  📜 Lisans

  Bu proje MIT Lisansı ile lisanslanmıştır.

  ---

  ✍️ Geliştirici

  [Senin Adın/Kullanıcı Adın]
  C# ile basit ama etkili oyun mekanikleri üzerine çalışmalar yapıyorum.
