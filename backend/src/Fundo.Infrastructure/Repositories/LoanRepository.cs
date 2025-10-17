using System.Linq.Expressions;
using Fundo.Domain.Entities;
using Fundo.Infrastructure.Context;
using Fundo.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Infrastructure.Repositories;

public class LoanRepository : IRepository<Loan>
{
    private readonly LoanDbContext _context;

    public LoanRepository(LoanDbContext context)
    {
        _context = context;
    }

    public async Task<Loan?> GetByIdAsync(Guid id)
    {
        return await _context.Loans.FindAsync(id);
    }

    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await _context.Loans.ToListAsync();
    }

    public async Task<IEnumerable<Loan>> FindAsync(Expression<Func<Loan, bool>> predicate)
    {
        return await _context.Loans.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(Loan entity)
    {
        await _context.Loans.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Loan entity)
    {
        _context.Loans.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Loan entity)
    {
        _context.Loans.Remove(entity);
        await _context.SaveChangesAsync();
    }
}