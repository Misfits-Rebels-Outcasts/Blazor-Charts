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
    public class VerticalBarChart : ComponentBase
    {
        [Parameter]
        public string InputData { get; set; }
        [Parameter]
        public string InputLabels { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var seq = 0;
            builder.OpenElement(seq, "figure");
            builder.AddAttribute(++seq, "class", "vertical-bar-chart");
            builder.OpenElement(++seq, "div");

            string[] inputDataArr = InputData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] inputLabelsArr = InputLabels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


            double boundHeight = 150.0;
            double boundWidth = 150.0;

            SVG svg = new SVG() { { "width", "100%" }, { "height", "100%" }, { "viewBox", "0 0 150 150" } };
            Rectangle rect = new Rectangle() { { "class", "background-rect" } };
            svg.AddItems(rect);

            double[] dAry = new double[inputDataArr.Length];
            int i = 0;
            double max=0.0;
            foreach (string iData in inputDataArr)
            {
                double data = 0;
                bool isDouble2=double.TryParse(iData,out data);
                dAry[i++] = data;
                if (max<data)
                    max=data;
            }
            double gridYUnits = 10;
            double gridXUnits = 10;

            //1. Determine number of input data values and use it for numVerticalLines
            int numVerticalLines = i;
            
            //2. Detemine max bar value and then use it calculate numHorizontalLines
            int numHorizontalLines = (int) (max / gridYUnits);

            //int numHorizontalLines = 10;
            //int numVerticalLines = 10;
            double verticalStartSpace = 25.0;
            double horizontalStartSpace = 25.0;
            double verticalEndSpace = 25.0;
            double horizontalEndSpace = 25.0;

            double verticalSpace = (boundHeight- verticalStartSpace-verticalEndSpace) / (numHorizontalLines);
            double horizontalSpace = (boundWidth - horizontalStartSpace-horizontalEndSpace) / (numVerticalLines);

            double totalGridWidth = ((double)(numVerticalLines-1)) * horizontalSpace;
            double totalGridHeight = ((double)(numHorizontalLines-1)) * verticalSpace;

            //Horizontal Lines
            double y = verticalStartSpace;
            double startGridY = 0;
            for (int counter = 0; counter <= numHorizontalLines; counter++)
            {

                Path path = new Path() { { "class", "horizontal-grid-lines"}, { "d", "M " + horizontalStartSpace.ToString() + " " + (boundHeight - y).ToString() + " L " + (boundWidth - horizontalEndSpace).ToString() + " " + (boundHeight - y).ToString() } };
                Text label = new Text() { { "class", "y-axis-labels" }, { "x", (horizontalStartSpace - 2).ToString() }, { "y", (boundHeight - y).ToString() }, { "content", (startGridY).ToString() } };
                svg.AddItems(path, label);

                y = y + verticalSpace;
                startGridY = startGridY + gridYUnits;
            }

            //Vertical Lines            
            double x = horizontalStartSpace;
            double startGridX = 0;
            i = 0;
            for (int counter = 0; counter < numVerticalLines; counter++)
            {
                string xLabels="";
                if (counter<inputLabelsArr.Length)
                    xLabels=inputLabelsArr[counter];

                Text label = new Text() { { "class", "x-axis-labels" }, {"transform", "translate("+x.ToString()+","+(boundHeight - verticalStartSpace + 5).ToString()+") rotate(-40)" },{"dx","+1em"},{"dy","0.30em"}, { "content", xLabels } };
                startGridX = startGridX + gridXUnits;
                if (i < (inputDataArr.Length))
                    {
                        Rectangle rectangle = new Rectangle() {{ "class", "bar" },  { "x", (x).ToString() }, { "y", (boundHeight-verticalStartSpace- (dAry[i]/max)*numHorizontalLines*verticalSpace).ToString() }, { "height", (dAry[i]/max)*numHorizontalLines*verticalSpace + "px" } };
                        svg.AddItems(label, rectangle);
                        i++;
                    }
                    else
                    {
                        svg.AddItems(label);
                    }

                x = x + horizontalSpace;
            }
            
            BlazorRenderer blazorRenderer = new BlazorRenderer();
            blazorRenderer.Draw(seq, builder, svg);
            builder.CloseElement();
            builder.CloseElement();
            


        }
    }
}
