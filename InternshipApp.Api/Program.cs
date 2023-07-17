using System.Net;
using System.Net.Mail;
using System.Text;
using AutoMapper;
using InternshipApp.Api.AppsettingConfig;
using InternshipApp.Api.DataObjects;
using InternshipApp.Contracts;
using InternshipApp.Core.Database;
using InternshipApp.Core.Entities;
using InternshipApp.Hubs;
using InternshipApp.Repository;
using InternshipApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//NewtonsoftJson

//Config
builder.Services
    .Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"))
    .Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));

//Cors
builder.Services.AddCors(p => p.AddPolicy("AllowAnySourceCors", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

//Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "InternShipApp.Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});

//Add database
builder.Services
    .AddDbContextPool<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));

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
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IInternGroupRepository, InternGroupRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<ISkillScoreRepository, SkillScoreRepository>();
builder.Services.AddScoped<IInternSettingsRepository, InternSettingsRepository>();
builder.Services.AddScoped<IMatchingService, MatchingService>();
builder.Services.AddScoped<IInternGroupServices, InternGroupServices>();
builder.Services.AddScoped<IStudentFormRepository, StudentFormRepository>();
builder.Services.AddScoped<ILabourMarketFormRepository, LabourMarketFormRepository>();

//Chat
builder.Services.AddSignalR();
builder.Services.AddScoped<ChatHub>();

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
builder.Services.AddAuthorization();

//AutoMapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

//Controllers
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/internship-app-chat");
});

app.UseHttpsRedirection();

app.UseCors("AllowAnySourceCors");

app.Run();
