using System.Windows;
using System.Windows.Controls;

namespace BravelyDefault2.Controls {
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