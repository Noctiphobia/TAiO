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
    /// <summary>
    /// Klasa rozszerzająca standardowy Canvas, pozwalająca na przewijanie go.
    /// </summary>
	public class AutoCanvas : Canvas
	{
        /// <summary>
        /// Spodziewana wysokość całej planszy.
        /// </summary>
		public double RequiredHeight { get; set; }
        /// <summary>
        /// Zmierzenie elementów.
        /// </summary>
        /// <param name="constraint">Ograniczenie, standardowy parametr.</param>
        /// <returns></returns>
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
