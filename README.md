# 🔐 Excel Encryption and Decryption

A desktop-based **Excel Encryption and Decryption application** developed using **C#, WPF, ADO.NET, and SQL Server**.

The application reads customer data from an Excel file, displays the data, encrypts sensitive information such as **Mobile Number, Account Number, and Email**, and stores the encrypted data securely in SQL Server. It also provides a **Decrypt** functionality to retrieve and display the original values.

## 📌 Features

- 📂 Read and Import Excel Files
- 📊 Display Excel Data
- 🔐 Encrypt Sensitive Data
- 🔓 Decrypt Encrypted Data
- 📱 Mobile Number Encryption
- 💳 Account Number Encryption
- 📧 Email Encryption
- 💾 Store Data in SQL Server
- 🔄 Retrieve and Decrypt Stored Data
- 🗑️ Replace Existing Database Records
- 🖥️ User-friendly WPF Interface

## 🛠️ Technologies Used

- C#
- WPF
- .NET Framework
- ADO.NET
- SQL Server
- Microsoft Excel
- Visual Studio

## 💾 Database

**Database:** ExcelDecryptionDB

The application uses SQL Server to store encrypted customer information.

### Database Table

**Table:** `ExcelEncryption`

Typical fields include:

- ID
- Name
- Address
- MobileNo
- AccountNo
- Email

Sensitive fields are encrypted before being stored in the database.

## 🔐 Encryption and Decryption Process

### Encryption

1. Select an Excel file.
2. Read the Excel data.
3. Display the data in the application.
4. Encrypt sensitive information.
5. Save the encrypted values into SQL Server.

### Decryption

1. Retrieve encrypted records from SQL Server.
2. Decrypt the encrypted values.
3. Display the original information.

🚀 How to Run
Clone the repository.
Open ExcelEncryption.sln in Visual Studio.
Create the required SQL Server database and table.
Update the SQL Server connection string in App.config.
Build the solution.
Run the application.
Select an Excel file.
Use the application to encrypt and decrypt the required data.

## 📂 Project Structure

```text
ExcelEncryption
│
├── ExcelEncryption
│   ├── Properties
│   ├── App.config
│   ├── DATAENCRYPTIONEXCEL.cs
│   ├── DATAENCRYPTIONEXCEL.Designer.cs
│   ├── DATAENCRYPTIONEXCEL.resx
│   ├── ExcelEncryption.csproj
│   └── Program.cs
│
├── ExcelEncryption.sln
├── .gitignore
└── README.md

🎯 Project Purpose

The main purpose of this project is to demonstrate how sensitive information
can be encrypted before database storage and decrypted when required, using C#, WPF, ADO.NET, and SQL Server.

👩‍💻 Author
Payal Saroj

GitHub:
https://github.com/payalsaroj553-boop
