namespace SwiftMt103Parser.Api.Data
{
    using Microsoft.Data.Sqlite;

    public class DatabaseInitializer
    {
        private readonly string connectionString;

        public DatabaseInitializer(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
        }

        public void Initialize()
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            string sql = @"
                    CREATE TABLE IF NOT EXISTS SwiftMessages (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        RawMessage TEXT NOT NULL,
                        TransactionReference TEXT,
                        BankOperationCode TEXT,
                        ValueDate TEXT,
                        Currency TEXT,
                        Amount REAL,
                        OrderingCustomer TEXT,
                        BeneficiaryCustomer TEXT,
                        PaymentDetails TEXT,
                        DetailsOfCharges TEXT,
                        CreatedOn DATETIME NOT NULL
                );    
            ";

            using SqliteCommand command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
}
