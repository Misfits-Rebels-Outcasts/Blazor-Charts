using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.man.svg;

namespace LineChartComponent
{
    public class LineChartMan : ComponentBase
    {
        [Parameter]
        protected string InputData { get; set; }

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
            builder.AddAttribute(++seq, "class", "linechart-main");

            int[] inputData = { 30, 30, 40 };
            string[] colors = { "#ce4b99", "#27A844", "#377bbc" };
            string[] labels = { "App Store", "Website", "Partners" };
            string[] inputDataArr = InputData.Split(',');

            SVG svg = new SVG() { { "width", "100%" }, { "height", "100%" }, { "viewBox", "0 0 100 100" } };
            Rectangle rect = new Rectangle() { { "width", "100%" }, { "height", "100%" }, { "fill", "white" }, { "stroke", "gray" }, {"stroke-width", "0.5" } };
            //Rectangle rect = new Rectangle() { { "width", "100%" }, { "height", "100%" }, { "fill", "cyan" }};
            svg.AddItems(rect);


            int numHorizontalLines = 10;
            int numVerticalLines = 10;
            double boundHeight = 100.0;
            double boundWidth = 100.0;
            double verticalStartSpace = 10.0;
            double horizontalStartSpace = 10.0;
            double verticalEndSpace = 5.0;
            double horizontalEndSpace = 5.0;

            bool skipLastVerticalLine = true;
            bool skipLastHorizontalLine = true;

            double verticalSpace = (boundHeight- verticalStartSpace-verticalEndSpace) / (numHorizontalLines-1);
            double horizontalSpace = (boundWidth - horizontalStartSpace-horizontalEndSpace) / (numVerticalLines - 1);

            //Horizontal Lines
            double y = verticalStartSpace;
            for (int counter=0;counter<numHorizontalLines;counter++)
            {
                if (counter == numHorizontalLines - 1 && skipLastHorizontalLine)
                    continue;

                Path path = new Path() { { "fill", "none" }, { "stroke", "gray" }, { "stroke-width", "0.2" }, { "d", "M "+horizontalStartSpace.ToString()+" "+(boundHeight - y).ToString() + " L "+(boundWidth-horizontalEndSpace).ToString()+" "+(boundHeight - y).ToString() } };
                Text label = new Text() { { "x", (horizontalStartSpace-2).ToString() }, { "y", (boundHeight - y).ToString() }, { "font-size", "4px" }, { "text-anchor", "end" }, { "content", (counter+7).ToString() } };
                svg.AddItems(path,label);
                y = y + verticalSpace;
            }

            //Vertical Lines
            
            double x = horizontalStartSpace;
            for (int counter = 0; counter < numVerticalLines; counter++)
            {
                if (counter == numVerticalLines - 1 && skipLastVerticalLine)
                    continue;

                Path path = new Path() { { "fill", "none" }, { "stroke", "gray" }, { "stroke-width", "0.2" }, { "d", "M " + x.ToString() +" "+ (boundHeight-verticalStartSpace).ToString() + " L "+ x.ToString() + " " +(verticalEndSpace).ToString() } };

                Text label = new Text() { {"x",x.ToString() }, { "y", (boundHeight - verticalStartSpace + 5).ToString() },{ "font-size", "4px" }, { "text-anchor", "middle" }, { "content", (counter+4).ToString() } };

                svg.AddItems(path,label);
                x = x + horizontalSpace;
            }
            
            BlazorRenderer blazorRenderer = new BlazorRenderer();
            blazorRenderer.Draw(seq, builder, svg);


            /*
                         <path d="M 25 50 L 450 50"
                  fill="none" stroke="gray" stroke-width="0.3" />

            builder.OpenElement(++seq, "figcaption");
            builder.AddAttribute(++seq, "class", "linechart-key");
            builder.OpenElement(++seq, "ul");
            builder.AddAttribute(++seq, "class", "linechart-key-list");
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
            */
            builder.CloseElement();
            builder.CloseElement();



        }
    }
}
