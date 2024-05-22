using Microsoft.EntityFrameworkCore;

namespace WhatInBus.Database;

public partial class PfHistoryContext : DbContext
{
    public PfHistoryContext()
    {
    }

    public PfHistoryContext(DbContextOptions<PfHistoryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Recognize> Recognizes { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=pf_history;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recognize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recognize_pkey");

            entity.ToTable("recognize");

            entity.HasIndex(e => e.Id, "recognize_id_index").HasAnnotation("Npgsql:StorageParameter:deduplicate_items", "true");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.ModelName)
                .HasColumnType("character varying")
                .HasColumnName("model_name");
            entity.Property(e => e.Result).HasColumnName("result");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
