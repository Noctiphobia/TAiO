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
		protected readonly int OutlineWidth = 4;
		/// <summary>
		/// Kolor konturu.
		/// </summary>
	    protected readonly Color OutlineColor = Color.FromArgb(0xFF, 0x54, 0x54, 0x5C);
		/// <summary>
		/// Kolor klocka.
		/// </summary>
	    protected readonly Color BlockColor = Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC);

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
			new FrameworkPropertyMetadata((int)0,
				FrameworkPropertyMetadataOptions.AffectsRender,
				(d, e) =>
				{
					(d as BoardView)?.Redraw();
				}));

		public static readonly DependencyProperty StepsPerChangeProperty = DependencyProperty.Register(
			nameof(StepsPerChangeProperty),
			typeof(int),
			typeof(BoardView),
			new FrameworkPropertyMetadata((int)0,
				FrameworkPropertyMetadataOptions.AffectsRender,
				(d, e) =>
				{
					(d as BoardView)?.Redraw();
				}));
		#endregion

		/// <summary>
		/// Funkcja rysująca linię.
		/// </summary>
		private void DrawLine(double x1, double y1, double x2, double y2)
	    {
		    Line line = new Line
		    {
			    Stroke = new SolidColorBrush(OutlineColor),
			    StrokeThickness = OutlineWidth,
			    X1 = x1,
			    Y1 = y1,
			    X2 = x2,
			    Y2 = y2
		    };
		    DrawingArea.Children.Add(line);
	    }

        /// <summary>
        /// Funkcja zajmująca się rysowaniem po planszy. Wywoływana przy każdej zmianie obecnego kroku oraz podmianie całej kolekcji.
        /// </summary>
        private void Redraw()
        {
	        if (DataSource == null)
		        return;
            int width = DataSource.Width;
            int height = DataSource.Height;
            Size field = new Size(ActualWidth/width, ActualHeight/height); //rozmiar pojedynczego pola na planszy
	        for (int j = 0; j < height; ++j)
	        {
		        for (int i = 0; i < width; ++i)
		        {
			        if (DataSource[i, j] > 0 && DataSource.Array[i, j] <= CurrentStep)
			        {
				        Rectangle rectangle = new Rectangle
				        {
					        Width = 0.5*field.Width,
					        Height = 0.5*field.Height,
							Fill = new SolidColorBrush(BlockColor),
							Stroke = new SolidColorBrush(),
							StrokeThickness = 0.0
				        };
				        DrawingArea.Children.Add(rectangle);
				        double xLeft = i * field.Width;
				        double yTop = (j + 1) * field.Height;
				        double xRight = (i + 1) * field.Width;
				        double yBottom = j * field.Height;
				        Canvas.SetLeft(rectangle, xLeft);
						Canvas.SetBottom(rectangle, yTop);
				        if (i == 0 || DataSource[i - 1, j] != DataSource[i, j])	//po lewej jest co innego
					        DrawLine(xLeft, yTop, xLeft, yBottom);
				        if (i == DataSource.Width - 1 || DataSource[i + 1, j] != DataSource[i, j])	//po prawej jest co innego
					        DrawLine(xRight, yTop, xRight, yBottom);
				        if (j == 0 || DataSource[i, j - 1] != DataSource[i, j]) //na dole jest co innego
					        DrawLine(xLeft, yBottom, xRight, yBottom);
				        if (j == DataSource.Height - 1 || DataSource[i, j + 1] != DataSource[i, j]) //na górze jest coś innego
					        DrawLine(xLeft, yTop, xRight, yTop);
			        }
		        }
	        }

        }

        public BoardView()
        {
            InitializeComponent();
        }
    }
}
