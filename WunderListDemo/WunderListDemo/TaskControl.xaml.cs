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

namespace WunderListDemo
{
    /// <summary>
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        /// <summary>
        /// Gets or sets the name which is displayed next to the field
        /// </summary>
        public String name
        {
            get { return (String)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("name", typeof(string),
              typeof(TaskControl), new PropertyMetadata(""));


        private string date { get; set; }

        private bool imp { get; set; }


        public TaskControl()
        {
            InitializeComponent();

            buildLabelImage();
        }

        /// <summary>
        /// 
        /// </summary>
        private void buildLabelImage()
        {
            if (!String.IsNullOrWhiteSpace(Convert.ToString(lbl_Imp.Content)))
            {
                lbl_Imp.Background = Brushes.Red;
            }
        }
    }
}
