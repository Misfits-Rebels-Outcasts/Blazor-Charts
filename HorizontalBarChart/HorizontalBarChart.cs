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
    public class HorizontalBarChart : ComponentBase
    {
        [Parameter]
        public string InputData { get; set; }
        /*
        private double pieRadius = 0.85;    
        private void getCoordinatesForPercent(double percent, out double x, out double y)
        {

            x = pieRadius * Math.Cos(2 * Math.PI * percent);
            y = pieRadius * Math.Sin(2 * Math.PI * percent);

            Console.WriteLine("xx:"+ x);
            Console.WriteLine("yy:"+ y);

        }
        */
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var seq = 0;
            builder.OpenElement(seq, "figure");
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "linechart-main");

            System.Diagnostics.Debug.WriteLine("ID"+InputData);

            string[] inputDataArr = InputData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int[] inputData = { 30, 70, 42, 50, 3, 55, 35, 22 };
            int[] list1 = new int[8] { 30, 70, 42, 50, 3, 55, 35, 22 };
            int[] list2 = new int[8] { 40, 50, 32, 70, 55, 15, 15, 12 };
            int[] list3 = new int[8] { 0, 10, 10, 10, 10, 20, 70, 70 };
            int[][] lists = new int[][] { list1, list2, list3};

            string[] colors = { "#fe2712", "#fc600a", "#fb9902", "#fccc1a", "#fefe33", "#b2d732", "#66b032", "#347c98", "#0247fe", "#4424d6", "#8601af", "#c21460" };

            string[] labels = { "App Store", "Website", "Partners", "App Store", "Website", "Partners", "App Store", "Website", "Partners", "App Store", "Website", "Partners" };

            SVG svg = new SVG() { { "width", "100%" }, { "height", "100%" }, { "viewBox", "0 0 100 100" } };
            Rectangle rect = new Rectangle() { { "width", "100%" }, { "height", "100%" }, { "fill", "white" }, { "stroke", "gray" }, {"stroke-width", "0.5" } };
            svg.AddItems(rect);
            
            int numHorizontalLines = 10;
            int numVerticalLines = 10;
            double boundHeight = 100.0;
            double boundWidth = 100.0;
            double verticalStartSpace = 10.0;
            double horizontalStartSpace = 10.0;
            double verticalEndSpace = 5.0;
            double horizontalEndSpace = 5.0;
            double gridYUnits = 10;
            double gridXUnits = 10;
            bool skipLastVerticalLine = true;
            bool skipLastHorizontalLine = true;

            double verticalSpace = (boundHeight- verticalStartSpace-verticalEndSpace) / (numHorizontalLines-1);
            double horizontalSpace = (boundWidth - horizontalStartSpace-horizontalEndSpace) / (numVerticalLines - 1);

            double totalGridWidth = ((double)(numVerticalLines-1)) * horizontalSpace;
            double totalGridHeight = ((double)(numHorizontalLines-1)) * verticalSpace;
            System.Diagnostics.Debug.WriteLine("TotalGridHeight:" + totalGridHeight+":"+ verticalSpace);

            double[] dAry = new double[inputDataArr.Length];
            int i = 0;
            foreach (string iData in inputDataArr)
            {
                System.Diagnostics.Debug.WriteLine("iData:" + iData);
                dAry[i] = double.Parse(inputDataArr[i++]);
            }
            System.Diagnostics.Debug.WriteLine("inputDataArr Length:" + inputDataArr.Length);

            //Horizontal Lines
            double y = verticalStartSpace;
            double startGridY = 0;
            i = 0;
            for (int counter=0;counter< numHorizontalLines; counter++)
            {
                System.Diagnostics.Debug.WriteLine("i:" + i);
                if (counter == numHorizontalLines - 1 && skipLastHorizontalLine)
                {
                    continue;
                }
                System.Diagnostics.Debug.WriteLine("y:" + i+":"+ inputDataArr.Length);

                Path path = new Path() { { "fill", "none" }, { "stroke", "gray" }, { "stroke-width", "0.2" }, { "d", "M "+horizontalStartSpace.ToString()+" "+(boundHeight - y).ToString() + " L "+(boundWidth-horizontalEndSpace).ToString()+" "+(boundHeight - y).ToString() } };
                Text label = new Text() { { "x", (horizontalStartSpace-2).ToString() }, { "y", (boundHeight - y).ToString() }, { "font-size", "4px" }, { "text-anchor", "end" }, { "content", (startGridY).ToString() } };
                System.Diagnostics.Debug.WriteLine("z:" + i);
                if (counter==0)
                    svg.AddItems(path,label);
                else
                {
                    if (i< (inputDataArr.Length))
                    {
                        System.Diagnostics.Debug.WriteLine("i:" + i + ":" + dAry[i].ToString() + "px");
                        System.Diagnostics.Debug.WriteLine("labelrect");
                        Rectangle rectangle = new Rectangle() { { "fill", "#ce4b99" }, { "x", (horizontalStartSpace).ToString() }, { "y", (boundHeight - y - 5).ToString() }, { "width", dAry[i].ToString() + "px" }, { "height", "5px" } };
                        svg.AddItems(label, rectangle);
                        i++;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("label");
                        svg.AddItems(label);
                    }
                }

                System.Diagnostics.Debug.WriteLine("Y:" + y);

                y = y + verticalSpace;
                startGridY = startGridY + gridYUnits;
            }

            System.Diagnostics.Debug.WriteLine("Vertical Lines");

            //Vertical Lines            
            double x = horizontalStartSpace;
            double startGridX = 0;
            for (int counter = 0; counter < numVerticalLines; counter++)
            {
                if (counter == numVerticalLines - 1 && skipLastVerticalLine)
                    continue;

                Path path = new Path() { { "fill", "none" }, { "stroke", "gray" }, { "stroke-width", "0.2" }, { "d", "M " + x.ToString() +" "+ (boundHeight-verticalStartSpace).ToString() + " L "+ x.ToString() + " " +(verticalEndSpace).ToString() } };
                Text label = new Text() { {"x",x.ToString() }, { "y", (boundHeight - verticalStartSpace + 5).ToString() },{ "font-size", "4px" }, { "text-anchor", "middle" }, { "content", (startGridX).ToString() } };
                startGridX = startGridX + gridXUnits;

                svg.AddItems(path,label);
                x = x + horizontalSpace;
            }
            


            BlazorRenderer blazorRenderer = new BlazorRenderer();
            blazorRenderer.Draw(seq, builder, svg);
            
            builder.OpenElement(++seq, "figcaption");
            builder.AddAttribute(++seq, "class", "linechart-key");
            builder.OpenElement(++seq, "ul");
            builder.AddAttribute(++seq, "class", "linechart-key-list");
            builder.AddAttribute(++seq, "aria-hidden", "true");
            builder.AddAttribute(++seq, "style", "list-style-type: none;");


            int colorcounter = 0;
            foreach (string iData in inputDataArr)
            {
                //int data = int.Parse(dataStr);
                System.Diagnostics.Debug.WriteLine("Color:"+iData);
                builder.OpenElement(++seq, "li");
                builder.OpenElement(++seq, "span");
                builder.AddAttribute(++seq, "class", "round-dot");
                builder.AddAttribute(++seq, "style", "background-color:" + colors[colorcounter]);

                builder.CloseElement();
                builder.AddContent(++seq, labels[colorcounter++] );
                builder.CloseElement();
            }

            builder.CloseElement();
            builder.CloseElement();
            

            builder.CloseElement();
            builder.CloseElement();



        }
    }
}
