using Microsoft.EntityFrameworkCore;
using Fundo.Domain.Entities;
using Fundo.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Fundo.Domain.Enums;

namespace Fundo.Infrastructure.Context;

public class LoanDbContext : DbContext
{
    public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options)
    {
    }

    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var moneyConverter = new ValueConverter<Money, decimal>(
            v => v.Value,
            v => new Money(v)
        );

        var applicantNameConverter = new ValueConverter<ApplicantName, string>(
            v => v.Value,
            v => new ApplicantName(v)
        );

        var statusConverter = new EnumToStringConverter<LoanStatus>();

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.ToTable("Loans");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Amount)
                .HasConversion(moneyConverter)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.CurrentBalance)
                .HasConversion(moneyConverter)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.ApplicantName)
                .HasConversion(applicantNameConverter)
                .HasMaxLength(100);

            entity.Property(e => e.Status)
                .HasConversion(statusConverter)
                .HasMaxLength(10);
        });
    }
}