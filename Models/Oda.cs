namespace MarinaMotel.Models;

public enum OdaDurumu
{
    Bos,
    Dolu,
    TemizlikBekliyor
}

public class Oda
{
    public int Id { get; set; }
    public int OdaNo { get; set; }
    public string Tip { get; set; } = string.Empty; // "Standart", "Deniz Manzaralı" gibi
    public decimal GecelikFiyat { get; set; }
    public OdaDurumu Durum { get; set; } = OdaDurumu.Bos;
}
