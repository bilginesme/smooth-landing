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
using Windows.ApplicationModel.Core;
using System.Threading.Tasks;

namespace SmoothLandingUWP
{
    public sealed partial class MainPage : Page
    {
        public enum FormStateEnum { VeryCompact, CompactWithImage, Medium }
        public enum ElementsVisibilityEnum { Visible, Hidden}

        #region Private Members
        const int ONE_SECOND = 1;
        Pomodoro pomodoro;
        SettingsInfo settings;
        List<StatsInfo> statistics = new List<StatsInfo>();

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

        DispatcherTimer TimerOneSecond;
        DispatcherTimer TimerBlink;
        DispatcherTimer TimerReminder;
        int numFileVista = 0;

        TimeSpan tsPausedTime;
        TimeSpan tsUpdatePomodoroFile = new TimeSpan();
        const int intervalForUpdatePomodoroFile = 10;

        Style stylePlayPauseButtonMin, stylePlayPauseButtonMax;

        ElementsVisibilityEnum elementsVisibility = ElementsVisibilityEnum.Hidden;
        #endregion

        #region Constructors
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            //strMainFolderPath = System.IO.Path.GetDirectoryName(Application.);
            strMainFolderPath = new System.IO.DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
            strPathSound = strMainFolderPath + @"\sound\";

            formState = FormStateEnum.VeryCompact;
            formSizes = new Dictionary<FormStateEnum, Size>();

            //formSizes.Add(FormStateEnum.VeryCompact, new Size(270, 160));
            //formSizes.Add(FormStateEnum.CompactWithImage, new Size(498, 236));

            // IDEAL
            formSizes.Add(FormStateEnum.VeryCompact, new Size(200, 160));
            formSizes.Add(FormStateEnum.CompactWithImage, new Size(500, 200));

             
            formSizes.Add(FormStateEnum.Medium, new Size(400, 300));
            
            settings = new SettingsInfo();

            SwitchToCompactMode();

            TimerOneSecond = new DispatcherTimer();
            TimerOneSecond.Tick += TimerOneSecondTick;
            TimerOneSecond.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            TimerOneSecond.Start();

            TimerBlink = new DispatcherTimer();
            TimerBlink.Tick += TimerBlinkTick;
            TimerBlink.Interval = new TimeSpan(0, 0, 0, 0, 10);
            TimerBlink.Start();

            TimerReminder = new DispatcherTimer();
            TimerReminder.Tick += TimerReminder_Tick;
            TimerReminder.Interval = new TimeSpan(0, 0, 300);
            TimerReminder.Start();


            InitPomodoro();

            Action<Pomodoro> callback = o => { object result = o; };
            XMLEngine.GetPomodoroFromXML(pomodoro, callback);
            pomodoro.Pause();

            ElementSoundPlayer.State = ElementSoundPlayerState.On;
            ElementSoundPlayer.Volume = 0.5;

            ConstructRingSlices();

            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            Windows.ApplicationModel.Core.CoreApplicationViewTitleBar coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            //theCanvas.Background = new SolidColorBrush(Windows.UI.Colors.Red);

            DisplayVistaImage();
            UpdateEverything();
            HidePrevNextButtons();
            UpdateVistaType();

            UpdateVistaType();
            //Uri targetUri = new Uri("ms-appdata:///local/phaser/index.html");
            //webView.Navigate(targetUri);

            stylePlayPauseButtonMin = (Style)this.Resources["PlayPauseButtonStyleMin"];
            stylePlayPauseButtonMax = (Style)this.Resources["PlayPauseButtonStyleMax"];

            UpdateFormDueToResize();
        }
        #endregion

