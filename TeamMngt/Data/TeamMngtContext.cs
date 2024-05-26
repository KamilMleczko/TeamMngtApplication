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
    }
}
