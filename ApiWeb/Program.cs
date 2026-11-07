using System.Text.Json.Serialization;
using ApiWeb.Data;
using ApiWeb.Models;
using Microsoft.EntityFrameworkCore;
using ApiWeb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // El código de inicialización va AQUÍ ADENTRO:
    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new Usuario
        {
            NombreUsuario = "gym",
            ContrasenaHash = Seguridad.Hashear("123")
        });
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Gym API v1");
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();
