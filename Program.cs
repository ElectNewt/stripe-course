using CourseStripe.Data;
using CourseStripe.Data.InMemory;
using CourseStripe.UseCases.Products;
using CourseStripe.UseCases.ShoppingCart;
using CourseStripe.UseCases.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


builder.Services
    .AddScoped<AddProduct>()
    .AddScoped<GetProducts>()
    .AddSingleton<IInMemoryShoppingCart, InMemoryShoppingCart>()
    .AddScoped<GetShoppingCart>()
    .AddScoped<AddToShoppingCart>()
    .AddScoped<SetPremium>()
    .AddScoped<RemovePremium>()
    .AddScoped<CancelSubscription>()
    .AddScoped<SetPremiumEnd>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}


StripeConfiguration.ApiKey = app.Configuration["StripeApiKey"];

app.Run();
