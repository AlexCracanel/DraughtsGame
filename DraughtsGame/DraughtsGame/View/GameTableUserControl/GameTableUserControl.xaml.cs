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

            AddBlackQueen();
            AddWhiteQueen();

            gameTableListBox.ItemsSource = pices;
        }

        private void AddBlackQueen()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = ((i + 1) % 2 == 0) ? 0 : 1; j < 8; j += 2)
                {
                    pices.Add(new Queen(1, i, j, @"\Images\blackCheker.png"));
                }
            }
        }

        private void AddWhiteQueen()
        {
            for (int i = 5; i < 8; i++)
            {
                for (int j = ((i + 1) % 2 == 0) ? 0 : 1; j < 8; j += 2)
                {
                    pices.Add(new Queen(1, i, j, @"\Images\whiteChecker.png"));
                }
            }
        }
    }
}
