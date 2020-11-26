using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Proyecto.Models
{
    public partial class paproyectoContext : DbContext
    {
        public paproyectoContext()
        {
        }

        public paproyectoContext(DbContextOptions<paproyectoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Mensaje> Mensaje { get; set; }
        public virtual DbSet<Pregunta> Pregunta { get; set; }
        public virtual DbSet<Respuesta> Respuesta { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:paproyecto123.database.windows.net,1433;Initial Catalog=paproyecto;Persist Security Info=False;User ID=propa;Password=Qwerty12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Catid);

                entity.ToTable("CATEGORIA");

                entity.Property(e => e.Catid)
                    .HasColumnName("CATID")
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Catdescripcion)
                    .HasColumnName("CATDESCRIPCION")
                    .HasColumnType("text");

                entity.Property(e => e.Catnombre)
                    .IsRequired()
                    .HasColumnName("CATNOMBRE")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Mensaje>(entity =>
            {
                entity.HasKey(e => e.Menid);

                entity.ToTable("MENSAJE");

                entity.Property(e => e.Menid)
                    .HasColumnName("MENID")
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Adminid)
                    .HasColumnName("ADMINID")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Mendetalle)
                    .IsRequired()
                    .HasColumnName("MENDETALLE")
                    .HasColumnType("text");

                entity.Property(e => e.Menfecha)
                    .HasColumnName("MENFECHA")
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Menhora)
                    .HasColumnName("MENHORA")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Mentitulo)
                    .IsRequired()
                    .HasColumnName("MENTITULO")
                    .HasColumnType("text");

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Mensaje)
                    .HasForeignKey(d => d.Adminid)
                    .HasConstraintName("FK_MENSAJE_ADMINMEN_USUARIO");
            });

            modelBuilder.Entity<Pregunta>(entity =>
            {
                entity.HasKey(e => e.Pregid);

                entity.ToTable("PREGUNTA");

                entity.Property(e => e.Pregid)
                    .HasColumnName("PREGID")
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Catid)
                    .HasColumnName("CATID")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Catnombre)
                    .IsRequired()
                    .HasColumnName("CATNOMBRE")
                    .HasColumnType("text");

                entity.Property(e => e.Pregdetalle)
                    .HasColumnName("PREGDETALLE")
                    .HasColumnType("text");

                entity.Property(e => e.Pregestado).HasColumnName("PREGESTADO");

                entity.Property(e => e.Pregfecha)
                    .HasColumnName("PREGFECHA")
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Preghora)
                    .HasColumnName("PREGHORA")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Pregmejorresp).HasColumnName("PREGMEJORRESP");

                entity.Property(e => e.Pregmulta).HasColumnName("pregmulta");

                entity.Property(e => e.Pregtexto)
                    .IsRequired()
                    .HasColumnName("PREGTEXTO")
                    .HasColumnType("text");

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.Pregunta)
                    .HasForeignKey(d => d.Catid)
                    .HasConstraintName("FK_PREGUNTA_CATPREG_CATEGORI");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Pregunta)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_PREGUNTA_USERPREG_USUARIO");
            });

            modelBuilder.Entity<Respuesta>(entity =>
            {
                entity.HasKey(e => e.Respid);

                entity.ToTable("RESPUESTA");

                entity.Property(e => e.Respid)
                    .HasColumnName("RESPID")
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Pregid)
                    .HasColumnName("PREGID")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Respfecha)
                    .HasColumnName("RESPFECHA")
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Resphora)
                    .HasColumnName("RESPHORA")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Resptexto)
                    .IsRequired()
                    .HasColumnName("RESPTEXTO")
                    .HasColumnType("text");

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.Preg)
                    .WithMany(p => p.Respuesta)
                    .HasForeignKey(d => d.Pregid)
                    .HasConstraintName("FK_RESPUEST_PREGRESP_PREGUNTA");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Userid);

                entity.ToTable("USUARIO");

                entity.HasIndex(e => e.Usernick)
                    .HasName("uniq_nick")
                    .IsUnique();

                entity.Property(e => e.Userid)
                    .HasColumnName("USERID")
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Useradmin).HasColumnName("USERADMIN");

                entity.Property(e => e.Userapellido)
                    .IsRequired()
                    .HasColumnName("USERAPELLIDO")
                    .HasColumnType("text");

                entity.Property(e => e.Useremail)
                    .IsRequired()
                    .HasColumnName("USEREMAIL")
                    .HasColumnType("text");

                entity.Property(e => e.Userfechanacimiento)
                    .HasColumnName("userfechanacimiento")
                    .HasColumnType("date");

                entity.Property(e => e.Userfoto)
                    .IsRequired()
                    .HasColumnName("userfoto")
                    .HasColumnType("text")
                    .HasDefaultValueSql("('https://w7.pngwing.com/pngs/81/570/png-transparent-profile-logo-computer-icons-user-user-blue-heroes-logo-thumbnail.png')");

                entity.Property(e => e.Usernick)
                    .IsRequired()
                    .HasColumnName("USERNICK")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Usernombre)
                    .IsRequired()
                    .HasColumnName("USERNOMBRE")
                    .HasColumnType("text");

                entity.Property(e => e.Userpass)
                    .IsRequired()
                    .HasColumnName("userpass")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Userpuntaje)
                    .HasColumnName("USERPUNTAJE")
                    .HasDefaultValueSql("((20))");

                entity.Property(e => e.Usersexo)
                    .IsRequired()
                    .HasColumnName("USERSEXO")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
