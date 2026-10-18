# Marina Otel
Küçük bir otel için hazırlanmış tanıtım web sitesi ve oda yönetim paneli uygulamasıdır. Lost Heaven Cafe projesindeki yapı, bir otel işletmesinin ihtiyaçlarına göre uyarlanmıştır.

Özellikler

Tanıtım sitesi: Hakkımızda, imkânlar, konum ve misafir yorumları bölümleri bulunur.
Rezervasyon talep formunda giriş-çıkış tarihi, oda tipi, yetişkin ve çocuk sayısı seçilir. Seçime göre tahmini tutar formda anlık gösterilir.
Formdan gelen talepler otomatik rezervasyon sayılmaz, otel personeli tarafından değerlendirilir.

Oda yönetimi: Tüm odalar tek ekranda listelenir. Her oda boş, dolu veya temizlik bekliyor olarak gösterilir.
Misafir girişinde ad, telefon, tarihler ve kişi sayısı kaydedilir.
Tutar; gece sayısına, yetişkin ve çocuk sayısına göre otomatik hesaplanır. Minibar gibi ek ücretler eklenebilir.
Çıkış yapıldığında oda "temizlik bekliyor" durumuna geçer, temizlik tamamlanınca tekrar boşa alınır.

Rezervasyon talepleri: Siteden gelen talepler listelenir; kişi sayısı, tarih ve notlar görüntülenir.
Değerlendirilen talepler "işlendi" olarak işaretlenir.

Kasa: Günlük toplam ve genel toplam ayrı gösterilir.
"Günü Bitir" işlemi yalnızca günlük toplamı sıfırlar.

Hatırlatmalar ve yorumlar: Tarihli notlar ve hatırlatmalar tutulur.
Sitede görünen misafir yorumları panelden eklenir ve silinir.

Diğer: Personel paneli yalnızca giriş yapmış kullanıcılara açıktır.
Tüm veriler veritabanında kalıcı olarak saklanır.
Fiyatlandırma

Örnek veriler şunlardır: Standart oda 2.800 ₺, Deniz Manzaralı oda 3.800 ₺ (gecelik). Gecelik fiyat iki yetişkini kapsar. İkiden fazla her yetişkin için %25, her çocuk için %15 eklenir. Bu değerler örnek olarak belirlenmiştir.

Kullanılan teknolojiler
Sunucu tarafı: C#, ASP.NET Core MVC (.NET 8)
Veritabanı: SQLite, Entity Framework Core
Giriş ve kayıt: ASP.NET Core Identity
Arayüz: HTML, CSS, JavaScript
Not

Proje geliştirilirken yapay zekâ araçlarından yararlanılmıştır.
