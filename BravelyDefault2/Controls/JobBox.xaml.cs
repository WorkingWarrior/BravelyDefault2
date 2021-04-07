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

namespace BravelyDefault2.Controls {
    /// <summary>
    /// Logika interakcji dla klasy JobBox.xaml
    /// </summary>
    public partial class JobBox : UserControl {
        public static readonly DependencyProperty GroupLabelProperty = DependencyProperty.Register("GroupLabel", typeof(string), typeof(JobBox), new PropertyMetadata());
        public static readonly DependencyProperty EXPProperty = DependencyProperty.Register("EXP", typeof(int), typeof(JobBox), new PropertyMetadata());
        public static readonly DependencyProperty IsLevelLimitedProperty = DependencyProperty.Register("IsLevelLimited", typeof(bool), typeof(JobBox), new PropertyMetadata());
        public string GroupLabel {
            get => GetValue(GroupLabelProperty) as string;
            set => SetValue(GroupLabelProperty, value);
        }
        public int EXP {
            get => (int)GetValue(EXPProperty);
            set => SetValue(EXPProperty, value);
        }
        public bool IsLevelLimited {
            get => (bool)GetValue(IsLevelLimitedProperty);
            set => SetValue(IsLevelLimitedProperty, value);
        }
        public JobBox() {
            InitializeComponent();
            MainBox.DataContext = this;
        }
    }
}