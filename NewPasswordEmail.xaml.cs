using LibraryManagement_PRJ01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
	/// Interaction logic for NewPasswordEmail.xaml
	/// </summary>
	public partial class NewPasswordEmail : Window
	{
		LibraryManagement4Context context = new LibraryManagement4Context();
		public NewPasswordEmail()
		{
			InitializeComponent();
		}

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			var student = context.SinhViens.FirstOrDefault(s => s.Email == txtGmail.Text);

			if (student != null)
			{
				Random random = new Random();
				int newPassword = random.Next(100000, 999999); 

				student.PasswordHash = newPassword.ToString(); 
				context.SaveChanges();

				string fromMail = "kakassj25@gmail.com";
				string fromPassword = "mviantbwravxpyon";

				MailMessage message = new MailMessage();
				message.From = new MailAddress(fromMail);
				message.Subject = "New Password";
				message.To.Add(new MailAddress($"{txtGmail.Text}"));
				message.Body = $"<html><body> Your new password is {newPassword}, please change it when login </body></html>";
				message.IsBodyHtml = true;

				var smtpClient = new SmtpClient("smtp.gmail.com")
				{
					Port = 587,
					Credentials = new NetworkCredential(fromMail, fromPassword),
					EnableSsl = true,
				};

				smtpClient.Send(message);

				MessageBox.Show("We are sending your new password to your Email, check it!",
								"Success!", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else
			{
				MessageBox.Show("Email Invalid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			StudentLoginForm form = new StudentLoginForm();
			this.Close();
			form.Show();
		}
	}
}
