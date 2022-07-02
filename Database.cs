using Encryptor.Services;
using System.Data.SqlClient;

namespace Encryptor
{
    public class Database
    {
        public Database(string connectionString, string tableDetails, IEncryptor encryptorService)
        {
            this.EncryptorService = encryptorService ?? throw new ArgumentNullException(nameof(encryptorService));
            this.TableDetails = tableDetails ?? throw new ArgumentNullException(nameof(tableDetails));
            this.Connection = new SqlConnection(connectionString);
        }

        private SqlConnection Connection { get; set; }

        private IEncryptor EncryptorService { get; set; }

        private string TableDetails { get; set; }

        public async Task EncryptColumn(string columnName, string condition)
        {
            var selectQuery = this.ConstructSelectQuery(condition);
            using (SqlConnection connection = this.Connection)
            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            var data = reader[columnName].ToString();
                            if (string.IsNullOrWhiteSpace(data))
                            {
                                continue;
                            }

                            var encryptedData = this.EncryptorService.Encrypt(data);
                            var query = this.ConstructUpdateQuery(columnName, data, encryptedData);
                            var updateCommand = new SqlCommand(query, connection);
                            updateCommand.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                }
            }
        }

        private string ConstructSelectQuery(string condition)
        {
            condition = string.IsNullOrWhiteSpace(condition) ? string.Empty : $"WHERE {condition}";
            var query = $"SELECT * FROM {TableDetails} {condition}";
            return query;
        }

        private string ConstructUpdateQuery(string columnName, string dataToBeUpdated, string updatedData)
        {
            var query = $"UPDATE {TableDetails} SET {columnName} = {updatedData} WHERE {columnName} = {dataToBeUpdated}";
            return query;
        }
    }
}
