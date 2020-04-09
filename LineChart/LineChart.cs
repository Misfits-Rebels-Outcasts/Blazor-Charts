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
        public string InputSeries1 { get; set; }
        [Parameter]
        public string InputSeries2 { get; set; }
        [Parameter]
        public string InputSeries3 { get; set; }
        [Parameter]
        public string InputSeries4 { get; set; }
        [Parameter]
        public string InputSeries5 { get; set; }
        [Parameter]
        public string InputSeries6 { get; set; }
        [Parameter]
        public string InputSeries7 { get; set; }
        [Parameter]
        public string InputSeries8 { get; set; }
        [Parameter]
        public string InputSeries9 { get; set; }
        [Parameter]
        public string InputSeries10 { get; set; }

        [Parameter]
        public string InputLabels { get; set; }
        [Parameter]
        public string XAxisLabels { get; set; }

        //1. xaxis text labels dx dy
        //2. expose gridyunits
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var seq = 0;
            builder.OpenElement(seq, "figure");
            builder.AddAttribute(++seq, "class", "line-chart");
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "main");

            string[] inputLabelsArr = InputLabels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] xAxisLabelsArr = XAxisLabels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int numLines = 0;
            if (InputSeries1!=null) numLines++;
            if (InputSeries2!=null) numLines++;
            if (InputSeries3!=null) numLines++;
            if (InputSeries4!=null) numLines++;
            if (InputSeries5!=null) numLines++;
            if (InputSeries6!=null) numLines++;
            if (InputSeries7!=null) numLines++;
            if (InputSeries8!=null) numLines++;
            if (InputSeries9!=null) numLines++;
            if (InputSeries10!=null) numLines++;

            string[] inputDataArr=new string[numLines];
            if (InputSeries1!=null) inputDataArr[0] = InputSeries1;
            if (InputSeries2!=null) inputDataArr[1] = InputSeries2;
            if (InputSeries3!=null) inputDataArr[2] = InputSeries3;
            if (InputSeries4!=null) inputDataArr[3] = InputSeries4;
            if (InputSeries5!=null) inputDataArr[4] = InputSeries5;
            if (InputSeries6!=null) inputDataArr[5] = InputSeries6;
            if (InputSeries7!=null) inputDataArr[6] = InputSeries7;
            if (InputSeries8!=null) inputDataArr[7] = InputSeries8;
            if (InputSeries9!=null) inputDataArr[8] = InputSeries9;
            if (InputSeries10!=null) inputDataArr[9] = InputSeries10;

            double maxY=0.0;
            int numValues=0;
            int numXLabels=xAxisLabelsArr.Length;
            foreach (string iData in inputDataArr)
            {
                string[] inputLineArr = iData.Split(',');
                double[] doubleAry=new double[inputLineArr.Length];
                if (numValues<inputLineArr.Length)
                    numValues=inputLineArr.Length;
                for (int i = 0; i < inputLineArr.Length; i++)
                {
                    double data = 0;
                    bool isDouble2=double.TryParse(inputLineArr[i],out data);
                    doubleAry[i]=data;
                    if (maxY<data)
                        maxY=data;
                }

            }

            double boundHeight = 150.0;
            double boundWidth = 150.0;

            SVG svg = new SVG() { { "width", "100%" }, { "height", "100%" }, { "viewBox", "0 0 150 150" } };
            Rectangle rect = new Rectangle() { { "class", "background-rect" }};
            svg.AddItems(rect);

            /*
            int numHorizontalLines = 10;
            int numVerticalLines = 10;
            */

            double gridYUnits = 10;
            double gridXUnits = 10; //not required

            //1. Determine number of input values in xaxis and use it for numVerticalLines
            int numVerticalLines = numValues;
            
            //2. Detemine max value in yaxis and then use it calculate numHorizontalLines
            int numHorizontalLines = ((int) (maxY / gridYUnits))+1;

            double verticalStartSpace = 25.0;
            double horizontalStartSpace = 25.0;
            double verticalEndSpace = 25.0;
            double horizontalEndSpace = 25.0;

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
                Path path = new Path() { { "class", "horizontal-grid-lines" }, { "d", "M "+horizontalStartSpace.ToString()+" "+(boundHeight - y).ToString() + " L "+(boundWidth-horizontalEndSpace).ToString()+" "+(boundHeight - y).ToString() } };
                Text label = new Text() { { "class", "y-axis-labels" }, { "x", (horizontalStartSpace-2).ToString() }, { "y", (boundHeight - y).ToString() }, { "content", (startGridY).ToString() } };

                svg.AddItems(path,label);
                //System.Diagnostics.Debug.WriteLine("Y:" + y);
                y = y + verticalSpace;
                startGridY = startGridY + gridYUnits;
                //note : gridYUnits is the value the user see
                //verticalSpace is the internal/actual value used to represent gridYUnits on the chart.
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
                double[] intAry=new double[inputLineArr.Length];
                for (int i = 0; i < inputLineArr.Length; i++)
                {
                    double data = 0;
                    bool isDouble2=double.TryParse(inputLineArr[i],out data);
                    intAry[i]=data;
                }


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
                        //if 5 verticalSapce represents 10 gridYUnits
                        //when you have 10 it becomes 10*5/10=5
                        double gridValue = ((double)i) * verticalSpace / gridYUnits;
                        gridValueY = boundHeight - (gridValueY + gridValue);                        
                        chartLine = chartLine + gridValueX.ToString() + " " + gridValueY.ToString();
                    }
                }
                Path linepath = new Path() { { "class", "line-"+(colorcounter+1).ToString() },{ "d", chartLine } };
                colorcounter++;
                svg.AddItems(linepath);

            }

            //Vertical Lines            
            double x = horizontalStartSpace;
            double startGridX = 0;
            int xLabelsCounter=0;

            for (int counter = 0; counter <= numVerticalLines; counter++)
            {

                Path path = new Path() { { "class", "vertical-grid-lines" }, { "d", "M " + x.ToString() +" "+ (boundHeight-verticalStartSpace).ToString() + " L "+ x.ToString() + " " +(verticalEndSpace).ToString() } };

                string xLabels="";
                if (xLabelsCounter<numXLabels)
                    xLabels=xAxisLabelsArr[xLabelsCounter++];

                Text label = new Text() { { "class", "x-axis-labels" }, {"transform", "translate("+x.ToString()+","+(boundHeight - verticalStartSpace + 5).ToString()+")" },{"dx","+1em"},{"dy","0.30em"}, { "content", xLabels } };

                //not required. just need number of grid lines
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

            colorcounter = 0;
            foreach (string iData in inputDataArr)
            {
                builder.OpenElement(++seq, "li");
                builder.OpenElement(++seq, "span");
                builder.AddAttribute(++seq, "class", "legend-"+(colorcounter+1).ToString());

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
