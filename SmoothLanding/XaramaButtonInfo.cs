using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SmoothLanding
{
    public class XaramaButtonInfo
    {
        public event EventHandler<ClickedArgs> OnClicked;
        public class ClickedArgs : EventArgs { public ClickedArgs() { }}

        #region Enums
        public enum ContextEnum
        {
            NA, Book, BookProcess, ProjectInfo, ProjectPlanner, Next, Present, Previous,
            Settings, Edit, WeeklyFocus, NewsItem, RefreshGoals, PragmaInstance, Segment, Block,
            Year, Quarter, Month, Week, Day, Idea, IdeaGroup, Task, Goal, IdeaDesktopSelect, IdeaDesktopEdit,
            ViewDay, ViewWeek, ViewMonth, ViewQuarter, ViewYear,
            TaskPause, TaskComplete, TaskLock
        }
        #endregion

        #region Private Members
        int id;
        Bitmap imgNormal, imgHovered, imgDisabled;
        bool isEnabled;
        bool isHovered;
        bool isHidden;
        Rectangle rect;
        string toolTip;
        ContextEnum context;
        int contextID;
        bool isMouseDown = false;
        #endregion

        #region Constructors
        public XaramaButtonInfo()
        {
            imgNormal = null;
            imgHovered = null;
            imgDisabled = null;
            id = 0;
            isEnabled = true;
            isHovered = false;
            isHidden = false;
            rect = new Rectangle(0, 0, 0, 0);
            toolTip = "";
            context = ContextEnum.NA;
            contextID = 0;
        }
        public XaramaButtonInfo(Bitmap imgNormal, Bitmap imgHovered, Bitmap imgDisabled, Rectangle rect, string toolTip)
        {
            this.imgNormal = imgNormal;
            this.imgHovered = imgHovered;
            this.imgDisabled = imgDisabled;
            this.rect = rect;
            this.toolTip = toolTip;
            isEnabled = true;
            isHovered = false;
        }
        public XaramaButtonInfo(Bitmap imgNormal, Bitmap imgHovered, Bitmap imgDisabled, Rectangle rect, string toolTip, int id)
        {
            this.id = id;
            this.imgNormal = imgNormal;
            this.imgHovered = imgHovered;
            this.imgDisabled = imgDisabled;
            this.rect = rect;
            this.toolTip = toolTip;
            isEnabled = true;
            isHovered = false;
        }
        #endregion

        #region Private methods
        private void Hover() { isHovered = true; }
        private void UnHover() { isHovered = false; }
        private bool IsWithin(Point p) { return rect.Contains(p); } 
        #endregion

        #region Public Methods
        public void Draw(Graphics dc) { dc.DrawImage(GetCurrent(), rect.Location); }
        public void Enable() { isEnabled = true; }
        public void Disable() { isEnabled = false; }

        public void Hide() { isHidden = true; }
        public void Show() { isHidden = false; }
        
        public Bitmap GetCurrent()
        {
            Bitmap bmp = imgHovered;
            if (isEnabled)
            {
                if (isHovered)
                    bmp = imgHovered;
                else
                    bmp = imgNormal;
            }
            else
                bmp = imgDisabled;

            return bmp;
        }
        public Rectangle GetZeroRect()
        {
            return new Rectangle(new Point(0, 0), rect.Size);
        }
        public bool MouseMove(Point p)
        {
            bool isUpdateNeeded = false;

            if (IsWithin(p))
            {
                if(!isHovered)
                {
                    isUpdateNeeded = true;
                    Hover();
                }
            }
            else
            {
                if (isHovered)
                {
                    isUpdateNeeded = true;
                    UnHover();
                }
            }
                

            return isUpdateNeeded;
        }
        public void MouseDown(Point p)
        {
            if (IsWithin(p))
                isMouseDown = true;
            else
                isMouseDown = false;
        }
        public void MouseUp(Point p)
        {
            if (IsWithin(p) && isMouseDown)
            {
                OnClicked(this, new ClickedArgs()); 
            }

            isMouseDown = false;
        }
        #endregion

        #region Public Properties
        public Bitmap ImgHovered { get { return imgHovered; } set { imgHovered = value; }}
        public Bitmap ImgNormal { get { return imgNormal; } set { imgNormal = value; }}
        public Bitmap ImgDisabled { get { return imgDisabled; } set { imgDisabled = value; }}
        public Rectangle Rect { get { return rect; } set { rect = value; }}
        public int ID { get { return id; } set { id = value; }}
        public bool IsEnabled { get { return isEnabled; }}
        public bool IsHovered { get { return isHovered; }}
        public bool IsHidden { get { return isHidden; }}
        public string ToolTip { get { return toolTip; } set { toolTip = value; }}
        public ContextEnum Context { get { return context; } set { context = value; }}
        public int ContextID { get { return contextID; } set { contextID = value; }}
        #endregion
    }
}
