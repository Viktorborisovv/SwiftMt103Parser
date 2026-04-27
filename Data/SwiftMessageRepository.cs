namespace SwiftMt103Parser.Api.Data
{
    using Microsoft.Data.Sqlite;
    using SwiftMt103Parser.Api.Models;
    using Microsoft.Extensions.Configuration;

    public class SwiftMessageRepository
    {
        private readonly string connectionString;

        public SwiftMessageRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
        }

        public async Task<int> AddAsync(SwiftMessage message)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            string insertQuery = @"
                INSERT INTO SwiftMessages 
                (
                    RawMessage,
                    TransactionReference,
                    BankOperationCode,
                    ValueDate,
                    Currency,
                    Amount,
                    OrderingCustomer,
                    BeneficiaryCustomer,
                    PaymentDetails,
                    DetailsOfCharges,
                    CreatedOn
                )
                VALUES
                (
                    @RawMessage,
                    @TransactionReference,
                    @BankOperationCode,
                    @ValueDate,
                    @Currency,
                    @Amount,
                    @OrderingCustomer,
                    @BeneficiaryCustomer,
                    @PaymentDetails,
                    @DetailsOfCharges,
                    @CreatedOn
                );

                SELECT last_insert_rowid();
            ";

            using SqliteCommand command = new SqliteCommand(insertQuery, connection);

            command.Parameters.AddWithValue("@RawMessage", message.RawMessage);
            command.Parameters.AddWithValue("@TransactionReference", (object?)message.TransactionReference ?? DBNull.Value);
            command.Parameters.AddWithValue("@BankOperationCode", (object?)message.BankOperationCode ?? DBNull.Value);
            command.Parameters.AddWithValue("@ValueDate", (object?)message.ValueDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Currency", (object?)message.Currency ?? DBNull.Value);
            command.Parameters.AddWithValue("@Amount", (object?)message.Amount ?? DBNull.Value);
            command.Parameters.AddWithValue("@OrderingCustomer", (object?)message.OrderingCustomer ?? DBNull.Value);
            command.Parameters.AddWithValue("@BeneficiaryCustomer", (object?)message.BeneficiaryCustomer ?? DBNull.Value);
            command.Parameters.AddWithValue("@PaymentDetails", (object?)message.PaymentDetails ?? DBNull.Value);
            command.Parameters.AddWithValue("@DetailsOfCharges", (object?)message.DetailsOfCharges ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedOn", message.CreatedOn.ToString("O"));

            object? result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task<List<SwiftMessage>> GetAllAsync()
        {
            List<SwiftMessage> messages = new List<SwiftMessage>();

            using SqliteConnection connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            string selectQuery = @"
                SELECT 
                    Id,
                    RawMessage,
                    TransactionReference,
                    BankOperationCode,
                    ValueDate,
                    Currency,
                    Amount,
                    OrderingCustomer,
                    BeneficiaryCustomer,
                    PaymentDetails,
                    DetailsOfCharges,
                    CreatedOn
                FROM SwiftMessages
                ORDER BY Id DESC;
            ";

            using SqliteCommand command = new SqliteCommand(selectQuery, connection);
            using SqliteDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                messages.Add(ReadSwiftMessage(reader));
            }

            return messages;
        }

        public async Task<SwiftMessage?> GetByIdAsync(int id)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            
            string selectQuery = @"
                SELECT 
                    Id,
                    RawMessage,
                    TransactionReference,
                    BankOperationCode,
                    ValueDate,
                    Currency,
                    Amount,
                    OrderingCustomer,
                    BeneficiaryCustomer,
                    PaymentDetails,
                    DetailsOfCharges,
                    CreatedOn
                FROM SwiftMessages
                WHERE Id = @Id;
            ";

            using SqliteCommand command = new SqliteCommand(selectQuery, connection);
            command.Parameters.AddWithValue("@Id", id);

            using SqliteDataReader reader = await command.ExecuteReaderAsync();
            
            
            if (await reader.ReadAsync())
            {
                return ReadSwiftMessage(reader);
            }

            return null;
        }

        private static SwiftMessage ReadSwiftMessage(SqliteDataReader reader)
        {
            return new SwiftMessage
            {
                Id = reader.GetInt32(0),
                RawMessage = reader.GetString(1),
                TransactionReference = reader.IsDBNull(2) ? null : reader.GetString(2),
                BankOperationCode = reader.IsDBNull(3) ? null : reader.GetString(3),
                ValueDate = reader.IsDBNull(4) ? null : reader.GetString(4),
                Currency = reader.IsDBNull(5) ? null : reader.GetString(5),
                Amount = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                OrderingCustomer = reader.IsDBNull(7) ? null : reader.GetString(7),
                BeneficiaryCustomer = reader.IsDBNull(8) ? null : reader.GetString(8),
                PaymentDetails = reader.IsDBNull(9) ? null : reader.GetString(9),
                DetailsOfCharges = reader.IsDBNull(10) ? null : reader.GetString(10),
                CreatedOn = reader.GetDateTime(11)
            };
        }
    }
}
