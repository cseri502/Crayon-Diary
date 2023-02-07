using ModernWpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        string[] subjects = { "Matematika", "Irodalom", "Biológia", "Testnevelés", "Informatika" }; 

        public MainWindow()
        {
            // https://github.com/Kinnara/ModernWpf/wiki/
            InitializeComponent();
        }

        private void SetColNames()
        {
            MarkGrid.Columns[0].Header = "Név";
            MarkGrid.Columns[1].Header = "Osztály";
            MarkGrid.Columns[2].Header = "Matematika";
            MarkGrid.Columns[3].Header = "Irodalom";
            MarkGrid.Columns[4].Header = "Biológia";
            MarkGrid.Columns[5].Header = "Testnevelés";
            MarkGrid.Columns[6].Header = "Informatika";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            ThemeManager.Current.AccentColor = Color.FromRgb(53, 126, 199);
            MarkGrid.ItemsSource = students;
            SetColNames();
            subjectsComboBox.ItemsSource = subjects;
            subjectsComboBox.SelectedIndex = 0;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            students.Add(new Student($"{addNewStudentInput.Text};?;0;0;0;0;0"));
            MarkGrid.ItemsSource = students;
            MarkGrid.Items.Refresh();
            SetColNames();
            personIndicator.Visibility = Visibility.Hidden;
            MarkGrid.Visibility = Visibility.Visible;
        }

        private void AddStudentClassBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = MarkGrid.SelectedIndex;

            if (selectedIndex >= 0) students.ElementAt(selectedIndex).Class = changeStudentClassInput.Text;
            else MessageBox.Show("Nem választott ki tanulót!");

            MarkGrid.Items.Refresh();
        }

        private void DelStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = MarkGrid.SelectedIndex;

            if (selectedIndex >= 0) students.RemoveAt(selectedIndex);
            else
            {
                MessageBox.Show("Nem választott ki tanulót!");
                return;
            }

            if (students.Count.Equals(0))
            {
                personIndicator.Visibility = Visibility.Visible;
                MarkGrid.Visibility = Visibility.Hidden;
            }

            MarkGrid.Items.Refresh();
        }

        private void ChangeThemeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeManager.Current.ApplicationTheme == ApplicationTheme.Dark) ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            else ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
        }

        private void AddNewStudentInput_GotFocus(object sender, RoutedEventArgs e)
        {
            addNewStudentInput.Text = "";
        }

        private void ChangeStudentClassInput_GotFocus(object sender, RoutedEventArgs e)
        {
            changeStudentClassInput.Text = "";
        }

        private void SetMarkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (markInput.Text.Length <= 0) MessageBox.Show("Jegy nélkül nem rögzíthető értékelés!", "Jegykezelési hiba", MessageBoxButton.OK, MessageBoxImage.Information);
            if (!int.TryParse(markInput.Text, out int value)) MessageBox.Show("Az értékelés csak számadat lehet!", "Jegykezelési hiba", MessageBoxButton.OK, MessageBoxImage.Information);
            if (int.Parse(markInput.Text) > 5 || int.Parse(markInput.Text) < 1) MessageBox.Show("Az értékelés nem lehet nagyobb, mint 5 vagy kisebb, mint 1!", "Jegykezelési hiba", MessageBoxButton.OK, MessageBoxImage.Information);

            int selectedIndex = MarkGrid.SelectedIndex;

            if (selectedIndex < 0)
            {
                MessageBox.Show("Nem választott ki tanulót!");
                return;
            }

            switch (subjectsComboBox.SelectedIndex)
            {
                case 0:
                    students.ElementAt(selectedIndex).Math = int.Parse(markInput.Text); 
                    break;
                case 1:
                    students.ElementAt(selectedIndex).Literature = int.Parse(markInput.Text); 
                    break;
                case 2:
                    students.ElementAt(selectedIndex).Biology = int.Parse(markInput.Text); 
                    break;
                case 3:
                    students.ElementAt(selectedIndex).PE = int.Parse(markInput.Text); 
                    break;
                case 4:
                    students.ElementAt(selectedIndex).IT = int.Parse(markInput.Text); 
                    break;
                default:
                    MessageBox.Show("Ismeretlen rendszerhiba történt!", "Ismeretlen hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            MarkGrid.Items.Refresh();
        }

        private void MarkInput_GotFocus(object sender, RoutedEventArgs e)
        {
            markInput.Text = "";
        }

        private void AverageCalcBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = MarkGrid.SelectedIndex;

            if (selectedIndex < 0) 
            {
                MessageBox.Show("Nem választott ki tanulót!");
                return;
            }

            averageLabel.Content = $"A kiválasztott tanuló átlaga: {(double)(students.ElementAt(selectedIndex).Math + students.ElementAt(selectedIndex).IT + students.ElementAt(selectedIndex).Biology + students.ElementAt(selectedIndex).Literature + students.ElementAt(selectedIndex).PE) / 5}";
        }

        private void WriteDataBtn_Click(object sender, RoutedEventArgs e)
        {
            if (students.Count > 0)
            {
                StreamWriter sw = new($"zsirkreta-naplo.txt");
                students.ForEach(x =>
                {
                    sw.WriteLine($"{x.Name};{x.Class};{x.Math};{x.Literature};{x.Biology};{x.PE};{x.IT}");
                });
                sw.Flush();
                sw.Close();
                MessageBox.Show("Sikeresen kiírta a rendszer a dokumentum tartalmát!", "Sikeres fájlbaírás", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ReadDataBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "zsirkreta-naplo",
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            bool? result = dialog.ShowDialog();

            if ((bool)result)
            {
                students.Clear();
                foreach (var item in File.ReadAllLines(dialog.FileName))
                {
                    students.Add(new Student(item));
                }
                MarkGrid.Items.Refresh();
                personIndicator.Visibility = Visibility.Hidden;
                MarkGrid.Visibility = Visibility.Visible;
            }
        }
    }
}
