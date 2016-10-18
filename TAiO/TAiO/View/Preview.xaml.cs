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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace TAiO
{
	/// <summary>
	/// Interaction logic for Preview.xaml
	/// </summary>
	public partial class Preview : MetroWindow
	{
		public Preview()
		{
			InitializeComponent();
			this.SizeChanged += PreviewSizeChanged;
		}

		private void PreviewSizeChanged(object sender, SizeChangedEventArgs e)
		{
			DrawingArea.Redraw();
		}
	}
}
