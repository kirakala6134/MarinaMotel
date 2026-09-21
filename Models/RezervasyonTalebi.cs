namespace MarinaMotel.Models;

// Halka açık siteden "Rezervasyon Al" formuyla gelen talep. Otomatik bir
// oda ataması yapmıyoruz — resepsiyon bunu görüp telefonla teyit eder,
// sonra Odalar ekranından check-in yaparak gerçek konaklamaya çevirir.
public class RezervasyonTalebi
{
    public int Id { get; set; }
    public string AdSoyad { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public DateTime GirisTarihi { get; set; }
    public DateTime CikisTarihi { get; set; }
    public string OdaTipi { get; set; } = string.Empty;
    public int YetiskinSayisi { get; set; } = 2;
    public int CocukSayisi { get; set; }
    public string? Not { get; set; }
    public DateTime OlusturmaZamani { get; set; }
    public bool Islendi { get; set; }
}
