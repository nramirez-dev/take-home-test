using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fundo.Domain.Entities;
using Fundo.Domain.Enums;
using Fundo.Domain.ValueObjects;
using Fundo.Services.Abstractions;
using Fundo.Services.DTOs;
using Fundo.Services.Services;
using Fundo.Shared.Exceptions;
using Moq;
using Xunit;

namespace Fundo.Services.Tests.Unit.Fundo.Services.Services
{
    public class LoanServiceTests
    {
        private readonly Mock<IRepository<Loan>> _repoMock;
        private readonly LoanService _service;

        public LoanServiceTests()
        {
            _repoMock = new Mock<IRepository<Loan>>();
            _service = new LoanService(_repoMock.Object);
        }

        [Fact]
        public async Task CreateLoanAsync_ShouldReturnCorrectLoanDto()
        {
            var dto = new CreateLoanDto { Amount = 5000, ApplicantName = "Lucía Rivas" };
            Loan? capturedLoan = null;

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Loan>()))
                .Callback<Loan>(loan => capturedLoan = loan)
                .Returns(Task.CompletedTask);


            var result = await _service.CreateLoanAsync(dto);


            Assert.NotNull(result);
            Assert.Equal("Lucía Rivas", result.ApplicantName);
            Assert.Equal(5000, result.Amount);
            Assert.Equal(5000, result.CurrentBalance);
            Assert.Equal(LoanStatus.Active, result.Status);
            Assert.Equal(capturedLoan?.Id, result.Id);
        }

        [Fact]
        public async Task GetLoanByIdAsync_ShouldReturnLoanDto_WhenFound()
        {
            var loan = new Loan(new Money(7000), new ApplicantName("Pedro Núñez"));
            _repoMock.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);


            var result = await _service.GetLoanByIdAsync(loan.Id);


            Assert.NotNull(result);
            Assert.Equal("Pedro Núñez", result!.ApplicantName);
            Assert.Equal(7000, result.Amount);
        }

        [Fact]
        public async Task GetLoanByIdAsync_ShouldThrowNotFoundException_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Loan?)null);


            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetLoanByIdAsync(id));
            Assert.Equal($"Loan with id '{id}' was not found.", ex.Message);
        }

        [Fact]
        public async Task ApplyPaymentAsync_ShouldReduceBalanceAndReturnUpdatedLoan()
        {
            var loan = new Loan(new Money(4000), new ApplicantName("Ana Torres"));
            _repoMock.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);

            var payment = new PaymentDto { PaymentAmount = 1500 };


            var result = await _service.ApplyPaymentAsync(loan.Id, payment);


            Assert.NotNull(result);
            Assert.Equal(2500, result!.CurrentBalance);
            Assert.Equal(LoanStatus.Active, result.Status);
        }

        [Fact]
        public async Task ApplyPaymentAsync_ShouldSetStatusToPaid_WhenBalanceZero()
        {
            var loan = new Loan(new Money(2500), new ApplicantName("Bruno Díaz"));
            _repoMock.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);

            var payment = new PaymentDto { PaymentAmount = 2500 };


            var result = await _service.ApplyPaymentAsync(loan.Id, payment);


            Assert.Equal(0, result!.CurrentBalance);
            Assert.Equal(LoanStatus.Paid, result.Status);
        }

        [Fact]
        public async Task ApplyPaymentAsync_ShouldThrow_WhenPaymentExceedsBalance()
        {
            var loan = new Loan(new Money(3000), new ApplicantName("Tomás Pérez"));
            _repoMock.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);

            var payment = new PaymentDto { PaymentAmount = 5000 };


            var ex = await Assert.ThrowsAsync<BusinessException>(() =>
                _service.ApplyPaymentAsync(loan.Id, payment)
            );

            Assert.Equal("Payment exceeds the current loan balance.", ex.Message);
        }

        [Fact]
        public async Task ApplyPaymentAsync_ShouldThrowNotFound_WhenLoanDoesNotExist()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Loan?)null);

            var payment = new PaymentDto { PaymentAmount = 1000 };


            var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.ApplyPaymentAsync(id, payment)
            );

            Assert.Equal($"Loan with id '{id}' was not found.", ex.Message);
        }

        [Fact]
        public async Task GetAllLoansAsync_ShouldReturnAllLoans()
        {
            var loans = new List<Loan>
            {
                new Loan(new Money(1000), new ApplicantName("Laura")),
                new Loan(new Money(2000), new ApplicantName("Carlos"))
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(loans);


            var result = await _service.GetAllLoansAsync();


            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, l => l.ApplicantName == "Laura");
            Assert.Contains(result, l => l.ApplicantName == "Carlos");
        }
    }
}