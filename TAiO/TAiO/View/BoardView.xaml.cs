using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TAiO.Model;

namespace TAiO.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : UserControl
    {
        /// <summary>
        /// Źródło danych kontrolki.
        /// </summary>
        public Array2D DataSource
        {
            get { return (Array2D)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        /// <summary>
        /// Informacja o obecnym kroku, który jest pokazywany. Zmiana kroku powoduje zmianę wyświetlanej planszy.
        /// </summary>
        public int CurrentStep
        {
            get { return (int)GetValue(CurrentStepProperty); }
            set { SetValue(CurrentStepProperty, value); }
        }

		/// <summary>
		/// Liczba kroków na zmianę.
		/// </summary>
		public int StepsPerChange
		{
			get { return (int)GetValue(StepsPerChangeProperty); }
			set { SetValue(StepsPerChangeProperty, value); }
		}

		/// <summary>
		/// Szerokość konturu.
		/// </summary>
		protected readonly int OutlineWidth = 2;

		/// <summary>
		/// Szerokość siatki.
		/// </summary>
	    protected readonly double GridWidth = 1;

	    /// <summary>
	    /// Kolor konturu.
	    /// </summary>
	    protected readonly Color OutlineColor = Colors.GhostWhite;

		/// <summary>
		/// Kolor siatki.
		/// </summary>
	    protected readonly Color GridColor = Colors.DarkGray;

	    protected readonly double BrowserMinDimensions = 180;
		/// <summary>
		/// Kolor klocka.
		/// </summary>
		protected readonly Color BlockColor = Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC);

	    protected readonly Color RecentColor = Color.FromArgb(0xFF, 0x00, 0xDD, 0xFF);

		#region Kod do DependencyProperty
		public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(
            nameof(DataSource),
            typeof(Array2D),
            typeof(BoardView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                (d, e) =>
                {
                    (d as BoardView)?.Redraw();
                }));

        public static readonly DependencyProperty CurrentStepProperty = DependencyProperty.Register(
            nameof(CurrentStep),
            typeof(int),
            typeof(BoardView),
			new FrameworkPropertyMetadata((int)1,
				FrameworkPropertyMetadataOptions.AffectsRender,
				(d, e) =>
				{
					(d as BoardView)?.Redraw();
				}));

		public static readonly DependencyProperty StepsPerChangeProperty = DependencyProperty.Register(
			nameof(StepsPerChange),
			typeof(int),
			typeof(BoardView),
			new FrameworkPropertyMetadata((int)1,
				FrameworkPropertyMetadataOptions.AffectsRender,
				(d, e) =>
				{
					(d as BoardView)?.Redraw();
				}));
		#endregion

		/// <summary>
		/// Funkcja rysująca linię.
		/// </summary>
		private void DrawLine(double x1, double y1, double x2, double y2, Color color, double width)
	    {
		    Line line = new Line
		    {
			    Stroke = new SolidColorBrush(color),
			    StrokeThickness = width,
			    X1 = x1,
			    Y1 = y1,
			    X2 = x2,
			    Y2 = y2
		    };
		    DrawingArea.Children.Add(line);
	    }

	    private Color GetBlockColor(int blockNumber)
	    {
		    int diff = 1 + CurrentStep - blockNumber;
		    if (diff > StepsPerChange)
			    return BlockColor;
		    return Color.FromArgb(0xFF,
			    (byte)(BlockColor.R + diff*((double)RecentColor.R - BlockColor.R) / StepsPerChange),
				(byte)(BlockColor.G + diff*((double)RecentColor.G - BlockColor.G) / StepsPerChange),
				(byte)(BlockColor.B + diff*((double)RecentColor.B - BlockColor.B) / StepsPerChange)
				);
	    }

        /// <summary>
        /// Funkcja zajmująca się rysowaniem po planszy. Wywoływana przy każdej zmianie obecnego kroku oraz podmianie całej kolekcji.
        /// </summary>
        public void Redraw()
        {
	        if (DataSource == null)
		        return;
			DrawingArea.Children.Clear();
            int width = DataSource.Width;
            int height = DataSource.Height;
            Size field = new Size(DrawingArea.ActualWidth/width, DrawingArea.ActualHeight/width); //rozmiar pojedynczego pola na planszy
	        if (Math.Abs(field.Width) < 0.0001 || Math.Abs(field.Height) < 0.0001)
				field = new Size(BrowserMinDimensions/width, BrowserMinDimensions/width);

			for (int i = 1; i < width; ++i)
				DrawLine(i * field.Width, 0, i * field.Width, height * field.Height, GridColor, GridWidth);
			for (int i = 1; i < height; ++i)
				DrawLine(0, i * field.Height, width * field.Width, i * field.Height, GridColor, GridWidth);


			for (int j = 0; j < height; ++j)
	        {
				for (int i = 0; i < width; ++i)
				{
					if (DataSource[i, j] > 0 && DataSource.Array[i, j] <= CurrentStep)
					{
						Rectangle rectangle = new Rectangle
						{
							Width = field.Width + 2,
							Height = field.Height,
							Fill = new SolidColorBrush(GetBlockColor(DataSource[i, j])),
							Stroke = new SolidColorBrush(),
							StrokeThickness = 0.0
						};
						DrawingArea.Children.Add(rectangle);
						Canvas.SetRight(rectangle, i * field.Width - 1);
						Canvas.SetTop(rectangle, j * field.Height);
					}
				}
			}
	        for (int j = 0; j < height; ++j)
	        {
		        for (int i = 0; i < width; ++i)
		        {
			        if (DataSource[i, j] > 0 && DataSource.Array[i, j] <= CurrentStep)
			        {
						double xLeft = (width - i) * field.Width;
						double yTop = (j + 1) * field.Height;
						double xRight = (width - (i + 1)) * field.Width;
						double yBottom = j * field.Height;
						if (i == 0 || DataSource[i - 1, j] != DataSource[i, j]) //po lewej jest co innego
							DrawLine(xLeft, yTop, xLeft, yBottom, OutlineColor, OutlineWidth);
						if (i == DataSource.Width - 1 || DataSource[i + 1, j] != DataSource[i, j])  //po prawej jest co innego
							DrawLine(xRight, yTop, xRight, yBottom, OutlineColor, OutlineWidth);
						if (j == 0 || DataSource[i, j - 1] != DataSource[i, j]) //na dole jest co innego
							DrawLine(xLeft, yBottom, xRight, yBottom, OutlineColor, OutlineWidth);
						if (j == DataSource.Height - 1 || DataSource[i, j + 1] != DataSource[i, j]) //na górze jest coś innego
							DrawLine(xLeft, yTop, xRight, yTop, OutlineColor, OutlineWidth);
					}
		        }
	        }
			InvalidateVisual();
        }

        public BoardView()
        {
            InitializeComponent();
        }
    }
}
