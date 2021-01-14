using Box2D.NetStandard.Dynamics.Bodies;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace gravity.Models
{
    class ButtonModel : ReactiveObject
    {
        double x, y, width, height, angle;
        public double X { get => x; set => this.RaiseAndSetIfChanged(ref x, value); }
        public double Y { get => y; set => this.RaiseAndSetIfChanged(ref y, value); }
        public double Width { get => width; set => this.RaiseAndSetIfChanged(ref width, value); }
        public double Height { get => height; set => this.RaiseAndSetIfChanged(ref height, value); }
        public double Angle { get => angle; set => this.RaiseAndSetIfChanged(ref angle, value); }

        public string Text { get; set; }
        public ICommand ActivateGravityCommand { get; set; }

        public Body Body;
    }
}
