using DraughtsGame.Model.DomainModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace DraughtsGame.View.ScoreUserControl
{
    /// <summary>
    /// Interaction logic for ScoreUserControl.xaml
    /// </summary>
    public partial class ScoreUserControl : UserControl
    {

        public static readonly DependencyProperty PlayerNameProperty =
        DependencyProperty.Register("PlayerName", typeof(string), typeof(ScoreUserControl), new FrameworkPropertyMetadata(string.Empty));

        public string PlayerName
        {
            get { return (string)GetValue(PlayerNameProperty); }
            set { SetValue(PlayerNameProperty, value); }
        }

        public static readonly DependencyProperty ScoreProperty =
        DependencyProperty.Register("Score", typeof(string), typeof(ScoreUserControl), new FrameworkPropertyMetadata("0"));

        public string Score
        {
            get { return (string)GetValue(ScoreProperty); }
            set { SetValue(PlayerNameProperty, value); }
        }

        public ScoreUserControl()
        {
            InitializeComponent();
            window.DataContext = this;
        }
    }
}
