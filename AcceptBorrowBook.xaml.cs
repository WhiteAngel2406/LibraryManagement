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
    /// Interaction logic for AcceptBorrowBook.xaml
    /// </summary>
    public partial class AcceptBorrowBook : Window
    {
        LibraryManagement4Context context = new LibraryManagement4Context();
		private dynamic bookDetails;
		public AcceptBorrowBook()
        {

            InitializeComponent();
            LoadDatagrid();
			CountPendingNumber();
			
		}

        public void LoadDatagrid()
        {
            var listPending = context.YeuCauMuonSaches.Select(e => new
            {
				StudentId = e.MaSinhVien,
                PendingId = e.MaYeuCau,
                BookId = e.SachId,
                BorrowDate = e.NgayYeuCau,
                Status = e.TrangThai
            }).ToList();

            dgPendingBook.ItemsSource = listPending;
        }

		private void dgPendingBook_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
		{
			var selectedRow = dgPendingBook.SelectedItem as dynamic;
			if (selectedRow != null)
			{
				txtMaYeuCau.Text = selectedRow.PendingId.ToString();
				txtMasSinhVien.Text = selectedRow.StudentId.ToString();
				txtMaBook.Text = selectedRow.BookId.ToString();
			}
		}

		private void btnAccept_Click(object sender, RoutedEventArgs e)
		{
			Addlungtung();
			RemoveLungtung();
			LoadDatagrid();
			CountPendingNumber();
			MessageBox.Show("Successfully!");
		}
		public void RemoveLungtung() {
			if (int.TryParse(txtMaYeuCau.Text, out int id))
			{

				var YeucauToRemove = context.YeuCauMuonSaches.FirstOrDefault(s => s.MaYeuCau == id);
				if(YeucauToRemove != null)
				{
					var result = MessageBox.Show("Agree to accept borrow?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
					if (result == MessageBoxResult.Yes)
					{
						context.YeuCauMuonSaches.Remove(YeucauToRemove);
						context.SaveChanges();
					}
				}
			}
		}
		public void Addlungtung()
		{

			var newMuonSach = new MuonSach
			{
				MaSinhVien = int.Parse(txtMasSinhVien.Text),
				SachId = int.Parse(txtMaBook.Text),
				AdminId = 1,
				NgayMuon = DateOnly.FromDateTime(DateTime.Now),
				NgayTra = DateOnly.FromDateTime(DateTime.Now).AddMonths(2),
				TrangThai = "DangMuon"
				

			};
			context.Add(newMuonSach);
			context.SaveChanges();
		}

		private void btnRefuse_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtMaYeuCau.Text, out int id))
			{
				var YeucauToRemove = context.YeuCauMuonSaches.FirstOrDefault(s => s.MaYeuCau == id);
				if (YeucauToRemove != null)
				{
					var bookToUpdate = context.Saches.FirstOrDefault(b => b.SachId == YeucauToRemove.SachId);
					if (bookToUpdate != null)
					{
						bookToUpdate.SoLuong += 1;
					}

					var result = MessageBox.Show("Agree to Remove?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
					if (result == MessageBoxResult.Yes)
					{
						context.YeuCauMuonSaches.Remove(YeucauToRemove);
						context.SaveChanges();

						LoadDatagrid();
						CountPendingNumber();
						MessageBox.Show("Remove Successful!");
					}
				}
			}
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
            AdminHome m = new AdminHome();
            this.Close();
            m.Show();
		}
		public void CountPendingNumber()
		{
			int count = context.YeuCauMuonSaches.Count();
			txtTotalRequest.Text = count.ToString();
		}

	}
}
