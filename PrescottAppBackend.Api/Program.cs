using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.OpenApi.Models;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;
using PrescottAppBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PrescottAppBackend.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PrescottAppBackend.Api.Authorization;

var builder = WebApplication.CreateBuilder(args);
var _appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.AddMemoryCache();
// Configure Firebase Admin SDK
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("prescott-firebase-adminsdk.json")
});

// Register the DbContext with a connection string
// builder.Services.AddDbContext<PrescottContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the DbContext with a connection string (for MySQL)
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Server=127.0.0.1;Database=Prescott;User=root;Password=;";
builder.Services.AddDbContext<PrescottContext>(options =>
   options.UseMySQL(connectionString));



builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // must be lower case
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);


    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securityScheme, new[] { "Bearer" } }
    };
    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _appSettings["LDAPDomain"],
            ValidAudience = _appSettings["LDAPDomain"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings["Secret"]))
        };
        //options.Events = new JwtBearerEvents
        //{
        //    OnAuthenticationFailed = async (context) =>
        //    {
        //        context.Response = ;
        //    }
        //}
    });

// Configure CORS for All Origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IJwtUtils, JwtUtils>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IDDLService, DDLService>();
builder.Services.AddTransient<IBuildingService, BuildingService>();
builder.Services.AddTransient<IAnnouncementService, AnnouncementService>();

builder.Services.AddControllers();

// Configure the application services and middleware by using the Startup class
var app = builder.Build();
var env = app.Services.GetRequiredService<IWebHostEnvironment>();

//if (env.IsDevelopment())
//{
app.MapSwagger();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SaapNet Book v1");
});
//}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// Add custom middleware to the pipeline
//app.UseMiddleware<CustomMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });





// Run the configured pipeline
app.Run();
