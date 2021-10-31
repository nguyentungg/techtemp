using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Windows;

namespace TechTemp
{
    public class Settings
    {
        public bool AdministratorCheck()
        {
            bool isAdmin;
            try
            {
                //get the currently logged in user

                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }

            catch (Exception ex)
            {
                isAdmin = false;
            }

            return isAdmin;
        }

        public bool CheckRegistryStartup()
        {
            bool Checkflag = false;
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (key.GetValue("TechTemp") != null) Checkflag = true;
                    else Checkflag = false;
                }
            }
            catch (Exception ex)
            {
                Checkflag = false;
                MessageBox.Show("" + ex);
            }
            return Checkflag;
        }
        public bool CheckRegistryAllStartup()
        {
            bool Checkflag = false;
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (key.GetValue("TechTemp") != null) Checkflag = true;
                    else Checkflag = false;
                }
            }
            catch (Exception ex)
            {
                Checkflag = false;
                MessageBox.Show("" + ex);
            }
            return Checkflag;
        }

        public void AddApplicationToCurrentUserStartup()
        {
            try {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    key.SetValue("TechTemp", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }

        public void AddApplicationToAllUserStartup()
        {

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("TechTemp", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
            }
        }

        public void RemoveApplicationFromCurrentUserStartup()
        {
            try {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    key.DeleteValue("TechTemp", false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }

        public void RemoveApplicationFromAllUserStartup()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue("TechTemp", false);
            }
        }

        public static bool IsUserAdministrator()
        {
            //bool value to hold our return value
            bool isAdmin;

            try
            {
                //get the currently logged in user

                WindowsIdentity user = WindowsIdentity.GetCurrent();

                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }

            catch (Exception ex)
            {
                isAdmin = false;
            }

            return isAdmin;
        }
    }
}
