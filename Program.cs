using Microsoft.EntityFrameworkCore;
using AtelierTamplarie.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurare pentru PostgreSQL să folosească UTC pentru DateTime
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Adaugă servicii la container
builder.Services.AddControllers();

// Configurare Entity Framework cu PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurare CORS pentru a permite accesul din frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
            ?? new[] { "http://localhost:5500" })
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configurare Swagger/OpenAPI pentru documentație
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurare HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANT: Ordinea acestor middleware-uri contează!

// 2. Servire fișier default (index.html)
app.UseDefaultFiles();

// 1. Servire fișiere statice (HTML, CSS, JS) din wwwroot
app.UseStaticFiles();


// 3. CORS
app.UseCors("AllowFrontend");

// 4. Authorization
app.UseAuthorization();

// 5. Controllers (API endpoints)
app.MapControllers();

Console.WriteLine("🔨 Atelier Tâmplărie - API pornit!");
Console.WriteLine("📍 Frontend: http://localhost:5000");
Console.WriteLine("📍 API: http://localhost:5000/api");
Console.WriteLine("📚 Swagger: http://localhost:5000/swagger");

app.Run();