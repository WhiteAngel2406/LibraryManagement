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
	/// Interaction logic for StudentBorrow.xaml
	/// </summary>
	public partial class StudentBorrow : Window
	{
		LibraryManagement4Context context = new LibraryManagement4Context();
		public StudentBorrow()
		{
			InitializeComponent();
			LoadDataGrid();
			LoadCombobox();
		}

		public void LoadDataGrid()
		{
			if (Application.Current.Properties.Contains("LoggedInStudentId"))
			{
				int? studentId = Application.Current.Properties["LoggedInStudentId"] as int?;

				if(studentId.HasValue)
				{
					var listBorrow = context.MuonSaches.Where(e => e.MaSinhVien ==  studentId.Value).Select(s => new
					{
						BorrowId = s.MaMuon,
						BookId = s.SachId,
						BorrowDate = s.NgayMuon,
						Expirationdate = s.NgayTra,
						Status = s.TrangThai
					}).ToList();

					dgStudentBorrow.ItemsSource = listBorrow;
				}
			}
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

		public void LoadCombobox()
		{
			if (Application.Current.Properties.Contains("LoggedInStudentId"))
			{
				int? studentId = Application.Current.Properties["LoggedInStudentId"] as int?;

				if (studentId.HasValue)
				{
					var ListCombo = context.MuonSaches.Where(e => e.MaSinhVien == studentId.Value).Select(s => s.TrangThai).Distinct().ToList();
					ListCombo.Insert(0, "All");
					cbbStatus.ItemsSource = ListCombo;
					cbbStatus.SelectedIndex = 0;
				}
			}
		}

		private void cbbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (Application.Current.Properties.Contains("LoggedInStudentId"))
			{
				int? studentId = Application.Current.Properties["LoggedInStudentId"] as int?;

				if (studentId.HasValue)
				{
					var seletedStatus = cbbStatus.SelectedItem as string;
					var list = context.MuonSaches.Where(Muon => seletedStatus == "All" && Muon.MaSinhVien == studentId.Value || Muon.TrangThai == seletedStatus && Muon.MaSinhVien == studentId.Value).Select(e => new
					{
						BorrowId = e.MaMuon,
						BookId = e.SachId,
						BorrowDate = e.NgayMuon,
						Expirationdate = e.NgayTra,
						Status = e.TrangThai
					}).ToList();

					dgStudentBorrow.ItemsSource = list;
				}
			}
			
		}
	}
}
