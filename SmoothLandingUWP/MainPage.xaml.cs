using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmoothLandingUWP
{
    public sealed partial class MainPage : Page
    {
        enum FormStateEnum { Compact, CompactWithImage }

        #region Private Members
        Pomodoro pomodoro;
        SettingsInfo settings;
        //FrmImageEnlarged frmImageEnlarged;

        Dictionary<FormStateEnum, Size> formSizes;
        FormStateEnum formState;
        //List<Bitmap> bmpVistas;
        int numVista;

        private bool mouseDown;
        private Point lastLocation;
        string strMainFolderPath;
        string strPathSound;
        //System.Media.SoundPlayer playerWorkJustCompleted, playerRestJustCompleted, playerAlert, playerClick;
        //List<XaramaButtonInfo> buttons;

        //Font fontTimer;
        //Rectangle rectTimer;
        Brush brushTimer;

        //Font fontNormal, fontBold, fontSmall, fontTiny;
        //StringFormat sfCenter, sfLeft;
        Brush brushNormal;

        //Bitmap bmpPomodoroNormal, bmpPomodoroTransparent;
        //Color borderColor = Color.FromArgb(60, 63, 153, 41);
        //Color insideColorWork = Color.FromArgb(255, 255, 69, 0);
        //Color insideColorWorkDisabled = Color.FromArgb(30, 255, 69, 0);
        //Color insideColorRest = Color.FromArgb(255, 255, 191, 135);
        //Color insideColorRestDisabled = Color.FromArgb(30, 255, 191, 135);
        //Color borderColorWork = Color.FromArgb(255, 255, 165, 0);
        //Color borderColorWorkDisabled = Color.FromArgb(100, 255, 165, 0);
        //Color borderColorRest = Color.FromArgb(255, 255, 204, 100);
        //Color borderColorRestDisabled = Color.FromArgb(100, 255, 204, 100);

        const int hProgress = 6;
        const int wProgressWork = 50;
        const int wProgressRest = 10;
        const int wProgressRestLong = 30;
        const int yProgress = 40;
        const int xStartProgress = 10;

        //const double opacityMin = 0.85, opacityMax = 1.0;
        const double opacityMin = 1.0, opacityMax = 1.0;
        bool isStartedDayTransition = false;
        #endregion


        #region Constructors
        public MainPage()
        {
            this.InitializeComponent();



            //strMainFolderPath = System.IO.Path.GetDirectoryName(Application.);
            strMainFolderPath = new System.IO.DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
            strPathSound = strMainFolderPath + @"\sound\";

            formState = FormStateEnum.Compact;
            formSizes = new Dictionary<FormStateEnum, Size>();
            formSizes.Add(FormStateEnum.Compact, new Size(300, 150));
            formSizes.Add(FormStateEnum.CompactWithImage, new Size(300, 300));

            settings = new SettingsInfo();

            ThisIsIt();
        }
        #endregion

        #region Private Methods
        private void InitPomodoro()
        {
            pomodoro = new Pomodoro();
            pomodoro.OnWorkJustCompleted += Pomodoro_OnWorkJustCompleted;
            pomodoro.OnRestJustCompleted += Pomodoro_OnRestJustCompleted;
            pomodoro.OnStarted += Pomodoro_OnStarted;
            pomodoro.OnPaused += Pomodoro_OnPaused;
            pomodoro.OnForceRePaint += Pomodoro_OnForceRePaint;
            pomodoro.OnPomodoroCompleted += Pomodoro_OnPomodoroCompleted;
            pomodoro.Pause();

            //SetTsDateText();
        }
        private void Pomodoro_OnPaused(object sender, Pomodoro.PausedArgs e)
        {
            /*
            if (buttons != null)
            {
                buttons.Find(i => i.Context == XaramaButtonInfo.ContextEnum.Pause).Hide();
                buttons.Find(i => i.Context == XaramaButtonInfo.ContextEnum.Start).Show();
            }
            */
        }
        private void Pomodoro_OnStarted(object sender, Pomodoro.StartedArgs e)
        {
            /*
            if (buttons != null)
            {
                buttons.Find(i => i.Context == XaramaButtonInfo.ContextEnum.Start).Hide();
                buttons.Find(i => i.Context == XaramaButtonInfo.ContextEnum.Pause).Show();
            }
            */
        }
        private void Pomodoro_OnWorkJustCompleted(object sender, Pomodoro.WorkJustCompletedArgs e)
        {
            /*
            DisplayBaloon();
            playerWorkJustCompleted.Play();
            */
        }
        private void Pomodoro_OnRestJustCompleted(object sender, Pomodoro.RestJustCompletedArgs e)
        {
            /*
            DisplayBaloon();
            playerRestJustCompleted.Play();
            */
        }
        private void Pomodoro_OnForceRePaint(object sender, Pomodoro.ForceRePaintArgs e)
        {
            //Invalidate();
        }
        private void Pomodoro_OnPomodoroCompleted(object sender, Pomodoro.PomodoroCompletedArgs e)
        {
            XMLEngine.UpdateStatistics(pomodoro);
        }
        private async void ThisIsIt()
        {
            ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            compactOptions.CustomSize = formSizes[FormStateEnum.Compact];
            ApplicationView.GetForCurrentView().SetPreferredMinSize(formSizes[FormStateEnum.Compact]);
            bool modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
        }
        #endregion

        #region Form Events
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryResizeView(new Windows.Foundation.Size(300, 150));
        }
        private void Page_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (formState == FormStateEnum.Compact)
                formState = FormStateEnum.CompactWithImage;
            else
                formState = FormStateEnum.Compact;

            ApplicationView.GetForCurrentView().TryResizeView(formSizes[formState]);
        }
        #endregion

        public static class Msgbox
        {
            static public async void Show(string mytext)
            {
                var dialog = new MessageDialog(mytext, "Testmessage");
                await dialog.ShowAsync();
            }
        }
    }
}
