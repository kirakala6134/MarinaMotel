using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarinaMotel.Data;
using MarinaMotel.Models;

namespace MarinaMotel.Controllers;

[Authorize]
public class YorumlarController : Controller
{
    private readonly ApplicationDbContext _db;
    public YorumlarController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Yonet()
    {
        var yorumlar = await _db.Yorumlar.ToListAsync();
        return View(yorumlar);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(string misafirAdi, int puan, string metin)
    {
        _db.Yorumlar.Add(new Yorum { MisafirAdi = misafirAdi, Puan = puan, Metin = metin });
        await _db.SaveChangesAsync();
        return RedirectToAction("Yonet");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var yorum = await _db.Yorumlar.FindAsync(id);
        if (yorum != null)
        {
            _db.Yorumlar.Remove(yorum);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Yonet");
    }
}
