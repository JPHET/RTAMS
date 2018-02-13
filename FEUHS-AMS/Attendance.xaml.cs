﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FEUHS_AMS
{
    /// <summary>
    /// Interaction logic for Attendance.xaml
    /// </summary>
    public partial class Attendance : Window
    {
        //"+ typeof(Attendance).Namespace +"
        private string path = "pack://application:,,,/FEUHS-AMS;component/Images/";
        private CancellationTokenSource cts;
        private int timeStatus;
        private string mainStatus = "";
        private string table_prefix = "";
        private bool isStop = false;

        public Attendance()
        {
            RealTimeAMS rtams = new RealTimeAMS();
            InitializeComponent();
            this.timeStatus = rtams.timeState();
            this.table_prefix = this.timeStatus == 1 ? "time_out" : "time_in";
            checkAttendance();
        }


        private async void checkAttendance()
        {
            while (!isStop)
            {
                RealTimeAMS rtams = new RealTimeAMS();
                await Task.Delay(100);

                    fullname1.Content = rtams.getFullName(this.table_prefix);

                    try
                    {
                        image.Source = new BitmapImage(new Uri(path + rtams.getImage(this.table_prefix)));
                    }
                    catch (ArgumentException e) { this.mainStatus = e.Message;  }

                    section1.Content = rtams.getSection(this.table_prefix);
                    stdnum1.Content = rtams.getStudentNumber(this.table_prefix);
                    //fullname2.Content = rtams.getLastRFID("time_in"); //Test to check RFID Number                
            }
        }

        private void showAdmin(object sender, MouseButtonEventArgs e)
        {
            cts = new CancellationTokenSource();
            isStop = true;
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
            if (cts != null)
            {
                cts.Cancel();
            }
        }
    }
}
