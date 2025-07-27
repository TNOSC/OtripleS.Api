# Otriples.Api

🚀 **Otriples.Api** is a modern reimplementation of the [OtripleS](https://github.com/hassanhabib/OtripleS) open-source school management system. This version introduces new architectural practices, including the **REPR Design Pattern (Request-Endpoint-Response)** and **CQRS (Command Query Responsibility Segregation)**, while staying aligned with the clean architecture and coding standards established by **Hassan Habib**.

## 🎯 Project Goals

- Rebuild the OtripleS system with a **cleaner, more scalable architecture**.
- Adopt **REPR Pattern** for consistent API structure and clear separation of concerns.
- Integrate **CQRS** to separate read and write responsibilities for better performance and maintainability.
- Stick to best practices for **domain modeling, testing, and clean code**.

---

## 🧱 Key Architectural Choices

### ✅ REPR (Request-Endpoint-Response) Pattern
- Ensures clear **endpoint contracts** using dedicated `Request` and `Response` DTOs.
- Promotes consistency across all API endpoints.
- Enhances readability and maintainability.

### ✅ CQRS (Command Query Responsibility Segregation)
- Clean separation between **read models** (queries) and **write models** (commands).
- Reduces complexity for larger systems and improves scalability.

---

## 📦 Technologies Used

- **.NET 9**
- **Entity Framework Core / Dapper**
- **ASP.NET Core Web API**
- **Swagger**
- **xUnit / NSubstitute**
- **FluentAssertions**

---

## 📖 Inspiration

This project is heavily inspired by:

- [OtripleS](https://github.com/hassanhabib/OtripleS)
- [Hassan Habib’s Coding Standards](https://github.com/hassanhabib/Standards)


---

## 📚 Getting Started

> 🚧 Under active development.

To run the project locally:

```bash
git clone https://github.com/TNOSC/OtripleS.Api.git
cd Otriples.Api/src
dotnet restore
dotnet run


