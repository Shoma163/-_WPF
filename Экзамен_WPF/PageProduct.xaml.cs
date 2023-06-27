using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Экзамен_WPF
{
    public class SortItem
    {
        public string Name { get; set; }
        public SortDescription Sort { get; set; }
    }
    public partial class PageProduct : Page
    {
        private ObservableCollection<ClassProduct> products { get; set; }
        public List<SortItem> SortDescriptions { get; set; }
        public SortItem SelectedSortDescription { get; set; }

        public PageProduct()
        {
            InitializeComponent();

            products = new ObservableCollection<ClassProduct>();

            BindingListForm();
            BindingcbManufacturer();
            BindingcbCategory();

            var view = CollectionViewSource.GetDefaultView(lvProducts.ItemsSource);
            if (view == null) { return; }
            view.SortDescriptions.Add(new SortDescription("id", ListSortDirection.Ascending));

            //view.Filter = o =>
            //{
            //    if (String.IsNullOrEmpty(filter.Text))
            //        return true;
            //    return ((o as ClassProduct).productname.IndexOf(filter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            //};

            SortDescriptions = new List<SortItem>()
            {
                new SortItem()
                {
                    Name = "Сортировка по возрастанию",
                    Sort = new SortDescription("id", ListSortDirection.Ascending),
                },
                new SortItem()
                {
                    Name = "Сортировка по убыванию",
                    Sort = new SortDescription("id", ListSortDirection.Descending),
                },
            };

            DataContext = this;
        }

        public void Filter()
        {
            var selectedManufacturer = cbManufacturer.SelectedItem as ClassManufacturer;
            var selectedCategory = cbCategory.SelectedItem as ClassCategory;
            string searchText = filter.Text.Trim();
            var view = CollectionViewSource.GetDefaultView(lvProducts.ItemsSource);

            view.Filter = o =>
            {
                ClassProduct product = o as ClassProduct;
                if (product == null)
                    return false;

                bool isVisible = true;

                if (searchText.Length > 0)
                {
                    isVisible = product.manufacturer.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1;
                }

                if (selectedManufacturer != null)
                {
                    isVisible = isVisible && product.manufacturer == selectedManufacturer.id;
                }

                if (searchText.Length > 0)
                {
                    isVisible = product.Category.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1;
                }

                if (selectedCategory != null)
                {
                    isVisible = isVisible && product.Category == selectedCategory.NameCategory;
                }

                return isVisible;
            };
            //if (String.IsNullOrEmpty(filter.Text))
            //    return true;
            //return ((o as ClassProduct).productname.IndexOf(filter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        


        public void BindingListForm()
        {
            Binding binding = new Binding();
            binding.Source = Connection.classProducts;
            lvProducts.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            Connection.SelectTableProduct();
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

        public void AddProduct()
        {
            string product1 = tbProduct.Text.Trim();

            ClassProduct product = new ClassProduct();

            ClassCategory category1 = cbCategory.SelectedItem as ClassCategory;
            if (category1 == null ) { return; }
            ClassManufacturer manufacturer1 = cbManufacturer.SelectedItem as ClassManufacturer;
            if (manufacturer1 == null) { return; }

            product.productname = product1;
            product.manufacturer = manufacturer1.id;
            product.Category = category1.NameCategory;

            products.Add(product);

            Connection.InsertTableProduct(product);

            Connection.classProducts.Add(product);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddProduct();
            cbCategory.SelectedItem = null;
            cbManufacturer.SelectedItem = null;
            tbProduct.Clear();
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            lvProducts.Items.SortDescriptions.Add(new SortDescription("id", ListSortDirection.Descending));
        }
        private void ApplySort()
        {
            var view = CollectionViewSource.GetDefaultView(lvProducts.ItemsSource);
            if (view == null) return;

            view.SortDescriptions.Clear();

            if (SelectedSortDescription == null) return;
            view.SortDescriptions.Add(SelectedSortDescription.Sort);
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySort();
        }

        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateWindow updateWindow = new UpdateWindow();
            ClassProduct item = lvProducts.SelectedItem as ClassProduct;
            updateWindow.tbProduct.Text = item.productname;
            updateWindow.cbManufacturer.Text = item.manufacturer.ToString();
            updateWindow.cbCategory.Text = item.Category;
            updateWindow.ShowDialog();
        }

        private void filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            //CollectionViewSource.GetDefaultView(lvProducts.ItemsSource).Refresh();
            Filter();
        }

        private void cbManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
            //CollectionViewSource.GetDefaultView(lvProducts.ItemsSource).Refresh();
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
    }
}
