using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kvikna.word
{
    public class WindowViewModel : BaseViewModel
    {

        #region Private Member

        private Window mWindow;
        private int mOuterMarginSize = 10;
        private int mWindowRadius = 10;

        #endregion


        #region Public Properties
        /// <summary>
        /// Resize Border around the window
        /// </summary>
        public int ResizeBorder { get; set; } = 6;

        /// <summary>
        /// The size of the resize border around the window and taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }


        /// <summary>
        /// The padding inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding { get { return new Thickness(ResizeBorder ); } }


        public int OuterMarginSize
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mOuterMarginSize;
                    
            }

            set
            {
                mOuterMarginSize = value;
            }

        }

        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } } 
      

        /// <summary>
        /// The Radius of the edge of the Window
        /// </summary>
        public int WindowRadius
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mWindowRadius;

            }

            set
            {
                mWindowRadius = value;
            }

        }

        /// <summary>
        /// The Radius of the edge of the Window
        /// </summary>
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// The GRidLength of the edge of the Window
        /// </summary>
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }

        public int TitleHeight { get; set; } = 42;


        #endregion

        /// <summary>
        /// Window minimum Height
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;


        /// <summary>
        /// Window Minimum width
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 400;

        #region Commands
        /// <summary>
        /// To minimise the Window
        /// </summary>
        public ICommand MinimizedCommand { get; set; }

        /// <summary>
        /// To Maximize the Window
        /// </summary>
        public ICommand MaximizedCommand { get; set; }


        /// <summary>
        /// To close the Window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// To System menu the Window
        /// </summary>
        public ICommand MenuCommand { get; set; }

        #endregion

        #region Constructor
        public WindowViewModel(Window window)
        {
            mWindow = window;
            mWindow.StateChanged += (sender, e) =>
            {

                //Fire off events for all properties tyhat are affected by a resize
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };

            //Create Commmands

            MinimizedCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizedCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand((mWindow.Close));
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            //fix Window resize issue
            var resizer = new WindowResizer(mWindow);

        }

        #endregion


        #region Private Helpers


        /// <summary>
        /// Get the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            var position = Mouse.GetPosition(mWindow);
            return new Point(position.X + mWindow.Left , position.Y + mWindow.Top);
        }

        #endregion

    }
}
