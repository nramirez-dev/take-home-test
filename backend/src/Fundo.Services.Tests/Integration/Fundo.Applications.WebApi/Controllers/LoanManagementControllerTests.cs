using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Fundo.Domain.Enums;
using Fundo.Services.DTOs;
using Fundo.Shared.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Fundo.Services.Tests.Integration
{
    public class LoanManagementControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public LoanManagementControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetAllLoans_ShouldReturnOk()
        {
            var createDto = new CreateLoanDto
            {
                Amount = 3000,
                ApplicantName = "María Gómez"
            };

            await _client.PostAsJsonAsync("/loan", createDto);

            var response = await _client.GetAsync("/loan");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var wrapper = await response.Content.ReadFromJsonAsync<ApiResponse<LoanDto[]>>();
            Assert.NotNull(wrapper);
            Assert.True(wrapper.Success);
            Assert.NotEmpty(wrapper.Data);
            Assert.Contains(wrapper.Data, l => l.ApplicantName == "María Gómez");
        }

        [Fact]
        public async Task CreateLoan_ShouldReturnCreatedLoan()
        {
            var createLoanDto = new CreateLoanDto
            {
                Amount = 4500.75m,
                ApplicantName = "Juan Pérez"
            };

            var response = await _client.PostAsJsonAsync("/loan", createLoanDto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var wrapper = await response.Content.ReadFromJsonAsync<ApiResponse<LoanDto>>();
            var loan = wrapper.Data;

            Assert.NotNull(loan);
            Assert.Equal("Juan Pérez", loan.ApplicantName);
            Assert.Equal(4500.75m, loan.Amount);
            Assert.Equal(LoanStatus.Active, loan.Status);
        }

        [Fact]
        public async Task GetLoanById_ShouldReturnLoan()
        {
            var createDto = new CreateLoanDto
            {
                Amount = 6000,
                ApplicantName = "Laura Sánchez"
            };
            var postResp = await _client.PostAsJsonAsync("/loan", createDto);
            var createdWrapper = await postResp.Content.ReadFromJsonAsync<ApiResponse<LoanDto>>();
            var createdLoan = createdWrapper.Data;

            var response = await _client.GetAsync($"/loan/{createdLoan.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var wrapper = await response.Content.ReadFromJsonAsync<ApiResponse<LoanDto>>();
            var loan = wrapper.Data;

            Assert.Equal("Laura Sánchez", loan.ApplicantName);
            Assert.Equal(6000, loan.Amount);
        }

        [Fact]
        public async Task GetLoanById_NotFound_ShouldReturn404()
        {
            var randomId = Guid.NewGuid();
            var response = await _client.GetAsync($"/loan/{randomId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ApplyPayment_ShouldReturnUpdatedLoan()
        {
            var createDto = new CreateLoanDto
            {
                Amount = 8000,
                ApplicantName = "Carlos Herrera"
            };
            var postResp = await _client.PostAsJsonAsync("/loan", createDto);
            var createdWrapper = await postResp.Content.ReadFromJsonAsync<ApiResponse<LoanDto>>();
            var createdLoan = createdWrapper.Data;

            var paymentDto = new PaymentDto { PaymentAmount = 2000 };
            var response = await _client.PostAsJsonAsync($"/loan/{createdLoan.Id}/payment", paymentDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var wrapper = await response.Content.ReadFromJsonAsync<ApiResponse<LoanDto>>();
            var updatedLoan = wrapper.Data;

            Assert.Equal(6000, updatedLoan.CurrentBalance);
            Assert.Equal(LoanStatus.Active, updatedLoan.Status);
        }

        [Fact]
        public async Task ApplyPayment_FullAmount_ShouldSetStatusToPaid()
        {
            var createDto = new CreateLoanDto
            {
                Amount = 2500,
                ApplicantName = "Mario Vega"
            };
            var postResp = await _client.PostAsJsonAsync("/loan", createDto);
            var createdWrapper = await postResp.Content.ReadFromJsonAsync<ApiResponse<LoanDto>>();
            var createdLoan = createdWrapper.Data;

            var paymentDto = new PaymentDto { PaymentAmount = 2500 };
            var response = await _client.PostAsJsonAsync($"/loan/{createdLoan.Id}/payment", paymentDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var wrapper = await response.Content.ReadFromJsonAsync<ApiResponse<LoanDto>>();
            var updatedLoan = wrapper.Data;

            Assert.Equal(0, updatedLoan.CurrentBalance);
            Assert.Equal(LoanStatus.Paid, updatedLoan.Status);
        }

        [Fact]
        public async Task ApplyPayment_InvalidLoan_ShouldReturn404()
        {
            var paymentDto = new PaymentDto { PaymentAmount = 999.99m };
            var response = await _client.PostAsJsonAsync($"/loan/{Guid.NewGuid()}/payment", paymentDto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task CreateLoan_InvalidData_ShouldReturnBadRequest()
        {
            var createDto = new CreateLoanDto
            {
                Amount = 0,
                ApplicantName = ""
            };

            var response = await _client.PostAsJsonAsync("/loan", createDto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var wrapper = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            Assert.False(wrapper.Success);
            Assert.NotNull(wrapper.Message);
        }

        [Fact]
        public async Task ApplyPayment_AmountGreaterThanBalance_ShouldReturnBadRequest()
        {
            var createDto = new CreateLoanDto
            {
                Amount = 1000,
                ApplicantName = "Lucía Torres"
            };
            var postResp = await _client.PostAsJsonAsync("/loan", createDto);
            var createdLoan = (await postResp.Content.ReadFromJsonAsync<ApiResponse<LoanDto>>())!.Data;

            var paymentDto = new PaymentDto { PaymentAmount = 5000 };
            var response = await _client.PostAsJsonAsync($"/loan/{createdLoan.Id}/payment", paymentDto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var wrapper = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            Assert.False(wrapper.Success);
            Assert.False(string.IsNullOrWhiteSpace(wrapper.Message));
        }
    }
}