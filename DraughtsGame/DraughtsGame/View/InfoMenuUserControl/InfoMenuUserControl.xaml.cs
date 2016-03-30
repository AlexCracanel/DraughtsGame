using System;
using System.Collections.Generic;
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

namespace DraughtsGame.View.InfoMenuUserControl
{
    /// <summary>
    /// Interaction logic for InfoMenuUserControl.xaml
    /// </summary>
    public partial class InfoMenuUserControl : UserControl
    {
        public static readonly DependencyProperty InfoTextProperty =
       DependencyProperty.Register("PlayerName", typeof(string), typeof(InfoMenuUserControl), new FrameworkPropertyMetadata(string.Empty));

        public string InfoText
        {
            get { return (string)GetValue(InfoTextProperty); }
            set { SetValue(InfoTextProperty, value); }
        }

        public InfoMenuUserControl()
        {
            InitializeComponent();
            _this.DataContext = this;
        }
    }
}
