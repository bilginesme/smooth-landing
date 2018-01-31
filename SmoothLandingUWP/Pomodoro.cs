using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothLandingUWP
{
    public class Pomodoro
    {
        #region Definitions
        public event EventHandler<WorkJustCompletedArgs> OnWorkJustCompleted;
        public class WorkJustCompletedArgs : EventArgs
        {
            public int SliceCompleted { get; set; }
            public WorkJustCompletedArgs(int sliceCompleted) { SliceCompleted = sliceCompleted; }
        }
        public event EventHandler<RestJustCompletedArgs> OnRestJustCompleted;
        public class RestJustCompletedArgs : EventArgs { public RestJustCompletedArgs() { } }

        public event EventHandler<PausedArgs> OnPaused;
        public class PausedArgs : EventArgs { public PausedArgs() { } }

        public event EventHandler<PomodoroCompletedArgs> OnPomodoroCompleted;
        public class PomodoroCompletedArgs : EventArgs { public PomodoroCompletedArgs() { } }

        public event EventHandler<ForceRePaintArgs> OnForceRePaint;
        public class ForceRePaintArgs : EventArgs { public ForceRePaintArgs() { } }

        public event EventHandler<StartedArgs> OnStarted;
        public class StartedArgs : EventArgs { public StartedArgs() { } }

        public enum StatusEnum { NA, Running, Paused }
        public enum StateEnum { Initial, Working, WorkCompleted, RestingShort, RestingShortCompleted, RestingLong, RestingLongCompleted }
        public enum WorkOrRestEnum { Work, Rest } 
        #endregion

        #region Private Members
        public static int NumSlicesForPomodoro = 4;
        private static float secondsWorking = 1500;
        private static float secondsRestingShort = 300;
        private static float secondsRestingLong = 900;
        TimeSpan tsDelta;
        int sliceNow;
        int numPomodorosToday;
        TimeSpan tsNow;
        StatusEnum status;
        StateEnum state;
        private DateTime theDay;
        #endregion

        #region Constructors
        public Pomodoro()
        {
            tsDelta = new TimeSpan(0, 0, 1);
            Init(true);
        }
        #endregion

        #region Private Methods
        private void ResetTime() { tsNow = new TimeSpan(); }
        private void Init(bool isResetDay)
        {
            if(isResetDay)
            {
                theDay = DateTime.Today;
                //theDate = new DateTime(2018, 1, 12);      // used for day transition, delete after 15th Feb 2018
                numPomodorosToday = 0;
            }
            
            sliceNow = 0;
            ResetTime();
            status = StatusEnum.NA;
            state = StateEnum.Initial;
        }
        private void HandleState()
        {
            float secondsPassed = (float)tsNow.TotalSeconds;

            if (state == StateEnum.Initial)
            {
                if (status == StatusEnum.Running)
                {
                    ResetTime();
                    sliceNow = 1;
                    state = StateEnum.Working;  // This is awkward, we should wire it to the Play() method
                }
            }
            else if (state == StateEnum.Working)
            {
                if(secondsPassed >= secondsWorking)
                {
                    state = StateEnum.WorkCompleted;
                    Pause();
                    if (this.OnWorkJustCompleted != null)
                        OnWorkJustCompleted(this, new WorkJustCompletedArgs(sliceNow));

                    if(sliceNow >= NumSlicesForPomodoro)
                    {
                        numPomodorosToday++;
                        if (this.OnForceRePaint != null)
                            OnForceRePaint(this, new ForceRePaintArgs());
                        if (this.OnPomodoroCompleted != null)
                            OnPomodoroCompleted(this, new PomodoroCompletedArgs());
                    }
                }
            }
            else if (state == StateEnum.WorkCompleted)
            {
                if(status == StatusEnum.Running)
                {
                    ResetTime();
                    if (sliceNow < NumSlicesForPomodoro)
                        state = StateEnum.RestingShort;
                    else
                        state = StateEnum.RestingLong;
                }
            }
            else if (state == StateEnum.RestingShort)
            {
                if (secondsPassed >= secondsRestingShort)
                {
                    Pause();
                    state = StateEnum.RestingShortCompleted;
                    if (this.OnRestJustCompleted != null)
                        OnRestJustCompleted(this, new RestJustCompletedArgs());
                }
            }
            else if (state == StateEnum.RestingShortCompleted)
            {
                if (status == StatusEnum.Running)
                {
                    ResetTime();
                    state = StateEnum.Working;
                    sliceNow++;
                }
            }
            else if (state == StateEnum.RestingLong)
            {
                if (secondsPassed >= secondsRestingLong)
                {
                    Pause();
                    state = StateEnum.RestingLongCompleted;
                    if (this.OnRestJustCompleted != null)
                        OnRestJustCompleted(this, new RestJustCompletedArgs());
                }
            }
            else if (state == StateEnum.RestingLongCompleted)
            {
                if (status == StatusEnum.Running)
                {
                    Init(false);
                    Pause();
                }
            }
        } 
        #endregion

        #region Public Methods
        public void Start()
        {
            status = StatusEnum.Running;
            if (this.OnStarted != null)
                OnStarted(this, new StartedArgs());
        }
        public void Pause()
        {
            status = StatusEnum.Paused;
            if (this.OnPaused != null)
                OnPaused(this, new PausedArgs());
            
        }
        public void AddOneSecond()
        {
            if (status == StatusEnum.Running)
                tsNow = tsNow.Add(tsDelta);

            HandleState();
        }
        public string GetSmartDisplay()
        {
            string strResult = string.Empty;


            long numTicks = 0;
            long factor = 10000000L;
            if (state == StateEnum.Working || state == StateEnum.WorkCompleted)
            {
                numTicks = (long)(factor * secondsWorking);
            }
            else if (state == StateEnum.RestingShort || state == StateEnum.RestingShortCompleted)
            {
                numTicks = (long)(factor * secondsRestingShort);
            }
            else if (state == StateEnum.RestingLong || state == StateEnum.RestingLongCompleted)
            {
                numTicks = (long)(factor * secondsRestingLong);
            }
            TimeSpan tsReverse = new TimeSpan(numTicks).Subtract(tsNow);

            string strMinutes = Convert.ToString((int)tsReverse.Minutes);
            string strSeconds = Convert.ToString((int)tsReverse.Seconds);

            if (strMinutes.Length == 1)
                strMinutes = "0" + strMinutes;
            if (strSeconds.Length == 1)
                strSeconds = "0" + strSeconds;

            strResult = strMinutes + ":" + strSeconds;

            return strResult;
        } 
        public int GetPercentage()
        {
            int percentage = 0;

            float secondsPassed = (float)tsNow.TotalSeconds;
            if (state == StateEnum.Working)
                percentage = (int)(100f * (float)secondsPassed / (float)secondsWorking);
            else if (state == StateEnum.RestingShort)
                percentage = (int)(100f * (float)secondsPassed / (float)secondsRestingShort);
            else if (state == StateEnum.RestingLong)
                percentage = (int)(100f * (float)secondsPassed / (float)secondsRestingLong);
            else if (state == StateEnum.Initial)
                percentage = 0;
            else
                percentage = 100;

            return percentage;
        }
        public void BackToLife(DateTime theDayFromFile, int sliceNow, int numPomodorosToday, StateEnum state, int minutes, int seconds)
        {
            if(theDayFromFile == theDay)
            {
                this.numPomodorosToday = numPomodorosToday;
                this.sliceNow = sliceNow;
                this.state = state;
                tsNow = new TimeSpan(0, minutes, seconds);
            }
            else
            {
                Init(true);
            }
            
            Pause();
        }
        public bool SkipSession()
        {
            bool result = false;

            if (state == StateEnum.RestingLong)
            {
                Init(false);
                result = true;
            }
            else if (state == StateEnum.RestingShort)
            {
                ResetTime();
                state = StateEnum.Working;
                sliceNow++;
                result = true;
            }
            else if (state == StateEnum.WorkCompleted)
            {
                state = StateEnum.Working;
                ResetTime();
                result = true;

                if (sliceNow < NumSlicesForPomodoro)
                    sliceNow++;
                else
                    sliceNow = 1;
            }

            if(result == true)
                Pause();

            return result;
        }
        public bool IsDateDifferent() { return theDay != DateTime.Today; }
        #endregion

        #region Public Properties
        public DateTime TheDay { get { return theDay; } }
        public StateEnum State { get { return state; } }
        public StatusEnum Status { get { return status; } }
        public int SliceNow { get { return sliceNow; } }
        public int NumPomodorosToday { get { return numPomodorosToday; } }
        public float Seconds { get { return (float)tsNow.Seconds; } }
        public float Minutes { get { return (float)tsNow.Minutes; } }
        #endregion
    }
}
