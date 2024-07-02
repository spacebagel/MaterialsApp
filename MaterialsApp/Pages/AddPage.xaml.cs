using MaterialsApp.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MaterialsApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        private byte[] _imageBytes = null;
        private void BaseConfig()
        {
            InitializeComponent();
            cbTypeProduct.ItemsSource = App.context.Materials.Select(x => x.MaterialType).Distinct().OrderBy(y => y.Id).ToList();
            cbUnitType.ItemsSource = App.context.Materials.Select(x => x.UnitType).Distinct().OrderBy(y => y.Id).ToList();
        }
        private Material currentProduct_;
        public AddPage()
        {
            BaseConfig();
            submitButton.Content = "Добавить";
        }

        public AddPage(Material product)
        {
            BaseConfig();
            submitButton.Content = "Сохранить";
            currentProduct_ = product;
            tbStockQuantity.Text = product.StockQuantity.ToString();
            tbMinQuantity.Text = product.MinQuantity.ToString();
            tbBoxQuantity.Text = product.BoxQuantity.ToString();
            tbName.Text = product.Name;
            //tbDescription.Text = product.Description;
            tbCost.Text = product.Cost.ToString();

            cbTypeProduct.SelectedValue = product.MaterialTypeId;
            cbUnitType.SelectedValue = product.UnitTypeId;

            BitmapImage bitmapImg;
            if (product.Image != null)
            {
                using (var ms = new MemoryStream(product.Image))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    bitmapImg = image;
                }
                productImage.Source = bitmapImg;
            }

        }
        private void CancelButtonClick(object sender, RoutedEventArgs e) => NavigationService.Navigate(new MaterialPage());

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (submitButton.Content == "Добавить")
            {
                try
                {
                    App.context.Materials.Add(new Material
                    {
                        Name = tbName.Text,
                        UnitTypeId = Convert.ToInt32(cbUnitType.SelectedValue),
                        BoxQuantity = int.Parse(tbMinQuantity.Text),
                        MinQuantity = int.Parse(tbMinQuantity.Text),
                        StockQuantity = int.Parse(tbStockQuantity.Text),
                        MaterialTypeId = Convert.ToInt32(cbTypeProduct.SelectedValue),
                        Cost = Convert.ToDecimal(tbCost.Text),
                        Image = _imageBytes
                    });
                    App.context.SaveChanges();
                    MessageBox.Show("Запись успешно сохранена");
                    NavigationService.Navigate(new MaterialPage());
                }
                catch
                {
                    MessageBox.Show("Ошибка добавления");
                }
            }
            else
            {
                try
                {
                    var product = currentProduct_;
                    product.Name = tbName.Text;
                    //product.description = tbDescription.Text;
                    product.StockQuantity = int.Parse(tbStockQuantity.Text);
                    product.MaterialTypeId = Convert.ToInt32(cbTypeProduct.SelectedValue);
                    product.Cost = Convert.ToDecimal(tbCost.Text);
                    product.MinQuantity = int.Parse(tbMinQuantity.Text);
                    product.BoxQuantity = int.Parse(tbBoxQuantity.Text);
                    product.UnitTypeId = Convert.ToInt32(cbUnitType.SelectedValue);
                    product.Image = _imageBytes;
                    App.context.SaveChanges();
                    MessageBox.Show("Запись успешно сохранена");
                    NavigationService.Navigate(new MaterialPage());
                }
                catch
                {
                    MessageBox.Show("Ошибка редактирования");
                }
            }
        }
        private void ConvertImage()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapImage)productImage.Source));
                    encoder.Save(stream);
                    _imageBytes = stream.ToArray();
                }
                catch { }
            }
        }

        private void OpenDialog()
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = "Images (*.jpg,*.png)|*.jpg;*.png|All Files(*.*)|*.*"
            };

            if (dialog.ShowDialog() != true) { return; }
            var path = dialog.FileName;
            productImage.Source = new BitmapImage(new Uri(path));
        }

        private void OpenImageCLick(object sender, MouseButtonEventArgs e)
        {
            OpenDialog();
            ConvertImage();
        }

        private void OpenImageButtonClick(object sender, RoutedEventArgs e)
        {
            OpenDialog();
            ConvertImage();
        }
    }
}
