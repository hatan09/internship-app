using System.Net;
using System.Net.Mail;
using System.Text;
using Blazored.LocalStorage;
using InternshipApp.Api.AppsettingConfig;
using InternshipApp.Contracts;
using InternshipApp.Core.Database;
using InternshipApp.Core.Entities;
using InternshipApp.Portal.WebApi;
using InternshipApp.Repository;
using InternshipApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.IdentityModel.Tokens;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"))
    .Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));

//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTMwMzc2M0AzMjMwMmUzNDJlMzBXTGVIY3JZS2NSaE81KzluWjBmQnhQeEcvRDZKTCtSTC9UZlBHZGdndzlBPQ==");

//Local Storage
builder.Services.AddBlazoredLocalStorage();

//Add database
builder.Services
    .AddDbContextPool<AppDbContext>(options => options.UseSqlServer(builder.Configuration
                                                        .GetConnectionString("LocalConnection"))
                                                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                                        .EnableSensitiveDataLogging(true));

//UserIdentity
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;

    //options.User.RequireUniqueEmail = true; //default false
    //options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddUserManager<UserManager>()
    .AddDefaultTokenProviders();

//Sub-user
builder.Services.AddIdentityCore<Student>()
    .AddRoles<Role>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Student, Role>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddUserManager<StudentManager>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityCore<Instructor>()
    .AddRoles<Role>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Instructor, Role>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddUserManager<InstructorManager>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityCore<Recruiter>()
    .AddRoles<Role>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Recruiter, Role>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddUserManager<RecruiterManager>()
    .AddDefaultTokenProviders();

//JWT
var key = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
var secretKey = Encoding.UTF8.GetBytes(key.JWT_Secret);

//Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

//IServiceCollections
builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();
builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IInternGroupRepository, InternGroupRepository>();
builder.Services.AddTransient<ISkillRepository, SkillRepository>();
builder.Services.AddTransient<IJobRepository, JobRepository>();
builder.Services.AddTransient<IEvaluationRepository, EvaluationRepository>();
builder.Services.AddTransient<IConversationRepository, ConversationRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<ISkillScoreRepository, SkillScoreRepository>();
builder.Services.AddTransient<IInternSettingsRepository, InternSettingsRepository>();
builder.Services.AddTransient<IMatchingService, MatchingService>();
builder.Services.AddTransient<IInternGroupServices, InternGroupServices>();
builder.Services.AddTransient<IStudentFormRepository, StudentFormRepository>();
builder.Services.AddTransient<ILabourMarketFormRepository, LabourMarketFormRepository>();

//HttpClients
builder.Services.AddHttpClient();
builder.Services.AddTransient<AuthClient>();
builder.Services.AddTransient<StudentHttpClient>();
builder.Services.AddTransient<InstructorHttpClient>();
builder.Services.AddTransient<RecruiterHttpClient>();
builder.Services.AddTransient<AdminHttpClient>();

//Email
builder.Services.AddScoped(provider =>
{
    var config = provider.GetRequiredService<IOptionsMonitor<EmailConfig>>().CurrentValue;
    SmtpClient client = new(config.Host, config.Port)
    {
        EnableSsl = config.EnableSsl,
        UseDefaultCredentials = config.UseDefaultCredentials,
        Credentials = new NetworkCredential(config.UserName, config.Password)
    };

    return client;
});
builder.Services.AddScoped<IEmailService, EmailService>();

//Authorization

builder.Services.AddFluentUIComponents();
builder.Services.AddSyncfusionBlazor();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
