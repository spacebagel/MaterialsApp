﻿using MaterialsApp.Pages;
using System.Windows;

namespace MaterialsApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            baseFrame.Navigate(new MaterialPage());
        }
    }
}
