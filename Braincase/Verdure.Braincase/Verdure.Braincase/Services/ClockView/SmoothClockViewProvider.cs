using System;
using AnalogClock.Controls;

namespace Verdure.Braincase.Services.ClockView
{
    public class SmoothClockViewProvider : IClockViewProvider
    {
        private readonly string _name = "SmoothView";
        public string Name => _name;

        public View CreateClockView(string viewName)
        {
            return new Clock1();
        }
    }
}

