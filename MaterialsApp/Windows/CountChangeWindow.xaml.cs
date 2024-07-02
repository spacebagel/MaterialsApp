using MaterialsApp.Models;
using System;
using System.Collections;
using System.Linq;
using System.Windows;

namespace MaterialsApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для CountChangeWindow.xaml
    /// </summary>
    public partial class CountChangeWindow : Window
    {
        private static IList products;
        public CountChangeWindow(IList productsList)
        {
            InitializeComponent();
            products = productsList;
        }

        private void SaveBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var product in products)
                {
                    var pr = product as Material;
                    var product_ = App.context.Materials.FirstOrDefault(x => x.Id == pr.Id);
                    product_.MinQuantity = Convert.ToInt32(tbNewValue.Text);
                }

                App.context.SaveChanges();
                this.Hide();
            }
            catch { MessageBox.Show("Ошибка"); }
        }
    }
}
