using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FootballManagerMVC.Data;
using FootballManagerMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=footballmanager.db"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    context.Database.EnsureCreated();
    
    // Create roles
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await roleManager.RoleExistsAsync("Customer"))
    {
        await roleManager.CreateAsync(new IdentityRole("Customer"));
    }
    
    // Create admin user
    if (await userManager.FindByEmailAsync("admin@footballmanager.vn") == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "admin@footballmanager.vn",
            Email = "admin@footballmanager.vn",
            Name = "Administrator",
            Phone = "0123456789",
            Role = "admin",
            EmailConfirmed = true
        };
        
        await userManager.CreateAsync(adminUser, "Admin123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
    
    // Create demo customer user
    if (await userManager.FindByEmailAsync("customer@demo.com") == null)
    {
        var customerUser = new ApplicationUser
        {
            UserName = "vanhaovx@gmail.com",
            Email = "vanhaovx@gmail.com",
            Name = "Nguyễn Văn Hảo",
            Phone = "0848123439",
            Role = "customer",
            EmailConfirmed = true
        };
        
        await userManager.CreateAsync(customerUser, "Hao@123456");
        await userManager.AddToRoleAsync(customerUser, "Customer");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();


app.Run();
