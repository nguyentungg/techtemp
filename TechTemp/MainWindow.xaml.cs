using System;
using System.Text;
using System.Windows;
using OpenHardwareMonitor.Hardware;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace TechTemp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Computer cp;
        private bool WhiteThemeFlag;
        private bool ChangeFlag;
        private bool OldCheckFlag;
        private bool NewCheckFlag;

        Settings setting = new Settings();
        public MainWindow()
        {
            InitializeComponent();

            cp = new Computer();
            
            cp.CPUEnabled = true;
            cp.HDDEnabled = true;
            cp.GPUEnabled = true;
            //cp.FanControllerEnabled = false;
            //cp.RAMEnabled = false;
            //cp.MainboardEnabled = false;

            cp.Open();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            label.Content = System.Environment.MachineName;

            OldCheckFlag = Properties.Settings.Default.Checkbox;
            NewCheckFlag = Properties.Settings.Default.Checkbox;

            CheckStarupException();
            GetName();
        }

        //public void GetHardwareInfomation(string hwclass, string syntax)
        //{ 
        //    try
        //    {
        //        //ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI",
        //        //    "SELECT * FROM MSAcpi_ThermalZoneTemperature");
        //        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2",
        //           "SELECT * FROM " + hwclass);
        //        foreach (ManagementObject queryObj in searcher.Get())
        //        {
        //            //Console.WriteLine("CurrentTemperature: {0}", queryObj["CurrentTemperature"]);
        //            label.Content = Convert.ToString(queryObj[syntax]);
        //        }
        //    }
        //    catch (ManagementException e)
        //    {
        //        MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
        //    }
        //}

        //public void GetHardwareTemp()
        //{
        //    try
        //    {
        //        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI",
        //                         "SELECT * FROM MSAcpi_ThermalZoneTemperature");

        //        foreach (ManagementObject obj in searcher.Get())
        //        {
        //            Double temp = Convert.ToDouble(obj.Properties["CurrentTemperature"].Value.ToString());
        //            label1.Content = (temp / 10) - 273.15 + "°C";

        //        }

        //    }
        //    catch (ManagementException e)
        //    {
        //        MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
        //    }
        //}

        StringBuilder hexColor = new StringBuilder();
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            GetTemp();
        }

        private void GetName()
        {
            StringBuilder strBuild = new StringBuilder();
            foreach (var hardwareItem in cp.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.CPU || hardwareItem.HardwareType == HardwareType.HDD || hardwareItem.HardwareType == HardwareType.GpuNvidia)
                {
                    hardwareItem.Update();
                    foreach (IHardware subHardware in hardwareItem.SubHardware)
                        subHardware.Update();

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if(sensor.Name == "Temperature") strBuild.Append(String.Format("{0}\r\n", "HDD"));
                            else strBuild.Append(String.Format("{0}\r\n",sensor.Name));
                        }
                    }
                }
            }
            this.ListName.Items.Clear();
            this.ListName.Items.Add(strBuild);
        }

        private void GetTemp()
        {
            StringBuilder strBuildValues = new StringBuilder();
            foreach (var hardwareItem in cp.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.CPU || hardwareItem.HardwareType == HardwareType.HDD || hardwareItem.HardwareType == HardwareType.GpuNvidia)
                {
                    hardwareItem.Update();
                    foreach (IHardware subHardware in hardwareItem.SubHardware)
                        subHardware.Update();

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            strBuildValues.Append(String.Format("{0} °C ({1} °F)\r\n",sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value", sensor.Value.HasValue ? (Math.Round((decimal)CelsiusToFahrenheit(sensor.Value))).ToString() : "no value"));
                        }
                    }
                }

            }
            this.ListValue.Items.Clear();
            this.ListValue.Items.Add(strBuildValues);
        }

        public static float? CelsiusToFahrenheit(float? valueInCelsius)
        {
            return valueInCelsius * 1.8f + 32;
        }

        private void CheckStarupException()
        {
            if (!setting.CheckRegistryStartup() && Properties.Settings.Default.Checkbox) Properties.Settings.Default.Checkbox = NewCheckFlag = false;
            else if (setting.CheckRegistryStartup() && !Properties.Settings.Default.Checkbox) Properties.Settings.Default.Checkbox = NewCheckFlag = true;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        #region ContextMenuColor
        private StringBuilder ChangeColor(string hex)
        {
            hexColor.Clear();
            hexColor.Append(hex);
            return hexColor;
        }

        private void RedMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E74C3C"));
            this.Background.Opacity = 0.5;
            label.Foreground = new SolidColorBrush(Colors.White);
            ListName.Foreground = new SolidColorBrush(Colors.White);
            ListValue.Foreground = new SolidColorBrush(Colors.White);
            ChangeColor("#E74C3C");
            WhiteThemeFlag = false;
            ChangeFlag = true;
        }

        private void GreenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00a300"));
            this.Background.Opacity = 0.5;
            label.Foreground = new SolidColorBrush(Colors.White);
            ListName.Foreground = new SolidColorBrush(Colors.White);
            ListValue.Foreground = new SolidColorBrush(Colors.White);
            ChangeColor("#00a300");
            WhiteThemeFlag = false;
            ChangeFlag = true;
        }
        private void OrangeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EB7F00"));
            this.Background.Opacity = 0.5;
            label.Foreground = new SolidColorBrush(Colors.White);
            ListName.Foreground = new SolidColorBrush(Colors.White);
            ListValue.Foreground = new SolidColorBrush(Colors.White);
            ChangeColor("#EB7F00");
            WhiteThemeFlag = false;
            ChangeFlag = true;
        }
        private void BlueMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3498DB"));
            this.Background.Opacity = 0.5;
            label.Foreground = new SolidColorBrush(Colors.White);
            ListName.Foreground = new SolidColorBrush(Colors.White);
            ListValue.Foreground = new SolidColorBrush(Colors.White);
            ChangeColor("#3498DB");
            WhiteThemeFlag = false;
            ChangeFlag = true;
        }
        private void PinkMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF8598"));
            this.Background.Opacity = 0.5;
            label.Foreground = new SolidColorBrush(Colors.White);
            ListName.Foreground = new SolidColorBrush(Colors.White);
            ListValue.Foreground = new SolidColorBrush(Colors.White);
            ChangeColor("#FF8598");
            WhiteThemeFlag = false;
            ChangeFlag = true;
        }
        private void YellowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFD34E"));
            this.Background.Opacity = 0.5;
            label.Foreground = new SolidColorBrush(Colors.White);
            ListName.Foreground = new SolidColorBrush(Colors.White);
            ListValue.Foreground = new SolidColorBrush(Colors.White);
            ChangeColor("#FFD34E");
            WhiteThemeFlag = false;
            ChangeFlag = true;
        }
        private void WhiteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.White);
            this.Background.Opacity = 0.5;
            label.Foreground = new SolidColorBrush(Colors.Black);
            ListName.Foreground = new SolidColorBrush(Colors.Black);
            ListValue.Foreground = new SolidColorBrush(Colors.Black);
            ChangeColor("#FFFFFFFF");
            WhiteThemeFlag = true;
            ChangeFlag = true;
        }

        private void BlackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.Black);
            this.Background.Opacity = 0.5;
            label.Foreground = new SolidColorBrush(Colors.White);
            ListName.Foreground = new SolidColorBrush(Colors.White);
            ListValue.Foreground = new SolidColorBrush(Colors.White);
            ChangeColor("#FF000000");
            WhiteThemeFlag = false;
            ChangeFlag = true;
        }
        #endregion

        
        private void CheckBoxMenuItem_Click(object sender, RoutedEventArgs e)
        {

            switch (NewCheckFlag)
            {
                case true:
                    Properties.Settings.Default.Checkbox = NewCheckFlag = false;
                    break;
                case false:
                    Properties.Settings.Default.Checkbox = NewCheckFlag = true;
                    break;
            }
            
        }
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutAuthors about = new AboutAuthors();
            about.ShowDialog();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            cp.Close();
            this.Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == (Key.LeftAlt | Key.E))
            {
                cp.Close();
                this.Close();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            var userPrefs = new UserPreferences();
            if (ChangeFlag == true)
            {
                userPrefs.SaveColor(hexColor, WhiteThemeFlag);
            }
            if (NewCheckFlag != OldCheckFlag)
            {
                switch (NewCheckFlag)
                {
                    case true:
                        setting.AddApplicationToCurrentUserStartup();
                        break;
                    case false:
                        setting.RemoveApplicationFromCurrentUserStartup();
                        break;
                }

                userPrefs.SaveStartUp(NewCheckFlag);
            }
            userPrefs.WindowHeight = this.Height;
            userPrefs.WindowWidth = this.Width;
            userPrefs.WindowTop = this.Top;
            userPrefs.WindowLeft = this.Left;
            userPrefs.WindowState = this.WindowState;

            userPrefs.Save();
        }

      
    }
}
