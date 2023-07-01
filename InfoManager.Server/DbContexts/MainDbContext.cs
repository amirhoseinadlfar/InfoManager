using InfoManager.Server.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;

namespace InfoManager.Server.DbContexts
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Space> Spaces { get; set; }
        public DbSet<SpaceMember> SpaceMembers { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<TableRow> TableRows { get; set; }
        public DbSet<TableField> TableFields { get; set; }
        public DbSet<TableCell> TableCells { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            EntityTypeBuilder<User> userTypeBuilder = modelBuilder.Entity<User>().ToTable("info_users");
            EntityTypeBuilder<Session> sessionTypeBuilder = modelBuilder.Entity<Session>().ToTable("info_sessions");
            EntityTypeBuilder<Space> spaceTypeBuilder = modelBuilder.Entity<Space>().ToTable("info_spaces");
            EntityTypeBuilder<SpaceMember> spaceMemberTypeBuilder = modelBuilder.Entity<SpaceMember>().ToTable("info_spaceMembers");
            EntityTypeBuilder<Table> tableTypeBuilder = modelBuilder.Entity<Table>().ToTable("info_tables");
            EntityTypeBuilder<TableRow> tableRowTypeBuilder = modelBuilder.Entity<TableRow>().ToTable("info_tableRows");
            EntityTypeBuilder<TableField> tableFieldTypeBuilder = modelBuilder.Entity<TableField>().ToTable("info_tableFields");
            EntityTypeBuilder<TableCell> tableCellTypeBuilder = modelBuilder.Entity<TableCell>().ToTable("info_tableCells");

            #region Users Configure
            userTypeBuilder.HasKey(x => x.Id);

            userTypeBuilder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            userTypeBuilder.Property(x => x.Name)
                .HasMaxLength(User.NameMaxLength)
                .IsRequired();

            userTypeBuilder.Property(x => x.UserName)
                .HasMaxLength(User.UsernameMaxLength)
                .IsRequired();

            userTypeBuilder.Property(x=>x.Password)
                .HasMaxLength(User.PasswordMaxLength)
                .IsRequired();
            #endregion
            #region Session Configure
            sessionTypeBuilder.HasKey(x => x.Id);
            sessionTypeBuilder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            sessionTypeBuilder.Property(x => x.Key)
                .HasMaxLength(Session.KeyLength)
                .IsRequired();
            #endregion
            #region Space Configure
            spaceTypeBuilder.HasKey(x => x.Id);
            spaceTypeBuilder.Property(x=>x.Id)
                .ValueGeneratedOnAdd();
            spaceTypeBuilder.Property(x=>x.Name)
                .HasMaxLength(Space.NameMaxLength)
                .IsRequired();
            #endregion
            #region SpaceMember Configure
            spaceMemberTypeBuilder.HasKey(x => x.Id);
            spaceMemberTypeBuilder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            #endregion
            #region Table Configure
            tableTypeBuilder.HasKey(x => x.Id);
            tableTypeBuilder.Property (x => x.Id)
                .ValueGeneratedOnAdd();

            tableTypeBuilder.Property(x => x.Name)
                .HasMaxLength(Table.NameLength)
                .IsRequired();
            #endregion
            #region TableRow Configure
            tableRowTypeBuilder.HasKey(x => x.Id);
            tableRowTypeBuilder.Property(x=>x.Id)
                .ValueGeneratedOnAdd();

            #endregion
            #region TableField Configure
            tableFieldTypeBuilder.HasKey(x => x.Id);
            tableFieldTypeBuilder.Property(x=>x.Id)
                .ValueGeneratedOnAdd ();
            tableFieldTypeBuilder.Property(x=>x.Name)
                .HasMaxLength(15)
                .IsRequired();
            tableFieldTypeBuilder.Property(x => x.FieldType)
                .IsRequired();
            #endregion
            #region TableCell Configure
            tableCellTypeBuilder.HasKey(x => x.Id);
            tableCellTypeBuilder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            tableCellTypeBuilder.Property(x => x.Value)
                .HasMaxLength(64)
                .IsRequired();
            #endregion

            #region rel
            userTypeBuilder.HasMany(x => x.Sessions)
                .WithOne(x => x.User)
                .HasForeignKey(x=>x.UserId);

            userTypeBuilder.HasMany(x=>x.MemberShips)
                .WithOne(x=>x.User)
                .HasForeignKey(x=>x.UserId);

            spaceTypeBuilder.HasMany(x=>x.Members)
                .WithOne(x=>x.Space)
                .HasForeignKey(x=>x.SpaceId);

            spaceTypeBuilder.HasMany(x => x.Tables)
                .WithOne(x => x.Space)
                .HasForeignKey(x => x.SpaceId);

            tableTypeBuilder.HasMany(x => x.Fields)
                .WithOne(x=>x.Table)
                .HasForeignKey(x=>x.TableId);

            tableTypeBuilder.HasMany(x => x.Rows)
                .WithOne(x => x.Table)
                .HasForeignKey(x => x.TableId);

            tableRowTypeBuilder.HasMany(x => x.Cells)
                .WithOne(x => x.Row)
                .HasForeignKey(x => x.RowId);

            tableFieldTypeBuilder.HasMany(x => x.Cells)
                .WithOne(x => x.Field)
                .HasForeignKey(x => x.FieldId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion
        }
    }
}
