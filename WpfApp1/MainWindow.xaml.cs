using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using WpfApp1.View;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DbContextOptions<ApplicationContext> options;

        // Коллекции автомобилей, клиентов и заказов
        ObservableCollection<Car> cars = new ObservableCollection<Car>();
        ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
        ObservableCollection<Order> orders = new ObservableCollection<Order>();

        public MainWindow()
        {
            InitializeComponent();

            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();
            // получаем строку подключения
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            options = optionsBuilder.UseSqlServer(connectionString).Options;

        }


        /// <summary>
        /// Загрузка автомобилей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btLoad_Click(object sender, RoutedEventArgs e)
        {
            //using (ApplicationContext db = new ApplicationContext(options))
            //{
            //    var cars = db.Cars.ToList();
            //    lvCar.ItemsSource = cars;
            //}

            ProgressBarCar.Visibility = Visibility.Visible;

            //ObservableCollection<Car> listCars = new ObservableCollection<Car>();

            Task tc = Task.Run(() =>
            {
                using (ApplicationContext db = new ApplicationContext(options))
                {
                    var query = db.Cars;
                    if (query.Count() != 0)
                    {
                        foreach (var c in query)
                            cars.Add(c);
                    }
                }
            });

            await tc;

            await Task.Run(() => Thread.Sleep(200));

            ProgressBarCar.Visibility = Visibility.Collapsed;
            lvCar.ItemsSource = cars;
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btLoadCust_Click(object sender, RoutedEventArgs e)
        {
            //using (ApplicationContext db = new ApplicationContext(options))
            //{
            //    var custs = db.Customers.ToList();
            //    lvCustomer.ItemsSource = custs;
            //}

            ProgressBarCustomer.Visibility = Visibility.Visible;

            customers.Clear();

            Task tc = Task.Run(() =>
            {
                using (ApplicationContext db = new ApplicationContext(options))
                {
                    var query = db.Customers;
                    if (query.Count() != 0)
                    {
                        foreach (var c in query)
                            customers.Add(c);
                    }
                }
            });

            await tc;

            await Task.Run(() => Thread.Sleep(200));

            ProgressBarCustomer.Visibility = Visibility.Collapsed;
            lvCustomer.ItemsSource = customers;
        }

        private void btAddCust_Click(object sender, RoutedEventArgs e)
        {
            Customer newCustomer = new Customer();
            CustomerView editCustomerView = new CustomerView();
            editCustomerView.Title = "Добавление нового клиента";
            editCustomerView.DataContext = newCustomer;
            editCustomerView.ShowDialog();

            if (editCustomerView.DialogResult == true)
            {
                using (ApplicationContext db = new ApplicationContext(options))
                {
                    try
                    {
                        db.Customers.Add(newCustomer);
                        db.SaveChanges();
                        customers.Add(newCustomer);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("\nОшибка добавления данных!\n" + ex.Message, "Предупреждение");
                    }
                }
            }

        }

        private void btEditCust_Click(object sender, RoutedEventArgs e)
        {
            Customer editCust = (Customer)lvCustomer.SelectedItem;

            CustomerView editCustomerView = new CustomerView();
            editCustomerView.Title = "Редактирование данных по клиенту";
            editCustomerView.DataContext = editCust;
            editCustomerView.ShowDialog();

            

            if (editCustomerView.DialogResult == true)
            {
                using (ApplicationContext db = new ApplicationContext(options))
                {
                    Customer cust = db.Customers.Find(editCust.Id);
                    if (cust.FirstName != editCust.FirstName)
                        cust.FirstName = editCust.FirstName.Trim();
                    if (cust.LastName != editCust.LastName)
                        cust.LastName = editCust.LastName.Trim();

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("\nОшибка редактирования данных!\n" + ex.Message, "Предупреждение");
                    }
                }
            }
            
        }

        private void btDeleteCust_Click(object sender, RoutedEventArgs e)
        {
            Customer delCustomer = (Customer)lvCustomer.SelectedItem;

            using (ApplicationContext db = new ApplicationContext(options))
            {
                // Поиск в контексте удаляемого автомобиля
                Customer delCust = db.Customers.Find(delCustomer.Id);

                if (delCust != null)
                {
                    MessageBoxResult result = MessageBox.Show("Удалить данные по клиенту: \n" + delCust.LastName + "  " + delCust.FirstName,
                  "Предупреждение", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        try
                        {
                            db.Customers.Remove(delCust);
                            db.SaveChanges();
                            customers.Remove(delCustomer);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("\nОшибка удаления данных!\n" + ex.Message, "Предупреждение");
                        }
                    }
                }
            }
        }

        private async void btLoadOrd_Click(object sender, RoutedEventArgs e)
        {
            //using (ApplicationContext db = new ApplicationContext(options))
            //{
            //    var ords = db.Orders
            //        .Include(c => c.Car)
            //        .Include(p => p.Cust)
            //        .ToList();

            //    lvOrder.ItemsSource = ords;
            //}

            ProgressBarOrder.Visibility = Visibility.Visible;

            Task tc = Task.Run(() =>
            {
                using (ApplicationContext db = new ApplicationContext(options))
                {
                    var query = db.Orders
                    .Include(c => c.Car)
                    .Include(p => p.Cust);
                    if (query.Count() != 0)
                    {
                        foreach (var o in query)
                            orders.Add(o);
                    }
                }
            });

            await tc;

            await Task.Run(() => Thread.Sleep(200));

            ProgressBarOrder.Visibility = Visibility.Collapsed;
            lvOrder.ItemsSource = orders;
        }

        private void btAddOrd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btEditOrd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btDeleteOrd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
