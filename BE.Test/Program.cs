using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Windows;
using BE.Data.Model;

namespace BE.Test
{
    class Program
    {
        public static string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=p:\Users\jesse\Documents\Visual Studio 2017\Projects\BiopropertiesEmployees\BE.Data\Files\Employee Database.mdb";
        static void Main(string[] args)
        {
            do
            {
                //SetSettingsLocation();
                GetTables();
                GetColumnsForTable("tblMain");
                GetData("tblMain");

            }
            while (Console.ReadLine() != "x");
        }

        public static void GetData(string table)
        {
            Console.WriteLine("----------------------------data----------------------------");

            string query = "SELECT * FROM " + table;
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();
                bool firstpass = true;
                while (reader.Read() && firstpass)
                {
                    //for (int i = 0; i < reader.FieldCount; i++)
                    //{
                    //    Console.WriteLine(reader[i].ToString());
                    //}

                    Employee e = new Employee(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(),
                            reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), reader[9].ToString(), reader[10].ToString(),
                            reader[11].ToString(), reader[12].ToString(), reader[13].ToString(), reader[14].ToString(), reader[15].ToString(), reader[16].ToString(),
                            reader[17].ToString(), reader[18].ToString(), reader[19].ToString(), reader[20].ToString(), reader[21].ToString(), reader[22].ToString(),
                            reader[23].ToString());

                    Console.WriteLine(e.ToString());
                    //Calls.Add(call);
                    firstpass = false;
                }
                reader.Close();
                connection.Close();
            }

            //sort decending
            //Calls.Reverse();

            //dgCalls.ItemsSource = null;
            //dgCalls.ItemsSource = Calls.ToList();
        }

        public static List<string> GetColumnsForTable(string tableName)
        {
            Console.WriteLine("---------------------------columns--------------------------");

            List<string> columnNames = new List<string>();
            using (var con = new OleDbConnection(connectionString))
            {
                con.Open();
                using (var cmd = new OleDbCommand("select * from " + tableName, con))
                using (var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    var table = reader.GetSchemaTable();
                    var nameCol = table.Columns["ColumnName"];

                    foreach (DataRow row in table.Rows)
                    {
                        Console.WriteLine(row[nameCol]);
                        columnNames.Add(row[nameCol].ToString());
                    }
                }
            }

            return columnNames;
        }

        public static List<string> GetTables()
        {
            Console.WriteLine("---------------------------tables---------------------------");

            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            DataTable userTables = null;
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                // We only want user tables, not system tables
                string[] restrictions = new string[4];
                restrictions[3] = "Table";
                connection.Open();
                userTables = connection.GetSchema("Tables", restrictions);
            }

            List<string> tableNames = new List<string>();
            for (int i = 0; i < userTables.Rows.Count; i++)
            {
                Console.WriteLine(userTables.Rows[i][2].ToString());
                tableNames.Add(userTables.Rows[i][2].ToString());
            }

            return tableNames;
        }

        public static void SetSettingsLocation()
        {
            if (Directory.Exists("Data") == false)
            {
                Directory.CreateDirectory("Data");
            }

            System.IO.File.WriteAllText("Data\\config.txt", "file location");
            MessageBox.Show("Database location has been set", "Success");
        }
    }
}
