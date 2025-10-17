# **Take-Home Test: Backend-Focused Full-Stack Developer (.NET C# & Angular)**

## **Objective**

This take-home test evaluates your ability to develop and integrate a .NET Core (C#) backend with an Angular frontend, focusing on API design, database integration, and basic DevOps practices.

## **Instructions**

1.  **Fork the provided repository** before starting the implementation.
2.  Implement the requested features in your forked repository.
3.  Once you have completed the implementation, **send the link** to your forked repository via email for review.

## **Task**

You will build a simple **Loan Management System** with a **.NET Core backend (C#)** exposing RESTful APIs and a **basic Angular frontend** consuming these APIs.

---

## **Requirements**

### **1. Backend (API) - .NET Core**

- Create a **RESTful API** in .NET Core to handle **loan applications**.
- Implement the following endpoints:
  - `POST /loans` → Create a new loan.
  - `GET /loans/{id}` → Retrieve loan details.
  - `GET /loans` → List all loans.
  - `POST /loans/{id}/payment` → Deduct from `currentBalance`.
- Loan example (feel free to improve it):

  ```json
  {
    "amount": 1500.0, // Amount requested
    "currentBalance": 500.0, // Remaining balance
    "applicantName": "Maria Silva", // User name
    "status": "active" // Status can be active or paid
  }
  ```

- Use **Entity Framework Core** with **SQL Server**.
- Create seed data to populate the loans (the frontend will consume this).
- Write **unit/integration tests for the API** (xUnit or NUnit).
- **Dockerize** the backend and create a **Docker Compose** file.
- Create a README with setup instructions.

### **2. Frontend - Angular (Simplified UI)**

Develop a **lightweight Angular app** to interact with the backend

#### **Features:**

- A **table** to display a list of existing loans.

#### **Mockup:**

[View Mockup](https://kzmgtjqt0vx63yji8h9l.lite.vusercontent.net/)  
(_The design doesn’t need to be an exact replica of the mockup—it serves as a reference. Aim to keep it as close as possible._)

---

## **Bonus (Optional, Not Required)**

- **Improve error handling and logging** with structured logs.
- Implement **authentication**.
- Create a **GitHub Actions** pipeline for building and testing the backend.

---

## **Evaluation Criteria**

✔ **Code quality** (clean architecture, modularization, best practices).

✔ **Functionality** (the API and frontend should work as expected).

✔ **Security considerations** (authentication, validation, secure API handling).

✔ **Testing coverage** (unit tests for critical backend functions).

✔ **Basic DevOps implementation** (Docker for backend).

---

## **Additional Information**

Candidates are encouraged to include a `README.md` file in their repository detailing their implementation approach, any challenges they faced, features they couldn't complete, and any improvements they would make given more time. Ideally, the implementation should be completed within **two days** of starting the test.

---

# 🧩 My Implementation Notes

## 🧠 Implementation Approach

I followed **Clean Architecture principles** to ensure a clear separation of concerns and a maintainable API design.  
For validation, I used **FluentValidation**, which I find very effective and clean for handling request validation.  
I also implemented **basic logging** using the built-in `.NET ILogger` to track request execution and performance.

## ⚙️ Challenges Faced

The main challenge I faced was with **Docker Compose** — sometimes the API couldn’t connect to the SQL Server container because the database service wasn’t fully ready when the API started.  
After fixing that startup dependency, everything worked smoothly.

## 🚫 Bonus Features Not Completed

- **Authentication** (bonus feature)
- **CI/CD Pipeline** (bonus feature)

## 💡 Future Improvements

If I had more time, I would:

- Add **JWT authentication**.
- Implement **CI/CD pipeline** with GitHub Actions.
- Improve the **frontend UX**.
