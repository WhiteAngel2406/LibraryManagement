using LibraryManagement_PRJ01.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryManagement_PRJ01
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		

		public MainWindow()
		{
			InitializeComponent();
		}

		private void btnStudentRegister_Click(object sender, RoutedEventArgs e)
		{
			StudentRegisterForm a = new StudentRegisterForm();
			this.Close();
			a.Show();
		}

		private void btnStudentLogin_Click(object sender, RoutedEventArgs e)
		{
		    StudentLoginForm m = new StudentLoginForm();
			this.Close();
			m.Show();
		}
	}
}