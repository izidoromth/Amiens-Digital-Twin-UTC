using DatabaseConnection.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseConnection.Context
{
    public class AmiensDigitalTwinDbContext : DbContext
    {
        private readonly string defaultSchema;
        public AmiensDigitalTwinDbContext(DbContextOptions<AmiensDigitalTwinDbContext> opts, string defaultSchema)
        : base(opts)
        {
            this.defaultSchema = defaultSchema;
        }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<BuildingNoGeom> BuildingsNoGeom { get; set; }
        public DbSet<FloodSector> FloodSectors { get; set; }
        public DbSet<Terrain> Terrains { get; set; }
        public DbSet<WaterFlood> WaterFloods { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(defaultSchema);

            builder.Entity<Building>(
                eb =>
                {
                    eb.ToTable("buildings");
                    eb.HasNoKey();
                    eb.Property(b => b.Id).HasColumnName("id");
                    eb.Property(b => b.Nature).HasColumnName("nature");
                    eb.Property(b => b.Usage1).HasColumnName("usage1");
                    eb.Property(b => b.Usage2).HasColumnName("usage2");
                    eb.Property(b => b.NbLogts).HasColumnName("nb_logts");
                    eb.Property(b => b.NbEtages).HasColumnName("nb_etages");
                    eb.Property(b => b.Hauter).HasColumnName("hauter");
                    eb.Property(b => b.ZMinSol).HasColumnName("z_min_sol");
                    eb.Property(b => b.ZMinToit).HasColumnName("z_min_toit");
                    eb.Property(b => b.Geometry).HasColumnName("geometry");
                });

            builder.Entity<BuildingNoGeom>(
                eb =>
                {
                    eb.ToTable("buildings_no_geom");
                    eb.HasNoKey();
                    eb.Property(b => b.Id).HasColumnName("id");
                    eb.Property(b => b.Nature).HasColumnName("nature");
                    eb.Property(b => b.Usage1).HasColumnName("usage1");
                    eb.Property(b => b.Usage2).HasColumnName("usage2");
                    eb.Property(b => b.NbLogts).HasColumnName("nb_logts");
                    eb.Property(b => b.NbEtages).HasColumnName("nb_etages");
                    eb.Property(b => b.Hauter).HasColumnName("hauter");
                    eb.Property(b => b.ZMinSol).HasColumnName("z_min_sol");
                    eb.Property(b => b.ZMinToit).HasColumnName("z_min_toit");
                });

            builder.Entity<FloodSector>(
                eb =>
                {
                    eb.ToTable("flood_sectors");
                    eb.HasNoKey();
                    eb.Property(b => b.SectorId).HasColumnName("sector_id");
                    eb.Property(b => b.Geometry).HasColumnName("geometry");
                });

            builder.Entity<Terrain>(
                eb =>
                {
                    eb.ToTable("terrains");
                    eb.HasNoKey();
                    eb.Property(b => b.Id).HasColumnName("id");
                    eb.Property(b => b.Geometry).HasColumnName("geometry");
                    eb.Property(b => b.Name).HasColumnName("name");
                });

            builder.Entity<WaterFlood>(
                eb =>
                {
                    eb.ToTable("water_flood");
                    eb.HasNoKey();
                    eb.Property(b => b.Id).HasColumnName("id");
                    eb.Property(b => b.Time).HasColumnName("time");
                    eb.Property(b => b.SectorId).HasColumnName("sector_id");
                    eb.Property(b => b.Level).HasColumnName("level");
                    eb.Property(b => b.Year).HasColumnName("year");
                });
        }
    }
}
