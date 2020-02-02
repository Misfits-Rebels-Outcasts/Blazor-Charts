using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAssemblyMan.SVGRender;

namespace WebAssemblyMan
{
    public class PieChart : ComponentBase
    {
        [Parameter]
        public string InputData { get; set; }

        private double pieRadius = 0.85;    
        private void getCoordinatesForPercent(double percent, out double x, out double y)
        {
            x = pieRadius * Math.Cos(2 * Math.PI * percent);
            y = pieRadius * Math.Sin(2 * Math.PI * percent);

            Console.WriteLine("xx:"+ x);
            Console.WriteLine("yy:"+ y);

        }
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var seq = 0;
            builder.OpenElement(seq, "figure");
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "piechart-main");

            int[] inputData = { 30, 30, 40 };
            string[] colors = { "#ce4b99", "#27A844", "#377bbc" };
            string[] labels = { "App Store", "Website", "Partners" };
            string[] inputDataArr = InputData.Split(',');


            SVG svg = new SVG() { { "width", "100%" }, { "height", "100%" }, { "viewBox", "-1 -1 2 2" },{"style","transform: rotate(-90deg)" } };

            double x, y;
            double px=0, py=0;
            double totalPercent = 0;
            string prStr = pieRadius.ToString();

            for (int icounter=0; icounter < inputDataArr.Length; icounter++)
            {
                double percent = double.Parse(inputDataArr[icounter])/100;
                totalPercent = totalPercent + percent;
                getCoordinatesForPercent(totalPercent, out x, out y);
                Path path = null;
                if (icounter == 0)
                    path = new Path() { { "fill", colors[icounter] }, { "d", "M " + prStr + " 0 A " + prStr + " " + prStr + " 0 0 1 " + x + " " + y + " L 0 0" } };
                else 
                {
                    if (percent > 0.5)
                        path = new Path() { { "fill", colors[icounter] }, { "d", "M " + px + " " + py + " A " + prStr + " " + prStr + " 0 1 1 " + x + " " + y + " L 0 0" } };
                    else
                        path = new Path() { { "fill", colors[icounter] }, { "d", "M " + px + " " + py + " A " + prStr + " " + prStr + " 0 0 1 " + x + " " + y + " L 0 0" } };
                }

                svg.AddItems(path);
                px = x; py = y;

            }
            BlazorRenderer blazorRenderer = new BlazorRenderer();
            blazorRenderer.Draw(seq, builder, svg);
            
            builder.OpenElement(++seq, "figcaption");
            builder.AddAttribute(++seq, "class", "piechart-key");
            builder.OpenElement(++seq, "ul");
            builder.AddAttribute(++seq, "class", "piechart-key-list");
            builder.AddAttribute(++seq, "aria-hidden", "true");
            builder.AddAttribute(++seq, "style", "list-style-type: none;");

            int counter = 0;
            foreach (string dataStr in inputDataArr)
            {
                int data = int.Parse(dataStr);
                builder.OpenElement(++seq, "li");
                builder.OpenElement(++seq, "span");
                builder.AddAttribute(++seq, "class", "round-dot");
                builder.AddAttribute(++seq, "style", "background-color:" + colors[counter]);

                builder.CloseElement();
                builder.AddContent(++seq, labels[counter++] + " " + "(" + data.ToString() + ")");
                builder.CloseElement();
            }

            builder.CloseElement();
            builder.CloseElement();
            
            builder.CloseElement();
            builder.CloseElement();

        }
    }
}
