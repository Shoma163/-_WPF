using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Экзамен_WPF
{
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();

            BindingcbManufacturer();
            BindingcbCategory();
        }


        public void BindingcbManufacturer()
        {
            Binding binding = new Binding();
            binding.Source = Connection.classManufacturers;
            cbManufacturer.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            Connection.SelectTableManufacturer();
        }
        public void BindingcbCategory()
        {
            Binding binding = new Binding();
            binding.Source = Connection.classCategories;
            cbCategory.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            Connection.SelectTableCategory();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            string ProductUP = tbProduct.Text.Trim();
            string ManufacturerUP = cbManufacturer.Text.Trim();
            string CategoryUP = cbCategory.Text.Trim();

            NpgsqlCommand command = Connection.GetCommand(
                "UPDATE \"Product\" SET \"productname\"=@productname, \"manufacturer\"=@manufacturer, \"Category\"=@Category returning id");
            try
            {
                command.Parameters.AddWithValue("@surname", NpgsqlDbType.Varchar, ProductUP);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, ManufacturerUP);
                command.Parameters.AddWithValue("@patronymic", NpgsqlDbType.Varchar, CategoryUP);
                int result = (int)command.ExecuteScalar();
                if (result == 1)
                {
                    MessageBox.Show("Success!");
                }
                
                }
                catch (Exception)
                {
                MessageBox.Show("no!");
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel(object sender, RoutedEventArgs e)
        {

        }
    }
}
