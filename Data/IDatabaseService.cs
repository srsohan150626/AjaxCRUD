using System.Data;

namespace AdvanceAjaxCRUD.Data
{
    public interface IDatabaseService
    {
        void Connect();
        void Disconnect();
        DataTable ExecuteQuery(string query);
        int ExecuteNonQuery(string query);
    }
}
