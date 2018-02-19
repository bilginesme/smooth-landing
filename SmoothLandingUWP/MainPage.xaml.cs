using System;
using System.Collections.Generic;
using System.Linq;
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
using Windows.System.Threading;

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

        const int hProgress = 6;
        const int wProgressWork = 50;
        const int wProgressRest = 10;
        const int wProgressRestLong = 30;
        const int yProgress = 40;
        const int xStartProgress = 10;

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
            formSizes.Add(FormStateEnum.Compact, new Size(300, 160));
            formSizes.Add(FormStateEnum.CompactWithImage, new Size(300, 260));

            settings = new SettingsInfo();

            /*
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(100, 100));
            if (!ApplicationView.GetForCurrentView().TryResizeView(new Size(100, 100)))
                Msgbox.Show("not possible");
                */

            ThisIsIt();

            /*
            TextBlock textBlock1 = new TextBlock();
            textBlock1.Text = "Hello, world!";
            textBlock1.RenderTransform = new TranslateTransform { X = 50, Y = 50 };
            theGrid.Children.Add(textBlock1);
            */

            CmdPlay.RenderTransform = new TranslateTransform { X = 200, Y = 20 };
            CmdPause.RenderTransform = new TranslateTransform { X = 200, Y = 20 };
            TxtCounter.RenderTransform = new TranslateTransform { X = 20, Y = 30 };

            InitPomodoro();

            Action<Pomodoro> callback = o => { object result = o; };
            XMLEngine.GetPomodoroFromXML(pomodoro, callback);
            pomodoro.Pause();

            ElementSoundPlayer.State = ElementSoundPlayerState.On;
            ElementSoundPlayer.Volume = 0.5;

            DispatcherTimer Timer60Seconds = new DispatcherTimer();
            Timer60Seconds.Tick += Timer60SecondsTick;
            Timer60Seconds.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            Timer60Seconds.Start();

            UpdateEverything();
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

        private async void UpdateEverything()
        {
            TxtCounter.Text = pomodoro.GetSmartDisplay();
            TxtStatus.Text = pomodoro.Status.ToString();
            TxtState.Text = pomodoro.State.ToString();
            await XMLEngine.UpdatePomodoro(pomodoro);
            HandleDayTransition();
        }

        private void Timer60SecondsTick(object sender, object e)
        {
            pomodoro.AddOneSecond();
            UpdateEverything();
        }
        private void HandleDayTransition()
        {
            /*
            XaramaButtonInfo cmdInfo = buttons.Find(i => i.Context == XaramaButtonInfo.ContextEnum.Info);
            if (pomodoro.IsDateDifferent())
                cmdInfo.Show();
            else
                cmdInfo.Hide();

            if (!isStartedDayTransition && pomodoro.IsDateDifferent() && settings.IsItAfterDayEndNow())
            {
                isStartedDayTransition = true;
                if (MessageBox.Show("As the new day started, the session will be initialized.\n\nYou can always change the new day starting hour from the settings.", "A new day started", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    InitPomodoro();
                    isStartedDayTransition = false;
                }
            }
            */
        }

        private void Pomodoro_OnPaused(object sender, Pomodoro.PausedArgs e)
        {
            CmdPause.Visibility = Visibility.Collapsed;
            CmdPlay.Visibility = Visibility.Visible;
        }
        private void Pomodoro_OnStarted(object sender, Pomodoro.StartedArgs e)
        {
            CmdPause.Visibility = Visibility.Visible;
            CmdPlay.Visibility = Visibility.Collapsed;
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
            UpdateEverything();
        }
        private void Pomodoro_OnPomodoroCompleted(object sender, Pomodoro.PomodoroCompletedArgs e)
        {
            XMLEngine.UpdateStatistics(pomodoro);
        }
        private async void ThisIsIt()
        {
            ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            compactOptions.CustomSize = formSizes[FormStateEnum.Compact];
            //ApplicationView.GetForCurrentView().SetPreferredMinSize(formSizes[FormStateEnum.Compact]);
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

            if (!ApplicationView.GetForCurrentView().TryResizeView(formSizes[formState]))
                Msgbox.Show("not possible");
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Call app specific code to subscribe to the service. For example:
            ContentDialog subscribeDialog = new ContentDialog
            {
                Title = "Subscribe to App Service?",
                Content = "Listen, watch, and play in high definition for only $9.99/month. Free to try, cancel anytime.",
                CloseButtonText = "Not Now",
                PrimaryButtonText = "Subscribe",
                SecondaryButtonText = "Try it"
            };

            ContentDialogResult result = await subscribeDialog.ShowAsync();
        }
        private void CmdPause_Click(object sender, RoutedEventArgs e)
        {
            pomodoro.Pause();
            UpdateEverything();
        }
        private void CmdPlay_Click(object sender, RoutedEventArgs e)
        {
            pomodoro.Start();
            UpdateEverything();
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
