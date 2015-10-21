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
using System.Windows.Controls.Primitives;

namespace WunderListDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WunderList : Window
    {

        private String name { get; set; }

        private bool isImp { get; set; }

        private DateTime date { get; set; }

        private List<TaskControl> taskList;

        private List<TaskControl> completedTaskList;

        private bool isShown { get; set; }


        public WunderList()
        {
            InitializeComponent();
            taskList = new List<TaskControl>();
            completedTaskList = new List<TaskControl>();
            this.formingView();

        }

        /// <summary>
        ///  Forming View 
        /// </summary>
        private void formingView()
        {
            #region textbox
            txt_name.Text = "Add a to-do..";
            txt_name.FontStyle = FontStyles.Italic;
            txt_name.GotFocus += txt_name_GotFocus;
            txt_name.LostFocus += txt_name_LostFocus;
            txt_name.KeyUp += txt_name_Keyup;
            #endregion

            #region datePicker

            dtp_Date.Loaded += delegate
            {
                var textBox1 = (TextBox)dtp_Date.Template.FindName("PART_TextBox", dtp_Date);
                textBox1.Background = dtp_Date.Background;
                textBox1.BorderBrush = dtp_Date.Background;
            };
            dtp_Date.KeyUp += dtp_Date_KeyUp;

            #endregion

            #region stackpanel
            stck_completedTask.Visibility = Visibility.Hidden;
            #endregion

            #region button_sort
            btn_Sort.Click += btn_Sort_Click;
            #endregion

            #region showComplete button

            btn_completed.Visibility = completedTaskList.Count > 0 ? Visibility.Visible : Visibility.Hidden;
            btn_completed.Click += btn_completed_Click;
            isShown = false;

            #endregion
        }



        /// <summary>
        /// 
        /// </summary>
        private void resetControls()
        {
            txt_name.Text = string.Empty;
            dtp_Date.SelectedDate = null;
            dtp_Date.DisplayDate = DateTime.Today;
            dtp_Date.Text = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        private void buildTask()
        {
            if (!String.IsNullOrEmpty(txt_name.Text) && !String.IsNullOrWhiteSpace(txt_name.Text))
            {
                TaskControl tsk = new TaskControl();
                tsk.lbl_taskName.Content = txt_name.Text;
                tsk.chk_Complete.Checked += chk_InComplete_Checked;
                tsk.chk_Complete.IsChecked = false;

                tsk.lbl_Date.Content = dtp_Date.SelectedDate != null ? dtp_Date.SelectedDate.Value.ToString("G") : DateTime.Now.ToString("G");
                tsk.lbl_Imp.Background = (bool)tglBtn.IsChecked ? Brushes.Red : null;

                Thickness margin = tsk.Margin;
                margin.Top = 2;
                tsk.Margin = margin;
                tsk.Background = Brushes.YellowGreen;

                taskList.Add(tsk);
                //lstBox.ItemsSource = taskList;

                stck_task.Children.Add(tsk);
            }

            resetControls();

        }

        private UIElement FindVisualChild<childitem>(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is UIElement)
                    return (UIElement)child;
                else
                {
                    UIElement childOfChild = FindVisualChild<UIElement>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        #region Event handlers



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_completed_Click(object sender, RoutedEventArgs e)
        {

            isShown = !isShown;

            if (isShown)
            {
                stck_completedTask.Visibility = Visibility.Visible;
                ((Button)sender).Content = "Hide Completed To-Dos";

                this.completedTaskList.ForEach(x =>
                {
                    x.chk_Complete.Unchecked += chk_Complete_Unchecked;

                });
            }
            else
            {
                stck_completedTask.Visibility = Visibility.Hidden;
                ((Button)sender).Content = "Show Completed To-Dos";
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_Complete_Unchecked(object sender, RoutedEventArgs e)
        {
            WrapPanel currentTaskPanel = (WrapPanel)LogicalTreeHelper.GetParent((CheckBox)sender);
            Border border = (Border)LogicalTreeHelper.GetParent(currentTaskPanel);
            TaskControl currentCompleteTask = (TaskControl)LogicalTreeHelper.GetParent(border);
            string taskName = Convert.ToString(currentCompleteTask.lbl_taskName.Content);
            completedTaskList.Remove(currentCompleteTask);
            currentCompleteTask.chk_Complete.IsChecked = false;
            currentCompleteTask.chk_Complete.Checked += chk_InComplete_Checked;
            if (!taskList.Contains(currentCompleteTask))
            {

                taskList.Add(currentCompleteTask);
                stck_completedTask.Children.Remove(currentCompleteTask);
                this.RemoveVisualChild(currentCompleteTask);
                Thickness margin = currentCompleteTask.Margin;
                margin.Top = 2;
                currentCompleteTask.Margin = margin;
                currentCompleteTask.Background = Brushes.YellowGreen;

                stck_task.Children.Add(currentCompleteTask);

            }

            btn_completed.Visibility = completedTaskList.Count > 0 ? Visibility.Visible : Visibility.Hidden;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_InComplete_Checked(object sender, RoutedEventArgs e)
        {

            bool isChecked = ((CheckBox)sender).IsChecked.Value;
            //TaskControl currentTask = (TaskControl)FindVisualChild<CheckBox>(stck_task);
            WrapPanel currentTaskPanel = (WrapPanel)LogicalTreeHelper.GetParent((CheckBox)sender);
            Border border = (Border)LogicalTreeHelper.GetParent(currentTaskPanel);
            TaskControl currentTask = (TaskControl)LogicalTreeHelper.GetParent(border);
            string taskName = Convert.ToString(currentTask.lbl_taskName.Content);

            currentTask.chk_Complete.Unchecked += chk_Complete_Unchecked;
            Thickness margin = currentTask.Margin;
            margin.Top = 2;
            currentTask.Margin = margin;
            currentTask.Background = Brushes.WhiteSmoke;
            taskList.Remove(currentTask);

            if (!completedTaskList.Contains(currentTask))
            {

                completedTaskList.Add(currentTask);
                stck_task.Children.Remove(currentTask);
                this.RemoveVisualChild(currentTask);

                stck_completedTask.Children.Add(currentTask);
            }

            btn_completed.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtp_Date_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!String.IsNullOrEmpty(txt_name.Text) && !String.IsNullOrWhiteSpace(txt_name.Text))
                {
                    buildTask();
                }
                resetControls();
            }
            else
            {
                return;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tglBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleButton button = (ToggleButton)sender;
            button.Background = Brushes.Green;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tglBtn_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton button = (ToggleButton)sender;
            button.Background = Brushes.Green;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_name_Keyup(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).FontStyle = FontStyles.Normal;
            if (e.Key == Key.Enter)
            {
                if (!String.IsNullOrEmpty(txt_name.Text) && !String.IsNullOrWhiteSpace(txt_name.Text))
                {
                    buildTask();
                }
                resetControls();
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Event for Lose focus for the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == string.Empty)
            {
                ((TextBox)sender).Text = "Add a to-do..";
                ((TextBox)sender).FontStyle = FontStyles.Italic;
            }
        }

        /// <summary>
        /// Event for On focus for the TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).FontStyle == FontStyles.Italic)
            {
                ((TextBox)sender).Text = string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Sort_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
            foreach (var item in (sender as Button).ContextMenu.Items)
            {
                MenuItem mItem = (MenuItem)item;
                mItem.Click += mItem_Click;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mItem_Click(object sender, RoutedEventArgs e)
        {
            String item = ((MenuItem)sender).Header.ToString();

            if (item.Contains("Date"))
            {
                sortDatewise();
            }
            else
            {
                sortImportantwise();
            }


        }
        #endregion

        #region sorting

        /// <summary>
        /// Sorting the list task datewise
        /// </summary>
        private void sortDatewise()
        {
            List<TaskControl> sortedList = taskList.OrderByDescending(task => task.lbl_Date.Content).ToList();
            stck_task.Children.Clear();
            this.taskList.Clear();
            this.taskList = sortedList;
            this.taskList.ForEach(task =>
            {
                stck_task.Children.Add(task);
            });
        }


        /// <summary>
        /// Sorting the task list according to the important tasks
        /// </summary>
        private void sortImportantwise()
        {
            List<TaskControl> sortedList = taskList.OrderByDescending(task => task.lbl_Date.Content).ToList().OrderByDescending(task => task.lbl_Imp.Background).ToList();
            stck_task.Children.Clear();
            this.taskList.Clear();
            this.taskList = sortedList;
            this.taskList.ForEach(task =>
            {
                stck_task.Children.Add(task);
            });
        }

        #endregion

    }
}
