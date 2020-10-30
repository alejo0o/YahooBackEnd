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
        public virtual DbSet<Pregunta> Pregunta { get; set; }
        public virtual DbSet<Respuesta> Respuesta { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        public virtual DbSet<PreguntaUsuario> PreguntaUsuario{get;set;}

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

            
            modelBuilder.Entity<PreguntaUsuario>(entity =>
            {
                entity.HasNoKey();

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

                entity.Property(e => e.Pregdetalle).HasColumnName("pregdetalle");

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

                entity.Property(e => e.Userpuntaje).HasColumnName("userpuntaje");

                entity.Property(e => e.Usersexo).HasColumnName("usersexo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
