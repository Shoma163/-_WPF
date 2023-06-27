using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace Экзамен_WPF
{
    public class Connection
    {
        public static NpgsqlConnection connection;
        public static void Connect(string host, string port, string user, string pass, string database)
        {
            string cs = string.Format("Server = {0}; Port = {1}; User Id = {2}; Password = {3}; Database = {4}", host, port, user, pass, database);

            connection = new NpgsqlConnection(cs);
            connection.Open();
        }

        public static NpgsqlCommand GetCommand(string sql)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = sql;
            return command;
        }


        public static ObservableCollection<ClassCategory> classCategories { get; set; } = new ObservableCollection<ClassCategory>();
        public static ObservableCollection<ClassClient> classClients { get; set; } = new ObservableCollection<ClassClient>();
        public static ObservableCollection<ClassManufacturer> classManufacturers { get; set; } = new ObservableCollection<ClassManufacturer>();
        public static ObservableCollection<ClassOrder> classOrders { get; set; } = new ObservableCollection<ClassOrder>();
        public static ObservableCollection<ClassProduct> classProducts { get; set; } = new ObservableCollection<ClassProduct>();

        public static void SelectTableCategory()
        {
            NpgsqlCommand cmd = GetCommand("SELECT \"NameCategory\" FROM \"Category\"");
            NpgsqlDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    classCategories.Add(new ClassCategory(result.GetString(0)));
                }

            }
            result.Close();
        }

        public static void SelectTableClient()
        {
            NpgsqlCommand cmd = GetCommand("SELECT \"Name\" FROM \"Client\"");
            NpgsqlDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    classClients.Add(new ClassClient(result.GetString(0)));
                }

            }
            result.Close();
        }
        public static void SelectTableManufacturer()
        {
            NpgsqlCommand cmd = GetCommand("SELECT \"id\",\"Name\" FROM \"Manufacturer\"");
            NpgsqlDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    classManufacturers.Add(new ClassManufacturer(result.GetInt32(0),result.GetString(1)));
                }

            }
            result.Close();
        }
        public static void SelectTableOrder()
        {
            NpgsqlCommand cmd = GetCommand("SELECT \"id\",\"product\",\"client\" FROM \"Order\"");
            NpgsqlDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    classOrders.Add(new ClassOrder(result.GetInt32(0),result.GetInt32(1), result.GetString(2)));
                }

            }
            result.Close();
        }
        public static void SelectTableProduct()
        {
            NpgsqlCommand cmd = GetCommand("SELECT \"id\",\"productname\",\"manufacturer\",\"Category\" FROM \"Product\"");
            NpgsqlDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    classProducts.Add(new ClassProduct(result.GetInt32(0), result.GetString(1), result.GetInt32(2), result.GetString(3)));
                }

            }
            result.Close();
        }

        public static void InsertTableProduct(ClassProduct classProduct)
        {
            NpgsqlCommand cmd = GetCommand("INSERT INTO \"Product\" (\"productname\", \"manufacturer\", \"Category\") VALUES (@product, @manufacturer, @category) returning id");
            cmd.Parameters.AddWithValue("@product", NpgsqlDbType.Varchar, classProduct.productname);
            cmd.Parameters.AddWithValue("@manufacturer", NpgsqlDbType.Integer, classProduct.manufacturer);
            cmd.Parameters.AddWithValue("@category", NpgsqlDbType.Varchar, classProduct.Category);

            var result = cmd.ExecuteScalar();
            if (result != null)
            {
                classProduct.id = (int)result;
            }
        }
    }
}
