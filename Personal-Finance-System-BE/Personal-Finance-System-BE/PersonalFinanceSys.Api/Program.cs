using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.AI;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Budgets;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Notifications;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Packages;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Payments;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Posts;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.RolePermission;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.SavingGoals;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Socials;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Transactions;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Users;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Hubs;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Services;
using System.IO;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Configuration["ConnectionStrings:PostgreSQLConnection"] = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__PostgreSQLConnection");
builder.Configuration["JwtSettings:SecretKey"] = Environment.GetEnvironmentVariable("APPSETTINGS__SECRETKEY");
builder.Configuration["JwtSettings:Issuer"] = Environment.GetEnvironmentVariable("APPSETTINGS__ISSUER");
builder.Configuration["JwtSettings:Audience"] = Environment.GetEnvironmentVariable("APPSETTINGS__AUDIENCE");
builder.Configuration["JwtSettings:AccessTokenExpirationMinutes"] = Environment.GetEnvironmentVariable("APPSETTINGS__ACCESSTOKENEXP");
builder.Configuration["JwtSettings:RefreshTokenExpirationDays"] = Environment.GetEnvironmentVariable("APPSETTINGS__REFRESHTOKENEXP");
builder.Configuration["RedisSettings:Host"] = Environment.GetEnvironmentVariable("REDISSETTINGS__HOST");
builder.Configuration["RedisSettings:Port"] = Environment.GetEnvironmentVariable("REDISSETTINGS__PORT");
builder.Configuration["RedisSettings:Password"] = Environment.GetEnvironmentVariable("REDISSETTINGS__PASSWORD");
builder.Configuration["SendGrid:ApiKey"] = Environment.GetEnvironmentVariable("SENDER_APIKEY");
builder.Configuration["SendGrid:Email"] = Environment.GetEnvironmentVariable("SENDER_EMAIL");
builder.Configuration["SendGrid:Name"] = Environment.GetEnvironmentVariable("SENDER_NAME");
builder.Configuration["CloudinarySettings:CloudName"] = Environment.GetEnvironmentVariable("CLOUDINARYSETTINGS__CLOUDNAME");
builder.Configuration["CloudinarySettings:ApiKey"] = Environment.GetEnvironmentVariable("CLOUDINARYSETTINGS__APIKEY");
builder.Configuration["CloudinarySettings:ApiSecret"] = Environment.GetEnvironmentVariable("CLOUDINARYSETTINGS__APISECRET");
builder.Configuration["GeminiSettings:ApiKey"] = Environment.GetEnvironmentVariable("APIKEY__GEMINI");
builder.Configuration["GeminiSettings:BaseUrl"] = Environment.GetEnvironmentVariable("GEMINI__APIURL");



// DbContext Registration
builder.Services.AddDbContext<PersonFinanceSysDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

builder.Services.AddDbContextFactory<PersonFinanceSysDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")),
    ServiceLifetime.Scoped); // Use Scoped lifetime for DbContextFactory

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Person Finance System API", Version = "v1" });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Nhập JWT Bearer token (chỉ phần token, không kèm 'Bearer ').",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(jwtScheme.Reference.Id, jwtScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtScheme, Array.Empty<string>() }
    });
});


// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInvalidatedTokenRepository, InvalidatedTokenRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInvestmentFundRepository, InvestmentFundRepository>();
builder.Services.AddScoped<IInvestmentAssetRepository, InvestmentAssetRepository>();
builder.Services.AddScoped<IInvestmentDetailRepository, InvestmentDetailRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ISavingGoalRepository, SavingGoalRepository>();
builder.Services.AddScoped<ISavingDetailRepository, SavingDetailRepository>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IEvaluateRepository, EvaluateRepository>();
builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

// Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IUpLoadImageFileService, UpLoadImageFileService>();
builder.Services.AddHttpClient<IGeminiService, GeminiService>();
builder.Services.AddScoped<IChatHistoryService, ChatHistoryService>();
builder.Services.AddScoped<INotificationHubService, NotificationHubService>();
builder.Services.AddScoped<IMessageHubService, MessageHubService>();


// Handlers
builder.Services.AddScoped<OtpHandler>();
builder.Services.AddScoped<AuthenHandler>();
builder.Services.AddScoped<RegisterHandler>();
builder.Services.AddScoped<UserHandler>();
builder.Services.AddScoped<UserFinanceHandler>();
builder.Services.AddScoped<InvestmentFundHandler>();
builder.Services.AddHttpClient<CryptoHandler>();
builder.Services.AddHttpClient<NewsHandler>();
builder.Services.AddScoped<InvestmentAssetHandler>();
builder.Services.AddScoped<InvestmentDetailHandler>();
builder.Services.AddScoped<TransactionHandler>();
builder.Services.AddScoped<SavingGoalHandler>();
builder.Services.AddScoped<SavingDetailHandler>();
builder.Services.AddScoped<BudgetHandler>();
builder.Services.AddScoped<ChatHandler>();
builder.Services.AddScoped<RoleHandler>();
builder.Services.AddScoped<PermissionHandler>();
builder.Services.AddScoped<PaymentHandler>();
builder.Services.AddScoped<PackageHandler>();
builder.Services.AddScoped<PostHandler>();
builder.Services.AddScoped<NotificationHandler>();
builder.Services.AddScoped<FavoriteHandler>();
builder.Services.AddScoped<EvaluateHandler>();
builder.Services.AddScoped<FriendshipHandler>();
builder.Services.AddScoped<MessageHandler>();


// Mapper Registration
builder.Services.AddAutoMapper(typeof(Program));

// SignalR Registration
builder.Services.AddSignalR();

// HttpContextAccessor Registration
builder.Services.AddHttpContextAccessor();

// Redis Registration
builder.Services.AddStackExchangeRedisCache(options =>
{
    var host = builder.Configuration["RedisSettings:Host"];
    var port = builder.Configuration["RedisSettings:Port"];
    var password = builder.Configuration["RedisSettings:Password"];
    options.Configuration = $"{host}:{port},password={password},ssl=true,abortConnect=false";
});

// SendGrid and FluentEmail Registration
builder.Services
    .AddFluentEmail(builder.Configuration["SendGrid:Email"], builder.Configuration["SendGrid:Name"])
    .AddRazorRenderer()
    .AddSendGridSender(builder.Configuration["SendGrid:ApiKey"]);

// Cloudinary Registration
var account = new Account(
    Environment.GetEnvironmentVariable("CLOUDINARYSETTINGS__CLOUDNAME"),
    Environment.GetEnvironmentVariable("CLOUDINARYSETTINGS__APIKEY"),
    Environment.GetEnvironmentVariable("CLOUDINARYSETTINGS__APISECRET")
);
builder.Services.AddSingleton(new Cloudinary(account));

// In-Memory Cache Registration
builder.Services.AddMemoryCache();

// Configure CORS to allow any origin, header, and method
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        //policy.WithOrigins("http://localhost:3000", 
        //                   "http://127.0.0.1:5500", 
        //                   "https://portfolio-management-fi.vercel.app") 
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Allow send Cookie
    });
});

// Authentication Config
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                code = "UNAUTHENTICATION",
                status = 401,
                message = "Vui lòng đăng nhập"
            });

            return context.Response.WriteAsync(result);
        },
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;


            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/hubs/notification")
                                                    || path.StartsWithSegments("/hubs/message")))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/hubs/notification");
app.MapHub<MessageHub>("/hubs/message");

app.MapControllers();

app.Run();