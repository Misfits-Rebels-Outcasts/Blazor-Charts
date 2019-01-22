using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.man.svg;

namespace PieChartComponent
{
    public class PieChartMan :BlazorComponent
    {
        [Parameter]
        protected string InputData { get; set; }

        private double pieRadius = 0.85;    
        private void getCoordinatesForPercent(double percent, out double x, out double y)
        {
            //x = Math.Cos(2 * Math.PI * percent);
            //y = Math.Sin(2 * Math.PI * percent);
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

            /*
                <svg width="84%" height="84%" viewBox="-1 -1 2 2" style="transform: rotate(-90deg);margin-left:8px;margin-top:20px">
                    <path fill="#ce4b99" d="M 1 0 A 1 1 0 0 1 0.8090169943749475 0.5877852522924731 L 0 0" />
                    <path fill="#27A844" d="M 0.8090169943749475 0.5877852522924731 A 1 1 0 1 1 -1.8369701987210297e-16 -1 L 0 0" />
                    <path fill="#377bbc" d="M -1.8369701987210297e-16 -1 A 1 1 0 0 1 0.9510565162951535 -0.3090169943749476 L 0 0" />
                </svg>
            //SVG svg = new SVG() { { "width", "70%" }, { "height", "70%" }, { "viewBox", "-1 -1 2 2" },{ "style", "transform: rotate(-90deg);margin-left:20px;margin-top:10px" } };
            //SVG svg = new SVG() { { "width", "100%" }, { "height", "100%" }, { "viewBox", "0 0 2 2" },{ "style", "transform: rotate(-90deg);" } };
            //Rectangle rect = new Rectangle() { { "width", "100%" }, { "height", "100%" }, { "fill", "cyan" } };
            //Rectangle rect = new Rectangle() { { "width", "50%" }, { "height", "50%" }, { "fill", "cyan" } };
            */

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
            /*
            Path path1 = new Path() { { "fill", "#ce4b99" }, { "d", "M " + prStr + " 0 A " + prStr + " " + prStr + " 0 0 1 " + x + " " + y + " L 0 0" } };
            getCoordinatesForPercent(0.1 + 0.65, out x, out y);
            Path path2 = new Path() { { "fill", "#27A844" }, { "d", "M " + px + " " + py + " A " + prStr + " " + prStr + " 0 1 1 " + x + " " + y + " L 0 0" } };
            px = x; py = y;
            getCoordinatesForPercent(0.1 + 0.65 + 0.25, out x, out y);
            Path path3 = new Path() { { "fill", "#377bbc" }, { "d", "M " + px + " " + py + " A " + prStr + " " + prStr + " 0 0 1 " + x + " " + y + " L 0 0" } };
            svg.AddItems(path1, path2, path3);
            */
            BlazorRenderer blazorRenderer = new BlazorRenderer();
            blazorRenderer.Draw(seq, builder, svg);


            /*
            //Path path1 = new Path() { { "fill", "#ce4b99" }, { "d", "M 1 0 A 1 1 0 0 1 0.8090169943749475 0.5877852522924731 L 0 0" } };
            Path path1 = new Path() { { "fill", "#ce4b99" }, { "d", "M 1 0 A 1 1 0 0 1 " + x + " " + y + " L 0 0" } };
            getCoordinatesForPercent(0.1+0.65, out x, out y);

            //Path path2 = new Path() { { "fill", "#27A844" }, { "d", "M 0.8090169943749475 0.5877852522924731 A 1 1 0 1 1 -1.8369701987210297e-16 -1 L 0 0" } };
            Path path2 = new Path() { { "fill", "#27A844" }, { "d", "M "+px+" "+py+ " A 1 1 0 1 1 "+x+" "+y+" L 0 0" } };
            px = x; py = y;
            getCoordinatesForPercent(0.1 + 0.65 +0.25, out x, out y);
            //Path path3 = new Path() { { "fill", "#377bbc" }, { "d", "M -1.8369701987210297e-16 -1 A 1 1 0 0 1 0.9510565162951535 -0.3090169943749476 L 0 0" } };
            Path path3 = new Path() { { "fill", "#377bbc" }, { "d", "M " + px + " " + py + " A 1 1 0 0 1 " + x + " " + y + " L 0 0" } };
            svg.AddItems(path1, path2, path3);
            //svg.AddItems(rect);
            */
            
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
