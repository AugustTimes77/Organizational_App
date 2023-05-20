﻿using System;
using System.Collections.Generic;
using System.IO;
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

        private string NotesFolderPath = "Notes";
        private TabItem[] tabItems;
        private TextBox[] textBoxes;
        private string[] fileNames;

        public MainWindow()
        {
            InitializeComponent();

            CreateNotesFolder();
            InitializeTabItemsandTextBoxes();
            LoadTextFromFiles();


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

            // closing function
            Closing += Window_Closing;

        }

        //universal functionality begins

        private string FindFilePath(string folderPath, string fileName)
        {
            string filePath = System.IO.Path.Combine(folderPath, fileName);
            return filePath;
        }

        private void CreateNotesFolder()
        {

            if (!Directory.Exists(NotesFolderPath))
            {
                Directory.CreateDirectory(NotesFolderPath);
            }
        }

        //universal functionality ends



        // calendar code begins



        // calendar code ends


        // timer code begin
        private void Timer_Tick(object? sender, EventArgs? e)
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
            if(ParseTimeInput(out TimeSpan time)){
                countdownTime = time;
                UpdateTimerText(countdownTime);
                timer.Start();
            }
        }

        private bool ParseTimeInput(out TimeSpan time)
        {
            int hours, minutes, seconds;
            // attempts to parse through each of the timer inputs, if it can't be converted into
            // an integer, move on and return false
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

        // checks if the input boxes have been clicked on, if so then clear them
        private void TimerInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = string.Empty;
        }

    // timer code end

    // notes code begins
        
        private void InitializeTabItemsandTextBoxes()
        {
            tabItems = new TabItem[]
            {
                Tab1,
                Tab2,
                Tab3,
                Tab4
            };
            textBoxes = new TextBox[]
            {
                Text1,
                Text2,
                Text3,
                Text4
            };
            fileNames = new string[]
            {
                "Tab1.txt",
                "Tab2.txt",
                "Tab3.txt",
                "Tab4.txt"
            };
        }

        private void LoadTextFromFiles()
        {
            for (int i = 0; i < tabItems.Length; i++)
            {
                string filePath = FindFilePath(NotesFolderPath, fileNames[i]);
                if (File.Exists(filePath))
                {
                    string text = File.ReadAllText(filePath);
                    textBoxes[i].Text = text;
                }
            }
        }

        private void SaveTextToFile(string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            for (int i = 0; i < tabItems.Length;i++)
            {
                string filePath = FindFilePath(NotesFolderPath, fileNames[i]);
                SaveTextToFile(filePath, textBoxes[i].Text);
            }
        }
        
    // notes code ends




    }
}
