using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Services.Abstractions;
using Fundo.Services.DTOs;
using Fundo.Shared.Behaviors;
using Fundo.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fundo.Applications.WebApi.Controllers;

[Route("loan")]
public class LoanManagementController : BaseController
{
    private readonly ILoanService _loanService;
    private readonly LoggingBehavior _loggerBehavior;

    public LoanManagementController(ILoanService loanService, LoggingBehavior loggerBehavior)
    {
        _loanService = loanService;
        _loggerBehavior = loggerBehavior;
    }

    // GET /loan
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<LoanDto>>>> GetAll()
    {
        var request = new { Operation = "GetAllLoans" };

        var result = await _loggerBehavior.ExecuteAsync(
            request,
            async () =>
            {
                var loans = await _loanService.GetAllLoansAsync();
                return loans;
            },
            operationName: "GetAllLoans"
        );

        return Success(result);
    }

    // GET /loan/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<LoanDto>>> GetById(Guid id)
    {
        var request = new { LoanId = id };

        var result = await _loggerBehavior.ExecuteAsync(
            request,
            async () =>
            {
                var loan = await _loanService.GetLoanByIdAsync(id);
                return loan;
            },
            operationName: "GetLoanById"
        );

        return Success(result);
    }

    // POST /loan
    [HttpPost]
    public async Task<ActionResult<ApiResponse<LoanDto>>> Create([FromBody] CreateLoanDto dto)
    {
        var request = dto;

        var result = await _loggerBehavior.ExecuteAsync(
            request,
            async () =>
            {
                var created = await _loanService.CreateLoanAsync(dto);
                return created;
            },
            operationName: "CreateLoan"
        );

        return Created(result, "Loan created successfully.");
    }

    // POST /loan/{id}/payment
    [HttpPost("{id:guid}/payment")]
    public async Task<ActionResult<ApiResponse<LoanDto>>> ApplyPayment(Guid id, [FromBody] PaymentDto dto)
    {
        var request = new { LoanId = id, Payment = dto };

        var result = await _loggerBehavior.ExecuteAsync(
            request,
            async () =>
            {
                var updated = await _loanService.ApplyPaymentAsync(id, dto);
                return updated;
            },
            operationName: "ApplyPayment"
        );

        return Success(result, "Payment applied successfully.");
    }
}