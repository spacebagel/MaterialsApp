using MaterialsApp.Models;
using System.Windows;

namespace MaterialsApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MittenMaterialEntities context = new MittenMaterialEntities();
    }
}
