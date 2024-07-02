using MaterialsApp.Models;
using MaterialsApp.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MaterialsApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для MaterialPage.xaml
    /// </summary>
    public partial class MaterialPage : Page
    {
        public int maxPages, currentPage = 0;
        public List<Material> itemsSource;
        public MaterialPage()
        {
            InitializeComponent();
            listViewMaterial.ItemsSource = App.context.Materials.ToList();
            listViewMaterial.ItemsSource = App.context.Materials.ToList().Take(15);
            itemsSource = App.context.Materials.ToList();
            maxPages = (App.context.Materials.ToList().Count - 1) / 15;
            
            var materialTypes = App.context.Materials.Select(x => x.MaterialType).Distinct().ToList();
            materialTypes.Insert(0, new MaterialType { Id = -1, Value = "Все типы", Materials = null });
            cbFilter.ItemsSource = materialTypes;
            cbFilter.SelectedIndex = 0;

            isa1.Text = (maxPages+1).ToString();
            isa2.Text = (currentPage+1).ToString();
        }

        private void SortFilterSelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateList();

        private void UpdateList()
        {
            listViewMaterial.ItemsSource = null;
            var filterValue = Convert.ToInt32(cbFilter.SelectedValue);
            var searchText = tbSearch.Text;
            itemsSource = App.context.Materials.ToList();

            if (filterValue > 0)
            {
                itemsSource = itemsSource.Where(x => x.MaterialTypeId == filterValue).ToList();
            }

            if (searchText != null) 
            {
                itemsSource = itemsSource.Where(x => x.Name.ToLower().Contains(searchText)).ToList();
            }

            if (cbSort.SelectedIndex >= 0)
            {
                var selectedId = cbSort.SelectedIndex;
                switch (selectedId)
                {
                    case 0:
                        itemsSource = itemsSource.OrderBy(x => x.Name).ToList();
                        break;
                    case 1:
                        itemsSource = itemsSource.OrderByDescending(x => x.Name).ToList();
                        break;
                    case 2:
                        itemsSource = itemsSource.OrderBy(x => x.StockQuantity).ToList();
                        break;
                    case 3:
                        itemsSource = itemsSource.OrderByDescending(x => x.StockQuantity).ToList();
                        break;
                    case 4:
                        itemsSource = itemsSource.OrderBy(x => x.Cost).ToList();
                        break;
                    case 5:
                        itemsSource = itemsSource.OrderByDescending(x => x.Cost).ToList();
                        break;
                }
            }

            listViewMaterial.ItemsSource = itemsSource.Take(20);
            currentPage = 0;
            pageNumber.Text = "1";

            isa1.Text = (maxPages + 1).ToString();
            isa2.Text = (currentPage + 1).ToString();
        }

        private void PageLogic()
        {
            listViewMaterial.ItemsSource = itemsSource.Skip(15 * currentPage).Take(15).ToList();

            isa1.Text = (maxPages + 1).ToString();
            isa2.Text = (currentPage + 1).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage + 1 <= (itemsSource.Count - 1) / 15 && itemsSource.Count > 15)
            {
                currentPage++;
                pageNumber.Text = (currentPage + 1).ToString();
                PageLogic();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (currentPage - 1 >= 0 && itemsSource.Count > 20)
            {
                currentPage--;
                pageNumber.Text = (currentPage + 1).ToString();
                PageLogic();
            }
        }

        private void OpenEditPageBtnClick(object sender, RoutedEventArgs e)
        {
            if (listViewMaterial.SelectedItems.Count > 0)
            {
                NavigationService.Navigate(new AddPage(listViewMaterial.SelectedItem as Material));
            }
            else
            {
                MessageBox.Show("Выберите продукты", "Внимание!", MessageBoxButton.OK);
            }

        }

        private void OpenAddPageBtnClick(object sender, RoutedEventArgs e) => NavigationService.Navigate(new Uri("Pages/AddPage.xaml", UriKind.RelativeOrAbsolute));

        private void DeleteProductBtnClick(object sender, RoutedEventArgs e)
        {
            if (listViewMaterial.SelectedItems.Count > 0)
            {
               
                    foreach (var i in listViewMaterial.SelectedItems)
                    {
                        var item = i as Material;
                        var product = App.context.Materials.FirstOrDefault(x => x.Id == item.Id);
                        App.context.Materials.Remove(product);
                    }
                App.context.SaveChanges();
                UpdateList();
                MessageBox.Show("Записи удалены");
            }
            else
            {
                MessageBox.Show("Не выбраны элементы");
            }
        }

        private void ChangeCountBtnClick(object sender, RoutedEventArgs e)
        {
            if (listViewMaterial.SelectedItems.Count > 0)
            {
                var productsList = listViewMaterial.SelectedItems;
                CountChangeWindow changeWindows = new CountChangeWindow(productsList);
                var a = changeWindows.ShowDialog();
                if (!a.Value)
                    UpdateList();
            }
            else
            {
                MessageBox.Show("Не выбраны элементы");
            }
        }

        private void SearchBoxTextChanged(object sender, TextChangedEventArgs e) => UpdateList();
    }
}