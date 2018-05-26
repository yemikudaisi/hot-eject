using Sru.Wpf.Controls;
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
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;

namespace Sru.Wpf
{
    /// <summary>
    /// Interaction logic for TaskbarIconView.xaml
    /// </summary>
    public partial class TaskbarIconView : TaskbarIconWindow
    {
        public TaskbarIconView()
        {
            InitializeComponent();
        }

        public override TaskbarIcon TaskBarIcon
        {
            get
            {
                return _taskbarIcon;
            }
        }
    }
}
