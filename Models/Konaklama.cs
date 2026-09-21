namespace MarinaMotel.Models;

// Bir odanın check-in'den check-out'a kadarki tek bir konaklama kaydı.
// Kafedeki "Adisyon"un otel karşılığı — tek fark, ürün ürün ekleme yerine
// gece sayısına göre otomatik hesaplama + tek kalemlik bir ekstra ücret var.
public class Konaklama
{
    public int Id { get; set; }

    public int OdaId { get; set; }
    public Oda? Oda { get; set; }

    public string MisafirAdi { get; set; } = string.Empty;
    public string? Telefon { get; set; }
    public int YetiskinSayisi { get; set; } = 2;
    public int CocukSayisi { get; set; }

    public DateTime GirisTarihi { get; set; }
    public DateTime CikisTarihi { get; set; }

    // Check-in anındaki, kişi sayısına göre ayarlanmış gecelik fiyat (oda
    // fiyatı sonradan değişse bile bu konaklamanın tutarı bozulmasın diye
    // ayrıca saklanıyor — bkz. OdalarController.CheckIn).
    public decimal GecelikFiyat { get; set; }

    // Minibar, ekstra kahvaltı gibi tek kalemlik bir ek ücret (opsiyonel).
    public decimal EkstraTutar { get; set; }
    public string? EkstraAciklama { get; set; }

    public bool Kapali { get; set; } // check-out yapıldı mı
    public DateTime? KapanisZamani { get; set; }

    public int? IsGunuId { get; set; }
    public IsGunu? IsGunu { get; set; }

    // Kaç gece kaldığını hesaplar (en az 1 gece).
    public int GeceSayisi => Math.Max(1, (CikisTarihi.Date - GirisTarihi.Date).Days);
    public decimal ToplamTutar => (GeceSayisi * GecelikFiyat) + EkstraTutar;
}
