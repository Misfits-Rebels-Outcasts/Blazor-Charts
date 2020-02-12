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
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var seq = 0;
            builder.OpenElement(seq, "figure");
            builder.AddAttribute(++seq, "class", "vertical-bar-chart");
            builder.OpenElement(++seq, "div");
            //builder.AddAttribute(++seq, "class", "main");

            System.Diagnostics.Debug.WriteLine("ID"+InputData);

            string[] inputDataArr = InputData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            /*
            int[] inputData = { 30, 70, 42, 50, 3, 55, 35, 22 };
            int[] list1 = new int[8] { 30, 70, 42, 50, 3, 55, 35, 22 };
            int[] list2 = new int[8] { 40, 50, 32, 70, 55, 15, 15, 12 };
            int[] list3 = new int[8] { 0, 10, 10, 10, 10, 20, 70, 70 };
            int[][] lists = new int[][] { list1, list2, list3};
            */
            string[] colors = { "#fe2712", "#fc600a", "#fb9902", "#fccc1a", "#fefe33", "#b2d732", "#66b032", "#347c98", "#0247fe", "#4424d6", "#8601af", "#c21460" };

            //string[] labels = { "App Store", "Website", "Partners", "App Store", "Website", "Partners", "App Store", "Website", "Partners", "App Store", "Website", "Partners" };
            string[] labels = { "App Store", "Website", "Partners", "Direct", "Channels", "Retail", "Distributors", "Affiliates", "Phone", "TV" ,"X"};

            double boundHeight = 110.0;
            double boundWidth = 120.0;

            SVG svg = new SVG() { { "width", "100%" }, { "height", "100%" }, { "viewBox", "0 0 120 110" } };
            //Rectangle rect = new Rectangle() { { "class", "background-rect" }, { "width", "100%" }, { "height", "100%" }, { "fill", "white" }, { "stroke", "gray" }, {"stroke-width", "0.5" } };
            Rectangle rect = new Rectangle() { { "class", "background-rect" } };
            svg.AddItems(rect);
            
            int numHorizontalLines = 10;
            int numVerticalLines = 10;
            double verticalStartSpace = 20.0;
            double horizontalStartSpace = 20.0;
            double verticalEndSpace = 5.0;
            double horizontalEndSpace = 5.0;
            double gridYUnits = 10;
            double gridXUnits = 10;
            bool skipLastVerticalLine = true;
            bool skipLastHorizontalLine = true;

            double verticalSpace = (boundHeight- verticalStartSpace-verticalEndSpace) / (numHorizontalLines);
            double horizontalSpace = (boundWidth - horizontalStartSpace-horizontalEndSpace) / (numVerticalLines);

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
            for (int counter = 0; counter <= numHorizontalLines; counter++)
            {
                //if (counter == numHorizontalLines - 1 && skipLastHorizontalLine)
                //    continue;

                Path path = new Path() { { "class", "horizontal-grid-lines"}, { "d", "M " + horizontalStartSpace.ToString() + " " + (boundHeight - y).ToString() + " L " + (boundWidth - horizontalEndSpace).ToString() + " " + (boundHeight - y).ToString() } };
                Text label = new Text() { { "class", "x-axis-labels" }, { "x", (horizontalStartSpace - 2).ToString() }, { "y", (boundHeight - y).ToString() }, { "content", (startGridY).ToString() } };
                svg.AddItems(path, label);
                System.Diagnostics.Debug.WriteLine("Y:" + y);

                y = y + verticalSpace;
                startGridY = startGridY + gridYUnits;
            }
            System.Diagnostics.Debug.WriteLine("Vertical Lines");

            //Vertical Lines            
            double x = horizontalStartSpace;
            double startGridX = 0;
            i = 0;
            for (int counter = 0; counter < numVerticalLines; counter++)
            {
                //if (counter == numVerticalLines - 1 && skipLastVerticalLine)
                //    continue;

                //Path path = new Path() { { "fill", "none" }, { "stroke", "gray" }, { "stroke-width", "0.2" }, { "d", "M " + x.ToString() +" "+ (boundHeight-verticalStartSpace).ToString() + " L "+ x.ToString() + " " +(verticalEndSpace).ToString() } };
                //Text label = new Text() { {"x",x.ToString() }, { "y", (boundHeight - verticalStartSpace + 5).ToString() },{ "font-size", "4px" }, { "text-anchor", "middle" }, { "content", (startGridX).ToString() } };
                Text label = new Text() { {"transform", "translate("+x.ToString()+","+(boundHeight - verticalStartSpace + 5).ToString()+") rotate(-40)" },{"dx","+1em"},{"dy","0.30em"},{ "font-size", "4px" }, { "text-anchor", "end" }, { "content", labels[counter] } };
                startGridX = startGridX + gridXUnits;
                if (i < (inputDataArr.Length))
                    {
                        System.Diagnostics.Debug.WriteLine("i:" + i + ":" + dAry[i].ToString() + "px");
                        System.Diagnostics.Debug.WriteLine("labelrect");
                        Rectangle rectangle = new Rectangle() { { "fill", "#27A844" }, { "x", (x).ToString() }, { "y", (boundHeight-verticalStartSpace- dAry[i]).ToString() }, { "width", "5px" }, { "height", dAry[i].ToString() + "px" } };
                        //svg.AddItems(label, rectangle,path);
                        svg.AddItems(label, rectangle);
                        i++;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("label");
                        //svg.AddItems(label,path);
                        svg.AddItems(label);
                    }

                /*
                if (counter==0)
                    svg.AddItems(label);
                    //svg.AddItems(path,label);
                else
                {

                    if (i < (inputDataArr.Length))
                    {
                        System.Diagnostics.Debug.WriteLine("i:" + i + ":" + dAry[i].ToString() + "px");
                        System.Diagnostics.Debug.WriteLine("labelrect");
                        Rectangle rectangle = new Rectangle() { { "fill", "#27A844" }, { "x", (x-3).ToString() }, { "y", (boundHeight-verticalStartSpace- dAry[i]).ToString() }, { "width", "5px" }, { "height", dAry[i].ToString() + "px" } };
                        //svg.AddItems(label, rectangle,path);
                        svg.AddItems(label, rectangle);
                        i++;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("label");
                        //svg.AddItems(label,path);
                        svg.AddItems(label);
                    }
                }
                */
                x = x + horizontalSpace;
            }
            


            BlazorRenderer blazorRenderer = new BlazorRenderer();
            blazorRenderer.Draw(seq, builder, svg);
            /*
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
                      */  

            builder.CloseElement();
            builder.CloseElement();
            


        }
    }
}
