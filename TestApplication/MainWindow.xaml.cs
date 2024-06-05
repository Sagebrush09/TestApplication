using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace TestApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new Models.test_dbEntities())
            {
                var query = from user in context.User
                            from course in user.Course
                            select new
                            {
                                UserId = user.Id,
                                CourseId = course.Id,
                                userName = user.UserName,
                                courseName = course.CourseName
                            };

                dataGrid.ItemsSource = query.ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataContext = button?.DataContext;

            if (dataContext != null)
            {
                dynamic item = dataContext;
                int userId = item.UserId;
                int courseId = item.CourseId;

                using (var context = new Models.test_dbEntities())
                {
                    var user = context.User.Include("Course").FirstOrDefault(u => u.Id == userId);
                    if (user != null)
                    {
                        var course = user.Course.FirstOrDefault(c => c.Id == courseId);
                        if (course != null)
                        {
                            user.Course.Remove(course);
                            context.SaveChanges();
                            LoadData();
                        }
                    }
                }
            }
        }
    }
}
