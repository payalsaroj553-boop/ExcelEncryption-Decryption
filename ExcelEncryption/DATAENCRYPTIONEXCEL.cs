using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace ExcelEncryption
{
    public partial class DATAENCRYPTIONEXCEL : Form
    {
        string connectionString =
            @"Data Source=LAPTOP-RLB4PQE5\SQLEXPRESS;
              Initial Catalog=WPFDB;
              Integrated Security=True";

        private readonly string encryptionKey =
            "1234567890123456";

        public DATAENCRYPTIONEXCEL()
        {
            InitializeComponent();
        }


        private void btnsave_Click(object sender, EventArgs e)
        {
            // First validate all rows
            foreach (DataGridViewRow row in dgvExcel.Rows)
            {
                if (row.IsNewRow)
                    continue;


                // -------------------------------------------------
                // ID validation
                // -------------------------------------------------
                if (row.Cells["ID"].Value == null ||
                    string.IsNullOrWhiteSpace(
                        row.Cells["ID"].Value.ToString()))
                {
                    MessageBox.Show("ID cannot be empty.");
                    return;
                }


                // -------------------------------------------------
                // Mobile Number
                // -------------------------------------------------
                string mobileNo = row.Cells["MobileNo"].Value == null
                    ? ""
                    : row.Cells["MobileNo"].Value.ToString().Trim();

                // Remove +91 if present
                if (mobileNo.StartsWith("+91"))
                {
                    mobileNo = mobileNo.Substring(3);
                }

                // Remove spaces
                mobileNo = mobileNo.Replace(" ", "");


                // Keep digits only
                string mobileDigits = "";

                foreach (char c in mobileNo)
                {
                    if (char.IsDigit(c))
                    {
                        mobileDigits += c;
                    }
                }


                // At least 10 digits required
                if (mobileDigits.Length < 10)
                {
                    MessageBox.Show(
                        "Mobile Number must contain at least 10 digits.");

                    return;
                }


                // Only first 10 digits
                mobileNo = mobileDigits.Substring(0, 10);


                // -------------------------------------------------
                // Email validation
                // -------------------------------------------------
                string email = row.Cells["Email"].Value == null
                    ? ""
                    : row.Cells["Email"].Value.ToString().Trim();

                if (!email.Contains("@") ||
                    !email.Contains("."))
                {
                    MessageBox.Show(
                        "Please enter a valid Email address.");

                    return;
                }


                // -------------------------------------------------
                // Landline Number
                // -------------------------------------------------
                string landlineNo =
                    row.Cells["LandlineNo"].Value == null
                    ? ""
                    : row.Cells["LandlineNo"].Value.ToString().Trim();

                // Remove spaces
                landlineNo = landlineNo.Replace(" ", "");


                // Keep digits only
                string landlineDigits = "";

                foreach (char c in landlineNo)
                {
                    if (char.IsDigit(c))
                    {
                        landlineDigits += c;
                    }
                }


                // Maximum 12 digits
                if (landlineDigits.Length > 12)
                {
                    landlineDigits =
                        landlineDigits.Substring(0, 12);
                }

                landlineNo = landlineDigits;
            }


            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                con.Open();

                SqlTransaction transaction =
                    con.BeginTransaction();

                try
                {
                    // ------------------------------------------------
                    // Delete old data
                    // ------------------------------------------------
                    string deleteQuery =
                        "DELETE FROM ExcelEncryption";

                    using (SqlCommand deleteCmd =
                        new SqlCommand(
                            deleteQuery,
                            con,
                            transaction))
                    {
                        deleteCmd.ExecuteNonQuery();
                    }


                    foreach (DataGridViewRow row in dgvExcel.Rows)
                    {
                        if (row.IsNewRow)
                            continue;


                        string id =
                            row.Cells["ID"].Value.ToString();

                        string name =
                            row.Cells["Name"].Value.ToString();

                        string address =
                            row.Cells["Address"].Value.ToString();


                        // Mobile
                        string mobileNo =
                            row.Cells["MobileNo"].Value == null
                            ? ""
                            : row.Cells["MobileNo"].Value.ToString()
                                .Trim();

                        if (mobileNo.StartsWith("+91"))
                        {
                            mobileNo =
                                mobileNo.Substring(3);
                        }

                        mobileNo =
                            mobileNo.Replace(" ", "");


                        string mobileDigits = "";

                        foreach (char c in mobileNo)
                        {
                            if (char.IsDigit(c))
                            {
                                mobileDigits += c;
                            }
                        }

                        mobileNo =
                            mobileDigits.Substring(0, 10);


                        // Account
                        string accountNo =
                            row.Cells["A/C No"].Value.ToString();


                        // Email
                        string email =
                            row.Cells["Email"].Value.ToString();


                        // Landline
                        string landlineNo =
                            row.Cells["LandlineNo"].Value == null
                            ? ""
                            : row.Cells["LandlineNo"].Value.ToString()
                                .Trim();

                        landlineNo =
                            landlineNo.Replace(" ", "");


                        string landlineDigits = "";

                        foreach (char c in landlineNo)
                        {
                            if (char.IsDigit(c))
                            {
                                landlineDigits += c;
                            }
                        }


                        // Maximum 12 digits
                        if (landlineDigits.Length > 12)
                        {
                            landlineDigits =
                                landlineDigits.Substring(0, 12);
                        }

                        landlineNo = landlineDigits;


                        // ------------------------------------------------
                        // Encrypt sensitive data
                        // ------------------------------------------------
                        string encryptedMobileNo =
                            Encrypt(mobileNo);

                        string encryptedAccountNo =
                            Encrypt(accountNo);

                        string encryptedEmail =
                            Encrypt(email);

                        string encryptedLandlineNo =
                            Encrypt(landlineNo);


                        // ------------------------------------------------
                        // Stored Procedure
                        // ------------------------------------------------
                        using (SqlCommand cmd =
                            new SqlCommand(
                                "USP_InsertExcelEncryption",
                                con,
                                transaction))
                        {
                            cmd.CommandType =
                                CommandType.StoredProcedure;


                            cmd.Parameters.AddWithValue(
                                "@ID",
                                Convert.ToInt32(id));

                            cmd.Parameters.AddWithValue(
                                "@Name",
                                name);

                            cmd.Parameters.AddWithValue(
                                "@Address",
                                address);

                            cmd.Parameters.AddWithValue(
                                "@MobileNo",
                                encryptedMobileNo);

                            cmd.Parameters.AddWithValue(
                                "@AccountNo",
                                encryptedAccountNo);

                            cmd.Parameters.AddWithValue(
                                "@Email",
                                encryptedEmail);

                            cmd.Parameters.AddWithValue(
                                "@LandlineNo",
                                encryptedLandlineNo);


                            cmd.ExecuteNonQuery();
                        }
                    }


                    // ------------------------------------------------
                    // Commit
                    // ------------------------------------------------
                    transaction.Commit();

                    MessageBox.Show(
                        "Data encrypted and saved successfully.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    MessageBox.Show(
                        "Data could not be saved.\n\nError: "
                        + ex.Message);
                }
            }
        }


        // =========================================================
        // READ EXCEL BUTTON
        // =========================================================
        private void btnReadExcel_Click(object sender, EventArgs e)
        {
            string filePath =
                @"D:\WPF\ExcelEncryption\EXCELFILE_READ.xlsx";


            if (!File.Exists(filePath))
            {
                MessageBox.Show("Excel file not found.");
                return;
            }


            string excelConnectionString =
                "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + filePath + ";" +
                "Extended Properties='Excel 12.0 Xml;HDR=YES;';";


            using (OleDbConnection con =
                new OleDbConnection(excelConnectionString))
            {
                con.Open();


                DataTable sheets =
                    con.GetOleDbSchemaTable(
                        OleDbSchemaGuid.Tables,
                        null);


                string sheetName =
                    sheets.Rows[0]["TABLE_NAME"].ToString();


                string query =
                    "SELECT * FROM [" + sheetName + "]";


                using (OleDbDataAdapter da =
                    new OleDbDataAdapter(query, con))
                {
                    DataTable dt =
                        new DataTable();

                    da.Fill(dt);


                    DataTable temp =
                        new DataTable();


                    temp.Columns.Add(
                        "ID",
                        typeof(string));

                    temp.Columns.Add(
                        "Name",
                        typeof(string));

                    temp.Columns.Add(
                        "Address",
                        typeof(string));

                    temp.Columns.Add(
                        "MobileNo",
                        typeof(string));

                    temp.Columns.Add(
                        "A/C No",
                        typeof(string));

                    temp.Columns.Add(
                        "Email",
                        typeof(string));

                    temp.Columns.Add(
                        "LandlineNo",
                        typeof(string));


                    foreach (DataRow excelRow in dt.Rows)
                    {
                        DataRow newRow =
                            temp.NewRow();


                        newRow["ID"] =
                            excelRow["ID"].ToString();

                        newRow["Name"] =
                            excelRow["Name"].ToString();

                        newRow["Address"] =
                            excelRow["Address"].ToString();

                        newRow["MobileNo"] =
                            excelRow["MobileNo"].ToString();

                        newRow["A/C No"] =
                            excelRow["A/C No"].ToString();

                        newRow["Email"] =
                            excelRow["Email"].ToString();

                        newRow["LandlineNo"] =
                            excelRow["LandlineNo"].ToString();


                        temp.Rows.Add(newRow);
                    }


                    dgvExcel.DataSource =
                        temp;
                }
            }


            MessageBox.Show(
                "Excel data read successfully.");
        }


        // =========================================================
        // ENCRYPTION METHOD
        // =========================================================
        private string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key =
                    Encoding.UTF8.GetBytes(
                        encryptionKey);

                aes.IV =
                    new byte[16];


                ICryptoTransform encryptor =
                    aes.CreateEncryptor(
                        aes.Key,
                        aes.IV);


                byte[] data =
                    Encoding.UTF8.GetBytes(
                        plainText);


                byte[] encryptedData =
                    encryptor.TransformFinalBlock(
                        data,
                        0,
                        data.Length);


                return Convert.ToBase64String(
                    encryptedData);
            }
        }


        // =========================================================
        // DECRYPTION METHOD
        // =========================================================
        private string Decrypt(string cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key =
                    Encoding.UTF8.GetBytes(
                        encryptionKey);

                aes.IV =
                    new byte[16];


                ICryptoTransform decryptor =
                    aes.CreateDecryptor(
                        aes.Key,
                        aes.IV);


                byte[] encryptedData =
                    Convert.FromBase64String(
                        cipherText);


                byte[] decryptedData =
                    decryptor.TransformFinalBlock(
                        encryptedData,
                        0,
                        encryptedData.Length);


                return Encoding.UTF8.GetString(
                    decryptedData);
            }
        }


        // =========================================================
        // DECRYPT BUTTON
        // =========================================================
        private void btndecrypt_Click(object sender, EventArgs e)
        {
            string encryptedConnectionString =
                @"Data Source=LAPTOP-RLB4PQE5\SQLEXPRESS;
                  Initial Catalog=WPFDB;
                  Integrated Security=True";


            string decryptedConnectionString =
                @"Data Source=LAPTOP-RLB4PQE5\SQLEXPRESS;
                  Initial Catalog=ExcelDecryptionDB;
                  Integrated Security=True";


            using (SqlConnection encryptedCon =
                new SqlConnection(
                    encryptedConnectionString))
            {
                encryptedCon.Open();


                using (SqlConnection decryptedCon =
                    new SqlConnection(
                        decryptedConnectionString))
                {
                    decryptedCon.Open();


                    SqlTransaction transaction =
                        decryptedCon.BeginTransaction();


                    try
                    {
                        // ---------------------------------------------
                        // Delete old decrypted data
                        // ---------------------------------------------
                        string deleteQuery =
                            "DELETE FROM ExcelDecryption";


                        using (SqlCommand deleteCmd =
                            new SqlCommand(
                                deleteQuery,
                                decryptedCon,
                                transaction))
                        {
                            deleteCmd.ExecuteNonQuery();
                        }


                        // ---------------------------------------------
                        // Read encrypted data
                        // ---------------------------------------------
                        string selectQuery = @"
                            SELECT ID,
                                   Name,
                                   Address,
                                   MobileNo,
                                   AccountNo,
                                   Email,
                                   LandlineNo
                            FROM ExcelEncryption";


                        using (SqlCommand selectCmd =
                            new SqlCommand(
                                selectQuery,
                                encryptedCon))
                        {
                            using (SqlDataReader reader =
                                selectCmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int id =
                                        Convert.ToInt32(
                                            reader["ID"]);


                                    string name =
                                        reader["Name"].ToString();


                                    string address =
                                        reader["Address"].ToString();


                                    string encryptedMobileNo =
                                        reader["MobileNo"].ToString();


                                    string encryptedAccountNo =
                                        reader["AccountNo"].ToString();


                                    string encryptedEmail =
                                        reader["Email"].ToString();


                                    string encryptedLandlineNo = "";


                                    if (reader["LandlineNo"] !=
                                        DBNull.Value)
                                    {
                                        encryptedLandlineNo =
                                            reader["LandlineNo"]
                                            .ToString();
                                    }


                                    // --------------------------------
                                    // Decrypt
                                    // --------------------------------
                                    string mobileNo =
                                        Decrypt(
                                            encryptedMobileNo);


                                    string accountNo =
                                        Decrypt(
                                            encryptedAccountNo);


                                    string email =
                                        Decrypt(
                                            encryptedEmail);


                                    string landlineNo = "";


                                    if (!string.IsNullOrWhiteSpace(
                                        encryptedLandlineNo))
                                    {
                                        landlineNo =
                                            Decrypt(
                                                encryptedLandlineNo);
                                    }


                                    // --------------------------------
                                    // Mobile
                                    // Only 10 digits
                                    // --------------------------------
                                    mobileNo =
                                        mobileNo.Trim();


                                    if (mobileNo.StartsWith("+91"))
                                    {
                                        mobileNo =
                                            mobileNo.Substring(3);
                                    }


                                    mobileNo =
                                        mobileNo.Replace(" ", "");


                                    string mobileDigits = "";


                                    foreach (char c in mobileNo)
                                    {
                                        if (char.IsDigit(c))
                                        {
                                            mobileDigits += c;
                                        }
                                    }


                                    if (mobileDigits.Length >= 10)
                                    {
                                        mobileNo =
                                            mobileDigits.Substring(
                                                0,
                                                10);
                                    }
                                    else
                                    {
                                        mobileNo = "";
                                    }


                                    // --------------------------------
                                    // Landline
                                    // Maximum 12 digits
                                    // --------------------------------
                                    landlineNo =
                                        landlineNo.Trim();

                                    landlineNo =
                                        landlineNo.Replace(
                                            " ",
                                            "");


                                    string landlineDigits = "";


                                    foreach (char c in landlineNo)
                                    {
                                        if (char.IsDigit(c))
                                        {
                                            landlineDigits += c;
                                        }
                                    }


                                    if (landlineDigits.Length > 12)
                                    {
                                        landlineDigits =
                                            landlineDigits.Substring(
                                                0,
                                                12);
                                    }


                                    landlineNo =
                                        landlineDigits;


                                    // --------------------------------
                                    // Insert into Decryption table
                                    // --------------------------------
                                    string insertQuery = @"
                                        INSERT INTO ExcelDecryption
                                        (
                                            ID,
                                            Name,
                                            Address,
                                            MobileNo,
                                            AccountNo,
                                            Email,
                                            LandlineNo
                                        )
                                        VALUES
                                        (
                                            @ID,
                                            @Name,
                                            @Address,
                                            @MobileNo,
                                            @AccountNo,
                                            @Email,
                                            @LandlineNo
                                        )";


                                    using (SqlCommand insertCmd =
                                        new SqlCommand(
                                            insertQuery,
                                            decryptedCon,
                                            transaction))
                                    {
                                        insertCmd.Parameters.AddWithValue(
                                            "@ID",
                                            id);

                                        insertCmd.Parameters.AddWithValue(
                                            "@Name",
                                            name);

                                        insertCmd.Parameters.AddWithValue(
                                            "@Address",
                                            address);

                                        insertCmd.Parameters.AddWithValue(
                                            "@MobileNo",
                                            mobileNo);

                                        insertCmd.Parameters.AddWithValue(
                                            "@AccountNo",
                                            accountNo);

                                        insertCmd.Parameters.AddWithValue(
                                            "@Email",
                                            email);

                                        // IMPORTANT:
                                        // @LandlineNo spelling
                                        // must be exactly same
                                        insertCmd.Parameters.AddWithValue(
                                            "@LandlineNo",
                                            landlineNo);


                                        insertCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }


                        transaction.Commit();


                        MessageBox.Show(
                            "Data decrypted and saved successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();


                        MessageBox.Show(
                            "Data could not be decrypted.\n\nError: "
                            + ex.Message);
                    }
                }
            }
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}