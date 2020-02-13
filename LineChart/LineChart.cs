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
        
        [Parameter]
        public string InputLabels { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var seq = 0;
            builder.OpenElement(seq, "figure");
            builder.AddAttribute(++seq, "class", "line-chart");
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "main");


            System.Diagnostics.Debug.WriteLine("ID"+InputData);
            
            string[] inputDataArrX = InputData.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            string[] inputLabelsArr = InputLabels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

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

            string[] colors = { "#ce4b99", "#27A844", "#377bbc" };
            string[] labels = { "App Store", "Website", "Partners" };            

            double boundHeight = 150.0;
            double boundWidth = 150.0;

            SVG svg = new SVG() { { "width", "100%" }, { "height", "100%" }, { "viewBox", "0 0 150 150" } };
            //Rectangle rect = new Rectangle() { { "class", "background-rect" }, { "width", "100%" }, { "height", "100%" }, { "fill", "white" }, { "stroke", "gray" }, {"stroke-width", "0.5" } };
            //Rectangle rect = new Rectangle() { { "width", "100%" }, { "height", "100%" }, { "fill", "cyan" }};
            Rectangle rect = new Rectangle() { { "class", "background-rect" }};
            svg.AddItems(rect);


            int numHorizontalLines = 10;
            int numVerticalLines = 10;
            double verticalStartSpace = 25.0;
            double horizontalStartSpace = 25.0;
            double verticalEndSpace = 25.0;
            double horizontalEndSpace = 25.0;
            double gridYUnits = 10;
            double gridXUnits = 10;
            //bool skipLastVerticalLine = true;
            //bool skipLastHorizontalLine = true;

            double verticalSpace = (boundHeight- verticalStartSpace-verticalEndSpace) / (numHorizontalLines);
            double horizontalSpace = (boundWidth - horizontalStartSpace-horizontalEndSpace) / (numVerticalLines);

            double totalGridWidth = ((double)(numVerticalLines-1)) * horizontalSpace;
            double totalGridHeight = ((double)(numHorizontalLines-1)) * verticalSpace;
            System.Diagnostics.Debug.WriteLine("TotalGridHeight:" + totalGridHeight+":"+ verticalSpace);

            //Horizontal Lines
            double y = verticalStartSpace;
            double startGridY = 0;
            for (int counter=0;counter<=numHorizontalLines;counter++)
            {
                //if (counter == numHorizontalLines - 1 && skipLastHorizontalLine)
                //    continue;

                Path path = new Path() { { "class", "horizontal-grid-lines" }, { "d", "M "+horizontalStartSpace.ToString()+" "+(boundHeight - y).ToString() + " L "+(boundWidth-horizontalEndSpace).ToString()+" "+(boundHeight - y).ToString() } };
                Text label = new Text() { { "class", "y-axis-labels" }, { "x", (horizontalStartSpace-2).ToString() }, { "y", (boundHeight - y).ToString() }, { "content", (startGridY).ToString() } };
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
               // Path linepath = new Path() { { "fill", "none" }, { "stroke", colors[colorcounter++] }, { "stroke-width", "1.0" }, { "d", chartLine } };
                Path linepath = new Path() { { "class", "line-"+(colorcounter+1).ToString() },{ "d", chartLine } };
                colorcounter++;
                svg.AddItems(linepath);

            }

            //Vertical Lines            
            double x = horizontalStartSpace;
            double startGridX = 0;
            for (int counter = 0; counter <= numVerticalLines; counter++)
            {
                //if (counter == numVerticalLines - 1 && skipLastVerticalLine)
                //    continue;

                Path path = new Path() { { "class", "vertical-grid-lines" }, { "d", "M " + x.ToString() +" "+ (boundHeight-verticalStartSpace).ToString() + " L "+ x.ToString() + " " +(verticalEndSpace).ToString() } };
                Text label = new Text() { { "class", "x-axis-labels" }, {"x",x.ToString() }, { "y", (boundHeight - verticalStartSpace + 5).ToString() }, { "content", (startGridX).ToString() } };
                startGridX = startGridX + gridXUnits;

                svg.AddItems(path,label);
                x = x + horizontalSpace;
            }
            
            BlazorRenderer blazorRenderer = new BlazorRenderer();
            blazorRenderer.Draw(seq, builder, svg);
            
            builder.OpenElement(++seq, "figcaption");
            builder.AddAttribute(++seq, "class", "key");
            builder.OpenElement(++seq, "ul");
            builder.AddAttribute(++seq, "class", "key-list");
            //builder.AddAttribute(++seq, "aria-hidden", "true");
            //builder.AddAttribute(++seq, "style", "list-style-type: none;");

            colorcounter = 0;
            foreach (string iData in inputDataArr)
            {
                //int data = int.Parse(dataStr);
                builder.OpenElement(++seq, "li");
                builder.OpenElement(++seq, "span");
                builder.AddAttribute(++seq, "class", "legend-"+(colorcounter+1).ToString());
                //builder.AddAttribute(++seq, "style", "background-color:" + colors[colorcounter]);

                builder.CloseElement();

                string label="";
                if (colorcounter<inputLabelsArr.Length)
                    label=inputLabelsArr[colorcounter];

                builder.AddContent(++seq, label);
                builder.CloseElement();
                colorcounter++;
            }

            builder.CloseElement();
            builder.CloseElement();
            

            builder.CloseElement();
            builder.CloseElement();



        }
    }
}
