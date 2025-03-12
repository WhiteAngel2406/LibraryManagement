using LibraryManagement_PRJ01.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for ManageStudent.xaml
    /// </summary>
    public partial class ManageStudent : Window
    {

        LibraryManagement4Context context = new LibraryManagement4Context();
        public ManageStudent()
        {
            InitializeComponent();
            LoadDataGridStudent();
        }

        public void LoadDataGridStudent()
        {
            var listStudent = context.SinhViens.Select(e => new
            {
                StudentID = e.MaSinhVien,
                StudentName = e.Ten,
                PhoneNumber = e.SoDienThoai,
                Email = e.Email, 
                Birthdate = e.NgaySinh,
                Class = e.Lop,
				Password = e.PasswordHash,
            }).ToList();
            dgStudent.ItemsSource = listStudent;
        }

		private void dgStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            var selectedRow = dgStudent.SelectedItem as dynamic;
            if (selectedRow != null)
            {
                txtStudentId.Text = selectedRow.StudentID.ToString();
                txtStudentName.Text = selectedRow.StudentName.ToString();
                txtEmail.Text = selectedRow.Email.ToString();
                txtPhoneNumber.Text = selectedRow.PhoneNumber.ToString();
                dpBirthDate.Text = selectedRow.Birthdate.ToString();
                txtClass.Text = selectedRow.Class.ToString();
				txtPassword.Password = selectedRow.Password.ToString();
			}
		}

		private void btnRefresh_Click(object sender, RoutedEventArgs e)
		{
            txtStudentId.Text = null;
            txtStudentName.Text = null;
            txtEmail.Text = null;
            txtPhoneNumber.Text = null;
            dpBirthDate.Text = null;
            txtClass.Text = null;
			txtPassword.Password = null;
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtStudentId.Text))
			{
				if (string.IsNullOrWhiteSpace(txtStudentName.Text) ||
					string.IsNullOrWhiteSpace(txtEmail.Text) ||
					string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ||
					string.IsNullOrWhiteSpace(dpBirthDate.Text) ||
					string.IsNullOrWhiteSpace(txtClass.Text)||
					string.IsNullOrWhiteSpace(txtPassword.Password))
				{
					MessageBox.Show("Please fill in all information of Student");
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

				var newStudent = new SinhVien
				{
					Ten = txtStudentName.Text,
					PasswordHash = txtPassword.Password,
					Email = txtEmail.Text,
					SoDienThoai = txtPhoneNumber.Text,
					NgaySinh = DateOnly.Parse(dpBirthDate.Text),
					Lop = txtClass.Text,
				};

				context.Add(newStudent);
				context.SaveChanges();

				LoadDataGridStudent();
				btnRefresh_Click(sender, e);
				MessageBox.Show("Add Student successful!");
			}
			else
			{
				MessageBox.Show("Please click on Refresh button to make the StudentId empty!");
			}
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			AdminHome a = new AdminHome();
			this.Close();
			a.Show();
		}

		private void btnRemove_Click(object sender, RoutedEventArgs e)
		{
			if(int.TryParse(txtStudentId.Text, out int id))
			{
				var studentToDelete = context.SinhViens.FirstOrDefault(s => s.MaSinhVien == id);
				if (studentToDelete != null)
				{
					var result = MessageBox.Show("Are you sure you want to delete this student?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
					if (result == MessageBoxResult.Yes)
					{
						context.SinhViens.Remove(studentToDelete);
						context.SaveChanges();

						LoadDataGridStudent();
						btnRefresh_Click(sender, e);
						MessageBox.Show("remove student successfull!");
					}
				}
				else
				{
					MessageBox.Show("Student not found!");
				}
			}
			else
			{
				MessageBox.Show("Please choose a student to remove!");
			}
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(txtStudentId.Text, out int id))
			{
				var studentToEdit = context.SinhViens.FirstOrDefault(s => s.MaSinhVien == id);
				if (studentToEdit != null)
				{
					var result = MessageBox.Show("Are you sure you want to Edit this student information?", "Confirm Edit", MessageBoxButton.YesNo, MessageBoxImage.Warning);
					if (result == MessageBoxResult.Yes)
					{
						
						studentToEdit.Ten = txtStudentName.Text;
						studentToEdit.Email = txtEmail.Text;
						studentToEdit.SoDienThoai = txtPhoneNumber.Text;
						studentToEdit.NgaySinh = DateOnly.Parse(dpBirthDate.Text);
						studentToEdit.PasswordHash = txtPassword.Password;


						context.SaveChanges();

						LoadDataGridStudent();
						btnRefresh_Click(sender, e);
						MessageBox.Show("Update student successfull!");
					}
				}
				else
				{
					MessageBox.Show("Student not found!");
				}
			}
			else
			{
				MessageBox.Show("Please choose a student to Edit!");
			}
		}

		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			int studentId;
			bool isNumeric = int.TryParse(txtSearch.Text, out studentId);

			var studentSearch = context.SinhViens
				.Where(s =>
					(isNumeric && s.MaSinhVien == studentId) ||
					s.Ten == txtSearch.Text ||
					s.Email == txtSearch.Text ||
					s.SoDienThoai == txtSearch.Text)
				.Select(s => new
				{
					StudentID = s.MaSinhVien,
					StudentName = s.Ten,
					PhoneNumber = s.SoDienThoai,
					Email = s.Email,
					Birthdate = s.NgaySinh,
					Class = s.Lop,
					Password = s.PasswordHash,
				})
				.ToList();

			dgStudent.ItemsSource = studentSearch;
		}

		private void btnExport_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.DefaultExt = ".xls";
			saveFile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
			if (saveFile.ShowDialog() == true)
			{
				try
				{

					dgStudent.SelectAllCells();
					dgStudent.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
					ApplicationCommands.Copy.Execute(null, dgStudent);
					String resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
					string result = (string)Clipboard.GetData(DataFormats.Text);
					dgStudent.UnselectAllCells();

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
	}
}

