using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using App.BLL;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.EF;
using App.Domain.Identity;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApp;
using WebApp.Helpers;
using AutoMapperProfile = WebApp.Helpers.AutoMapperProfile;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IAppUnitOfWork, AppUOW>();
builder.Services.AddScoped<IAppBLL, AppBLL>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services
    .AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
    .AddDefaultUI()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// clear default claims
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services
    .AddAuthentication()
    .AddCookie(options => { options.SlidingExpiration = true; })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = builder.Configuration.GetValue<string>("JWT:issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("JWT:audience"),
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration.GetValue<string>("JWT:key")
                    )
                ),
            ClockSkew = TimeSpan.Zero,
        };
    });


builder.Services.AddControllersWithViews(
    options => { options.ModelBinderProviders.Insert(0, new CustomLangStrBinderProvider()); }
).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.AllowTrailingCommas = true;
});


var supportedCultures = builder.Configuration
    .GetSection("SupportedCultures")
    .GetChildren()
    .Select(x => new CultureInfo(x.Value))
    .ToArray();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // datetime and currency support
    options.SupportedCultures = supportedCultures;
    // UI translated strings
    options.SupportedUICultures = supportedCultures;
    // if nothing is found, use this
    // TODO: why does it fall back to et-EE, even if default is something else
    options.DefaultRequestCulture =
        new RequestCulture(
            builder.Configuration["DefaultCulture"],
            builder.Configuration["DefaultCulture"]);
    options.SetDefaultCulture(builder.Configuration["DefaultCulture"]);

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        // Order is important, its in which order they will be evaluated
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsAllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});


// reference any class from class library to be scanned for mapper configurations
builder.Services.AddAutoMapper(
    typeof(App.DAL.EF.AutoMapperProfile),
    typeof(App.BLL.AutoMapperProfile),
    typeof(AutoMapperProfile)
);

var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});
apiVersioningBuilder.AddApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

// configure low level translations
builder.Services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureModelBindingLocalization>();


// ===================================================
var app = builder.Build();
// ===================================================

SetupAppData(app);


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

app.UseCors("CorsAllowAll");

app.UseRequestLocalization(options:
    app.Services.GetService<IOptions<RequestLocalizationOptions>>()?.Value!
);

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant()
        );
    }
    // serve from root
    // options.RoutePrefix = string.Empty;
});

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();


static void SetupAppData(WebApplication app)
{
    using var serviceScope = ((IApplicationBuilder) app).ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();
    using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Database.ProviderName!.Contains("InMemory"))
    {
        context.Database.Migrate();
    }

    using var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    using var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    var res = roleManager.CreateAsync(new AppRole()
    {
        Name = "Admin"
    }).Result;

    if (!res.Succeeded)
    {
        Console.WriteLine(res.ToString());
    }

    var user = new AppUser()
    {
        Email = "admin@eesti.ee",
        UserName = "admin@eesti.ee",
        FirstName = "Admin",
        LastName = "Eesti"
    };
    res = userManager.CreateAsync(user, "Kala.maja1").Result;
    if (!res.Succeeded)
    {
        Console.WriteLine(res.ToString());
    }

    res = userManager.AddToRoleAsync(user, "Admin").Result;
    if (!res.Succeeded)
    {
        Console.WriteLine(res.ToString());
    }
}

// needed for unit testing, to change generated top level statement class to public
public partial class Program
{
}