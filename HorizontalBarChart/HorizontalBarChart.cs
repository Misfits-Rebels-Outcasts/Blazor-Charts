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

            string[] inputDataArr = InputData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] inputLabelsArr = InputLabels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            double boundHeight = 150.0;
            double boundWidth = 150.0;

            SVG svg = new SVG() { { "width", "100%" }, { "height", "100%" }, { "viewBox", "0 0 150 150" }};
            Rectangle rect = new Rectangle() { { "class", "background-rect" }};
            svg.AddItems(rect);

            double[] inputDataArrDouble = new double[inputDataArr.Length];
            int i = 0;
            double max=0.0;
            foreach (string iData in inputDataArr)
            {
                double data = 0;
                bool isDouble2=double.TryParse(iData,out data);
                inputDataArrDouble[i++] = data;               
                if (max<data)
                    max=data;
            }
            double gridYUnits = 10;
            double gridXUnits = 10;

            //1. Determine number of input data values and use it for numHorizontalLines
            int numHorizontalLines = i;
            
            //2. Detemine max bar value and then use it calculate numVerticalLines
            int numVerticalLines = (int) (max / gridXUnits);

            //int numHorizontalLines = 10;
            //int numVerticalLines = 10;

            double verticalStartSpace = 25.0;
            double horizontalStartSpace = 30.0;
            double verticalEndSpace = 25.0;
            double horizontalEndSpace = 20.0;
            bool skipLastVerticalLine = false;
            bool skipLastHorizontalLine = false;

            double verticalSpace = (boundHeight- verticalStartSpace-verticalEndSpace) / (numHorizontalLines);
            double horizontalSpace = (boundWidth - horizontalStartSpace-horizontalEndSpace) / (numVerticalLines);

            double totalGridWidth = ((double)(numVerticalLines)) * horizontalSpace;
            double totalGridHeight = ((double)(numHorizontalLines)) * verticalSpace;


            //Vertical Lines            
            double x = horizontalStartSpace;
            double startGridX = 0;
            for (int counter = 0; counter <= numVerticalLines; counter++)
            {
                
                if (counter == numVerticalLines && skipLastVerticalLine)
                    continue;
                
                Path path = new Path() { { "class", "vertical-grid-lines" }, { "d", "M " + x.ToString() +" "+ (boundHeight-verticalStartSpace).ToString() + " L "+ x.ToString() + " " +(verticalEndSpace).ToString() } };
                Text label = new Text() { { "class", "x-axis-labels" }, {"x",x.ToString() }, { "y", (boundHeight - verticalStartSpace + 5).ToString() },{ "content", (startGridX).ToString() } };
                
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
                if (counter == numHorizontalLines  && skipLastHorizontalLine)
                {
                    continue;
                }

                Path path = new Path() { { "class", "horizontal-grid-lines"}, { "d", "M "+(horizontalStartSpace).ToString()+" "+(boundHeight - y).ToString() + " L "+(horizontalStartSpace+numVerticalLines*horizontalSpace).ToString()+" "+(boundHeight - y).ToString() } };                
                string xLabels="";
                if (counter<inputLabelsArr.Length)
                    xLabels=inputLabelsArr[counter];
                Text label = new Text() { { "class", "y-axis-labels" }, { "x", (horizontalStartSpace-2).ToString() }, { "y", (boundHeight - y).ToString() }, { "content", xLabels } };
                
                if (counter==0)
                    svg.AddItems(path,label);
                if (i< (inputDataArr.Length))
                {
                    Rectangle bar = new Rectangle() { { "class", "bar" }, { "x", (horizontalStartSpace).ToString() }, { "y", (boundHeight - y - 5).ToString() }, { "width", (inputDataArrDouble[i]/max)*numVerticalLines*horizontalSpace + "px" }, { "height", "5px" } };
                    svg.AddItems(label, bar);
                    i++;
                }
                else
                {
                    if (counter<numHorizontalLines)
                        svg.AddItems(label);
                }

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
