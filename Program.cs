using System.Text.Json.Serialization;
using Dmlcafepos.Data.Concrete.EfCoreIdentity;
using Dmlcafepos.Entities;
using Dmlcafepos.Models;
using DmlCafePos.Data.Abstract;
using DmlCafePos.Data.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
       .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        });

// builder.Services.AddDbContext<EfPosContext>(options =>{
//     var config = builder.Configuration;
//     var connectionString = config.GetConnectionString("Default");
//     options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));
// });

builder.Services.AddDbContext<EfPosContext>(options => {
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("Default");
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IAreaRepository,AreaRepository>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<ITableRepository,TableRepository>();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<ICustomerRepository,CustomerRepository>();
builder.Services.AddScoped<IPaymentRepository,PaymentRepository>();

builder.Services.AddSingleton<string>("bunedir");
builder.Services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<EfPosContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options => 
{
    options.LoginPath = "/AuthLog/Login";
    options.AccessDeniedPath = "/AuthLog/DeniedPath";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(15);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=Orders}/{id?}");

// IdentitySeedData.IdentityTestUser(app);

app.Run();
