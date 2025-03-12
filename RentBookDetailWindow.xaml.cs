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
    /// Interaction logic for RentBookDetailWindow.xaml
    /// </summary>
    public partial class RentBookDetailWindow : Window
    {
		LibraryManagement4Context context = new LibraryManagement4Context();
		private dynamic bookDetails;

		public RentBookDetailWindow(dynamic selectedBook)
		{
			InitializeComponent();
			bookDetails = selectedBook;

		
			txtTitle.Text = bookDetails.Title;
			txtAuthor.Text = bookDetails.Author;
			txtCategory.Text = bookDetails.CategoryName;
			txtContent.Text = bookDetails.Content;
		}

		private void btnConfirmRent_Click(object sender, RoutedEventArgs e)
		{
			if (bookDetails.Amount > 0)
			{
				if (Application.Current.Properties.Contains("LoggedInStudentId"))
				{
					int? studentId = Application.Current.Properties["LoggedInStudentId"] as int?;

					if (studentId.HasValue)
					{
						YeuCauMuonSach rentRequest = new YeuCauMuonSach
						{
							MaSinhVien = studentId.Value, 
							SachId = bookDetails.ID,
							NgayYeuCau = DateOnly.FromDateTime(DateTime.Now),
							AdminId = 1,
							TrangThai = "ChoDuyet"
						};
						
						context.YeuCauMuonSaches.Add(rentRequest);

						var book = context.Saches.Find(bookDetails.ID);
						if (book != null)
						{
							book.SoLuong -= 1;

							if (book.SoLuong < 0)
							{
								MessageBox.Show("Sorry, this book is out of stock.", "Out of Stock", MessageBoxButton.OK, MessageBoxImage.Error);
								return;
							}

							context.SaveChanges();
							MessageBox.Show("Your book rental request has been sent for approval.", "Request Sent", MessageBoxButton.OK, MessageBoxImage.Information);
							this.Close(); 
						}
					}
					else
					{
						MessageBox.Show("Invalid student ID. Please login again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
				else
				{
					MessageBox.Show("Please login first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			else
			{
				MessageBox.Show("Sorry, this book is out of stock.", "Out of Stock", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
