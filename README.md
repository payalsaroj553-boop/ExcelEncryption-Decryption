# 🔐 Excel Encryption and Decryption

A desktop-based **Excel Encryption and Decryption application** developed using **C#, WPF, ADO.NET, and SQL Server**.

The application reads data from an Excel file, displays the records, encrypts sensitive information, stores the encrypted data in a SQL Server database, and provides decryption functionality to retrieve and store the original information.

## 📌 Features

- 📂 Read and Import Excel Files
- 📊 Display Excel Data
- 🔐 Encrypt Sensitive Data
- 🔓 Decrypt Encrypted Data
- 📱 Mobile Number Encryption
- 💳 Account Number Encryption
- 📧 Email Encryption
- 💾 Store Encrypted Data in SQL Server
- 🔄 Retrieve and Decrypt Data
- 🗄️ Separate Database for Encryption and Decryption
- 📋 Separate Tables for Encrypted and Decrypted Data
- 🖥️ WPF-based User Interface

## 🛠️ Technologies Used

- C#
- WPF
- .NET Framework
- ADO.NET
- SQL Server
- Microsoft Excel
- Visual Studio

## 💾 Database

The application uses **two separate SQL Server databases** for encryption and decryption operations.

### 🔐 Encryption Database

**Database Name:** `WPFDB`

This database stores the encrypted customer information.

**Table Name:** `ExcelEncryption`

### 🔓 Decryption Database

**Database Name:** `ExcelDecryptionDB`

This database stores the decrypted customer information.

**Table Name:** `ExcelDecryption`

## 📋 Database Tables

### 🔐 ExcelEncryption Table

The `ExcelEncryption` table stores customer information after sensitive fields have been encrypted.

Typical fields include:

- ID
- Name
- Address
- MobileNo
- AccountNo
- Email

### 🔓 ExcelDecryption Table

The `ExcelDecryption` table stores the customer information after the encrypted values have been decrypted.

Typical fields include:

- ID
- Name
- Address
- MobileNo
- AccountNo
- Email

## 🔐 Encryption Process

1. Select an Excel file.
2. Read the Excel data.
3. Display the records in the application.
4. Encrypt sensitive information such as Mobile Number, Account Number, and Email.
5. Save the encrypted data into the `WPFDB` database.
6. Store the encrypted records in the `ExcelEncryption` table.

## 🔓 Decryption Process

1. Retrieve the encrypted records.
2. Decrypt the encrypted values.
3. Restore the original information.
4. Save the decrypted data into the `ExcelDecryptionDB` database.
5. Store the decrypted records in the `ExcelDecryption` table.

🚀 How to Run
Clone the repository.
Open ExcelEncryption.sln in Visual Studio.
Create the required SQL Server databases:
WPFDB
ExcelDecryptionDB
Create the required tables:
ExcelEncryption
ExcelDecryption
Update the SQL Server connection strings in App.config.
Build the solution.
Run the application.
Select an Excel file.
Use the Encryption functionality to encrypt and store the data.
Use the Decryption functionality to retrieve and store the decrypted data.

## 📂 Project Structure

```text
ExcelEncryption-Decryption
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

🎯 Project Purpose

The main purpose of this project is to demonstrate how sensitive customer information can be
 encrypted before database storage and decrypted when required.

👩‍💻 Author
Payal Saroj

GitHub:
https://github.com/payalsaroj553-boop

├── .gitignore
└── README.md
