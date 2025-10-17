using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Services.Abstractions;
using Fundo.Services.DTOs;
using Fundo.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fundo.Applications.WebApi.Controllers;

[Route("loan")]
public class LoanManagementController : BaseController
{
    private readonly ILoanService _loanService;

    public LoanManagementController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    // GET /loan
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<LoanDto>>>> GetAll()
    {
        var loans = await _loanService.GetAllLoansAsync();
        return Success(loans);
    }

    // GET /loan/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<LoanDto>>> GetById(Guid id)
    {
        var loan = await _loanService.GetLoanByIdAsync(id);
        return Success(loan);
    }

    // POST /loan
    [HttpPost]
    public async Task<ActionResult<ApiResponse<LoanDto>>> Create([FromBody] CreateLoanDto dto)
    {
        var created = await _loanService.CreateLoanAsync(dto);
        return Created(created, "Loan created successfully.");
    }

    // POST /loan/{id}/payment
    [HttpPost("{id:guid}/payment")]
    public async Task<ActionResult<ApiResponse<LoanDto>>> ApplyPayment(Guid id, [FromBody] PaymentDto dto)
    {
        var updated = await _loanService.ApplyPaymentAsync(id, dto);
        return Success(updated, "Payment applied successfully.");
    }
}