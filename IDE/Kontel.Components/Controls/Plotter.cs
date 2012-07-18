using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ZedGraph;
using System.Drawing;
using System.Windows.Forms;

namespace Kontel.Relkon
{
    /// <summary>
    /// Элемент управления, отрисовывающий график изменения значения некоторого
    /// параметра от времени
    /// </summary>
    public sealed class Plotter : ZedGraphControl
    {
        private Color[] colors = new Color[10];
        private int index = 0;

        public Plotter()
        {
            this.GraphPane = this.CreateGraphPane();
            this.colors[0] = Color.FromArgb(0, 131, 215);
            this.colors[1] = Color.FromArgb(0, 153, 0);
            this.colors[2] = Color.FromArgb(255, 51, 0);
            this.colors[3] = Color.FromArgb(255, 204, 0);
            this.colors[4] = Color.FromArgb(0, 51, 153);
            this.colors[5] = Color.FromArgb(0, 102, 0);
            this.colors[6] = Color.FromArgb(153, 0, 0);
            this.colors[7] = Color.FromArgb(153, 102, 0);
            this.colors[8] = Color.FromArgb(102, 102, 204);
            this.colors[9] = Color.FromArgb(255, 153, 51);
            this.index = 0;
        }

        private GraphPane CreateGraphPane()
        {
            GraphPane pane = new GraphPane(new RectangleF(0, 0, this.Width, this.Height), "", "Дата", "Значение");
            pane.Fill.Color = Color.White;
            pane.XAxis.Type = AxisType.Date;
            pane.YAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.Color = Color.White;
            return pane;
        }

        public void AddGraphic(string Name, DateTime[] DatePoints, double[] ValuePoints)
        {
            this.EraseGraphic(Name);
            PointPairList points = new PointPairList();
            int n = Math.Min(DatePoints.Length, ValuePoints.Length);
            for (int i = 0; i < n; i++)
            {
                points.Add((double)new XDate(DatePoints[i]), ValuePoints[i]);
            }
            LineItem line = this.GraphPane.AddCurve(Name, points, this.colors[this.index++], SymbolType.None);
            line.Line.Width = 1;
            this.AxisChange();
            this.Refresh();
            if (this.index == 10)
                this.index = 0;
        }

        public void Clear()
        {
            this.GraphPane = this.CreateGraphPane();
            this.index = 0;
        }

        public void EraseGraphic(string GraphicName)
        {
            int idx = this.GraphPane.CurveList.IndexOf(GraphicName);
            if (idx != -1)
            {
                this.GraphPane.CurveList.RemoveAt(idx);
                this.AxisChange();
                this.Refresh();
            }
        }
    }
}
