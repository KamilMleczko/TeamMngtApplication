using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamMngt.Models;

namespace TeamMngt.Data
{
    public class TeamMngtContext : DbContext
    {
        public TeamMngtContext (DbContextOptions<TeamMngtContext> options)
            : base(options)
        {
        }

        public DbSet<TeamMngt.Models.Pracownik> Pracownik { get; set; } = default!;
        public DbSet<TeamMngt.Models.Zespol> Zespol { get; set; } = default!;
        public DbSet<TeamMngt.Models.Zadanie> Zadanie { get; set; } = default!;
        public DbSet<TeamMngt.Models.ModulProjektu> ModulProjektu { get; set; } = default!;
        public DbSet<TeamMngt.Models.Projekt> Projekt { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("TeamMngt.Models.ModulProjektu",b =>
            {
                b.HasOne("TeamMngt.Models.Projekt", "Projekt")
                    .WithMany("ModulyProjektu")
                    .HasForeignKey("ProjektId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.Navigation("Projekt");
            });
            
            modelBuilder.Entity("TeamMngt.Models.Pracownik", b =>
            {
                b.HasOne("TeamMngt.Models.Zespol", "Zespol")
                    .WithMany("Pracownicy")
                    .HasForeignKey("ZespolId")
                    .OnDelete(DeleteBehavior.SetNull);
                b.Navigation("Zespol");
            });
            
            modelBuilder.Entity("TeamMngt.Models.Zadanie", b =>
            {
                b.HasOne("TeamMngt.Models.ModulProjektu", "ModulProjektu")
                    .WithMany("Zadania")
                    .HasForeignKey("ModulProjektuId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("TeamMngt.Models.Pracownik", "Pracownik")
                    .WithMany("Zadania")
                    .HasForeignKey("PracownikId");

                b.Navigation("ModulProjektu");

                b.Navigation("Pracownik");
            });
            
            modelBuilder.Entity("TeamMngt.Models.Zespol", b =>
            {
                b.HasOne("TeamMngt.Models.ModulProjektu", "ModulProjektu")
                    .WithMany("Zespoly")
                    .HasForeignKey("ModulProjektuId")
                    .OnDelete(DeleteBehavior.SetNull);

                b.Navigation("ModulProjektu");
            });
            
        }
    }
}
