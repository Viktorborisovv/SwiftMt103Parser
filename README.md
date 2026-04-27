# SwiftMt103Parser 💳

SwiftMt103Parser is a simple ASP.NET Core Web API application for processing SWIFT MT103 messages.

The API accepts a SWIFT MT103 message from an uploaded file or as raw text, parses the important fields, and stores them in a SQLite database using raw SQL queries without Entity Framework.

---

## 🚀 Features

### 📤 SWIFT MT103 Upload

Upload a `.txt` file containing a SWIFT MT103 message

Read the uploaded file content

Process the message through the API

### 🧾 Raw Text Processing

Submit a SWIFT MT103 message directly as raw text

Useful for quick testing through Swagger

### 🔍 MT103 Field Parsing

The application extracts the following fields:

| SWIFT Field | Description |
|---|---|
| `:20:` | Transaction Reference |
| `:23B:` | Bank Operation Code |
| `:32A:` | Value Date, Currency, Amount |
| `:50K:` | Ordering Customer |
| `:59:` | Beneficiary Customer |
| `:70:` | Payment Details |
| `:71A:` | Details of Charges |

### 💾 SQLite Storage

Parsed messages are stored in a SQLite database

Database file is created automatically on application startup

Communication with SQLite is implemented without Entity Framework

### 📖 API Documentation

Swagger UI is available for testing all endpoints

### 📝 Logging

Logging is implemented using NLog

Logs are written to the `logs` folder

---

## 🧠 Architecture

The project follows a simple and clean structure:

```txt
Controller → Service → Parser → Repository → SQLite
```

### Key Design Principles

✔️ Simple Web API structure  
✔️ Service Layer  
✔️ Repository for database access  
✔️ Manual parsing logic  
✔️ Raw SQL queries  
✔️ Dependency Injection  
✔️ No Entity Framework  
✔️ No Authentication / Authorization  

---

## 🛠️ Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core Web API | Web API framework |
| C# | Main programming language |
| SQLite | Database |
| Microsoft.Data.Sqlite | SQLite communication |
| Swagger / OpenAPI | API documentation and testing |
| NLog | Logging |
| Visual Studio | Development environment |

---

## ⚙️ Getting Started

### 1️⃣ Prerequisites

.NET SDK

Visual Studio 2022 / Visual Studio Code

Git

---

### 2️⃣ Clone the repository

```bash
git clone https://github.com/Viktorborisovv/SwiftMt103Parser.git
cd SwiftMt103Parser/SwiftMt103Parser.Api
```

---

### 3️⃣ Restore dependencies

```bash
dotnet restore
```

---

### 4️⃣ Run the application

```bash
dotnet run
```

---

### 5️⃣ Open Swagger

Use one of the URLs shown in the console.

Example:

```txt
https://localhost:7128/swagger
```

or

```txt
http://localhost:5052/swagger
```

---

## 🌐 API Endpoints

### Upload MT103 file

```http
POST /api/SwiftMessages/upload
```

Accepts a `.txt` file containing a SWIFT MT103 message.

---

### Process MT103 message from raw text

```http
POST /api/SwiftMessages/text
```

Example request body:

```json
{
  "rawMessage": "{1:F21PRCBBGSFAXXX2082167565}{4:{177:1602161334}{451:0}}{1:F01PRCBBGSFAXXX2082167565}{2:I103COBADEFFXXXXN}{3:{119:STP}}{4:\n:20:160216270075956\n:23B:CRED\n:32A:160217EUR540,00\n:33B:EUR540,00\n:50K:/BG95RZBB91556261794271\nOKO 1000 OOD\nTZAR IVAN SHISHMAN ? 11\nSOFIA, BULGARIA\n:57A:INGDDEFFXXX\n:59:/DE83500105172667785918\nFRANCA CEVALES\nMUNCHENER STR. 35, GERMANY\n:70:ACCOMODATION 11-11.02.16  INVOICE\n027/2016\n:71A:SHA\n-}{5:{MAC:00000000}{CHK:6BC2D5BE9937}}"
}
```

---

### Get all saved messages

```http
GET /api/SwiftMessages
```

---

### Get message by id

```http
GET /api/SwiftMessages/{id}
```

Example:

```http
GET /api/SwiftMessages/1
```

---

## ✅ Example Response

```json
{
  "id": 1,
  "transactionReference": "160216270075956",
  "bankOperationCode": "CRED",
  "valueDate": "2016-02-17",
  "currency": "EUR",
  "amount": 540,
  "orderingCustomer": "/BG95RZBB91556261794271\nOKO 1000 OOD\nTZAR IVAN SHISHMAN ? 11\nSOFIA, BULGARIA",
  "beneficiaryCustomer": "/DE83500105172667785918\nFRANCA CEVALES\nMUNCHENER STR. 35, GERMANY",
  "paymentDetails": "ACCOMODATION 11-11.02.16  INVOICE\n027/2016",
  "detailsOfCharges": "SHA",
  "createdOn": "2026-04-26T18:11:05.7105868Z"
}
```

---

## 🗄️ Database

The application uses SQLite.

The database file is created automatically when the application starts:

```txt
swift_message.db
```

The `SwiftMessages` table is also created automatically if it does not already exist.

### Stored Fields

```txt
Id
RawMessage
TransactionReference
BankOperationCode
ValueDate
Currency
Amount
OrderingCustomer
BeneficiaryCustomer
PaymentDetails
DetailsOfCharges
CreatedOn
```

---

## 📝 Logging

Logging is implemented with NLog.

Log files are created in:

```txt
logs/app-log-yyyy-MM-dd.txt
```

Example log entries:

```txt
Started processing MT103 message.
MT103 message saved successfully. Id: 1, TransactionReference: 160216270075956
Getting all saved MT103 messages.
```

---

## 📁 Project Structure

```txt
SwiftMt103Parser.Api/
├── Controllers
│   └── SwiftMessagesController.cs
├── Data
│   ├── DatabaseInitializer.cs
│   └── SwiftMessageRepository.cs
├── DTOs
│   ├── CreateSwiftMessageRequest.cs
│   └── SwiftMessageResponse.cs
├── Models
│   └── SwiftMessage.cs
├── Services
│   ├── SwiftParserService.cs
│   └── SwiftMessageService.cs
├── appsettings.json
├── nlog.config
├── Program.cs
└── README.md
```

---

## 🔐 Security

This project does not include Authentication or Authorization.

This is intentional because the technical task explicitly requires:

```txt
Without Authorization & Authentication
```

---

## 📄 Notes

This project was built as a technical task for a .NET Developer Intern position.

The main goal was to keep the implementation simple, readable, and easy to explain while covering all required functionality.

---

## ✅ Project Status

✔️ Web API working  
✔️ File upload working  
✔️ Raw text processing working  
✔️ MT103 parsing working  
✔️ SQLite storage working  
✔️ Manual SQL queries without Entity Framework  
✔️ Swagger testing available  
✔️ NLog logging implemented  

---

## 📄 License

MIT License

---

## 📬 Contact

Viktor Borisov  
📧 vsborisov7@gmail.com  

🔗 GitHub: https://github.com/Viktorborisovv/SwiftMt103Parser
