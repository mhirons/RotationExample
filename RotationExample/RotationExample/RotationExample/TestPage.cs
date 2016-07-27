using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace RotationExample
{
    public class TestPage : ContentPage
    {
        public TestPage()
        {
            var al = new AbsoluteLayout();

            // Add a 50px grid to make it easier to see movement
            BoxView[] hgrid = new BoxView[7];
            BoxView[] vgrid = new BoxView[7];
            for (int i = 0; i < 7; i++)
            {
                hgrid[i] = new BoxView() { Color = Color.Silver };
                vgrid[i] = new BoxView() { Color = Color.Silver };
                al.Children.Add(hgrid[i]);
                al.Children.Add(vgrid[i]);
                AbsoluteLayout.SetLayoutBounds(hgrid[i], new Rectangle(0, (i + 1) * 50, 350, 1));
                AbsoluteLayout.SetLayoutBounds(vgrid[i], new Rectangle((i + 1) * 50, 0, 1, 350));
            }

            // Add the line to rotate
            var lineStart = new Point(200, 50);
            var lineEnd = new Point(300, 100);
            BoxView line = new BoxView() { Color = Color.Lime };
            line.AnchorX = 0; // Rotate around the start (left end) of the line
            line.AnchorY = .5; // Rotate around the middle of the line thickness
            line.Rotation = CalcRotation(lineStart, lineEnd);
            al.Children.Add(line);
            AbsoluteLayout.SetLayoutBounds(line, new Rectangle(lineStart.X, lineStart.Y, lineStart.Distance(lineEnd), 5));

            // Button that moves the start of the line down incrementally
            Button btn = new Button() { Text = "Move" };
            al.Children.Add(btn);
            AbsoluteLayout.SetLayoutBounds(btn, new Rectangle(0, 350, 350, 50));

            // Label that shows what the position of the line should be.
            Label lbl = new Label();
            al.Children.Add(lbl);
            AbsoluteLayout.SetLayoutBounds(lbl, new Rectangle(0, 400, 350, 100));
            lbl.Text = "Start X: " + lineStart.X + " Y: " + lineStart.Y + "\n" + "End X: " + lineEnd.X + " Y: " + lineEnd.Y;

            Content = al;

            btn.Clicked += (s, e) =>
            {
                if (lineStart.Y < 350)
                    lineStart.Y += 50; // Incrementally move the start of the line down.
                else
                    lineStart.Y = 50; // Move the start of the line back to the top.

                lbl.Text = "Start X: " + lineStart.X + " Y: " + lineStart.Y + "\n" + "End X: " + lineEnd.X +  " Y: " + lineEnd.Y;

                // Calculate the angle of the line
                // This will keep the end of the line in the same place.
                line.Rotation = CalcRotation(lineStart, lineEnd);

                // Specify the starting point of the line, as well as the length of the line.
                // The rotation above results in the line stopping at the desired end point.
                AbsoluteLayout.SetLayoutBounds(line, new Rectangle(lineStart.X, lineStart.Y, lineStart.Distance(lineEnd), 5));
            };
        }

        /// <summary>
        /// Calculate the angle between the x axis and a line
        /// given by two points.
        /// </summary>
        /// <param name="start">The starting point of the line.</param>
        /// <param name="end">The ending point of the line.</param>
        /// <returns>The angle in degrees.</returns>
        private double CalcRotation(Point start, Point end)
        {
            // Calculate the angle (in radians) of the line
            // https://en.wikipedia.org/wiki/Atan2
            double radians = Math.Atan2(end.Y - start.Y, end.X - start.X); // in radians
            double degrees = radians * (180d / Math.PI); // radians to degrees
            return degrees;
        }
    }
}
