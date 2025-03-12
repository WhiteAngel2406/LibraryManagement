using LibraryManagement_PRJ01.Models;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace LibraryManagement_PRJ01
{
    /// <summary>
    /// Interaction logic for StudentLoginForm.xaml
    /// </summary>
    public partial class StudentLoginForm : Window
    {
		LibraryManagement4Context context = new LibraryManagement4Context();
        public StudentLoginForm()
        {
            InitializeComponent();
        }

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
            MainWindow m = new MainWindow();
            this.Close();
            m.Show();
        }

		public static int LoggedInStudentId { get; set; }

		private void btnLogin_Click(object sender, RoutedEventArgs e)
		{
			string username = txtUserName.Text.Trim();
			string password = txtPass.Password;

			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				MessageBox.Show("Please enter both username and password.");
				return;
			}

			var student = context.SinhViens
				.FirstOrDefault(a => a.Email == username && a.PasswordHash == password);
			var admin = context.Admins
					.FirstOrDefault(a => a.Username == username && a.PasswordHash == password);


			if (student != null)
			{
					Application.Current.Properties["LoggedInStudentId"] = student.MaSinhVien;

					StudentHome s = new StudentHome();
					this.Close();
					s.Show();
			} else if (admin != null)
			{
				AdminHome adminHome = new AdminHome();
				adminHome.Show();
				this.Close();
			}
			else
			{
				MessageBox.Show("Invalid username or password. Please try again.");
			}
		}

		private void btnForget_Click(object sender, RoutedEventArgs e)
		{
			NewPasswordEmail m = new NewPasswordEmail();	
			this.Close();
			m.Show();
        }
    }
}
