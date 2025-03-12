
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
	/// Interaction logic for SendMail.xaml
	/// </summary>
	public partial class SendMail : Window
	{
		public SendMail()
		{
			InitializeComponent();
		}

		private void btnSendMail_Click(object sender, RoutedEventArgs e)
		{
			Random random = new Random();
			int randomNumber = random.Next(100000, 1000000); // Số ngẫu nhiên từ 100000 đến 999999
			Console.WriteLine(randomNumber);

			string fromMail = "kakassj25@gmail.com";
			string fromPassword = "mviantbwravxpyon";

			MailMessage message = new MailMessage();
			message.From = new MailAddress(fromMail);
			message.Subject = "Test Subject";
			message.To.Add(new MailAddress("kakassj25@gmail.com"));
			message.Body = $"<html><body> {randomNumber} </body></html>";
			message.IsBodyHtml = true;

			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential(fromMail, fromPassword),
				EnableSsl = true,
			};

			smtpClient.Send(message);
		}
    }
}
