using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add EF Core with SQLite
builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseSqlite("Data Source=bookstore.db"));

// Add ASP.NET Core Identity with strong password policy
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<BookStoreContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Seed admin role, user, and sample books
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var db = services.GetRequiredService<BookStoreContext>();
    string adminRole = "Admin";
    string adminEmail = "admin@bookstore.com";
    string adminPassword = "Admin123!";
    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
    else if (!await userManager.IsInRoleAsync(adminUser, adminRole))
    {
        await userManager.AddToRoleAsync(adminUser, adminRole);
    }
    // Seed sample books if none exist
    if (!db.Books.Any())
    {
        db.Books.AddRange(new[]
        {
            new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Price = 15.99M, Genre = "Classic", PublishDate = new DateTime(1925, 4, 10), Description = "A novel set in the Roaring Twenties." },
            new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Price = 12.99M, Genre = "Classic", PublishDate = new DateTime(1960, 7, 11), Description = "A story of racial injustice in the Deep South." },
            new Book { Title = "1984", Author = "George Orwell", Price = 14.99M, Genre = "Dystopian", PublishDate = new DateTime(1949, 6, 8), Description = "A dystopian novel about totalitarianism." },
            new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Price = 10.99M, Genre = "Fantasy", PublishDate = new DateTime(1937, 9, 21), Description = "A fantasy adventure preceding The Lord of the Rings." }
        });
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
