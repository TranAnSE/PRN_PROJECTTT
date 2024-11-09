using System;
using System.IO;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace QuanLiCuaHang
{
    public class DatabaseManager
    {
        private const string ConnectionString = "Server=DESKTOP-1VV0QS6;Database=QLCUAHANG;User Id=sa;Password=quocan123;TrustServerCertificate=True;";

        public static void BackupDatabase(string backupPath)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string backupQuery = $"BACKUP DATABASE QLCUAHANG TO DISK = '{backupPath}'";
                    using (SqlCommand command = new SqlCommand(backupQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Sao lưu dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sao lưu dữ liệu: {ex.Message}");
            }
        }

        public static void RestoreDatabase(string backupPath)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string restoreQuery = $@"
                    USE master;
                    ALTER DATABASE QLCUAHANG SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    RESTORE DATABASE QLCUAHANG FROM DISK = '{backupPath}' WITH REPLACE;
                    ALTER DATABASE QLCUAHANG SET MULTI_USER;";
                    using (SqlCommand command = new SqlCommand(restoreQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Khôi phục dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khôi phục dữ liệu: {ex.Message}");
            }
        }
    }
}