
using Ecomm_Project_2003.DataAccess.Data;
using Ecomm_Project_2003.DataAccess.Repository;                        // This is a main file of a project and automatically Generate File.
using Ecomm_Project_2003.DataAccess.Repository.IRepository;
using Ecomm_Project_2003.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("conStr");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// identity work na kare to.
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<IdentityUser,IdentityRole>().
    AddDefaultTokenProviders() .AddEntityFrameworkStores<ApplicationDbContext>();   
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();  // Than add two file .
//builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();       // Manditory ha likhna.
//builder.Services.AddScoped<ICoverTypeRepository, CoverTypeRepository>();     // Manditory ha likhna.
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Login Logout Seccion.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
// it is a Open Authentication.
//builder.Services.AddAuthentication().AddFacebook(options =>
//{
//    options.AppId = "";
//    options.AppSecret = "";

//});
 builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "146352557548-cc6salf5pt5rms7vutgrhhpu71p6erpm.apps.googleusercontent.com";
    options.ClientSecret = "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";

});
//This Code Is Configration
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(10);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
// This Code A Payment Setting
builder.Services.Configure<StripeSettings>
    (builder.Configuration.GetSection("StripeSettings"));
// Email Message Sender
builder.Services.Configure<EmailSettings>
    (builder.Configuration.GetSection("EmailSettings"));
// SMS Sender
builder.Services.Configure<SMSSettings>
    (builder.Configuration.GetSection("SMSSettings"));


// Middle Wear.
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
StripeConfiguration.ApiKey = builder.Configuration.
    GetSection("StripeSettings")["SecretKey"];
app.UseSession(); // ADD seccion.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
