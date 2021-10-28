using System;
using System.Windows;
using Tida.CAD.Events;
using System.Linq;
using System.Collections.Generic;
using Tida.CAD.Extensions;
using System.Windows.Media;
using Tida.CAD.Input;

namespace Tida.CAD.DrawObjects {
    /// <summary>
    /// The base drawobject of lines;
    /// </summary>
    public abstract class LineBase : DrawObject 
    {
        public LineBase(Point start, Point end) 
        {
            Start = start;
            End = end;
        }
        
        private Point _start;
        public Point Start
        {
            get => _start;
            set
            {
                _start = value;
                RaiseVisualChanged();
            }
        }

        private Point _end;
        public Point End
        {
            get => _end;
            set
            {
                _end = value;
                RaiseVisualChanged();
            }
        }

        public override CADRect? GetBoundingRect() 
        {
            var bottomLeft = new Point(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
            var width = Math.Abs(Start.X - End.X);
            var height = Math.Abs(Start.Y - End.Y);
            return new CADRect(bottomLeft,new Size(width,height));
        }

        public override bool ObjectInRectangle(CADRect rect, ICADScreenConverter cadScreenConverter, bool anyPoint)
        {
            //if both two points in inside the rect,then return true;
            if (rect.Contains(Start) && rect.Contains(End))
            {
                return true;
            }
            //or if anypoint is true,check the line is interesecting with any border of the rect;
            else if (anyPoint)
            {               
                return rect.GetBorders()?.Any(p => GeometryExtensions.GetIntersectPoint(p.start,p.end,Start,End) != null) ?? false;
            }

            return false;
        }
        

        public override void Draw(ICanvas canvas) {
           
            //绘制自身;
            canvas.DrawLine(Pen, Start,End);
        }

        private Pen _pen;
        /// <summary>
        /// 绘制线段的笔;
        /// </summary>
        public Pen Pen {
            get => _pen;
            set {
                _pen = value;
                RaiseVisualChanged();
            }
        }


       

    }
}
