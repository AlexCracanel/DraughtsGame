using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DraughtsGame.Model.DomainModel
{
    class Piece : DependencyObject
    {

        public int Row
        {
            get { return (int)GetValue(RowProperty); }
            set { SetValue(RowProperty, value); }
        }

        public static readonly DependencyProperty RowProperty =
            DependencyProperty.Register("Row", typeof(int), typeof(Piece), new PropertyMetadata(0));

        public int Column
        {
            get { return (int)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Column.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.Register("Column", typeof(int), typeof(Piece), new PropertyMetadata(0));

        public BitmapImage ImageSource
        {
            get { return (BitmapImage)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(BitmapImage), typeof(Piece), new PropertyMetadata(new BitmapImage()));

        public GameUtil.PieceType Type
        {
            get { return (GameUtil.PieceType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(GameUtil.PieceType), typeof(Piece), new PropertyMetadata(GameUtil.PieceType.EMPTY));


        public Piece(GameUtil.PieceType type,int row,int column,string source)
        {
            this.Type = type;
            this.Row = row;
            this.Column = column;

            ImageSource = new BitmapImage();

            ImageSource.BeginInit();
            ImageSource.UriSource = new Uri(source, UriKind.RelativeOrAbsolute);
            ImageSource.EndInit();
        }

        public Piece(GameUtil.PieceType type, int row, int column)
        {
            this.Type = type;
            this.Row = row;
            this.Column = column;
        }

    }
}