        #region Pomodoro Events
        private void Pomodoro_OnPaused(object sender, Pomodoro.PausedArgs e)
        {
            CmdPause.Visibility = Visibility.Collapsed;
            CmdPlay.Visibility = Visibility.Visible;

            TimerBlink.Start();
            tsPausedTime = new TimeSpan();
            TxtPausedTime.Visibility = Visibility.Visible;
        }
        private void Pomodoro_OnStarted(object sender, Pomodoro.StartedArgs e)
        {
            CmdPause.Visibility = Visibility.Visible;
            CmdPlay.Visibility = Visibility.Collapsed;

            TimerBlink.Stop();
            tsPausedTime = new TimeSpan();
            TxtPausedTime.Visibility = Visibility.Collapsed;
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

            double radius = 31;
            double thickness = 6;
            double xPos = 4, yPos = 4;

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
                InnerRadius = 3,
                Stroke = brushInner
            };
            rsInner.RenderTransform = new TranslateTransform { X = xPos + thickness, Y = yPos + thickness };
            theCanvas.Children.Add(rsInner);
        }
        private async void UpdateEverything()
        {
            TxtCounter.Text = pomodoro.GetSmartDisplay();
            TxtStatus.Text = pomodoro.Status.ToString();
            TxtState.Text = pomodoro.State.ToString();
            
            await HandleDayTransition();
            UpdatePomodoros();
            UpdateRings();

            if(tsPausedTime != null)
                TxtPausedTime.Text = DTC.GetSmartPausedTime(tsPausedTime);

            TxtTrashTime.Text = DTC.GetSmartTrashTime(pomodoro.TSIdleTime);

            tsUpdatePomodoroFile = tsUpdatePomodoroFile.Add(new TimeSpan(0, 0, ONE_SECOND));
            if (tsUpdatePomodoroFile.TotalSeconds > intervalForUpdatePomodoroFile)
            {
                await XMLEngine.UpdatePomodoroFile(pomodoro);
                tsUpdatePomodoroFile = new TimeSpan();
            }
        }
        private void TimerOneSecondTick(object sender, object e)
        {
            pomodoro.AddOneSecond();
            tsPausedTime = tsPausedTime.Add(new TimeSpan(0, 0, ONE_SECOND));
            UpdateEverything();
        }
        private void TimerBlinkTick(object sender, object e)
        {
            double opacity = rsInner.Opacity + 0.02;
            if (opacity > 1)
                opacity = 0.5;

            rsInner.Opacity = opacity;
        }
        private async void TimerReminder_Tick(object sender, object e)
        {
            if (pomodoro != null && pomodoro.Status == Pomodoro.StatusEnum.Paused)
                await PlaySound();
        }
        private async Task<bool> HandleDayTransition()
        {
            if (pomodoro.IsDateDifferent())
                CmdInfo.Visibility = Visibility.Visible;
            else
                CmdInfo.Visibility = Visibility.Collapsed;

            if (!isStartedDayTransition && pomodoro.IsDateDifferent() && settings.IsItAfterDayEndNow())
            {
                isStartedDayTransition = true;
                await DisplayPromptDialogDayTransition();
            }

            return true;
        }

        private async void SwitchToCompactMode()
        {
            ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            compactOptions.ViewSizePreference = ViewSizePreference.Custom;
            compactOptions.CustomSize = formSizes[FormStateEnum.VeryCompact];
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
            toastTextElements[0].AppendChild(toastXml.CreateTextNode("Smooth Takeoff"));
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
        private async void DisplayVistaImage()
        {
            var localizationDirectory = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\vista");
            var filesVista = await localizationDirectory.GetFilesAsync();

            string url = "ms-appx:///Assets/vista/" + filesVista[numFileVista].Name;
            imgVista.Source = new BitmapImage(new Uri(url, UriKind.Absolute));
        }
        private async void SelectPrevNextVistaImage(int minusOrPlus)
        {
            var localizationDirectory = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\vista");
            var filesVista = await localizationDirectory.GetFilesAsync();

            if(minusOrPlus > 0)
            {
                numFileVista++;
                if (numFileVista >= filesVista.Count)
                    numFileVista = 0;
            }
            else
            {
                numFileVista--;
                if (numFileVista < 0)
                    numFileVista = filesVista.Count - 1;
            }

            DisplayVistaImage();
        }
        private void SetTsDateText()
        {
            menuItemFirst.Text = DTC.GetSmartDate(pomodoro.TheDay, false);

            if (!pomodoro.IsDateDifferent())
            {
                menuItemFirst.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                menuItemFirst.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
            }
            else
            {
                menuItemFirst.Background = new SolidColorBrush(Windows.UI.Colors.Tomato);
                menuItemFirst.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            }
        }
        private async Task<bool> PlaySound()
        {
            var element = new MediaElement();
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //var file = await storageFolder.GetFileAsync("chimes.wav");

            string strFileName = string.Empty;

            if (tsPausedTime.TotalMinutes >= 40)
                strFileName = "back-to-work-level-3.wav";
            else if (tsPausedTime.TotalMinutes >= 20)
                 strFileName = "back-to-work-level-2.wav";
            else
                strFileName = "back-to-work-level-1.wav";

            var file = await storageFolder.GetFileAsync(strFileName);
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            element.SetSource(stream, "");
            element.Volume = 0.1;
            element.Play();

            return true;
        }
        private void HidePrevNextButtons()
        {
            CmdImgPrev.Visibility = Visibility.Collapsed;
            CmdImgNext.Visibility = Visibility.Collapsed;
        }
        private void ShowPrevNextButtons()
        {
            CmdImgPrev.Visibility = Visibility.Visible;
            CmdImgNext.Visibility = Visibility.Visible;
        }
        private void ToggleFormSize()
        {
            if (formState == FormStateEnum.VeryCompact)
            {
                formState = FormStateEnum.CompactWithImage;
            }
            else
            {
                formState = FormStateEnum.VeryCompact;
            }

            UpdateFormDueToResize();

            HideElements();

            if (!ApplicationView.GetForCurrentView().TryResizeView(formSizes[formState]))
                Msgbox.Show("not possible", "Error");
        }
        private void UpdateFormDueToResize()
        {
            imgVista.Width = formSizes[formState].Width;
            imgVista.Height = formSizes[formState].Height - Canvas.GetTop(imgVista);

            if (formState == FormStateEnum.VeryCompact)
            {
                webView.Height = formSizes[formState].Height;
                webView.Width = webView.Height * 2.5;
                Canvas.SetTop(webView, 0);
                Canvas.SetLeft(webView, - webView.Width / 2 + 100);
            }
            else
            {
                webView.Width = formSizes[formState].Width;
                webView.Height = formSizes[formState].Height;
                Canvas.SetTop(webView, 0);
                Canvas.SetLeft(webView, 0);
            }
            

            rectBG.Width = formSizes[formState].Width;
            rectBG.Height = formSizes[formState].Height;

            Canvas.SetTop(TxtTrashTime, 44);
            Canvas.SetTop(TxtPausedTime, 55);

            if (formState == FormStateEnum.VeryCompact)
            {
                CmdPlay.Style = stylePlayPauseButtonMin;
                CmdPause.Style = stylePlayPauseButtonMin;
                Canvas.SetTop(CmdPlay, 33);
                Canvas.SetTop(CmdPause, 33);
                Canvas.SetLeft(CmdPlay, 231);
                Canvas.SetLeft(CmdPause, 231);

                TxtTrashTime.Visibility = Visibility.Collapsed;
                TxtPausedTime.Visibility = Visibility.Collapsed;

                TxtWeeklyPomodorosTitle.Visibility = Visibility.Collapsed;
                TxtWeeklyPomodorosValue.Visibility = Visibility.Collapsed;
                TxtLastSevenDaysTitle.Visibility = Visibility.Collapsed;
                TxtLastSevenDaysValue.Visibility = Visibility.Collapsed;
            }
            else if (formState == FormStateEnum.CompactWithImage)
            {
                CmdPlay.Style = stylePlayPauseButtonMax;
                CmdPause.Style = stylePlayPauseButtonMax;
                Canvas.SetTop(CmdPlay, 7);
                Canvas.SetTop(CmdPause, 7);
                Canvas.SetLeft(CmdPlay, formSizes[formState].Width - 110);
                Canvas.SetLeft(CmdPause, formSizes[formState].Width - 110);

                Canvas.SetLeft(TxtTrashTime, 450);
                Canvas.SetLeft(TxtPausedTime, 466);

                Canvas.SetTop(TxtWeeklyPomodorosTitle, 20);
                Canvas.SetTop(TxtWeeklyPomodorosValue, 10);
                Canvas.SetLeft(TxtWeeklyPomodorosTitle, 250);
                Canvas.SetLeft(TxtWeeklyPomodorosValue, 353);

                Canvas.SetTop(TxtLastSevenDaysTitle, 45);
                Canvas.SetTop(TxtLastSevenDaysValue, 35);
                Canvas.SetLeft(TxtLastSevenDaysTitle, 250);
                Canvas.SetLeft(TxtLastSevenDaysValue, 335);

                TxtTrashTime.Visibility = Visibility.Visible;
                if(pomodoro.Status == Pomodoro.StatusEnum.Paused)
                    TxtPausedTime.Visibility = Visibility.Visible;

                TxtWeeklyPomodorosTitle.Visibility = Visibility.Visible;
                TxtWeeklyPomodorosValue.Visibility = Visibility.Visible;
                TxtLastSevenDaysTitle.Visibility = Visibility.Visible;
                TxtLastSevenDaysValue.Visibility = Visibility.Visible;

                statistics = new List<StatsInfo>();
                Action<List<StatsInfo>> callback = o =>
                {
                    object result = o;
                    statistics = (List<StatsInfo>)result;
                    TxtWeeklyPomodorosValue.Text = statistics.FindAll(i => i.TheDate >= DTC.GetStartOfWeek(pomodoro.TheDay)).Sum(i => i.NumPomodorosRipe).ToString();
                    TxtLastSevenDaysValue.Text = statistics.FindAll(i => i.TheDate > pomodoro.TheDay.AddDays(-7)).Sum(i => i.NumPomodorosRipe).ToString();
                };
                XMLEngine.GetStatisticsFromXML(statistics, callback);
            }
        }
        private void UpdateVistaType()
        {
            if (settings.VistaType == SettingsInfo.VistaTypeEnum.HTML5)
            {
                imgVista.Visibility = Visibility.Collapsed;
                webView.Visibility = Visibility.Visible;
                Uri targetUri = new Uri("ms-appdata:///local/" + settings.GetCurrentAnimationFolder() + "/index.html");
                webView.Navigate(targetUri);
            }
            else if (settings.VistaType == SettingsInfo.VistaTypeEnum.Image)
            {
                imgVista.Visibility = Visibility.Visible;
                Uri targetUri = new Uri("ms-appdata:///local/" + settings.GetBlankAnimationFolder() + "/blank.html");
                webView.Navigate(targetUri);
                webView.Visibility = Visibility.Collapsed;
            }
        }

        private void ToggleShowHideElements()
        {
            if(elementsVisibility == ElementsVisibilityEnum.Hidden)
            {
                ShowElements();
            }
            else
            {
                HideElements();
            }
        }

        private void HideElements()
        {
            Canvas.SetTop(TxtCounter, -20);
            Canvas.SetLeft(TxtCounter, 10);
            TxtCounter.FontSize = 80;

            //TxtCounter.Visibility = Visibility.Collapsed;
            TxtLastSevenDaysTitle.Visibility = Visibility.Collapsed;
            TxtLastSevenDaysValue.Visibility = Visibility.Collapsed;
            TxtPausedTime.Visibility = Visibility.Collapsed;
            txtPlus.Visibility = Visibility.Collapsed;
            TxtWeeklyPomodorosTitle.Visibility = Visibility.Collapsed;
            TxtWeeklyPomodorosValue.Visibility = Visibility.Collapsed;
            TxtTrashTime.Visibility = Visibility.Collapsed;
            CmdPause.Visibility = Visibility.Collapsed;
            CmdInfo.Visibility = Visibility.Collapsed;
            CmdPlay.Visibility = Visibility.Collapsed;
            imgPomodoro1.Visibility = Visibility.Collapsed;
            imgPomodoro2.Visibility = Visibility.Collapsed;
            imgPomodoro3.Visibility = Visibility.Collapsed;

            rsInner.Visibility = Visibility.Collapsed;
            foreach(int i in ringSlicesRest.Keys)
                ringSlicesRest[i].Visibility = Visibility.Collapsed;
            foreach (int i in ringSlicesWork.Keys)
                ringSlicesWork[i].Visibility = Visibility.Collapsed;

            elementsVisibility = ElementsVisibilityEnum.Hidden;
        }
        private void ShowElements()
        {
            Canvas.SetTop(TxtCounter, 14);
            Canvas.SetLeft(TxtCounter, 78);
            TxtCounter.FontSize = 48;

            TxtLastSevenDaysTitle.Visibility = Visibility.Visible;
            TxtLastSevenDaysValue.Visibility = Visibility.Visible;
            TxtPausedTime.Visibility = Visibility.Visible;
            txtPlus.Visibility = Visibility.Visible;
            TxtWeeklyPomodorosTitle.Visibility = Visibility.Visible;
            TxtWeeklyPomodorosValue.Visibility = Visibility.Visible;
            TxtTrashTime.Visibility = Visibility.Visible;
            CmdPause.Visibility = Visibility.Visible;
            CmdInfo.Visibility = Visibility.Visible;
            CmdPlay.Visibility = Visibility.Visible;
            imgPomodoro1.Visibility = Visibility.Visible;
            imgPomodoro2.Visibility = Visibility.Visible;
            imgPomodoro3.Visibility = Visibility.Collapsed;

            rsInner.Visibility = Visibility.Visible;
            foreach (int i in ringSlicesRest.Keys)
                ringSlicesRest[i].Visibility = Visibility.Visible;
            foreach (int i in ringSlicesWork.Keys)
                ringSlicesWork[i].Visibility = Visibility.Visible;

            elementsVisibility = ElementsVisibilityEnum.Visible;
        }
        #endregion

        #region Form Events
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            bool result = ApplicationView.GetForCurrentView().TryResizeView(formSizes[FormStateEnum.VeryCompact]);
        }
        private void Page_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ToggleFormSize();
        }
        private void webView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ToggleFormSize();
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
        private void CmdInfo_Click(object sender, RoutedEventArgs e)
        {
            MenuItemFirstItem_Click(null, null);
        }
        private async void CmdSound_Click(object sender, RoutedEventArgs e)
        {
            await PlaySound();
        }
        private void CmdImgNext_Click(object sender, RoutedEventArgs e)
        {
            SelectPrevNextVistaImage(1);
        }
        private void CmdImgPrev_Click(object sender, RoutedEventArgs e)
        {
            SelectPrevNextVistaImage(-1);
        }
        private void Page_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if(settings.VistaType == SettingsInfo.VistaTypeEnum.Image)
                ShowPrevNextButtons();
        }
        private void Page_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (settings.VistaType == SettingsInfo.VistaTypeEnum.Image)
                HidePrevNextButtons();
        }
        #endregion

        #region Menu
        private void MenuItemFirstItem_Click(object sender, RoutedEventArgs e)
        {
            if (pomodoro.IsDateDifferent())
                Msgbox.Show("The program's date is different than today.\nIf this is what you intended, it's OK. If it's not, you may use the menu item Initialize.", "Different Date");
        }
        private void MenuItemStatistics_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StatisticsPage), pomodoro);
            bool result = ApplicationView.GetForCurrentView().TryResizeView(formSizes[FormStateEnum.Medium]);


            /*
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(Statistics), pomodoro);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
            */
        }
        private void MenuItemSkipSession_Click(object sender, RoutedEventArgs e)
        {
            if (!pomodoro.SkipSession())
                Msgbox.Show("Cannot skip working sessions for now.", "Action not supported");
        }
        private async void MenuItemInitialize_Click(object sender, RoutedEventArgs e)
        {
            await DisplayPromptDialogInitialize();
        }
        private void MenuItemClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
            //CoreApplication.Exit();
        }
        private async void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {

            await ShowDialog();
        }
        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            /*
            int compactViewId = 0;
            await CoreApplication.CreateNewView().Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                compactViewId = ApplicationView.GetForCurrentView().Id;
                frame.Navigate(typeof(SettingsPage));
                Window.Current.Content = frame;
                Window.Current.Activate();
                ApplicationView.GetForCurrentView().Title = "CompactOverlay Window";
            });

            ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            compactOptions.CustomSize = new Windows.Foundation.Size(500, 300);

            bool viewShown = await ApplicationViewSwitcher.TryShowAsViewModeAsync(compactViewId, ApplicationViewMode.CompactOverlay, compactOptions);
            */
        }
        private void MenuItemToggleSize_Click(object sender, RoutedEventArgs e)
        {
            ToggleFormSize();
        }
        private void MenuItemToggleVistaType_Click(object sender, RoutedEventArgs e)
        {
            if (settings.VistaType == SettingsInfo.VistaTypeEnum.Image)
            {
                settings.VistaType = SettingsInfo.VistaTypeEnum.HTML5;
            }
            else if (settings.VistaType == SettingsInfo.VistaTypeEnum.HTML5)
            {
                settings.VistaType = SettingsInfo.VistaTypeEnum.Image;
            }

            UpdateVistaType();
        }
        private void MenuItemPrevImage_Click(object sender, RoutedEventArgs e)
        {
            if(settings.VistaType == SettingsInfo.VistaTypeEnum.Image)
                SelectPrevNextVistaImage(-1);
        }
        private void MenuItemNextImage_Click(object sender, RoutedEventArgs e)
        {
            if (settings.VistaType == SettingsInfo.VistaTypeEnum.Image)
                SelectPrevNextVistaImage(+1);
        }
        private void MenuItemToggleVisibility_Click(object sender, RoutedEventArgs e)
        {
            ToggleShowHideElements();
        }
        
        private void MenuFlyout_Opening(object sender, object e)
        {
            SetTsDateText();
        }

        private void MenuItemAnimation01_Click(object sender, RoutedEventArgs e)
        {
            settings.NumAnimation = 1;
            UpdateVistaType();
        }
        private void MenuItemAnimation02_Click(object sender, RoutedEventArgs e)
        {
            settings.NumAnimation = 2;
            UpdateVistaType();
        }
        private void MenuItemAnimation03_Click(object sender, RoutedEventArgs e)
        {
            settings.NumAnimation = 3;
            UpdateVistaType();
        }
        #endregion

        private async Task<bool> ShowDialog()
        {
            ContentDialogExample dialog = new ContentDialogExample();
            var result = await dialog.ShowAsync(ContentDialogPlacement.InPlace);

            if (result == ContentDialogResult.Primary)
            {
               
            }
            else if (result == ContentDialogResult.Secondary)
            {
                 
            }
            else
            {
              
            }

            return true;
        }
        public static class Msgbox
        {
            static public async void Show(string txtBody, string txtTitle)
            {
                var dialog = new MessageDialog(txtBody, txtTitle);
                await dialog.ShowAsync();
            }
        }

        private async Task<bool> DisplayPromptDialogInitialize()
        {
            ContentDialog promptDialog = new ContentDialog
            {
                Title = "Initialize?",
                Content = "Are you sure to initialize the session?",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "OK"
            };

            ContentDialogResult result = await promptDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                InitPomodoro();
                await XMLEngine.UpdatePomodoroFile(pomodoro);

                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task<bool> DisplayPromptDialogDayTransition()
        {
            ContentDialog promptDialog = new ContentDialog
            {
                Title = "A new day started",
                Content = "As the new day started, the session will be initialized.\n\nYou can always change the new day starting hour from the settings.",
                PrimaryButtonText = "OK",
            };

            ContentDialogResult result = await promptDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                InitPomodoro();
                await XMLEngine.UpdatePomodoroFile(pomodoro);
                isStartedDayTransition = false;
                return true;
            }
            else
            {

                return false;
            }
        }

        private void TermsOfUseContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            // Ensure that the check box is unchecked each time the dialog opens.
            ConfirmAgeCheckBox.IsChecked = false;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

        }
  
        private void webView_ScriptNotify(object sender, NotifyEventArgs e)
        {

        }

        private void ConfirmAgeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            termsOfUseContentDialog.IsPrimaryButtonEnabled = true;
        }
        private void ConfirmAgeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            termsOfUseContentDialog.IsPrimaryButtonEnabled = false;
        }
    }
}
