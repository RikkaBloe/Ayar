public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        // DbContext için veritabanı bağlantı dizesi ve sağlayıcısı
        services.AddDbContext<ProjectContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

        // Identity ayarları
        services.AddIdentity<AppUser, IdentityRole>(x =>
        {
            x.User.RequireUniqueEmail = true;
            x.Password.RequiredLength = 8; // Şifre uzunluğunu 
            x.Password.RequireLowercase = true; // Küçük harf zorunlu
            x.Password.RequireUppercase = true; // Büyük harf zorunlu
            x.Password.RequireDigit = true; // Rakam zorunlu
            x.Password.RequireNonAlphanumeric = true; // 
            x.Password.RequiredUniqueChars = 3;  // En az 3 farklı karakter türü 
        }).AddEntityFrameworkStores<ProjectContext>()
          .AddDefaultTokenProviders();

        // AutoMapper
        services.AddAutoMapper(typeof(Mappers));

        // Scoped Repository Servisleri
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "area",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
