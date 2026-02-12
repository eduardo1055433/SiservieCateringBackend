using Microsoft.EntityFrameworkCore;
using SiservieCatering.API.Data;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── EF Core + PostgreSQL ─────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// ── Middleware ─────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
