namespace DVCP.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DVCPContext : DbContext
    {
        public DVCPContext()
            : base("name=DVCPContext")
        {
        }

        public virtual DbSet<info> info { get; set; }
        public virtual DbSet<Tbl_HotPost> Tbl_HotPost { get; set; }
        public virtual DbSet<tbl_POST> tbl_POST { get; set; }
        public virtual DbSet<Tbl_Series> Tbl_Series { get; set; }
        public virtual DbSet<tbl_User> tbl_User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_POST>()
                .HasMany(e => e.Tbl_Series)
                .WithMany(e => e.tbl_POST)
                .Map(m => m.ToTable("Tbl_SeriesPost").MapLeftKey("PostID").MapRightKey("seriesID"));

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.userrole)
                .IsUnicode(false);
        }
    }
}
