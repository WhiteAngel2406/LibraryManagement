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
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Win32;
using System.Net.Mail;
using System.Net;

namespace LibraryManagement_PRJ01
{
	/// <summary>
	/// Interaction logic for AdminBorrowManage.xaml
	/// </summary>
	public partial class AdminBorrowManage : Window
	{

		LibraryManagement4Context context = new LibraryManagement4Context();
		public AdminBorrowManage()
		{
			InitializeComponent();
			UpdateOverdueStatus();
			LoadDataGrid();
			LoadCombobox();
			CountBookNumber();
		}

		public void LoadDataGrid()
		{
			var listMuonSach = context.MuonSaches.Select(e => new
			{
				BorrowId = e.MaMuon,
				StudentId = e.MaSinhVien,
				BookId = e.SachId,
				BorrowDate = e.NgayMuon,
				Expirationdate = e.NgayTra,
				Status = e.TrangThai,

			}).ToList();
			dgMuonSach.ItemsSource = listMuonSach;
		}

		private void dgMuonSach_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			var item = e.Row.Item as dynamic;

			if (item != null && item.Status == "QuaHan")
			{
				e.Row.Background = new SolidColorBrush(Colors.LightCoral);
				e.Row.Foreground = new SolidColorBrush(Colors.White);
			}
		}

		public void SendEmail(object sender, RoutedEvent e)
		{
			string fromMail = "kakassj25@gmail.com";
			string fromPassword = "mviantbwravxpyon";

			MailMessage message = new MailMessage();
			message.From = new MailAddress(fromMail);
			message.Subject = "Test Subject";
			message.To.Add(new MailAddress("kakassj25@gmail.com"));
			message.Body = $"<html><body> aloalo </body></html>";
			message.IsBodyHtml = true;

			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential(fromMail, fromPassword),
				EnableSsl = true,
			};

			smtpClient.Send(message);
		}
		public void UpdateOverdueStatus()
		{
			DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

			var overdueBooks = context.MuonSaches
				.Where(m => m.NgayTra < currentDate && m.TrangThai != "QuaHan")
				.ToList();
			foreach (var record in overdueBooks)
			{
				record.TrangThai = "QuaHan";
			}

			context.SaveChanges();

			LoadDataGrid();
			LoadCombobox();
		}

		public void LoadCombobox()
		{
			var ListStatus = context.MuonSaches.Select(e => e.TrangThai).Distinct().ToList();
			ListStatus.Insert(0, "All");
			cbbStatus.ItemsSource = ListStatus;
			cbbStatus.SelectedIndex = 0;
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			AdminHome a = new AdminHome();
			this.Close();
			a.Show();
		}

		private void btnReturn_Click(object sender, RoutedEventArgs e)
		{
			var selectedRow = dgMuonSach.SelectedItem as dynamic;
			if (selectedRow != null)
			{
				int borrowId = selectedRow.BorrowId;
				int bookId = selectedRow.BookId;

				var borrowRecord = context.MuonSaches.FirstOrDefault(m => m.MaMuon == borrowId);
				if (borrowRecord != null)
				{
					var bookToUpdate = context.Saches.FirstOrDefault(s => s.SachId == bookId);
					if (bookToUpdate != null)
					{
						bookToUpdate.SoLuong += 1;
					}

					context.MuonSaches.Remove(borrowRecord);
					context.SaveChanges();

					LoadDataGrid();
					CountBookNumber();
					MessageBox.Show("Book returned successfully!");
				}
			}
			else
			{
				MessageBox.Show("Please select a record to return.");
			}
		}

		public void CountBookNumber()
		{
			int countTotal = context.MuonSaches.Count();
			int countOnLoan = context.MuonSaches.Where(e => e.TrangThai == "DangMuon").Count();
			int countQuaHan = context.MuonSaches.Where(e => e.TrangThai == "QuaHan").Count();

			txtOverDue.Text = countQuaHan.ToString();
			txtBookOnLoan.Text = countOnLoan.ToString();
			txtTotal.Text = countTotal.ToString();
		}

		private void cbbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedStatus = cbbStatus.SelectedItem as string;
			var listMuonSachByStatus = context.MuonSaches.Where(Muon => selectedStatus == "All" || Muon.TrangThai == selectedStatus).Select(e => new
			{
				BorrowId = e.MaMuon,
				StudentId = e.MaSinhVien,
				BookId = e.SachId,
				BorrowDate = e.NgayMuon,
				Expirationdate = e.NgayTra,
				Status = e.TrangThai,
			}).ToList();
			dgMuonSach.ItemsSource = listMuonSachByStatus;
		}

		private void txtExport_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.DefaultExt = ".xls";
			saveFile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
			if (saveFile.ShowDialog() == true)
			{
				try
				{

					dgMuonSach.SelectAllCells();
					dgMuonSach.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
					ApplicationCommands.Copy.Execute(null, dgMuonSach);
                    System.String resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
					string result = (string)Clipboard.GetData(DataFormats.Text);
					dgMuonSach.UnselectAllCells();

					System.IO.StreamWriter file = new System.IO.StreamWriter(saveFile.FileName);
					file.WriteLine(result.Replace(',', ' '));
					file.Close();
					MessageBox.Show("Successfull!");
				}
				catch (Exception ex)
				{
					MessageBox.Show("Unsuccess");
				}

			}
		}

		private void txtSendEmail_Click(object sender, RoutedEventArgs e)
		{
			var overdueBorrows = context.MuonSaches
		.Where(m => m.TrangThai == "QuaHan")
		.Select(m => m.MaSinhVien)
		.Distinct() 
		.ToList();
			foreach(var studentId in overdueBorrows)
			{
				var student = context.SinhViens.FirstOrDefault(s => s.MaSinhVien == studentId);
				if (student == null || string.IsNullOrEmpty(student.Email))
				{
					continue;
				}
				string studentEmail = student.Email;

				string fromMail = "kakassj25@gmail.com";
				string fromPassword = "mviantbwravxpyon";

				MailMessage message = new MailMessage();
				message.From = new MailAddress(fromMail);
				message.Subject = "OverDue";
				message.To.Add(new MailAddress($"{studentEmail}"));
				message.Body = $"<html><body> Your book loan is overdue. Please return it as soon as possible. </body></html>";
				message.IsBodyHtml = true;

				var smtpClient = new SmtpClient("smtp.gmail.com")
				{
					Port = 587,
					Credentials = new NetworkCredential(fromMail, fromPassword),
					EnableSsl = true,
				};

				smtpClient.Send(message);
				MessageBox.Show("Send Successfull!");

			}
			

			
		}
	}
}
