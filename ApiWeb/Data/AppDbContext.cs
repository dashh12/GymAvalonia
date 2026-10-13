using ApiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Miembro> Miembros => Set<Miembro>();
    public DbSet<Membresia> Membresias => Set<Membresia>();
    public DbSet<Pago> Pagos => Set<Pago>();
    public DbSet<Entrenador> Entrenadores => Set<Entrenador>();
    public DbSet<Rutina> Rutinas => Set<Rutina>();
    public DbSet<Maquinaria> Maquinarias => Set<Maquinaria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Miembro 1 --- N Pago
        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Miembro)
            .WithMany(m => m.Pagos)
            .HasForeignKey(p => p.MiembroId)
            .OnDelete(DeleteBehavior.Cascade);

        // Membresia 1 --- N Miembro
        modelBuilder.Entity<Miembro>()
            .HasOne(m => m.Membresia)
            .WithMany(ms => ms.Miembros)
            .HasForeignKey(m => m.MembresiaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Entrenador 1 --- N Rutina
        modelBuilder.Entity<Rutina>()
            .HasOne(r => r.Entrenador)
            .WithMany(e => e.Rutinas)
            .HasForeignKey(r => r.EntrenadorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Rutina 1 --- N Miembro (un miembro tiene una rutina asignada, opcional)
        modelBuilder.Entity<Miembro>()
            .HasOne(m => m.Rutina)
            .WithMany(r => r.Miembros)
            .HasForeignKey(m => m.RutinaId)
            .OnDelete(DeleteBehavior.SetNull);

        // Precisión de dinero
        modelBuilder.Entity<Pago>().Property(p => p.Monto).HasPrecision(10, 2);
        modelBuilder.Entity<Membresia>().Property(m => m.Precio).HasPrecision(10, 2);

        // Email único
        modelBuilder.Entity<Miembro>().HasIndex(m => m.Email).IsUnique();

        // Datos iniciales
        modelBuilder.Entity<Membresia>().HasData(
            new Membresia { Id = 1, Tipo = "Mensual", Precio = 25000m },
            new Membresia { Id = 2, Tipo = "Trimestral", Precio = 65000m },
            new Membresia { Id = 3, Tipo = "Anual", Precio = 230000m });

        modelBuilder.Entity<Entrenador>().HasData(
            new Entrenador { Id = 1, Nombre = "Lucas", Apellido = "Fernández", Especialidad = "Musculación" },
            new Entrenador { Id = 2, Nombre = "Sofía", Apellido = "Gómez", Especialidad = "Funcional" });

        modelBuilder.Entity<Rutina>().HasData(
            new Rutina { Id = 1, Descripcion = "Full body 3 días", NivelDificultad = "Principiante", EntrenadorId = 1 },
            new Rutina { Id = 2, Descripcion = "Pecho y espalda", NivelDificultad = "Intermedio", EntrenadorId = 1 },
            new Rutina { Id = 3, Descripcion = "Circuito funcional", NivelDificultad = "Intermedio", EntrenadorId = 2 });

        modelBuilder.Entity<Maquinaria>().HasData(
            new Maquinaria { Id = 1, Nombre = "Press de banca", Musculo = "Pecho", Estado = "Operativa", FechaUltimoMantenimiento = new DateTime(2026, 9, 1) },
            new Maquinaria { Id = 2, Nombre = "Polea alta", Musculo = "Espalda", Estado = "Operativa", FechaUltimoMantenimiento = new DateTime(2026, 9, 5) },
            new Maquinaria { Id = 3, Nombre = "Prensa de piernas", Musculo = "Piernas", Estado = "En mantenimiento", FechaUltimoMantenimiento = new DateTime(2026, 10, 2) });

        modelBuilder.Entity<Miembro>().HasData(
            new Miembro { Id = 1, Nombre = "Martín", Apellido = "Pérez", Email = "martin@mail.com", MembresiaId = 1, RutinaId = 1 },
            new Miembro { Id = 2, Nombre = "Camila", Apellido = "López", Email = "camila@mail.com", MembresiaId = 2, RutinaId = 3 });

        modelBuilder.Entity<Pago>().HasData(
            new Pago { Id = 1, FechaPago = new DateTime(2026, 10, 1), Monto = 25000m, MiembroId = 1 },
            new Pago { Id = 2, FechaPago = new DateTime(2026, 9, 20), Monto = 65000m, MiembroId = 2 });
    }
}
