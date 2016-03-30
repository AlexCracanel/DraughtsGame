using DraughtsGame.Model.DomainModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DraughtsGame.View.GameTableUserControl
{
    /// <summary>
    /// Interaction logic for GameTableUserControl.xaml
    /// </summary>
    public partial class GameTableUserControl : UserControl
    {
        ObservableCollection<Queen> pices;

        public GameTableUserControl()
        {
            InitializeComponent();

            pices = new ObservableCollection<Queen>();

            pices.Add(new Queen(1, 1, 4, @"D:\Repository\DraughtsGame\DraughtsGame\DraughtsGame\Images\blackCheker.png"));
            pices.Add(new Queen(1, 4, 4, @"D:\Repository\DraughtsGame\DraughtsGame\DraughtsGame\Images\blackCheker.png"));
            pices.Add(new Queen(1, 6, 2, @"D:\Repository\DraughtsGame\DraughtsGame\DraughtsGame\Images\blackCheker.png"));

            gameTableListBox.ItemsSource = pices;
        }
    }
}
