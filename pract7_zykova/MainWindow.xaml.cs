using pract7_zykova.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace pract7_zykova
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly ApiService _api = new ApiService();

        public string FilterTeacher { get; set; }
        public string FilterMinDuration { get; set; }

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                OnPropertyChanged(); 
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private async void Output_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var courses = await _api.GetCourses(null, null);
                CoursesGrid.ItemsSource = courses;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void Filter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? minDur = int.TryParse(FilterMinDuration, out int d) ? d : null;
                var courses = await _api.GetCourses(FilterTeacher, minDur);
                CoursesGrid.ItemsSource = courses;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FilterTeacher = "";
                FilterMinDuration = "";

                DataContext = null;
                DataContext = this;

                var courses = await _api.GetCourses(null, null);
                CoursesGrid.ItemsSource = courses;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newCourse = new Course
                {
                    Title = "Новый курс " + DateTime.Now.Second,
                    Teacher = "Преподаватель",
                    DurationHours = 10,
                    Price = 1000,
                    IsActive = true
                };

                await _api.CreateCourse(newCourse);

                var courses = await _api.GetCourses(null, null);
                CoursesGrid.ItemsSource = courses;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedCourse == null)
                {
                    MessageBox.Show("Выберите курс из списка!");
                    return;
                }

                await _api.UpdateCourse(SelectedCourse);

                var courses = await _api.GetCourses(null, null);
                CoursesGrid.ItemsSource = courses;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void Patch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedCourse == null)
                {
                    MessageBox.Show("Выберите курс из списка!");
                    return;
                }

                var patchData = new { Title = SelectedCourse.Title + " (изменено)" };
                await _api.PatchCourse(SelectedCourse.Id, patchData);

                var courses = await _api.GetCourses(null, null);
                CoursesGrid.ItemsSource = courses;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedCourse == null)
                {
                    MessageBox.Show("Выберите курс из списка!");
                    return;
                }

                await _api.DeleteCourse(SelectedCourse.Id);

                var courses = await _api.GetCourses(null, null);
                CoursesGrid.ItemsSource = courses;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}