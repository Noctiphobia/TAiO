using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace TAiO.View
{
	public class AutoCanvas : Canvas
	{
		public double RequiredHeight { get; set; }
		protected override Size MeasureOverride(Size constraint)
		{
			Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			foreach (UIElement element in InternalChildren)
			{
				element?.Measure(availableSize);
			}
			return new Size (0, RequiredHeight);
		}
	}
}
