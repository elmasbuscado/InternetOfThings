﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;
using Drivers;
using DvdPlotter.Extension;

namespace DvdPlotter
{
    public class Painter
    {
        private Plotter plotter;
        private ILogger logger;

        public Painter(Plotter plotter, ILogger logger)
        {
            this.plotter = plotter;
            this.logger = logger;
        }

        public async Task Squares()
        {
            await this.plotter.PenUp();
            
            for (var x = 0; x < 140; x+= 10)
            {
                plotter.GoToXY(x, x);
                await this.plotter.PenDown();
                plotter.GoToXY(310 - x, x);
                plotter.GoToXY(310 - x, 310 - x);
                plotter.GoToXY(x, 310 - x);
                plotter.GoToXY(x, x);
                await this.plotter.PenUp();
            }
            this.plotter.Stop();
        }

        public async Task Diagonal()
        {
            await this.plotter.PenUp();

            plotter.GoToXY(50, 50);
            await this.plotter.PenDown();
            plotter.GoToXY(250, 50);
            plotter.GoToXY(250, 250);
            plotter.GoToXY(50, 250);
            plotter.GoToXY(50, 50);
            await this.plotter.PenUp();

            plotter.GoToDiagonal(250, 250);

            await this.plotter.PenUp();
            this.plotter.Stop();
        }

        public async Task Sun()
        {
            await this.plotter.PenUp();
            for (var phi = 0.0; phi < 2*Math.PI; phi += Math.PI/6)
            {
                plotter.GoToXY(150, 150);
                await this.plotter.PenDown();
                plotter.GoToDiagonal((int)(150 + 150.0 * Math.Cos(phi)), (int)(150 + 150.0 * Math.Sin(phi)));
                await this.plotter.PenUp();
            }
            this.plotter.Stop();
        }

        public async Task Hilbert()
        {
            const int order = 5;

            await this.plotter.PenUp();
            this.plotter.GoToXY(0, 0);
            await this.plotter.PenDown();


            HilbertUp(order, 320 >> order);
            await this.plotter.PenUp();
            this.plotter.Stop();
        }

        private void HilbertUp(int order, int lineLength)
        {
            if (order == 0)
            {
                return;
            }
            HilbertRight(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y + lineLength);
            HilbertUp(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X + lineLength, plotter.Y);
            HilbertUp(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y - lineLength);
            HilbertLeft(order - 1, lineLength);
        }

        private void HilbertLeft(int order, int lineLength)
        {
            if (order == 0)
            {
                return;
            }
            HilbertDown(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X - lineLength, plotter.Y);
            HilbertLeft(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y - lineLength);
            HilbertLeft(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X + lineLength, plotter.Y);
            HilbertUp(order - 1, lineLength);
        }

        private void HilbertDown(int order, int lineLength)
        {
            if (order == 0)
            {
                return;
            }
            HilbertLeft(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y - lineLength);
            HilbertDown(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X - lineLength, plotter.Y);
            HilbertDown(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y + lineLength);
            HilbertRight(order - 1, lineLength);
        }

        private void HilbertRight(int order, int lineLength)
        {
            if (order == 0)
            {
                return;
            }
            HilbertUp(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X + lineLength, plotter.Y);
            HilbertRight(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X, plotter.Y + lineLength);
            HilbertRight(order - 1, lineLength);
            this.plotter.GoToXY(plotter.X - lineLength, plotter.Y);
            HilbertDown(order - 1, lineLength);
        }

        public async Task Test()
        {
            plotter.GoToXY(0, 0);
            await this.plotter.PenDown();
            plotter.GoToXY(310, 0);
            plotter.GoToXY(310, 310);
            plotter.GoToXY(0, 310);
            plotter.GoToXY(0, 0);
            await this.plotter.PenUp();
            plotter.GoToXY(0, 100);
            await this.plotter.PenDown();
            plotter.GoToXY(300, 100);
            await this.plotter.PenUp();

            for (int i = 0; i < 3; i++)
            {
                plotter.GoToXY(i*100, 100);
                await this.plotter.PenDown();
                plotter.GoToDiagonal((i+1) * 100, 200);
                await this.plotter.PenUp();
            }
            this.plotter.Stop();
        }

        public async Task Star()
        {
            const int corners = 9;
            const int step = 4;
            const int r = 145;

            logger.WriteLn("Drawing a star with {0} corners.".Fmt(corners), LogType.Info);

            var angle = 0.0;
            await plotter.PenUp();
            plotter.GoToXY(150 + r, 150);
            await plotter.PenDown();
            for (var i = 0; i < corners; i++)
            {
                angle += step*2*Math.PI/corners;
                plotter.GoToDiagonal(150 + (int)(r * Math.Cos(angle)), 150 + (int)(r * Math.Sin(angle)));
            }
            await plotter.PenUp();
        }

