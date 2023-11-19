using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using max_api.Models;

namespace max_api.DAL
{
    public class DbmaxContext : DbContext
    {
        public DbSet<vmUserAdd> UserEntities { get; set; }
        public DbSet<vmEstadoCdInfo> EstadoCdInfoEntities { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                // CONFIGURACION DE SQLITE IN-MEMORY
                optionsBuilder.UseSqlite("Filename=./dbmax.db;");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error configuring the db: {ex.Message}");

                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO
            // Console.WriteLine("Sample debug output");
            ConfigurePostEntity(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        private void ConfigurePostEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vmUserAdd>()
                .ToTable("users")
                .HasKey(p => p.Id);   // Primary key

            modelBuilder.Entity<vmUserAdd>()
                  .Property(p => p.Id)
                  .HasColumnName("id")
                  .IsRequired();

            modelBuilder.Entity<vmUserAdd>()
                 .Property(p => p.Nombre)
                 .HasColumnName("nombre")
                 .IsRequired();
            modelBuilder.Entity<vmUserAdd>()
               .Property(p => p.Email)
               .HasColumnName("correo")
               .IsRequired();
            modelBuilder.Entity<vmUserAdd>()
               .Property(p => p.Telefono)
               .HasColumnName("telefono")
               .IsRequired();
            modelBuilder.Entity<vmUserAdd>()
               .Property(p => p.Fecha)
               .HasColumnName("fecha")
               .IsRequired();
            modelBuilder.Entity<vmUserAdd>()
               .Property(p => p.CiudadEstadoId)
               .HasColumnName("ciudad_estado_id")
               .IsRequired();


            modelBuilder.Entity<vmEstadoCdInfo>()
            .ToTable("cat_estado_ciudad")
            .HasKey(p => p.id);   // Primary key
            modelBuilder.Entity<vmEstadoCdInfo>()
            .Property(p => p.estado_cd)
            .HasColumnName("estado_cd")
            .IsRequired();

        }
        public override int SaveChanges()
        {
            ValidateEntities();

            return base.SaveChanges();
        }
        private void ValidateEntities()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    ValidateEntity(entry);
                }
            }
        }
        private void ValidateEntity(EntityEntry entry)
        {
            var validationContext = new ValidationContext(entry.Entity, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entry.Entity, validationContext, validationResults, validateAllProperties: true))
            {
                var validationErrors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                var errorMessage = string.Join("\n", validationErrors);

                throw new ValidationException($"Validation data: {entry.Entity.GetType().Name}. Errors: {errorMessage}");
            }
        }
    }

}
