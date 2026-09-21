namespace MarinaMotel.Models;

public class Yorum
{
    public int Id { get; set; }
    public string MisafirAdi { get; set; } = string.Empty;
    public int Puan { get; set; } = 5; // 1-5
    public string Metin { get; set; } = string.Empty;
}
