using ModernWpf;
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

namespace CrayonDiary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly List<Student> students = new();
        public MainWindow()
        {
            // https://github.com/Kinnara/ModernWpf/wiki/
            InitializeComponent();
        }

        private void SetColNames()
        {
            MarkGrid.Columns[0].Header = "Név";
            MarkGrid.Columns[0].Header = "Osztály";
            MarkGrid.Columns[0].Header = "Matematika";
            MarkGrid.Columns[0].Header = "Irodalom";
            MarkGrid.Columns[0].Header = "Biológia";
            MarkGrid.Columns[0].Header = "Testnevelés";
            MarkGrid.Columns[0].Header = "Informatika";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            ThemeManager.Current.AccentColor = Color.FromRgb(53, 126, 199);
            MarkGrid.ItemsSource = students;
            SetColNames();
        }

        private void addStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            students.Add(new Student($"{addNewStudentInput.Text};?;0;0;0;0;0"));
            MarkGrid.ItemsSource = students;
            MarkGrid.Items.Refresh();
            SetColNames();
        }

        private void addStudentClassBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = MarkGrid.SelectedIndex;

            if (selectedIndex >= 0) students.ElementAt(selectedIndex).Class = changeStudentClassInput.Text;
            else MessageBox.Show("Nem választottál ki tanulót!");

            MarkGrid.Items.Refresh();
        }

        private void delStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = MarkGrid.SelectedIndex;

            if (selectedIndex >= 0) students.RemoveAt(selectedIndex);
            else MessageBox.Show("Nem választottál ki tanulót!");

            MarkGrid.Items.Refresh();
        }
    }
}
