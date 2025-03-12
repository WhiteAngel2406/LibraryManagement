using LibraryManagement_PRJ01.Models;
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
	/// Interaction logic for AdminLogin.xaml
	/// </summary>
	public partial class AdminLogin : Window
	{
		LibraryManagement4Context context = new LibraryManagement4Context();

		public AdminLogin()
		{
			InitializeComponent();
		}


		private void btnLogin_Click(object sender, RoutedEventArgs e)
		{
			string username = txtUserName.Text.Trim();
			string password = txtPass.Password;

			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				MessageBox.Show("Please enter both username and password.");
				return;
			}
				var admin = context.Admins
					.FirstOrDefault(a => a.Username == username && a.PasswordHash == password);

				if (admin != null)
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

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{

			MainWindow m = new MainWindow();
			this.Close();
			m.Show();
		}
	}
}
