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
using System.Linq;
using System.Net.Mail;
using System.Net;

namespace LibraryManagement_PRJ01
{
    /// <summary>
    /// Interaction logic for AdminHome.xaml
    /// </summary>
    public partial class AdminHome : Window
    {

        LibraryManagement4Context context = new LibraryManagement4Context();
        public AdminHome()
        {
            InitializeComponent();
            LoadDatagrid();
            CountPendingRegister();
        }

		

		private void btnLogout_Click(object sender, RoutedEventArgs e)
		{
            StudentLoginForm a = new StudentLoginForm();
            this.Close();
            a.Show();
		}

        public void LoadDatagrid()
        {
            var ListRegisterStudent = context.SinhVienRegisters.Select(e => new
            {
                Id = e.MaSinhVien,
                Name = e.Ten,
                Email = e.Email,
                PhoneNumber = e.SoDienThoai,
                BirthDate = e.NgaySinh,
                Class = e.Lop,
				Password = e.PasswordHash,
            }).ToList();

            dgPendingStudent.ItemsSource = ListRegisterStudent;
        }

		private void btnStudent_Click(object sender, RoutedEventArgs e)
		{
            ManageStudent m = new ManageStudent();
            this.Close();
            m.Show();
		}

		private void dgBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            var selectedRow = dgPendingStudent.SelectedItem as dynamic;
            if (selectedRow != null)
            {
                txtStudentId.Text = selectedRow.Id.ToString();
                txtStudentName.Text = selectedRow.Name.ToString();
                txtEmail.Text = selectedRow.Email.ToString();
                txtPhoneNumber.Text = selectedRow.PhoneNumber.ToString();
                dpBirthDate.Text = selectedRow.BirthDate.ToString();
                txtClass.Text = selectedRow.Class.ToString();
				txtPassword.Password = selectedRow.Password.ToString();
            }
		}

        public void CountPendingRegister()
        {

			int count = context.SinhVienRegisters.Count();
            txtPendingNumber.Text = count.ToString();

		}

		private void btnAccept_Click(object sender, RoutedEventArgs e)
		{
            if(string.IsNullOrWhiteSpace(txtStudentId.Text) ||
                string.IsNullOrWhiteSpace(txtStudentName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(dpBirthDate.Text) ||
                string.IsNullOrWhiteSpace(txtClass.Text) ||
				string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Please choose Student!");
            } else
            {
				var newStudent = new SinhVien
				{
					Ten = txtStudentName.Text,
					PasswordHash = txtPassword.Password,
					Email = txtEmail.Text,
					SoDienThoai = txtPhoneNumber.Text,
					NgaySinh = DateOnly.Parse(dpBirthDate.Text),
					Lop = txtClass.Text,

				};

				if (int.TryParse(txtStudentId.Text, out int id))
				{
					var RemovePendingStudent = context.SinhVienRegisters.FirstOrDefault(s => s.MaSinhVien == id);
					if (RemovePendingStudent != null)
					{
						var Result = MessageBox.Show("Agree to accept this student?", "Confirm Accept", MessageBoxButton.YesNo, MessageBoxImage.Warning);
						if (Result == MessageBoxResult.Yes)
						{
							context.Add(newStudent);
							string fromMail = "kakassj25@gmail.com";
							string fromPassword = "mviantbwravxpyon";

							MailMessage message = new MailMessage();
							message.From = new MailAddress(fromMail);
							message.Subject = "Your Library Account is now available!";
							message.To.Add(new MailAddress($"{txtEmail.Text}"));
							message.Body = $"<html><body> Your Account are accepted, now you can join the Library Application to borrow books!" +
								$"Thank you! </body></html>";
							message.IsBodyHtml = true;

							var smtpClient = new SmtpClient("smtp.gmail.com")
							{
								Port = 587,
								Credentials = new NetworkCredential(fromMail, fromPassword),
								EnableSsl = true,
							};

							smtpClient.Send(message);

							context.SinhVienRegisters.Remove(RemovePendingStudent);
							context.SaveChanges();
							txtStudentId.Text = null;
							txtStudentName.Text = null;
							txtEmail.Text = null;
							txtPhoneNumber.Text = null;
							txtClass.Text = null;
							dpBirthDate.Text = null;
							txtPassword.Password = null;
							LoadDatagrid();
							CountPendingRegister();
							MessageBox.Show("Add student successfull!");

							
						}
					}

				}
				else
				{
					MessageBox.Show("Please choose Student!");
				}
			}
            

            
		}

		private void btnRemove_Click(object sender, RoutedEventArgs e)
		{
            if(int.TryParse(txtStudentId.Text, out int id))
            {
				var RemovePendingStudent = context.SinhVienRegisters.FirstOrDefault(s => s.MaSinhVien == id);
				if (RemovePendingStudent != null)
				{
					var Result = MessageBox.Show("Confirm to Remove this student?", "Confirm Remove", MessageBoxButton.YesNo, MessageBoxImage.Warning);
					if (Result == MessageBoxResult.Yes)
					{
						context.SinhVienRegisters.Remove(RemovePendingStudent);
                        context.SaveChanges() ;
						txtStudentId.Text = null;
						txtStudentName.Text = null;
						txtEmail.Text = null;
						txtPhoneNumber.Text = null;
						txtClass.Text = null;
						dpBirthDate.Text = null;
						txtPassword.Password = null;
                        LoadDatagrid();
						CountPendingRegister();
						MessageBox.Show("Remove successfull!");
					}
				}
			}

           
        }

		private void btnBook_Click(object sender, RoutedEventArgs e)
		{
			ManageBooks m = new ManageBooks();
			this.Close();
			m.Show();
		}

		private void Pending_Click(object sender, RoutedEventArgs e)
		{
			AcceptBorrowBook a = new AcceptBorrowBook();
			this.Close();
			a.Show();
		}

		private void btnBorrow_Click(object sender, RoutedEventArgs e)
		{
			AdminBorrowManage a = new AdminBorrowManage();
			this.Close();
			a.Show();
		}
	}
}
