using System;
namespace Verdure.Braincase.Services.ClockView
{
    public interface IClockViewProvider
    {
        public string Name
        {
            get;
        }
        View CreateClockView(string viewName);
    }

}

