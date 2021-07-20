using System;
using System.Data.SqlClient;

namespace SignalRChatDBCon
{
    public class DbCon
    {
        public static string ConnectionString = @"Server=localhost;Database=SignalRChatDB;Trusted_Connection=True;";
        public SqlConnection Connection = new SqlConnection(ConnectionString);

        public void Connect()
        {
            Connection.Open();
        }
        public void Disconnect()
        {
            Connection.Close();
            Connection.Dispose();
        }
    }
}
