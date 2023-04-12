using Microsoft.Data.SqlClient;
using System.Data;

namespace AdvanceAjaxCRUD.Data
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Connect()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public void Disconnect()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            Connect();

            SqlCommand command = new SqlCommand(query, _connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            Disconnect();

            return table;
        }

        public int ExecuteNonQuery(string query)
        {
            Connect();

            SqlCommand command = new SqlCommand(query, _connection);
            int result = command.ExecuteNonQuery();

            Disconnect();

            return result;
        }
    }
}
