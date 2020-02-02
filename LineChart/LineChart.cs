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
    public class LineChart : ComponentBase
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
            
            string[] inputDataArrX = InputData.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            int numLines = 0;
            System.Diagnostics.Debug.WriteLine("Start");
            foreach (string inputLine in inputDataArrX)
            {
                if (inputLine.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) >= 0)
                {
                    numLines++;
                }
            }
            System.Diagnostics.Debug.WriteLine("End");
            string[] inputDataArr = new string[numLines];
            int lineCounter = 0;
            foreach (string inputLine in inputDataArrX)
            {
                if (inputLine.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) >= 0)
                {
                    inputDataArr[lineCounter++] = inputLine;
                    System.Diagnostics.Debug.WriteLine("IL:" + inputLine);
                }
            }
            
            int[] inputData = { 30, 70, 42, 50, 3, 55, 35, 22 };
            int[] list1 = new int[8] { 30, 70, 42, 50, 3, 55, 35, 22 };
            int[] list2 = new int[8] { 40, 50, 32, 70, 55, 15, 15, 12 };
            int[] list3 = new int[8] { 0, 10, 10, 10, 10, 20, 70, 70 };
            int[][] lists = new int[][] { list1, list2, list3};

            string[] colors = { "#ce4b99", "#27A844", "#377bbc" };
            string[] labels = { "App Store", "Website", "Partners" };            

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
            double gridYUnits = 10;
            double gridXUnits = 10;
            bool skipLastVerticalLine = true;
            bool skipLastHorizontalLine = true;

            double verticalSpace = (boundHeight- verticalStartSpace-verticalEndSpace) / (numHorizontalLines-1);
            double horizontalSpace = (boundWidth - horizontalStartSpace-horizontalEndSpace) / (numVerticalLines - 1);

            double totalGridWidth = ((double)(numVerticalLines-1)) * horizontalSpace;
            double totalGridHeight = ((double)(numHorizontalLines-1)) * verticalSpace;
            System.Diagnostics.Debug.WriteLine("TotalGridHeight:" + totalGridHeight+":"+ verticalSpace);

            //Horizontal Lines
            double y = verticalStartSpace;
            double startGridY = 0;
            for (int counter=0;counter<numHorizontalLines;counter++)
            {
                if (counter == numHorizontalLines - 1 && skipLastHorizontalLine)
                    continue;

                Path path = new Path() { { "fill", "none" }, { "stroke", "gray" }, { "stroke-width", "0.2" }, { "d", "M "+horizontalStartSpace.ToString()+" "+(boundHeight - y).ToString() + " L "+(boundWidth-horizontalEndSpace).ToString()+" "+(boundHeight - y).ToString() } };
                Text label = new Text() { { "x", (horizontalStartSpace-2).ToString() }, { "y", (boundHeight - y).ToString() }, { "font-size", "4px" }, { "text-anchor", "end" }, { "content", (startGridY).ToString() } };
                svg.AddItems(path,label);
                System.Diagnostics.Debug.WriteLine("Y:" + y);

                y = y + verticalSpace;
                startGridY = startGridY + gridYUnits;
            }

            //Chart Line
            double gridx=0, gridy = 0;
            gridx = horizontalStartSpace;
            gridy = boundHeight - verticalStartSpace;
            int colorcounter = 0;
            foreach (string iData in inputDataArr)
            {
                string chartLine = "";
                double gridValueX = 0;
                double gridValueY = 0;
                bool firstTime = true;

                string[] inputLineArr = iData.Split(',');
                int[] intAry=new int[inputLineArr.Length];
                for (int i = 0; i < inputLineArr.Length; i++)
                    intAry[i] = int.Parse(inputLineArr[i]);

                foreach (int i in intAry)
                {
                    if (firstTime)
                    {
                        chartLine = chartLine + "M ";
                        firstTime = false;
                        gridValueX = horizontalStartSpace;
                        gridValueY = verticalStartSpace;
                        double gridValue = ((double)i) * verticalSpace / gridYUnits;
                        gridValueY = boundHeight - (gridValueY + gridValue);
                        chartLine = chartLine + gridValueX.ToString() + " " + gridValueY.ToString();
                    }
                    else
                    {
                        chartLine = chartLine + " L ";
                        gridValueX = gridValueX + horizontalSpace;
                        gridValueY = verticalStartSpace;
                        double gridValue = ((double)i) * verticalSpace / gridYUnits;
                        gridValueY = boundHeight - (gridValueY + gridValue);
                        chartLine = chartLine + gridValueX.ToString() + " " + gridValueY.ToString();
                    }
                }
                //System.Diagnostics.Debug.WriteLine("CL:" + chartLine);
                Path linepath = new Path() { { "fill", "none" }, { "stroke", colors[colorcounter++] }, { "stroke-width", "1.0" }, { "d", chartLine } };
                svg.AddItems(linepath);

            }

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

            colorcounter = 0;
            foreach (string iData in inputDataArr)
            {
                //int data = int.Parse(dataStr);
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
            

            /*
             *             <path d="M 30 250 L 130 120
L 230 150 L 330 80 L 430 200"
                  fill="none" stroke="#27A844" stroke-width="2.5" />


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
