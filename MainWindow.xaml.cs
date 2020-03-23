/* #############################
 * 
 * Author: Johnathon Mc Grory
 * Date : 23/3/2020
 * Description : Lab 6 (Linq)
 * 
 * ############################# */
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabSix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NORTHWNDEntities db = new NORTHWNDEntities();
        public MainWindow()
        {
            InitializeComponent();
        }
        //exercise 1
        private void Ex1Button_Click_1(object sender, RoutedEventArgs e)
        {
            var query = from c in db.Categories
                        join p in db.Products on c.CategoryName equals p.Category.CategoryName
                        orderby c.CategoryName
                        select new { Category = c.CategoryName, Product = p.ProductName };

            var results = query.ToList();

            Ex1DvgDisplay.ItemsSource = results;

            Ex1TblkCount.Text = results.Count.ToString();
        }
        //exercise 2
        private void Ex2Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in db.Products
                        orderby p.Category.CategoryName, p.ProductName
                        select new { Category = p.Category.CategoryName, Product = p.ProductName };

            var results = query.ToList();

            Ex2DvgDisplay.ItemsSource = results;

            Ex2TblkCount.Text = results.Count.ToString();
        }
        //exercise 3
        private void Ex3Button_Click(object sender, RoutedEventArgs e)
        {
            var query2 = (from detail in db.Order_Details
                          where detail.ProductID == 7
                          select detail.UnitPrice * detail.Quantity);

            var query1 = (from detail in db.Order_Details
                          where detail.ProductID == 7
                          select detail);

            int numberOfOrders = query1.Count();
            decimal totalValue = query2.Sum();
            decimal averageValue = query2.Average();

            Ex3TblkCount.Text = string.Format(
                "total number of orders: {0}\nValue of Orders: {1:C}\nAverage Order Value: {2:C}",
                numberOfOrders, totalValue, averageValue);
        }
        //exercise 4
        private void Ex4Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in db.Customers
                        where customer.Orders.Count >= 20
                        select new
                        {
                            Name = customer.CompanyName,
                            OrderCount = customer.Orders.Count
                        };
            Ex4DvgDisplay.ItemsSource = query.ToList();
        }
        //exercise 5
        private void Ex5Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in db.Customers
                        where customer.Orders.Count < 3
                        select new
                        {
                            Company = customer.CompanyName,
                            City = customer.City,
                            Region = customer.Region,
                            OrderCount = customer.Orders.Count
                        };
            Ex5DvgDisplay.ItemsSource = query.ToList();
        }
        //exercise 6 (part 1)
        private void Ex6Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in db.Customers
                        orderby customer.CompanyName
                        select customer.CompanyName;
            LbxEx6Customers.ItemsSource = query.ToList();
        }
        //exercise 6 (part 2)
        private void LbxEx6Customers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string company = (string)LbxEx6Customers.SelectedItem;

            if (company != null)
            {
                var query = (from detail in db.Order_Details
                             where detail.Order.Customer.CompanyName == company
                             select detail.UnitPrice * detail.Quantity).Sum();

                Ex6TblkSummary.Text = string.Format("total for supplier {0}\n\n{1:C}", company, query);
            }
        }
        //exercise 7
        private void Ex7Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in db.Products
                        group p by p.Category.CategoryName into g
                        orderby g.Count() descending
                        select new
                        {
                            Category = g.Key,
                            Count = g.Count()
                        };
            Ex7DvgDisplay.ItemsSource = query.ToList();
        }

       
    }
}
