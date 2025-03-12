using LibraryManagement_PRJ01.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace LibraryManagement_PRJ01
{
    /// <summary>
    /// Interaction logic for StudentRegisterForm.xaml
    /// </summary>
    public partial class StudentRegisterForm : Window
    {
		LibraryManagement4Context context = new LibraryManagement4Context();
        public StudentRegisterForm()
        {
            InitializeComponent();
        }

		private void btnRegister_Click(object sender, RoutedEventArgs e)
		{
            if(string.IsNullOrWhiteSpace(txtName.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ||
                    string.IsNullOrWhiteSpace(txtClass.Text) ||
					string.IsNullOrWhiteSpace(dpBirthDate.Text)||
					string.IsNullOrWhiteSpace(txtPassword.Password) ||
					string.IsNullOrWhiteSpace(txtRePassword.Password))
            {
				MessageBox.Show("Please fill in all information of Student");
				return;
			}

			if (txtRePassword.Password != txtPassword.Password)
			{
				MessageBox.Show("Please check again, RePassword is not the same with Password");
				return;
			}

			string phoneNumberPattern = @"^\d+$";
			if (!Regex.IsMatch(txtPhoneNumber.Text, phoneNumberPattern))
			{
				MessageBox.Show("Phone number must contain only numbers");
				return;
			}

			string emailPattern = @"^[\w-\.]+@gmail\.com$";
			if (!Regex.IsMatch(txtEmail.Text, emailPattern))
			{
				MessageBox.Show("Email must be in the format of @gmail.com");
				return;
			}

			if (context.SinhViens.Any(sv => sv.Email == txtEmail.Text))
			{
				MessageBox.Show("This email is already registered. Please use a different email.");
				return;
			}

			var pendingStudent = new SinhVienRegister
			{
				Ten = txtName.Text,
				PasswordHash = txtPassword.Password,
				Email = txtEmail.Text,
				SoDienThoai = txtPhoneNumber.Text,
				NgaySinh = DateOnly.Parse(dpBirthDate.Text),
				Lop = txtClass.Text,

			};

			context.Add(pendingStudent);
			context.SaveChanges();
			MessageBox.Show("Now you are on waiting list, wait to accept by admin!");

			MainWindow m = new MainWindow();
			this.Close();
			m.Show();

			
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			MainWindow m = new MainWindow();
			this.Close();
			m.Show();	
		}
	}
}
