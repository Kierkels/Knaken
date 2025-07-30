using Kierkels.Knaken.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kierkels.Knaken.Infrastructure.Configurations;

public class TransactionEntityConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        // Table name
        builder.ToTable("Transactions");

        // Primary key
        builder.HasKey(t => t.Id);

        // Map fields to columns with maximum lengths and required constraints
        builder.Property(t => t.Date)
            .IsRequired()
            .HasColumnName("Datum");

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("NaamOmschrijving");
            
        builder.Property(t => t.Account)
            .IsRequired()
            .HasMaxLength(34)
            .HasColumnName("Rekening");

        builder.Property(t => t.CounterAccount)
            .HasMaxLength(34)
            .HasColumnName("Tegenrekening");

        builder.Property(t => t.Code)
            .HasMaxLength(10)
            .HasColumnName("Code");

        builder.Property(t => t.DebitCredit)
            .IsRequired()
            .HasMaxLength(3)
            .HasColumnName("AfBij");

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("BedragEUR");

        builder.Property(t => t.MutationType)
            .HasMaxLength(50)
            .HasColumnName("Mutatiesoort");

        builder.Property(t => t.Remarks)
            .HasColumnName("Mededelingen");

        builder.Property(t => t.BalanceAfterMutation)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("SaldoNaMutatie");

        builder.Property(t => t.Tag)
            .HasMaxLength(50)
            .HasColumnName("Tag");

        // Additional configurations (if needed)
    }
}
