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
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media.Imaging;
 
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

        RingSlice rsInner;
        Dictionary<int, RingSlice> ringSlicesWork;
        Dictionary<int, RingSlice> ringSlicesRest;
        double opacityWorkMin = 0.25, opacityRestMin = 0.15;
        double opacityWorkMax = 1, opacityRestMax = 1;
        #endregion

        DispatcherTimer Timer60Seconds;
        DispatcherTimer TimerBlink;

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

            CmdPlay.RenderTransform = new TranslateTransform { X = 250, Y = 10 };
            CmdPause.RenderTransform = new TranslateTransform { X = 250, Y = 10 };

            //TxtCounter.RenderTransform = new TranslateTransform { X = 80, Y = 18 };

            Timer60Seconds = new DispatcherTimer();
            Timer60Seconds.Tick += Timer60SecondsTick;
            Timer60Seconds.Interval = new TimeSpan(0, 0, 0, 0, 10);
            Timer60Seconds.Start();

            TimerBlink = new DispatcherTimer();
            TimerBlink.Tick += TimerBlinkTick;
            TimerBlink.Interval = new TimeSpan(0, 0, 0, 0, 10);
            TimerBlink.Start();

            InitPomodoro();

            Action<Pomodoro> callback = o => { object result = o; };
            XMLEngine.GetPomodoroFromXML(pomodoro, callback);
            pomodoro.Pause();

            ElementSoundPlayer.State = ElementSoundPlayerState.On;
            ElementSoundPlayer.Volume = 0.5;

            UpdateEverything();

            /*
            Image imgNew = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri("ms-appx:///Assets/tomato-normal.png");
            bitmapImage.UriSource = uri;
            imgNew.Source = bitmapImage;
            Canvas.SetLeft(imgNew, 20);
            theCanvas.Children.Add(imgNew);
            */

            ConstructRingSlices();

            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            Windows.ApplicationModel.Core.CoreApplicationViewTitleBar coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            //theCanvas.Background = new SolidColorBrush(Windows.UI.Colors.Red);
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
        private void ConstructRingSlices()
        {
            Brush brushInner = new SolidColorBrush(Windows.UI.Color.FromArgb(200, 84, 168, 168));
            Brush brushWork = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 218, 99, 89));
            Brush brushRest = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 226, 237, 53));

            double angleWork = 360 *  (double)Pomodoro.GetPercentageWorkSessionOverTotal() / 100;
            double angleRestShort = 360 * (double)Pomodoro.GetPercentageRestingShortSessionOverTotal() / 100;
            double angleRestLong = 360 * (double)Pomodoro.GetPercentageRestingLongSessionOverTotal() / 100;

            double startAngle = 0;
            double endAngle = 0;

            double radius = 36;
            double thickness = 7;
            double xPos = 5, yPos = 0;

            ringSlicesWork = new Dictionary<int, RingSlice>();
            ringSlicesRest = new Dictionary<int, RingSlice>();

            for (int i=1;i<=Pomodoro.NumSlicesForPomodoro;i++)
            {
                startAngle = (i - 1) * (angleWork + angleRestShort);
                endAngle = startAngle + angleWork;

                RingSlice rsWork = new RingSlice()
                {
                    StartAngle = startAngle,
                    EndAngle = endAngle,
                    Fill = brushWork,
                    Radius = radius,
                    InnerRadius = radius-thickness,
                    Stroke = brushWork
                };
                rsWork.RenderTransform = new TranslateTransform { X = xPos, Y = yPos };
                theCanvas.Children.Add(rsWork);
                ringSlicesWork.Add(i, rsWork);

                startAngle = endAngle;
                if(i != Pomodoro.NumSlicesForPomodoro)
                    endAngle = startAngle + angleRestShort;
                else
                    endAngle = startAngle + angleRestLong;

                if (endAngle >= 360)
                    endAngle = 359;

                RingSlice rsRest = new RingSlice()
                {
                    StartAngle = startAngle,
                    EndAngle = endAngle,
                    Fill = brushRest,
                    Radius = radius,
                    InnerRadius = radius - thickness,
                    Stroke = brushRest
                };
                rsRest.RenderTransform = new TranslateTransform { X = xPos, Y = yPos };
                theCanvas.Children.Add(rsRest);
                ringSlicesRest.Add(i, rsRest);
            }

            rsInner = new RingSlice()
            {
                StartAngle = 0,
                EndAngle = 0,
                Fill = brushInner,
                Radius = radius - thickness,
                InnerRadius = 1,
                Stroke = brushInner
            };
            rsInner.RenderTransform = new TranslateTransform { X = xPos + thickness, Y = thickness };
            theCanvas.Children.Add(rsInner);
        }

        private async void UpdateEverything()
        {
            TxtCounter.Text = pomodoro.GetSmartDisplay();
            TxtStatus.Text = pomodoro.Status.ToString();
            TxtState.Text = pomodoro.State.ToString();
            await XMLEngine.UpdatePomodoro(pomodoro);
            HandleDayTransition();
            UpdatePomodoros();
            UpdateRings();
        }

        private void Timer60SecondsTick(object sender, object e)
        {
            pomodoro.AddOneSecond();
            UpdateEverything();
        }
        private void TimerBlinkTick(object sender, object e)
        {
            double opacity = rsInner.Opacity + 0.02;
            if (opacity > 1)
                opacity = 0.5;

            rsInner.Opacity = opacity;
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
            TimerBlink.Start();
        }
        private void Pomodoro_OnStarted(object sender, Pomodoro.StartedArgs e)
        {
            CmdPause.Visibility = Visibility.Visible;
            CmdPlay.Visibility = Visibility.Collapsed;
            TimerBlink.Stop();
        }
        private void Pomodoro_OnWorkJustCompleted(object sender, Pomodoro.WorkJustCompletedArgs e)
        {
            DisplayToast();
        }
        private void Pomodoro_OnRestJustCompleted(object sender, Pomodoro.RestJustCompletedArgs e)
        {
            DisplayToast();
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
        private void DisplayToast()
        {
            string strMessage = string.Empty;
            if (pomodoro.State == Pomodoro.StateEnum.WorkCompleted)
            {
                if (pomodoro.SliceNow == 4)
                    strMessage = "Now you can have a larger break of 15 minutes";
                else
                    strMessage = "Time to rest for 5 minutes";
            }
            else if (pomodoro.State == Pomodoro.StateEnum.RestingShortCompleted)
            {
                strMessage = "OK! Rest is over, back to work.";
            }
            else if (pomodoro.State == Pomodoro.StateEnum.RestingLongCompleted)
            {
                strMessage = "Rest is over.\nIf you have enough time to handle another pomodoro, go for it!";
            }

            // Clear all existing notifications
            ToastNotificationManager.History.Clear();

            var longTime = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("longtime");
            DateTimeOffset expiryTime = DateTime.Now.AddSeconds(15);
            string expiryTimeString = longTime.Format(expiryTime);

            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText03);
            //Find the text component of the content
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");

            // Set the text on the toast. 
            // The first line of text in the ToastText02 template is treated as header text, and will be bold.
            toastTextElements[0].AppendChild(toastXml.CreateTextNode("Smooth Landing"));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(strMessage));

            // Set the duration on the toast
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");

            // Create the actual toast object using this toast specification.
            ToastNotification toast = new ToastNotification(toastXml);

            toast.ExpirationTime = expiryTime;

            // Send the toast.
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
        private void UpdatePomodoros()
        {
            imgPomodoro1.Opacity = 0.2;
            imgPomodoro2.Opacity = 0.2;
            imgPomodoro3.Opacity = 0.2;
            txtPlus.Text = "";
            txtPlus.Visibility = Visibility.Collapsed;

            int n = pomodoro.NumPomodorosToday;

            if (n > 0)
                imgPomodoro1.Opacity = 1;
            if (n > 1)
                imgPomodoro2.Opacity = 1;
            if (n > 2)
                imgPomodoro3.Opacity = 1;

            if (n > 3)
            {
                txtPlus.Text = "+" + (n - 3);
                txtPlus.Visibility = Visibility.Visible;
            }

            return;

            int posY = 20;
            for (int i = 0; i < 3; i++)
            {
                int posX = 50 + 25 * i;

                Image img = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                Uri uri = new Uri("ms-appx:///Assets/tomato-normal.png");

                if (i < pomodoro.NumPomodorosToday)
                    new Uri("ms-appx:///Assets/tomato-normal.png");
                else
                    new Uri("ms-appx:///Assets/tomato-transparent.png");
                 
                bitmapImage.UriSource = uri;
                img.Source = bitmapImage;
                img.Width = 24;
                img.Height = 24;
                Canvas.SetLeft(img, posX);
                Canvas.SetTop(img, posY);

                theCanvas.Children.Add(img);
            }
          
            //if (pomodoro.NumPomodorosToday > 3)
                //dc.DrawString("+" + (pomodoro.NumPomodorosToday - 3), fontBold, brushNormal, 206, 11);
        }
        private void UpdateRings()
        {
            int percentage = (int)(100f * pomodoro.GetActualTimeForThisPomodoro() / Pomodoro.GetTotalPomodoroTime());
            txtPercentage.Text = percentage + "%";
            rsInner.StartAngle = 0;
            rsInner.EndAngle = (int)(percentage * 360f / 100f);
            if (rsInner.EndAngle == 360)
                rsInner.EndAngle = 359.99999;
           
            double opacityWork = 1, opacityRest = 1;

            for (int i = 1; i <= Pomodoro.NumSlicesForPomodoro; i++)
            {
                
                if (pomodoro.State == Pomodoro.StateEnum.Working)
                {
                    if (i < pomodoro.SliceNow)
                    {
                        opacityWork = opacityWorkMax;
                        opacityRest = opacityRestMax;
                    }
                    else
                    {
                        opacityWork = opacityWorkMin;
                        opacityRest = opacityRestMin;
                    }
                }
                else if (pomodoro.State == Pomodoro.StateEnum.WorkCompleted)
                {
                    if (i <= pomodoro.SliceNow)
                        opacityWork = opacityWorkMax;
                    else
                        opacityWork = opacityWorkMin;

                    if (i <= pomodoro.SliceNow - 1)
                        opacityRest = opacityRestMax;
                    else
                        opacityRest = opacityRestMin;
                }
                else if (pomodoro.State == Pomodoro.StateEnum.RestingShort || pomodoro.State == Pomodoro.StateEnum.RestingLong)
                {
                    if (i < pomodoro.SliceNow)
                        opacityRest = opacityRestMax;
                    else
                        opacityRest = opacityRestMin;

                    if (i <= pomodoro.SliceNow)
                        opacityWork = opacityWorkMax;
                    else
                        opacityWork = opacityWorkMin;
                }
                else if (pomodoro.State == Pomodoro.StateEnum.RestingShortCompleted || pomodoro.State == Pomodoro.StateEnum.RestingLongCompleted)
                {
                    if (i <= pomodoro.SliceNow)
                        opacityRest = opacityRestMax;
                    else
                        opacityRest = opacityRestMin;

                    if (i <= pomodoro.SliceNow)
                        opacityWork = opacityWorkMax;
                    else
                        opacityWork = opacityWorkMin;
                }

                ringSlicesWork[i].Opacity = opacityWork;
                ringSlicesRest[i].Opacity = opacityRest;
            }

            
        }
        #endregion

        #region Form Events
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryResizeView(new Windows.Foundation.Size(300, 190));
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
