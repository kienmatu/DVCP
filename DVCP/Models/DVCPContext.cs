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
        public virtual DbSet<Tbl_Tags> Tbl_Tags { get; set; }
        public virtual DbSet<Tbl_POST> Tbl_POST { get; set; }
        public virtual DbSet<Tbl_Series> Tbl_Series { get; set; }
        public virtual DbSet<tbl_User> tbl_User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tbl_POST>()
                .HasMany(e => e.Tbl_Series)
                .WithMany(e => e.Tbl_POST)
                .Map(m => m.ToTable("Tbl_SeriesPost").MapLeftKey("PostID").MapRightKey("seriesID"));

            modelBuilder.Entity<Tbl_POST>()
                .HasMany(e => e.Tbl_Tags)
                .WithMany(e => e.Tbl_POST)
                .Map(m => m.ToTable("Tbl_PostTags").MapLeftKey("PostID").MapRightKey("TagID"));

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
