using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarinaMotel.Data;
using MarinaMotel.Models;

namespace MarinaMotel.Controllers;

[Authorize]
public class OdalarController : Controller
{
    private readonly ApplicationDbContext _db;
    public OdalarController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var odalar = await _db.Odalar.OrderBy(o => o.OdaNo).ToListAsync();
        return View(odalar);
    }

    public async Task<IActionResult> Detay(int odaNo)
    {
        var oda = await _db.Odalar.FirstAsync(o => o.OdaNo == odaNo);
        var konaklama = await _db.Konaklamalar
            .Where(k => k.OdaId == oda.Id && !k.Kapali)
            .FirstOrDefaultAsync();

        ViewBag.Oda = oda;
        return View(konaklama); // Oda boşsa konaklama null gelir, view check-in formu gösterir
    }

    // Boş bir odaya yeni misafir giriş yapar. Fiyat, sitedeki rezervasyon
    // formuyla aynı kuralla hesaplanıyor: gecelik fiyat 2 yetişkini kapsar,
    // fazla her yetişkin için %25, her çocuk için %15 ek yapılır.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckIn(int odaNo, string misafirAdi, string? telefon, DateTime girisTarihi, DateTime cikisTarihi, int yetiskinSayisi, int cocukSayisi)
    {
        var oda = await _db.Odalar.FirstAsync(o => o.OdaNo == odaNo);
        var aktifGun = ApplicationDbContext.AktifGunuGetirVeyaOlustur(_db);

        var ekstraYetiskin = Math.Max(0, yetiskinSayisi - 2);
        var efektifGecelikFiyat = oda.GecelikFiyat
            * (1 + (0.25m * ekstraYetiskin) + (0.15m * cocukSayisi));

        _db.Konaklamalar.Add(new Konaklama
        {
            OdaId = oda.Id,
            MisafirAdi = misafirAdi,
            Telefon = telefon,
            YetiskinSayisi = yetiskinSayisi,
            CocukSayisi = cocukSayisi,
            GirisTarihi = girisTarihi,
            CikisTarihi = cikisTarihi,
            GecelikFiyat = efektifGecelikFiyat,
            Kapali = false,
            IsGunuId = aktifGun.Id
        });
        oda.Durum = OdaDurumu.Dolu;
        await _db.SaveChangesAsync();

        return RedirectToAction("Detay", new { odaNo });
    }

    // Ek ücret (minibar, ekstra kahvaltı vs.) eklemek için.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EkstraEkle(int odaNo, decimal ekstraTutar, string? ekstraAciklama)
    {
        var oda = await _db.Odalar.FirstAsync(o => o.OdaNo == odaNo);
        var konaklama = await _db.Konaklamalar.FirstOrDefaultAsync(k => k.OdaId == oda.Id && !k.Kapali);
        if (konaklama != null)
        {
            konaklama.EkstraTutar += ekstraTutar;
            konaklama.EkstraAciklama = string.IsNullOrWhiteSpace(konaklama.EkstraAciklama)
                ? ekstraAciklama
                : $"{konaklama.EkstraAciklama}, {ekstraAciklama}";
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Detay", new { odaNo });
    }

    // Check-out: konaklamayı kapatır, oda "Temizlik Bekliyor" durumuna geçer.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut(int odaNo)
    {
        var oda = await _db.Odalar.FirstAsync(o => o.OdaNo == odaNo);
        var konaklama = await _db.Konaklamalar.FirstOrDefaultAsync(k => k.OdaId == oda.Id && !k.Kapali);

        if (konaklama != null)
        {
            konaklama.Kapali = true;
            konaklama.KapanisZamani = DateTime.Now;
            oda.Durum = OdaDurumu.TemizlikBekliyor;
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    // Temizlik bittiğinde oda tekrar "Boş" durumuna alınır.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TemizlikTamamlandi(int odaNo)
    {
        var oda = await _db.Odalar.FirstAsync(o => o.OdaNo == odaNo);
        oda.Durum = OdaDurumu.Bos;
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
