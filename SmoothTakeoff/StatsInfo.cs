using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothLanding
{
    public class StatsInfo
    {
        DateTime theDate;
        int numPomodorosRipe, numPomodorosUnripe;

        public StatsInfo(DateTime theDate, int numPomodorosRipe, int numPomodorosUnripe)
        {
            this.theDate = theDate;
            this.numPomodorosRipe = numPomodorosRipe;
            this.numPomodorosUnripe = numPomodorosUnripe;
        }

        public void SetStatistics(int numPomodorosRipe, int numPomodorosUnripe)
        {
            this.numPomodorosRipe = numPomodorosRipe;
            this.numPomodorosUnripe = numPomodorosUnripe;
        }

        #region Public Properties
        public DateTime TheDate { get { return theDate; } }
        public int NumPomodorosRipe { get { return numPomodorosRipe; } }
        public int NumPomodorosUnripe { get { return numPomodorosUnripe; } }
        #endregion
    }
}
