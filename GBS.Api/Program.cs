using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.OpenApi.Models;
using GBS.Service;
using Microsoft.EntityFrameworkCore;
using GBS.Api;
using GBS.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GBS.Api.Authorization;
using Microsoft.Extensions.FileProviders;
using GBS.Entities.DbModels;

var builder = WebApplication.CreateBuilder(args);
var _appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.AddMemoryCache();
// Configure Firebase Admin SDK
//FirebaseApp.Create(new AppOptions()
//{
//    Credential = GoogleCredential.FromFile("prescott-firebase-adminsdk.json")
//});

// Register the DbContext with a connection string
// builder.Services.AddDbContext<PrescottContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the DbContext with a connection string (for MySQL)
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GBS_DbContext>(options =>
   options.UseSqlServer(connectionString));



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
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IMediaService, MediaService>();

builder.Services.AddControllers();

// Configure the application services and middleware by using the Startup class
var app = builder.Build();
var env = app.Services.GetRequiredService<IWebHostEnvironment>();
Console.WriteLine("Environment:", env);
if (env.IsDevelopment())
{
    app.MapSwagger();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Globulars Admin v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();


// Enable serving of static files
app.UseStaticFiles();

// Serve files from the "other_project_files" folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "www")),
    RequestPath = "/files" // URL path to access the files
});


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
