using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor
{
    class FramesCounter
    {
        private double PreviousFPS = 0d;
        private double AccumulatedFrames = 0;

        private double AccumulatedTime = 0;
        private Stopwatch FramesTimer = new Stopwatch();

        public double FPS { get => ((1d - AccumulatedTime) * PreviousFPS + AccumulatedTime * AccumulatedFrames / AccumulatedTime); }
        public void DoFrame()
        {
            AccumulatedFrames++;

            AccumulatedTime += FramesTimer.ElapsedMilliseconds / 1000d;
            FramesTimer.Restart();

            if (AccumulatedTime >= 1)
            {
                PreviousFPS = AccumulatedFrames / AccumulatedTime;
                AccumulatedTime = 0d;
                AccumulatedFrames = 0d;
            }
        }
    }
}
