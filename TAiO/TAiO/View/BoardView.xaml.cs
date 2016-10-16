﻿using System;
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
        public int[,] DataSource
        {
            get { return (int[,])GetValue(DataSourceProperty); }
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

        #region Kod do DependencyProperty
        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(
            nameof(DataSource),
            typeof(int[,]),
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
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                (d, e) =>
                {
                    (d as BoardView)?.Redraw();
                }));
        #endregion

        /// <summary>
        /// Funkcja zajmująca się rysowaniem po planszy. Wywoływana przy każdej zmianie obecnego kroku oraz podmianie całej kolekcji.
        /// </summary>
        private void Redraw()
        {
            int width = DataSource.GetLength(0);
            int height = DataSource.GetLength(1);
            Size field = new Size(ActualWidth/width, ActualHeight/height); //rozmiar pojedynczego pola na planszy

        }

        public BoardView()
        {
            InitializeComponent();
        }
    }
}