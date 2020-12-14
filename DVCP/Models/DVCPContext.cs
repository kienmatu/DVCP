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

        public virtual DbSet<WebInfo> WebInfo { get; set; }
     
        public virtual DbSet<StickyPost> StickyPosts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(e => e.Tbl_Series)
                .WithMany(e => e.Tbl_POST)
                .Map(m => m.ToTable("Tbl_SeriesPost").MapLeftKey("PostID").MapRightKey("seriesID"));

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Tbl_Tags)
                .WithMany(e => e.Tbl_POST)
                .Map(m => m.ToTable("Tbl_PostTags").MapLeftKey("PostID").MapRightKey("TagID"));

            modelBuilder.Entity<User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.userrole)
                .IsUnicode(false);
        }
    }
}
