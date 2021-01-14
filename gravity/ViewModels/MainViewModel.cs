using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.Fixtures;
using DynamicData;
using gravity.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using Box2D.NetStandard.Collision.Shapes;
using System.Windows.Media;

namespace gravity.ViewModels
{
    class MainViewModel : ReactiveObject
    {
        public ObservableCollection<ButtonModel> Buttons { get; } = new();

        readonly HashSet<ButtonModel> buttonsWithGravity = new();

        readonly World world;
        readonly Body groundBody;

        public MainViewModel(double width, double height)
        {
            world = new(gravity: new(0, 60f));
            world.SetAllowSleeping(false);

            var rng = new Random();
            Buttons.AddRange(Enumerable.Range(0, rng.Next(20, 30)).Select(idx =>
                {
                    var (btnWidth, btnHeight) = (rng.Next(30, 70), rng.Next(15, 35));
                    var button = new ButtonModel
                    {
                        X = rng.NextDouble() * (width - btnWidth),
                        Y = rng.NextDouble() * (height - btnHeight) * .9,
                        Width = btnWidth,
                        Height = btnHeight,
                        Text = $"Button {idx + 1}",
                    };

                    button.Body = world.CreateBody(new() { type = BodyType.Static, position = new((float)(button.X + button.Width / 2), (float)(button.Y + button.Height / 2)) });
                    button.Body.CreateFixture(new() { shape = new PolygonShape((float)button.Width / 2, (float)button.Height / 2), density = 1f, friction = .5f, restitution = .2f });

                    button.ActivateGravityCommand = ReactiveCommand.Create(() =>
                    {
                        if (buttonsWithGravity.Add(button))
                            button.Body.SetType(BodyType.Dynamic);
                    });

                    return button;
                }));

            groundBody = world.CreateBody(new() { type = BodyType.Static, position = new(0, (float)height - 40) });
            groundBody.CreateFixture(new() { shape = new PolygonShape((float)width * 2, 10), density = 1f });

            bool firstStep = true;
            DateTime lastStep = default;
            CompositionTarget.Rendering += (s, e) =>
            {
                var now = DateTime.Now;
                var deltaSec = firstStep ? 0 : (now - lastStep).TotalSeconds;
                (firstStep, lastStep) = (false, now);

                world.Step((float)deltaSec, 10, 1);

                foreach (var btn in buttonsWithGravity)
                {
                    var pos = btn.Body.GetPosition();
                    var angle = btn.Body.GetAngle();

                    (btn.X, btn.Y, btn.Angle) = (pos.X - btn.Width / 2, pos.Y - btn.Height / 2, angle / Math.PI * 180);
                }
            };
        }
    }
}
