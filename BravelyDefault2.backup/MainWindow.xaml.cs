﻿using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BravelyDefault2 {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            if(dlg.ShowDialog() == false) {
                return;
            }

            SaveData.Instance().Open(dlg.FileName);
            DataContext = new ViewModel();
        }

        private void MenuItemFileSave_Click(object sender, RoutedEventArgs e) {
            SaveData.Instance().Save();
        }

        private void MenuItemFileSaveAs_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog dlg = new SaveFileDialog();
            if(dlg.ShowDialog() == false) {
                return;
            }

            SaveData.Instance().SaveAs(dlg.FileName);
        }

        private void MenuItemFileImport_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            if(dlg.ShowDialog() == false) {
                return;
            }

            SaveData.Instance().Import(dlg.FileName);
        }

        private void MenuItemFileExport_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog dlg = new SaveFileDialog();
            if(dlg.ShowDialog() == false) {
                return;
            }

            SaveData.Instance().Export(dlg.FileName);
        }

        private void MenuItemFileExit_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}