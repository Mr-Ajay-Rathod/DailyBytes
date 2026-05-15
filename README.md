# 📰 DailyBytes — Full-Stack News Platform

> 🚀 Production-style full-stack application built with ASP.NET Core Web API & Angular

---

###🔗 Live Link - https://thedailybytes.vercel.app/

---


## ✨ Overview

**DailyBytes** is a scalable news platform that enables users to browse, read, and interact with articles through a modern web interface.

The project is designed to demonstrate **end-to-end system design**, including backend APIs, database modeling, and frontend integration.

---

## 🚀 Key Features

* 📰 Article management (CRUD operations)
* 👤 User authentication (Register / Login)
* ❤️ Favorites system (user-specific)
* 🚨 Article reporting mechanism
* 📂 Category-based organization
* ⚡ Seamless Angular + API integration

---

## 🛠️ Tech Stack

**Backend:** ASP.NET Core Web API (.NET 6), C#
**Database:** SQL Server (LocalDB / Express), SQLite (optional)
**ORM:** Entity Framework Core
**Frontend:** Angular, TypeScript, RxJS
**Tools:** Visual Studio, Node.js, Git

---

## 📂 Project Structure

```id="nx1"
DailyBytes/
├── DailyBytes.API/        # REST API (Controllers, DbContext)
├── DailyBytes.DAL/        # SQL scripts
├── client/                # Angular frontend
└── README.md
```

---

## ⚙️ Running the Project Locally

### 🔧 1. Clone Repository

```id="nx2"
git clone https://github.com/your-username/dailybytes-fullstack.git
cd dailybytes-fullstack
```

---

### 🗄️ 2. Setup Database

#### Option A — SQL Server (Recommended)

```id="nx3"
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "DailyBytes.DAL\DailyBytesDB.sql"
```

✔ Creates schema + seed data

---

#### Option B — SQLite (Quick Setup)

Update DbContext configuration:

```id="nx4"
UseSqlite("Data Source=dailybytes.db")
```

---

### ⚙️ 3. Configure Connection String

Edit:

```id="nx5"
DailyBytes.API/appsettings.json
```

Example:

```id="nx6"
"ConnectionStrings": {
  "Default": "Server=(localdb)\\MSSQLLocalDB;Database=DailyBytesDB;Trusted_Connection=True;"
}
```

---

### 🚀 4. Run Backend API

```id="nx7"
dotnet restore
dotnet build
dotnet run --project DailyBytes.API
```

👉 API runs on: https://localhost:5001 (or similar)

---

### 🌐 5. Run Angular Client

```id="nx8"
cd client
npm install
npm start
```

👉 App runs on: http://localhost:4200

---

## 🔗 API Overview

* **Articles:** Full CRUD operations
* **Users:** Register & Login
* **Favorites:** Add / Remove / Fetch
* **Reports:** Submit & View

---

## 📸 Preview

> Add UI screenshots (Home | Article View | Favorites)

---

## ⚠️ Notes

* Development-focused setup (no JWT/auth middleware yet)
* Passwords stored in plain text (to be improved)

---

## 🚀 Future Enhancements

* JWT-based authentication & authorization
* Admin panel for content management
* Search, filtering & pagination
* Docker-based deployment

---

## 💼 Why This Project Matters

* Demonstrates **full-stack development capability**
* Covers **API design, DB modeling, and frontend integration**
* Built with **industry-relevant technologies**

---

## 👨‍💻 Author

**Ajay**

---

⭐ If you find this useful, consider giving it a star!
