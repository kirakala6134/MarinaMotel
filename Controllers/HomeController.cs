using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarinaMotel.Data;
using MarinaMotel.Models;

namespace MarinaMotel.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    public HomeController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var yorumlar = await _db.Yorumlar.ToListAsync();

        // Aynı tipten birden fazla oda varsa o tipin fiyatını temsilen ilkini alıyoruz.
        var odalar = await _db.Odalar.ToListAsync();
        ViewBag.OdaTipleri = odalar.Select(o => o.Tip).Distinct().ToList();
        ViewBag.OdaFiyatlari = odalar
            .GroupBy(o => o.Tip)
            .ToDictionary(g => g.Key, g => g.First().GecelikFiyat);

        return View(yorumlar);
    }

    // Sitedeki "Rezervasyon Al" formundan gelen talep — otomatik oda ataması
    // yapmıyor, resepsiyon RezervasyonTalepleri ekranından görüp arar.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RezervasyonGonder(string adSoyad, string telefon, DateTime girisTarihi, DateTime cikisTarihi, string odaTipi, int yetiskinSayisi, int cocukSayisi, string? not)
    {
        _db.RezervasyonTalepleri.Add(new RezervasyonTalebi
        {
            AdSoyad = adSoyad,
            Telefon = telefon,
            GirisTarihi = girisTarihi,
            CikisTarihi = cikisTarihi,
            OdaTipi = odaTipi,
            YetiskinSayisi = yetiskinSayisi,
            CocukSayisi = cocukSayisi,
            Not = not,
            OlusturmaZamani = DateTime.Now,
            Islendi = false
        });
        await _db.SaveChangesAsync();

        TempData["RezervasyonBasarili"] = true;
        return Redirect(Url.Action("Index") + "#rezervasyon");
    }
}
