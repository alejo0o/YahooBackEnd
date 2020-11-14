using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Proyecto.Models
{
    public partial class PAProyectoContext : DbContext
    {
        public PAProyectoContext()
        {
        }

        public PAProyectoContext(DbContextOptions<PAProyectoContext> options)
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
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Catid)
                    .HasName("pk_categoria");

                entity.ToTable("categoria");

                entity.HasIndex(e => e.Catid)
                    .HasName("categoria_pk")
                    .IsUnique();

                entity.Property(e => e.Catid).HasColumnName("catid");

                entity.Property(e => e.Catdescripcion).HasColumnName("catdescripcion");

                entity.Property(e => e.Catnombre).HasColumnName("catnombre");
            });

            modelBuilder.Entity<Mensaje>(entity =>
            {
                entity.HasKey(e => e.Menid)
                    .HasName("pk_mensaje");

                entity.ToTable("mensaje");

                entity.Property(e => e.Menid).HasColumnName("menid");

                entity.Property(e => e.Adminid).HasColumnName("adminid");

                entity.Property(e => e.Mendetalle)
                    .IsRequired()
                    .HasColumnName("mendetalle");

                entity.Property(e => e.Menfecha)
                    .HasColumnName("menfecha")
                    .HasColumnType("date")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Menhora)
                    .HasColumnName("menhora")
                    .HasColumnType("time without time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Mentitulo)
                    .IsRequired()
                    .HasColumnName("mentitulo");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.MensajeAdmin)
                    .HasForeignKey(d => d.Adminid)
                    .HasConstraintName("fk_admin_mensaje");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MensajeUser)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("fk_user_mensaje");
            });

            modelBuilder.Entity<Pregunta>(entity =>
            {
                entity.HasKey(e => e.Pregid)
                    .HasName("pk_pregunta");

                entity.ToTable("pregunta");

                entity.HasIndex(e => e.Catid)
                    .HasName("posee_fk");

                entity.HasIndex(e => e.Pregid)
                    .HasName("pregunta_pk")
                    .IsUnique();

                entity.Property(e => e.Pregid).HasColumnName("pregid");

                entity.Property(e => e.Catid).HasColumnName("catid");

                entity.Property(e => e.Catnombre).HasColumnName("catnombre");

                entity.Property(e => e.Pregdetalle).HasColumnName("pregdetalle");

                entity.Property(e => e.Pregestado).HasColumnName("pregestado");

                entity.Property(e => e.Pregfecha)
                    .HasColumnName("pregfecha")
                    .HasColumnType("date")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Preghora)
                    .HasColumnName("preghora")
                    .HasColumnType("time without time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Pregmejorresp).HasColumnName("pregmejorresp");

                entity.Property(e => e.Pregtexto).HasColumnName("pregtexto");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.Pregunta)
                    .HasForeignKey(d => d.Catid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pregunta_posee_categori");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Pregunta)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("fk_pregunta_realiza_usuario");
            });

            modelBuilder.Entity<Respuesta>(entity =>
            {
                entity.HasKey(e => e.Respid)
                    .HasName("pk_respuesta");

                entity.ToTable("respuesta");

                entity.HasIndex(e => e.Pregid)
                    .HasName("tiene_fk");

                entity.HasIndex(e => e.Respid)
                    .HasName("respuesta_pk")
                    .IsUnique();

                entity.Property(e => e.Respid).HasColumnName("respid");

                entity.Property(e => e.Pregid).HasColumnName("pregid");

                entity.Property(e => e.Respfecha)
                    .HasColumnName("respfecha")
                    .HasColumnType("date")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Resphora)
                    .HasColumnName("resphora")
                    .HasColumnType("time without time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Resptexto).HasColumnName("resptexto");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Preg)
                    .WithMany(p => p.Respuesta)
                    .HasForeignKey(d => d.Pregid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_respuest_tiene_pregunta");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Respuesta)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("fk_respuest_da_usuario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("pk_usuario");

                entity.ToTable("usuario");

                entity.HasIndex(e => e.Useremail)
                    .HasName("email_uniq")
                    .IsUnique();

                entity.HasIndex(e => e.Usernick)
                    .HasName("nick_uniq")
                    .IsUnique();

                entity.HasIndex(e => new { e.Userid, e.Usernick })
                    .HasName("usuario_pk")
                    .IsUnique();

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Useradmin)
                    .HasColumnName("useradmin")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Userapellido).HasColumnName("userapellido");

                entity.Property(e => e.Useremail).HasColumnName("useremail");

                entity.Property(e => e.Userfechanacimiento)
                    .HasColumnName("userfechanacimiento")
                    .HasColumnType("date");

                entity.Property(e => e.Userfoto).HasColumnName("userfoto");

                entity.Property(e => e.Usernick)
                    .IsRequired()
                    .HasColumnName("usernick");

                entity.Property(e => e.Usernombre).HasColumnName("usernombre");

                entity.Property(e => e.Userpass).HasColumnName("userpass");

                entity.Property(e => e.Userpuntaje)
                    .HasColumnName("userpuntaje")
                    .HasDefaultValueSql("20");

                entity.Property(e => e.Usersexo)
                    .IsRequired()
                    .HasColumnName("usersexo")
                    .HasMaxLength(9)
                    .IsFixedLength()
                    .HasDefaultValueSql("'Otro'::bpchar");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
