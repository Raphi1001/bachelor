using System;


namespace tourPlanner.UIL.ViewModels
{
    public interface ICloseWindow
    {
        public Action? Close { get; set; }
    }
}