        public async Task DemoXY()
        {
            await plotter.PenUp();
            plotter.GoToXY(0, 150);
            await Task.Delay(3000);
            plotter.GoToXY(300, 150);
            await Task.Delay(500);
            plotter.GoToXY(150, 150);
            await Task.Delay(500);
            plotter.GoToXY(150, 0);
            await Task.Delay(500);
            plotter.GoToXY(150, 300);
            await Task.Delay(500);
            plotter.GoToXY(150, 150);
        }

        public async Task PCB()
        {
            await plotter.PenUp();
            plotter.GoToXY(0, 0);
            await plotter.PenDown();

            var w = 11;

            // outer frame
            for (var i = 0; i < w/2; i++)
            {
                plotter.GoToXY(i, i);
                plotter.GoToXY(300-i, i);
                plotter.GoToXY(300 - i, 300 - i);
                plotter.GoToXY(i, 300 - i);
                plotter.GoToXY(i, i);
            }
            await plotter.PenUp();

            // I
            var ix = 25;
            var iy = 70;
            plotter.GoToXY(ix, iy);
            await plotter.PenDown();
            
            for (var i = 0; i < w; i++)
            {
                plotter.GoToXY(ix + i, iy + i);
                plotter.GoToXY(ix + i, iy + 20 - i);
                plotter.GoToXY(ix + 25 + i, iy + 20 - i);
                plotter.GoToXY(ix + 25 + i, iy + 140 + i);
                plotter.GoToXY(ix + i, iy + 140 + i);
                plotter.GoToXY(ix + i, iy + 160 - i);
                plotter.GoToXY(ix + 70 - i, iy + 160 - i);
                plotter.GoToXY(ix + 70 - i, iy + 140 + i);
                plotter.GoToXY(ix + 45 - i, iy + 140 + i);
                plotter.GoToXY(ix + 45 - i, iy + 20 - i);
                plotter.GoToXY(ix + 70 - i, iy + 20 - i);
                plotter.GoToXY(ix + 70 - i, iy + i);
                plotter.GoToXY(ix + i, iy + i);
            }
            await plotter.PenUp();

            // O
            var ox = 110;
            var oy = 70;
            plotter.GoToXY(ox, oy);
            await plotter.PenDown();

            for (var i = 0; i < w * 1.5; i++)
            {
                plotter.GoToXY(ox + i, oy + i);
                plotter.GoToXY(ox + i, oy + 120 - i);
                plotter.GoToXY(ox + 80 - i, oy + 120 - i);
                plotter.GoToXY(ox + 80 - i, oy + i);
                plotter.GoToXY(ox + i, oy + i);
            }
            await plotter.PenUp();
            
            // T
            var tx = 220;
            var ty = 70;
            plotter.GoToXY(tx, ty);
            await plotter.PenDown();

            for (var i = 0; i < w; i++)
            {
                plotter.GoToXY(tx + i, ty + i);
                plotter.GoToXY(tx + i, ty + 140 + i);
                plotter.GoToXY(tx - 40 + i, ty + 140 + i);
                plotter.GoToXY(tx - 40 + i, ty + 160 - i);
                plotter.GoToXY(tx + 60 - i, ty + 160 - i);
                plotter.GoToXY(tx + 60 - i, ty + 140 + i);
                plotter.GoToXY(tx + 20 - i, ty + 140 + i);
                plotter.GoToXY(tx + 20 - i, ty + i);
                plotter.GoToXY(tx + i, ty + i);
            }
            await plotter.PenUp();
        }

        public async Task PenDemo()
        {
            await plotter.PenDown();
            await Task.Delay(2000);
            await plotter.PenUp();
        }

        public async Task DrawCursor(int x, int y)
        {
            await this.plotter.PenUp();
            plotter.GoToXY(x, y);
            await this.plotter.PenDown();
            plotter.GoToXY(x, y - 70);
            plotter.GoToDiagonal(x+17, y - 63);
            plotter.GoToDiagonal(x + 30, y - 90);
            plotter.GoToDiagonal(x + 45, y - 80);
            plotter.GoToDiagonal(x + 34, y - 55);
            plotter.GoToDiagonal(x + 50, y - 47);
            plotter.GoToDiagonal(x, y);
            await this.plotter.PenUp();
        }
    }
}
