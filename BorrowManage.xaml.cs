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
	/// Interaction logic for BorrowManage.xaml
	/// </summary>
	public partial class BorrowManage : Window
	{
		LibraryManagement4Context context = new LibraryManagement4Context();
		
		public BorrowManage()
		{
			InitializeComponent();
		}
	}
}
