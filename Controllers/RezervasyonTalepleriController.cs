using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarinaMotel.Data;

namespace MarinaMotel.Controllers;

[Authorize]
public class RezervasyonTalepleriController : Controller
{
    private readonly ApplicationDbContext _db;
    public RezervasyonTalepleriController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var talepler = await _db.RezervasyonTalepleri
            .OrderBy(t => t.Islendi)
            .ThenBy(t => t.GirisTarihi)
            .ToListAsync();
        return View(talepler);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IslendiIsaretle(int id)
    {
        var talep = await _db.RezervasyonTalepleri.FindAsync(id);
        if (talep != null)
        {
            talep.Islendi = !talep.Islendi;
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var talep = await _db.RezervasyonTalepleri.FindAsync(id);
        if (talep != null)
        {
            _db.RezervasyonTalepleri.Remove(talep);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
