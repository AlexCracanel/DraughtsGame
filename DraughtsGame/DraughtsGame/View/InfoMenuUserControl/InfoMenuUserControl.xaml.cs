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


        public string InfoText
        {
            get { return (string)GetValue(InfoTextProperty); }
            set { SetValue(InfoTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InfoText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InfoTextProperty =
            DependencyProperty.Register("InfoText", typeof(string), typeof(InfoMenuUserControl), new PropertyMetadata(String.Empty));

        public InfoMenuUserControl()
        {
            InitializeComponent();
            window.DataContext = this;
        }
    }
}
