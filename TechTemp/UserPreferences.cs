using System.Text;

namespace TechTemp
{
    class UserPreferences
    {
                #region Member Variables

        private double _windowTop;
        private double _windowLeft;
        private double _windowHeight;
        private double _windowWidth;
        private bool _checkboxflag;
        private System.Windows.WindowState _windowState;

        #endregion //Member Variables

        #region Public Properties

        public double WindowTop
        {
            get { return _windowTop; }
            set { _windowTop = value; }
        }

        public double WindowLeft
        {
            get { return _windowLeft; }
            set { _windowLeft = value; }
        }

        public double WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }

        public double WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        public System.Windows.WindowState WindowState
        {
            get { return _windowState; }
            set { _windowState = value; }
        }

        public bool Checkboxflag
        {
            get { return _checkboxflag; }
            set { _checkboxflag = value; }
        }
        #endregion //Public Properties

        #region Constructor

        public UserPreferences()
        {
            //Load the settings
            Load();

            //Move the window at least partially into view
            MoveIntoView();
        }

        #endregion //Constructor

        #region Functions

        /// <summary>
        /// If the window is more than half off of the screen move it up and to the left 
        /// so half the height and half the width are visible.
        /// </summary>
        public void MoveIntoView()
        {
            if (_windowTop + _windowHeight / 2 > System.Windows.SystemParameters.VirtualScreenHeight)
            {
                _windowTop = System.Windows.SystemParameters.VirtualScreenHeight - _windowHeight;
            }

            if (_windowLeft + _windowWidth / 2 > System.Windows.SystemParameters.VirtualScreenWidth)
            {
                _windowLeft = System.Windows.SystemParameters.VirtualScreenWidth - _windowWidth;
            }

            if (_windowTop < 0)
            {
                _windowTop = 0;
            }

            if (_windowLeft < 0)
            {
                _windowLeft = 0;
            }
        }

        private void Load()
        {
            _windowTop = Properties.Settings.Default.WindowTop;
            _windowLeft = Properties.Settings.Default.WindowLeft;
            _windowHeight = Properties.Settings.Default.WindowHeight;
            _windowWidth = Properties.Settings.Default.WindowWidth;
        }

        public void Save()
        {
            if (_windowState != System.Windows.WindowState.Minimized)
            {
                Properties.Settings.Default.WindowTop = _windowTop;
                Properties.Settings.Default.WindowLeft = _windowLeft;

                Properties.Settings.Default.Save();
            }
        }
        public void SaveColor(StringBuilder hexColor, bool WhiteThemeFlag)
        {
            Properties.Settings.Default.Theme = hexColor.ToString();
            Properties.Settings.Default.Foreground = WhiteThemeFlag == true ? "#FF000000" : "#FFFFFFFF";
        }
        
        public void SaveStartUp(bool NewCheckFlag)
        {
            Properties.Settings.Default.Checkbox = NewCheckFlag;
        }
    
        #endregion //Functions
    }
}
