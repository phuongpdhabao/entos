using ENTOS.Module.BusinessObjects;
namespace ENTOS.Module.Services
{
    public class MySqlManagementService : IMySqlManagementService, SharedKernel.Interfaces.ITransientDependency
    {
        public bool ExecuteNonQuery(string connectString, string commandText)
        {
            using (Devart.Data.MySql.MySqlConnection conn = new Devart.Data.MySql.MySqlConnection(connectString))
            {
                conn.Unicode = true;
                using (Devart.Data.MySql.MySqlCommand cmd = new Devart.Data.MySql.MySqlCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return true;
        }

        public bool CreateDatabase(string connectString, string name)
        {
            string command = $"CREATE DATABASE IF NOT EXISTS `{name}` character set  utf8mb4 COLLATE utf8mb4_vietnamese_ci;";
            return ExecuteNonQuery(connectString, command);
        }

        public bool ExportToFile(string connectString, string fileName)
        {            
            using (Devart.Data.MySql.MySqlConnection conn = new Devart.Data.MySql.MySqlConnection(connectString))
            {
                conn.Unicode = true;
                using (Devart.Data.MySql.MySqlCommand cmd = new Devart.Data.MySql.MySqlCommand())
                {
                    using (Devart.Data.MySql.MySqlBackup mb = new Devart.Data.MySql.MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportInfo.AddCreateDatabase = false;
                        mb.ExportInfo.ExportTableStructure = true;
                        mb.ExportToFile(fileName);
                        conn.Close();
                    }
                }
            }
            return true;
        }
        public bool ImportFromFile(string connectString, string fileName)
        {
            using (Devart.Data.MySql.MySqlConnection conn = new Devart.Data.MySql.MySqlConnection(connectString))
            {
                conn.Unicode = true;
                using (Devart.Data.MySql.MySqlCommand cmd = new Devart.Data.MySql.MySqlCommand())
                {
                    using (Devart.Data.MySql.MySqlBackup mb = new Devart.Data.MySql.MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromFile(fileName);
                        conn.Close();
                    }
                }
            }
            return true;
        }
    }
}
