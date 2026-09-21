using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarinaMotel.Data;
using MarinaMotel.Models;

namespace MarinaMotel.Controllers;

public class KasaGorunumu
{
    public decimal AnaToplam { get; set; }
    public decimal GunlukToplam { get; set; }
    public DateTime AktifGunBaslangici { get; set; }
    public List<Hatirlatma> YaklasanHatirlatmalar { get; set; } = new();
}

[Authorize]
public class KasaController : Controller
{
    private readonly ApplicationDbContext _db;
    public KasaController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var aktifGun = ApplicationDbContext.AktifGunuGetirVeyaOlustur(_db);

        // SQLite decimal Sum'ı desteklemediği için önce çekip bellekte topluyoruz.
        var kapaliKonaklamalar = await _db.Konaklamalar
            .Where(k => k.Kapali)
            .ToListAsync();

        var anaToplam = kapaliKonaklamalar.Sum(k => k.ToplamTutar);
        var gunlukToplam = kapaliKonaklamalar.Where(k => k.IsGunuId == aktifGun.Id).Sum(k => k.ToplamTutar);

        return View(new KasaGorunumu
        {
            AnaToplam = anaToplam,
            GunlukToplam = gunlukToplam,
            AktifGunBaslangici = aktifGun.BaslangicZamani,
            YaklasanHatirlatmalar = await _db.Hatirlatmalar
                .Where(h => !h.TamamlandiMi)
                .OrderBy(h => h.Tarih)
                .Take(3)
                .ToListAsync()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult GunuBitir()
    {
        var aktifGun = ApplicationDbContext.AktifGunuGetirVeyaOlustur(_db);
        aktifGun.BitisZamani = DateTime.Now;
        _db.SaveChanges();

        ApplicationDbContext.AktifGunuGetirVeyaOlustur(_db);
        return RedirectToAction("Index");
    }
}
