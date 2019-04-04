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

        public virtual DbSet<info> infoes { get; set; }
        public virtual DbSet<tbl_POST> tbl_POST { get; set; }
        public virtual DbSet<tbl_User> tbl_User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.userrole)
                .IsUnicode(false);
            modelBuilder.Entity<tbl_User>()
                .HasIndex(x => x.username)
                .IsUnique();
        }
    }
}
