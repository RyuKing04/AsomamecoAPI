using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AsomamecoAPI.Models;

public partial class AsomamecoContext : DbContext
{
    public AsomamecoContext()
    {
    }

    public AsomamecoContext(DbContextOptions<AsomamecoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asociado> Asociados { get; set; }

    public virtual DbSet<CateringService> CateringServices { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asociado>(entity =>
        {
            entity.ToTable("Asociado");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Apellidos).HasColumnName("apellidos");
            entity.Property(e => e.Cedula).HasColumnName("cedula");
            entity.Property(e => e.Correo).HasColumnName("correo");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
        });

        modelBuilder.Entity<CateringService>(entity =>
        {
            entity.ToTable("CateringService");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .HasColumnName("correo");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.ToTable("Evento");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdCateringService).HasColumnName("idCateringService");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Lugar).HasColumnName("lugar");
            entity.Property(e => e.Tiquete).HasColumnName("tiquete");

            entity.HasOne(d => d.IdCateringServiceNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdCateringService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evento_CateringService");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evento_Usuario");

            entity.HasMany(d => d.IdAsociados).WithMany(p => p.IdEventos)
                .UsingEntity<Dictionary<string, object>>(
                    "Asistencium",
                    r => r.HasOne<Asociado>().WithMany()
                        .HasForeignKey("IdAsociado")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Asistencia_Asociado"),
                    l => l.HasOne<Evento>().WithMany()
                        .HasForeignKey("IdEvento")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Asistencia_Evento"),
                    j =>
                    {
                        j.HasKey("IdEvento", "IdAsociado");
                        j.ToTable("Asistencia");
                        j.IndexerProperty<int>("IdEvento").HasColumnName("id_evento");
                        j.IndexerProperty<int>("IdAsociado").HasColumnName("id_asociado");
                    });
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Apellidos).HasColumnName("apellidos");
            entity.Property(e => e.Cedula).HasColumnName("cedula");
            entity.Property(e => e.Contraseña).HasColumnName("contraseña");
            entity.Property(e => e.Correo).HasColumnName("correo");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Telefono).HasColumnName("telefono");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
