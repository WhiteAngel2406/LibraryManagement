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
    /// Interaction logic for StudentHome.xaml
    /// </summary>
    public partial class StudentHome : Window
    {

		LibraryManagement4Context context = new LibraryManagement4Context();
		private dynamic selectedBook;

		public static object LoggedInStudentId { get; internal set; }

		public StudentHome()
        {
            InitializeComponent();
			LoadCbbtest();
			SendMessage();
			LoaddgBook();
		}

		public void LoadCbbtest()
		{
			var list = context.TheLoais.Select(e => e.TenTheLoai).ToList();
			list.Insert(0, "All");

			cbbtest.ItemsSource = list;
			cbbtest.SelectedIndex = 0;
		}
		public void LoaddgBook()
        {
			var listBooks = context.Saches.Include(Sach => Sach.TheLoai)
				.Select(e => new
				{
					ID = e.SachId,
					CategoryName = e.TheLoai.TenTheLoai,
					Title = e.Title,
					Content = e.NoiDung,
					Author = e.TacGia,
					Amount = e.SoLuong,
					PublicationYear = e.NamXuatBan,
					PublicationHouse = e.NhaXuatBan
				}).ToList();

			dgbookDataGrid.ItemsSource = listBooks;
			
		}

		private void dgbookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedBook = dgbookDataGrid.SelectedItem;
		}

		private void btnRent_Click(object sender, RoutedEventArgs e)
		{
			if (selectedBook != null)
			{
				RentBookDetailWindow rentWindow = new RentBookDetailWindow(selectedBook);
				rentWindow.ShowDialog();
			}
			else
			{
				MessageBox.Show("Please select a book to rent.", "No book selected", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			LoaddgBook();
		}

		private void btnLogout_Click(object sender, RoutedEventArgs e)
		{
			MainWindow m = new MainWindow();
			this.Close();
			m.Show();
		}

		private void btnBorrow_Click(object sender, RoutedEventArgs e)
		{
			StudentBorrow m = new StudentBorrow();
			m.ShowDialog();
		}

		public void SendMessage()
		{
			if (Application.Current.Properties.Contains("LoggedInStudentId"))
			{
				int? studentId = Application.Current.Properties["LoggedInStudentId"] as int?;

				if (studentId.HasValue)
				{
					var overdueBorrow = context.MuonSaches.FirstOrDefault(e => e.MaSinhVien == studentId.Value && e.TrangThai == "QuaHan");

					if (overdueBorrow != null)
					{
						MessageBox.Show("Your book loan is overdue. Please check and return it as soon as possible.", "Overdue Notice", MessageBoxButton.OK, MessageBoxImage.Warning);
					}
				}
			}
		}


		private void cbbtest_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedCategoryName = cbbtest.SelectedItem as string;
			var listBookByCategory = context.Saches.Include(sach => sach.TheLoai)
		.Where(sach => selectedCategoryName == "All" || sach.TheLoai.TenTheLoai == selectedCategoryName)
		.Select(e => new
		{
			ID = e.SachId,
			CategoryName = e.TheLoai.TenTheLoai,
			Title = e.Title,
			Content = e.NoiDung,
			Author = e.TacGia,
			Amount = e.SoLuong,
			PublicationYear = e.NamXuatBan,
			PublicationHouse = e.NhaXuatBan
		}).ToList();

			dgbookDataGrid.ItemsSource = listBookByCategory;
		}

		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			var bookSearch = context.Saches.Include(Sach => Sach.TheLoai).Where(e => e.TacGia == txtSearch.Text ||
			e.TheLoai.TenTheLoai == txtSearch.Text ||
			e.Title == txtSearch.Text ||
			e.NhaXuatBan == txtSearch.Text)
				.Select(e => new
				{
					ID = e.SachId,
					CategoryName = e.TheLoai.TenTheLoai,
					Title = e.Title,
					Content = e.NoiDung,
					Author = e.TacGia,
					Amount = e.SoLuong,
					PublicationYear = e.NamXuatBan,
					PublicationHouse = e.NhaXuatBan
				}).ToList();

			dgbookDataGrid.ItemsSource = bookSearch;
		}

		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			ChangePassword c = new ChangePassword();
			c.ShowDialog();
		}
	}
}
