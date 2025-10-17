using Fundo.Domain.Entities;
using Fundo.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundo.Infrastructure.Context
{
    public static class LoanDbContextSeed
    {
        public static async Task SeedAsync(LoanDbContext context)
        {
            if (!context.Loans.Any())
            {
                var loan1 = new Loan(new Money(10000), new ApplicantName("Alice Mendoza"));
                loan1.ApplyPayment(new Money(2000));

                var loan2 = new Loan(new Money(5000), new ApplicantName("Carlos Rivas"));
                loan2.ApplyPayment(new Money(5000));

                var loan3 = new Loan(new Money(12000), new ApplicantName("Laura Núñez"));


                var loan4 = new Loan(new Money(7500), new ApplicantName("Pedro Suárez"));
                loan4.ApplyPayment(new Money(2500));

                context.Loans.AddRange(loan1, loan2, loan3, loan4);
                await context.SaveChangesAsync();
            }
        }
    }
}