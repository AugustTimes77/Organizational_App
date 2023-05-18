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
using System.Windows.Threading;

namespace OrgApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer timer;
        private TimeSpan countdownTime;

        public MainWindow()
        {
            InitializeComponent();

            //initialize font style
            Style = (Style)FindResource(typeof(Window));


            // timer code initializes
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1);

            // attach GotFocus event handlers to clear the text
            TimerInput_H.GotFocus += TimerInput_GotFocus;
            TimerInput_M.GotFocus += TimerInput_GotFocus;
            TimerInput_S.GotFocus += TimerInput_GotFocus;
            // end timer code initializes

        }
        
        // timer code begin
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (countdownTime.TotalSeconds > 0)
            {
                countdownTime = countdownTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateTimerText(countdownTime);
            }
            else {
                timer.Stop();
                MessageBox.Show("Times up!");
            }
        }

        private void Button_Timer_Start_Click (object sender, RoutedEventArgs e)
        {
            if(ParseTimeInupt(out TimeSpan time)){
                countdownTime = time;
                UpdateTimerText(countdownTime);
                timer.Start();
            }
        }

        private bool ParseTimeInupt(out TimeSpan time)
        {
            int hours, minutes, seconds;

            if (int.TryParse(TimerInput_H.Text, out hours) &&
                int.TryParse(TimerInput_M.Text, out minutes) &&
                int.TryParse(TimerInput_S.Text, out seconds))
            {
                time = new TimeSpan(hours, minutes, seconds);
                return true;
            }

            MessageBox.Show("Invalid Time Input!");
            time = TimeSpan.Zero;
            return false;
        }

        private void UpdateTimerText(TimeSpan time)
        {
            TimerItself.Text = time.ToString(@"hh\:mm\:ss");
        }

        private void TimerInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = string.Empty;
        }

        // timer code end

    }
}
