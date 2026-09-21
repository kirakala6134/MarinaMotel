using Microsoft.AspNetCore.Identity;

namespace MarinaMotel.Models;

public class ApplicationUser : IdentityUser
{
    public string? AdSoyad { get; set; }
}
