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
	/// Interaction logic for ChangePassword.xaml
	/// </summary>
	public partial class ChangePassword : Window
	{
		LibraryManagement4Context context = new LibraryManagement4Context();
		public ChangePassword()
		{
			InitializeComponent();
		}

		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			if (Application.Current.Properties.Contains("LoggedInStudentId"))
			{
				int? studentId = Application.Current.Properties["LoggedInStudentId"] as int?;

				if (studentId.HasValue)
				{
					var student = context.SinhViens.FirstOrDefault(s => s.MaSinhVien == studentId.Value);

					if (student == null)
					{
						MessageBox.Show("Student not found!");
						return;
					}
					if (student.PasswordHash != txtOldpassword.Password) 
					{
						MessageBox.Show("Incorrect old password.");
						return;
					}

					if (txtNewpassword.Password != txtNewpassworcheck.Password)
					{
						MessageBox.Show("New passwords do not match.");
						return;
					}

					student.PasswordHash = txtNewpassword.Password; 

					context.SaveChanges();

					MessageBox.Show("Password changed successfully!");
					this.Close(); 
				}
				else
				{
					MessageBox.Show("Invalid student ID.");
				}
			}
			else
			{
				MessageBox.Show("No student is logged in.");
			}
		}

	}
}
