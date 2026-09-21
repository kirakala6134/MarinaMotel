using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MarinaMotel.Models;

namespace MarinaMotel.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Oda> Odalar => Set<Oda>();
    public DbSet<Konaklama> Konaklamalar => Set<Konaklama>();
    public DbSet<RezervasyonTalebi> RezervasyonTalepleri => Set<RezervasyonTalebi>();
    public DbSet<IsGunu> IsGunleri => Set<IsGunu>();
    public DbSet<Hatirlatma> Hatirlatmalar => Set<Hatirlatma>();
    public DbSet<Yorum> Yorumlar => Set<Yorum>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Oda>().HasIndex(o => o.OdaNo).IsUnique();

        // Geçmiş konaklama kayıtları, oda silinse/değişse bile bozulmasın.
        builder.Entity<Konaklama>()
            .HasOne(k => k.Oda)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }

    // 2026 yazı için başlangıç fiyatları — Armutlu/Fıstıklı bölgesindeki
    // benzer küçük otellerin gecelik fiyatlarına bakarak koydum (bkz. sohbet).
    // Bu bir tahmin/başlangıç noktası, gerçek talebe göre Oda Yönetimi'nden
    // (ileride eklenebilir) güncellemen gerekecek.
    public static void OrnekVeriyiOlustur(ApplicationDbContext db)
    {
        if (!db.Odalar.Any())
        {
            for (int i = 1; i <= 12; i++)
            {
                var denizManzarali = i % 3 == 0; // her 3 odadan biri deniz manzaralı
                db.Odalar.Add(new Oda
                {
                    OdaNo = i,
                    Tip = denizManzarali ? "Deniz Manzaralı" : "Standart",
                    GecelikFiyat = denizManzarali ? 3800 : 2800,
                    Durum = OdaDurumu.Bos
                });
            }
            db.SaveChanges();
        }

        if (!db.Yorumlar.Any())
        {
            db.Yorumlar.AddRange(
                new Yorum { MisafirAdi = "Elif K.", Puan = 5, Metin = "Deniz manzaralı odamız harikaydı, sabah kahvaltısını terasta güneşi izleyerek yaptık. Kesinlikle tekrar geliriz." },
                new Yorum { MisafirAdi = "Mert Y.", Puan = 5, Metin = "Personel çok ilgiliydi, oda tertemizdi. Fıstıklı'ya bu kadar yakın, sakin bir yer bulmak zordu." },
                new Yorum { MisafirAdi = "Aylin S.", Puan = 4, Metin = "Havuz ve deniz manzarası gerçekten güzel, fiyat/performans olarak bölgede iyi bir seçenek." }
            );
            db.SaveChanges();
        }
    }

    public static IsGunu AktifGunuGetirVeyaOlustur(ApplicationDbContext db)
    {
        var aktif = db.IsGunleri.FirstOrDefault(g => g.BitisZamani == null);
        if (aktif != null) return aktif;

        aktif = new IsGunu { BaslangicZamani = DateTime.Now };
        db.IsGunleri.Add(aktif);
        db.SaveChanges();
        return aktif;
    }
}
