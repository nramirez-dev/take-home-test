---
# 🏦 Loan Management API

A **RESTful API** for managing loans and payments, built with **.NET 6**, **Entity Framework Core**, and **SQL Server**.

It follows **Clean Architecture** with **tactical DDD** (Domain-Driven Design) to enforce a clear separation between the Domain, Application, Infrastructure, and API layers.

Includes data seeding, **xUnit** tests, and **Docker Compose** for easy setup.
---

## ✨ Key Highlights

- **EF Core persistence** with automatic migrations and seed data.
- **Application Services** orchestrate business use cases (create loan, apply payment).
- **Validation** using **FluentValidation** and centralized exception handling.
- **CORS policy** configured for an Angular frontend (`http://localhost:4200`).

---

## ⚙ Technologies Used

- **.NET 6** (ASP.NET Core Web API)
- **Entity Framework Core** + **SQL Server**
- **xUnit** for testing
- **Docker Compose**
- **FluentValidation**

## 🧱 Architecture (Clean + DDD)

```
backend/
└── src/
    ├── Fundo.Domain/              # Core domain: Entities, Value Objects, business rules
    ├── Fundo.Services/            # Application layer: DTOs, use cases (LoanService), contracts
    ├── Fundo.Infrastructure/      # EF Core: DbContext, Repositories, Migrations, Seed data
    ├── Fundo.Applications.WebApi/ # Presentation: Controllers, Middleware, Filters, DI, Swagger
    └── Fundo.Shared/              # Shared responses and exceptions
```

---

---

## 🐳 Running the Backend (Docker Setup)

To run the backend using **Docker Compose**, follow these steps:

1.  **Navigate to the backend folder:**

    ```sh
    cd take-home-test\backend\src
    ```

2.  **Build and start the containers:**

    ```sh
    docker compose up -d --build
    ```

    This will start:

    - `fundo_sql` → **SQL Server** (port `1433`)
    - `fundo_api` → **.NET 6 Web API** (port `8080`)

3.  **Access the API (Swagger UI):**

    Once the containers are running, open the **Swagger UI** to explore and test all endpoints:

    ```http
    http://localhost:8080/index.html
    ```

    You can execute requests such as:

    - `GET /loan` → Retrieve all loans
    - `POST /loan` → Create a new loan
    - `GET /loan/{id}` → Retrieve a specific loan
    - `POST /loan/{id}/payment` → Apply a payment

4.  **Verify the API manually:**

    ```http
    GET -> http://localhost:8080/loan
    ```

    _Expected result_ → `200 OK` and a list of seeded loans.

5.  **View logs (optional):**

    To monitor the output from the API container in real-time:

    ```sh
    docker logs -f fundo_api
    ```

6.  **Stop and remove containers:**

    To stop and remove the containers, networks, and volumes created by Docker Compose:

    ```sh
    docker compose down -v
    ```

---

## 🚀 Commands (.NET CLI)

To build the backend, navigate to the `src` folder and run:

```sh
dotnet build
```

To run all tests:

```sh
dotnet test
```

To start the main API:

```sh
cd Fundo.Applications.WebApi
dotnet run
```

The following endpoint should return **200 OK**:

```http
GET -> https://localhost:5001/loan
```

---

## 📚 API Endpoints

| **Method** | **Endpoint**         | **Description**                |
| ---------- | -------------------- | ------------------------------ |
| **POST**   | `/loan`              | Create a new loan              |
| **GET**    | `/loan`              | Retrieve all existing loans    |
| **GET**    | `/loan/{id}`         | Retrieve a specific loan by ID |
| **POST**   | `/loan/{id}/payment` | Apply a payment to the loan    |

---

### 🆕 Create a Loan

**POST** `/loan`

**Request body:**

```json
{
  "amount": 1500.0,
  "applicantName": "Maria Silva"
}
```

**Response (201 Created):**

```json
{
  "success": true,
  "message": "Loan created successfully.",
  "data": {
    "id": "ee4aabef-4f45-41e6-bf59-968f4e7ddc88",
    "amount": 1500.0,
    "currentBalance": 1500.0,
    "applicantName": "Maria Silva",
    "status": "Active"
  }
}
```

---

### 📄 Get All Loans

**GET** `/loan`

**Response (200 OK):**

```json
{
  "success": true,
  "message": null,
  "data": [
    {
      "id": "b11d8b5b-e721-4d32-8e5f-19a53fb2f550",
      "amount": 5000.0,
      "currentBalance": 0.0,
      "applicantName": "Carlos Rivas",
      "status": "Paid"
    },
    {
      "id": "ba522c27-1481-4121-a7ef-384e80e4da2e",
      "amount": 12000.0,
      "currentBalance": 12000.0,
      "applicantName": "Laura Núñez",
      "status": "Active"
    }
  ]
}
```

---

### 🔍 Get Loan by ID

**GET** `/loan/{id}`

**Example:**

```http
GET /loan/0fe664f6-668d-47c6-80d8-6f0cab824455
```

**Response (200 OK):**

```json
{
  "success": true,
  "message": null,
  "data": {
    "id": "0fe664f6-668d-47c6-80d8-6f0cab824455",
    "amount": 7500,
    "currentBalance": 5000,
    "applicantName": "Pedro Suárez",
    "status": "Active"
  }
}
```

---

### 💸 Apply Payment

**POST** `/loan/{id}/payment`

**Request body:**

```json
{
  "paymentAmount": 300.0
}
```

**Response (200 OK):**

```json
{
  "success": true,
  "message": "Payment applied successfully.",
  "data": {
    "id": "0fe664f6-668d-47c6-80d8-6f0cab824455",
    "amount": 7500.0,
    "currentBalance": 4700.0,
    "applicantName": "Pedro Suárez",
    "status": "Active"
  }
}
```

---

## 🧪 Running Tests (Unit + Integration)

This project includes **two types of tests**:

- ✅ **Unit Tests** → validate business logic (e.g., loan creation, payment rules).
- 🧩 **Integration Tests** → verify API endpoints using an in-memory test server (`WebApplicationFactory`).

To run **all tests**, simply execute:

```bash
dotnet test


```

## 🔒 Security & Validation

- **CORS policy:** Configured to allow the Angular frontend (`http://localhost:4200`).
- **Validation:** Implemented via **FluentValidation** and **DTO attributes**.
- **Business Logic Checks:** Logic within services ensures that operations (like payments) are valid before execution.
- **Exception Handling:** Centralized exception **middleware** provides clear and consistent error responses.
- **Logging Behavior:** Custom **Logging Behavior** for centralized request/response logging.

## Notes

Feel free to modify the code as needed, but try to **respect and extend the current architecture**, as this is intended to be a replica of the Fundo codebase.
