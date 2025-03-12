using LibraryManagement_PRJ01.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
    /// Interaction logic for ManageBooks.xaml
    /// </summary>
    public partial class ManageBooks : Window
    {

        LibraryManagement4Context context = new LibraryManagement4Context();
		
		public ManageBooks()
        {
            InitializeComponent();
            LoadDgBooks();
            LoadTitleCombobox();
			CountBookNumber();
			CountTotalBook();
		}

        public void LoadDgBooks()
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

            dgBook.ItemsSource = listBooks;
        }

        public void LoadTitleCombobox()
        {
            var listTitle = context.TheLoais.Select(e => e.TenTheLoai).ToList();

			listTitle.Insert(0, "All");

			cbTitle.ItemsSource = listTitle;
			cbTitle.SelectedIndex = 0;
		}

		private void cbTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedCategoryName = cbTitle.SelectedItem as string;
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

			dgBook.ItemsSource = listBookByCategory;
			txtSearch.Text = "";

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

			dgBook.ItemsSource = bookSearch;
		}

		private void dgBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedRow = dgBook.SelectedItem as dynamic;
			if (selectedRow != null)
			{
				txtId.Text = selectedRow.ID.ToString();
				txtCategory.Text = selectedRow.CategoryName.ToString();
				txtTitle.Text = selectedRow.Title.ToString();
				txtDescription.Text = selectedRow.Content.ToString();
				txtAuthor.Text = selectedRow.Author.ToString();
				txtAmount.Text = selectedRow.Amount.ToString();
				txtPubYear.Text = selectedRow.PublicationYear.ToString();
				txtPubHouse.Text = selectedRow.PublicationHouse.ToString();
			}
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			AdminHome a = new AdminHome();
			this.Close();
			a.Show();
		}

		private void btnRefresh_Click(object sender, RoutedEventArgs e)
		{
			txtId.Text = null;
			txtCategory.Text = null;
			txtTitle.Text = null;
			txtDescription.Text = null;
			txtAuthor.Text = null;
			txtAmount.Text = null;
			txtPubYear.Text = null;
			txtPubHouse.Text = null;
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtId.Text))
			{
				if (string.IsNullOrWhiteSpace(txtCategory.Text) ||
					string.IsNullOrWhiteSpace(txtTitle.Text) ||
					string.IsNullOrWhiteSpace(txtDescription.Text) ||
					string.IsNullOrWhiteSpace(txtAuthor.Text) ||
					string.IsNullOrWhiteSpace(txtAmount.Text) ||
					string.IsNullOrWhiteSpace(txtPubYear.Text) ||
					string.IsNullOrWhiteSpace(txtPubHouse.Text))
				{
					MessageBox.Show("Please fill in all information of book");
					return;
				}

				if (!int.TryParse(txtAmount.Text, out int amount) || amount < 0)
				{
					MessageBox.Show("The amount must be more or equal to 0!");
					return;
				}

				if(!int.TryParse(txtPubYear.Text, out int pubYear) || pubYear < 0)
				{
					MessageBox.Show("Please check the year publication!");
				}

					var category = context.TheLoais
										  .FirstOrDefault(c => c.TenTheLoai == txtCategory.Text.Trim());

					if (category == null)
					{
						MessageBox.Show("Category not found. Please enter a valid category.");
						return;
					}

					var newBook = new Sach
					{
						Title = txtTitle.Text.Trim(),
						NoiDung = txtDescription.Text.Trim(),
						NhaXuatBan = txtPubHouse.Text.Trim(),
						SoLuong = amount,
						TacGia = txtAuthor.Text.Trim(),
						TheLoaiId = category.TheLoaiId,
						NamXuatBan = int.Parse(txtPubYear.Text.Trim()),
					};

					context.Saches.Add(newBook);
					context.SaveChanges();
					LoadDgBooks();
				CountBookNumber();
				CountTotalBook();
				MessageBox.Show("New book added successfully!");
			}
			else
			{
				MessageBox.Show("Please click on Refresh button to make the Id empty!");
			}
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtId.Text, out int id))
			{
				if (string.IsNullOrWhiteSpace(txtCategory.Text) ||
					string.IsNullOrWhiteSpace(txtTitle.Text) ||
					string.IsNullOrWhiteSpace(txtDescription.Text) ||
					string.IsNullOrWhiteSpace(txtAuthor.Text) ||
					string.IsNullOrWhiteSpace(txtAmount.Text) ||
					string.IsNullOrWhiteSpace(txtPubYear.Text) ||
					string.IsNullOrWhiteSpace(txtPubHouse.Text))
				{
					MessageBox.Show("Please fill in all information of book");
					return;
				}

				if (!int.TryParse(txtAmount.Text, out int amount) || amount < 0)
				{
					MessageBox.Show("The amount must be more or equal to 0!");
					return;
				}

				if (!int.TryParse(txtPubYear.Text, out int pubYear) || pubYear < 0)
				{
					MessageBox.Show("Please check the year publication!");
				}


				using (var context = new LibraryManagement4Context())
				{
					var bookToEdit = context.Saches.FirstOrDefault(s => s.SachId == id);

					if (bookToEdit != null)
					{
						var result = MessageBox.Show("Are you sure you want to edit this book information?", "Confirm Edit", MessageBoxButton.YesNo, MessageBoxImage.Warning);
						if (result == MessageBoxResult.Yes)
						{
							bookToEdit.Title = txtTitle.Text.Trim();
							bookToEdit.NhaXuatBan = txtPubHouse.Text.Trim();
							bookToEdit.NoiDung = txtDescription.Text.Trim();
							bookToEdit.SoLuong = int.Parse(txtAmount.Text.Trim());
							bookToEdit.NamXuatBan = int.Parse(txtPubYear.Text.Trim());
							bookToEdit.TacGia = txtAuthor.Text.Trim();

							var category = context.TheLoais.FirstOrDefault(c => c.TenTheLoai == txtCategory.Text.Trim());
							if (category != null)
							{
								bookToEdit.TheLoaiId = category.TheLoaiId; 
							}
							else
							{
								MessageBox.Show("Category not found. Please enter a valid category.");
								return;
							}

							context.SaveChanges();
							LoadDgBooks();
							CountBookNumber();
							CountTotalBook();
							MessageBox.Show("Book information updated successfully!");
						}
					}
					else
					{
						MessageBox.Show("Book not found. Please enter a valid book ID.");
					}
				}
			}
			else
			{
				MessageBox.Show("Please enter a valid numeric book ID.");
			}
		}

		private void btnRemove_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtId.Text, out int id))
			{
				var bookToDelete = context.Saches.FirstOrDefault(s => s.SachId == id);
				if (bookToDelete != null)
				{
					var result = MessageBox.Show("Are you sure you want to delete this book?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
					if (result == MessageBoxResult.Yes)
					{
						context.Saches.Remove(bookToDelete);
						context.SaveChanges();

						LoadDgBooks();
						btnRefresh_Click(sender, e);
						CountBookNumber();
						CountTotalBook();
						MessageBox.Show("remove student successfull!");
					}
				}
				else
				{
					MessageBox.Show("Book not found!");
				}
			}
			else
			{
				MessageBox.Show("Please choose a book to remove!");
			}
		}

		public void CountBookNumber()
		{
			int count = context.Saches.Count();
			txtBookAmount.Text = count.ToString();
		}

		public void CountTotalBook()
		{
			int totalAmount = context.Saches.Sum(s => s.SoLuong ?? 0);
			txtTotalBook.Text = totalAmount.ToString();
		}

		private void btnExport_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.DefaultExt = ".xls";
			saveFile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"; 
			if(saveFile.ShowDialog() == true)
			{
				try
				{

					dgBook.SelectAllCells();
					dgBook.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
					ApplicationCommands.Copy.Execute(null, dgBook);
					String resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
					string result = (string)Clipboard.GetData(DataFormats.Text);
					dgBook.UnselectAllCells();

					System.IO.StreamWriter file = new System.IO.StreamWriter(saveFile.FileName);
					file.WriteLine(result.Replace(',', ' '));
					file.Close();
					MessageBox.Show("Successfull!");
				} catch(Exception ex)
				{
					MessageBox.Show("Unsuccess");
				}

			}
		}
	}
}
