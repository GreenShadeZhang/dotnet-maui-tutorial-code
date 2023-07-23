using System;
using AnalogClock.Controls;

namespace Verdure.Braincase.Services.ClockView
{
    public class DefaultClockViewProvider : IClockViewProvider
    {
        private readonly string _name = "DefaultView";
        public string Name => _name;

        public View CreateClockView(string viewName)
        {
            return new Clock2();
        }
    }
}

