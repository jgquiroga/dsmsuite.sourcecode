﻿using System.Windows.Controls;

namespace DsmSuite.DsmViewer.View.UserControls
{
    /// <summary>
    /// Interaction logic for MatrixView.xaml
    /// </summary>
    public partial class MatrixView : UserControl
    {
        public MatrixView()
        {
            InitializeComponent();
        }

        private void CellsViewOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Canvas.SetLeft(ColumnHeaderView, -e.HorizontalOffset);
            Canvas.SetTop(RowHeaderView, -e.VerticalOffset);
        }
    }
}
