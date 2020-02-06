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
    /*TODO
    Calculate Start Space based on Labels? Violate simple principle?
    */
    public class HorizontalBarChart : ComponentBase
    {
        [Parameter]
        public string InputData { get; set; }
        [Parameter]
        public string InputLabels { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var seq = 0;
            builder.OpenElement(seq, "figure");
            builder.AddAttribute(++seq, "class", "horizontal-bar-chart");
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "main");

            System.Diagnostics.Debug.WriteLine("ID"+InputData);

            string[] inputDataArr = InputData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] inputLabelsArr = InputLabels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            /*
            int[] inputData = { 30, 70, 42, 50, 3, 55, 35, 22 };
            int[] list1 = new int[8] { 30, 70, 42, 50, 3, 55, 35, 22 };
            int[] list2 = new int[8] { 40, 50, 32, 70, 55, 15, 15, 12 };
            int[] list3 = new int[8] { 0, 10, 10, 10, 10, 20, 70, 70 };
            int[][] lists = new int[][] { list1, list2, list3};
            */
            double boundHeight = 100.0;
            double boundWidth = 150.0;

            string[] colors = { "#fe2712", "#fc600a", "#fb9902", "#fccc1a", "#fefe33", "#b2d732", "#66b032", "#347c98", "#0247fe", "#4424d6", "#8601af", "#c21460" };
            string[] labels = { "App Store", "Website", "Partners", "Direct", "Channels", "Retail", "Distributors", "Affiliates", "Phone", "TV" ,"X"};


            SVG svg = new SVG() { { "class", "svg" }, { "width", "100%" }, { "height", "100%" }, { "viewBox", "0 0 150 100" }};
            //Rectangle rect = new Rectangle() { { "class", "background-rect" }, { "width", "100%" }, { "height", "100%" }, { "fill", "white" }, { "stroke", "gray" }, {"stroke-width", "0.5" } };
            Rectangle rect = new Rectangle() { { "class", "background-rect" }};
            svg.AddItems(rect);
            
            int numHorizontalLines = 10;
            int numVerticalLines = 10;
            double verticalStartSpace = 10.0;
            double horizontalStartSpace = 30.0;
            double verticalEndSpace = 5.0;
            double horizontalEndSpace = 20.0;
            double gridYUnits = 10;
            double gridXUnits = 10;
            bool skipLastVerticalLine = false;
            bool skipLastHorizontalLine = false;

            double verticalSpace = (boundHeight- verticalStartSpace-verticalEndSpace) / (numHorizontalLines);
            double horizontalSpace = (boundWidth - horizontalStartSpace-horizontalEndSpace) / (numVerticalLines);

            double totalGridWidth = ((double)(numVerticalLines)) * horizontalSpace;
            double totalGridHeight = ((double)(numHorizontalLines)) * verticalSpace;
            System.Diagnostics.Debug.WriteLine("TotalGridHeight:" + totalGridHeight+":"+ verticalSpace);

            double[] inputDataArrDouble = new double[inputDataArr.Length];
            int i = 0;
            foreach (string iData in inputDataArr)
            {
                System.Diagnostics.Debug.WriteLine("iData:" + iData);
                inputDataArrDouble[i] = double.Parse(inputDataArr[i++]);
            }
            System.Diagnostics.Debug.WriteLine("inputDataArr Length:" + inputDataArr.Length);

            System.Diagnostics.Debug.WriteLine("Vertical Lines");

            //Vertical Lines            
            double x = horizontalStartSpace;
            double startGridX = 0;
            for (int counter = 0; counter <= numVerticalLines; counter++)
            {
                
                if (counter == numVerticalLines && skipLastVerticalLine)
                    continue;
                /*
                Path path = new Path() { { "class", "vertical-grid-lines" }, { "fill", "none" }, { "stroke", "gray" }, { "stroke-width", "0.2" }, { "d", "M " + x.ToString() +" "+ (boundHeight-verticalStartSpace).ToString() + " L "+ x.ToString() + " " +(verticalEndSpace).ToString() } };
                Text label = new Text() { { "class", "y-axis-labels" }, {"x",x.ToString() }, { "y", (boundHeight - verticalStartSpace + 5).ToString() },{ "font-size", "4px" }, { "text-anchor", "middle" }, { "content", (startGridX).ToString() } };
                */
                
                Path path = new Path() { { "class", "vertical-grid-lines" }, { "d", "M " + x.ToString() +" "+ (boundHeight-verticalStartSpace).ToString() + " L "+ x.ToString() + " " +(verticalEndSpace).ToString() } };
                Text label = new Text() { { "class", "y-axis-labels" }, {"x",x.ToString() }, { "y", (boundHeight - verticalStartSpace + 5).ToString() },{ "content", (startGridX).ToString() } };
                
                startGridX = startGridX + gridXUnits;

                svg.AddItems(path,label);
                x = x + horizontalSpace;
            }

            //Horizontal Lines
            
            double y = verticalStartSpace;
            double startGridY = 0;
            i = 0;
            for (int counter=0;counter<= numHorizontalLines; counter++)
            {
                System.Diagnostics.Debug.WriteLine("i:" + i);
                if (counter == numHorizontalLines  && skipLastHorizontalLine)
                {
                    continue;
                }
                System.Diagnostics.Debug.WriteLine("y:" + i+":"+ inputDataArr.Length);

                /*                
                Path path = new Path() { { "class", "horizontal-grid-lines" }, { "fill", "none" }, { "stroke", "gray" }, { "stroke-width", "0.2" }, { "d", "M "+(horizontalStartSpace).ToString()+" "+(boundHeight - y).ToString() + " L "+(horizontalStartSpace+numHorizontalLines*gridXUnits).ToString()+" "+(boundHeight - y).ToString() } };
                Text label = new Text() { { "class", "x-axis-labels" }, { "x", (horizontalStartSpace-2).ToString() }, { "y", (boundHeight - y).ToString() }, { "font-size", "4px" }, { "text-anchor", "end" }, { "content", labels[counter] } };
                */
                Path path = new Path() { { "class", "horizontal-grid-lines"}, { "d", "M "+(horizontalStartSpace).ToString()+" "+(boundHeight - y).ToString() + " L "+(horizontalStartSpace+numHorizontalLines*gridXUnits).ToString()+" "+(boundHeight - y).ToString() } };                
                string xLabels="";
                if (counter<inputLabelsArr.Length)
                    xLabels=inputLabelsArr[counter];
                Text label = new Text() { { "class", "x-axis-labels" }, { "x", (horizontalStartSpace-2).ToString() }, { "y", (boundHeight - y).ToString() }, { "content", xLabels } };
                

                System.Diagnostics.Debug.WriteLine("z:" + i);
                if (counter==0)
                    svg.AddItems(path,label);
                if (i< (inputDataArr.Length))
                {
                    System.Diagnostics.Debug.WriteLine("i:" + i + ":" + inputDataArrDouble[i].ToString() + "px");
                    System.Diagnostics.Debug.WriteLine("labelrect");
                    //Rectangle bar = new Rectangle() { { "fill", "#ce4b99" }, { "x", (horizontalStartSpace).ToString() }, { "y", (boundHeight - y - 5).ToString() }, { "width", inputDataArrDouble[i].ToString() + "px" }, { "height", "5px" } };
                    Rectangle bar = new Rectangle() { { "class", "bar" }, { "x", (horizontalStartSpace).ToString() }, { "y", (boundHeight - y - 5).ToString() }, { "width", inputDataArrDouble[i].ToString() + "px" }, { "height", "5px" } };
                    svg.AddItems(label, bar);
                    i++;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("label");
                    if (counter<numHorizontalLines)
                        svg.AddItems(label);
                }

                System.Diagnostics.Debug.WriteLine("Y:" + y);

                y = y + verticalSpace;
                startGridY = startGridY + gridYUnits;
            }

            
            BlazorRenderer blazorRenderer = new BlazorRenderer();
            blazorRenderer.Draw(seq, builder, svg);

            builder.CloseElement();
            builder.CloseElement();



        }
    }
}
