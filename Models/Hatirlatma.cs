namespace MarinaMotel.Models;

public class Hatirlatma
{
    public int Id { get; set; }
    public DateTime Tarih { get; set; }
    public string Baslik { get; set; } = string.Empty;
    public string? Not { get; set; }
    public bool TamamlandiMi { get; set; }
}
