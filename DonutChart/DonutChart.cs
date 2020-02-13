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
    //TODO
    /*
    1. Total as Label Number
    2. Input for Label Text
    3. CSS Consistency for key and keylist in donut and pie chart
    4. 
    */
    public class DonutChart : ComponentBase
    {
        [Parameter]
        public string InputData { get; set; }

        [Parameter]
        public string InputLabels { get; set; }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {

            var seq = 0;
            builder.OpenElement(seq, "figure");
            builder.AddAttribute(++seq, "class", "donut-chart");
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "main");

            SVG svg = new SVG() { { "width", "80%" }, { "height", "80%" }, { "viewBox", "0 0 42 42" } };
            Rectangle rect = new Rectangle() { { "class", "background-rect" }, { "width", "100%" }, { "height", "100%" }, { "fill", "white" } };
            /*
            Circle hole = new Circle() { { "class", "hole" }, { "cx", "21" }, { "cy", "21" }, { "r", "15.915" }, { "fill", "#fff" } };
            Circle ring = new Circle() { { "class", "ring" }, { "cx", "21" }, { "cy", "21" }, { "r", "15.915" }, { "fill", "transparent" }, { "stroke-width", "3" }, { "stroke", "#d2d3d4" }};
            */
            Circle hole = new Circle() { { "class", "hole" }, { "cx", "21" }, { "cy", "21" }, { "r", "15.915" } };
            Circle ring = new Circle() { { "class", "ring" }, { "cx", "21" }, { "cy", "21" }, { "r", "15.915" }};

            int[] inputData = { 30, 30, 40 };
            //string[] colors = { "#ce4b99", "#27A844", "#377bbc" };
            //string[] colors = { "#ff0000", "#ffff00", "#0000ff", "#fccc1a", "#fefe33", "#b2d732", "#66b032", "#347c98", "#0247fe", "#4424d6","#8601af","#c21460" };
            //string[] colors = { "#fe2712", "#fc600a", "#fb9902","#fccc1a", "#fefe33", "#b2d732", "#66b032", "#347c98", "#0247fe", "#4424d6","#8601af","#c21460" };
            string[] colors = { "#ce4b99", "#27A844", "#377bbc","#fe2712", "#fc600a", "#fb9902","#fccc1a", "#fefe33", "#b2d732", "#66b032", "#347c98", "#0247fe", "#4424d6","#8601af","#c21460" };
            
            //string[] colors = {"#fe2712", "#fefe33","#0247fe","#fc600a","#b2d732","#4424d6","#fb9902","#66b032","#8601af","#fccc1a","#347c98","#c21460"};
            //string[] labels = { "App Store", "Website", "Partners", "App Store", "Website", "Partners", "App Store", "Website", "Partners", "App Store", "Website", "Partners" };

            double counterClockwiseDefaultOffset = 25;
            double preceedingTotalPercent = 0;
            double offset = counterClockwiseDefaultOffset;
            List<Circle> segments = new List<Circle>();
            //int colorCounter = 0;
            string[] inputDataArr = InputData.Split(',');
            string[] inputLabelsArr = InputLabels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int counter = 0;            
            foreach (string dataStr in inputDataArr)
            {
                double data = double.Parse(dataStr);
                double percent = data;
                double reversePercent = 100 - percent;
                offset = 100 - preceedingTotalPercent + counterClockwiseDefaultOffset;
                preceedingTotalPercent = preceedingTotalPercent + percent;
                //Circle segment = new Circle() { {"class", "segment-"+(counter+1).ToString()}, { "cx", "21" }, { "cy", "21" }, { "r", "15.915" }, { "fill", "transparent" }, { "stroke-width", "5" }, { "stroke", colors[colorCounter++] }, { "stroke-dasharray", percent + " " + reversePercent }, { "stroke-dashoffset", offset.ToString() } };
                Circle segment = new Circle() { {"class", "segment-"+(1+counter++).ToString()}, { "cx", "21" }, { "cy", "21" }, { "r", "15.915" }, { "stroke-dasharray", percent + " " + reversePercent }, { "stroke-dashoffset", offset.ToString() } };
                segments.Add(segment);
            }

            Text numberText = new Text() { { "x", "50%" }, { "y", "50%" }, { "class", "donut-number" }, { "content", "100" } };
            //Text labelText = new Text() { { "x", "50%" }, { "y", "50%" }, { "id", "dcolor" }, { "class", "donut-label" }, { "content", "Sales" } };
            Text labelText = new Text() { { "x", "50%" }, { "y", "50%" }, { "class", "donut-label" }, { "content", "Sales" } };
            Group grp = new Group() { { "class", "donut-text" } };
            grp.AddItems(numberText,labelText);

            svg.AddItems(rect, hole, ring);
            
            foreach (Circle segment in segments)
            {
                svg.AddItems(segment);
            }
            svg.AddItems(grp);
            
            BlazorRenderer blazorRenderer = new BlazorRenderer();
            blazorRenderer.Draw(seq, builder, svg);
            
            builder.OpenElement(++seq, "figcaption");
            builder.AddAttribute(++seq, "class", "donut-key");
            builder.OpenElement(++seq, "ul");
            builder.AddAttribute(++seq, "class", "donut-key-list");
            builder.AddAttribute(++seq, "aria-hidden", "true");
            builder.AddAttribute(++seq, "style", "list-style-type: none;");

            counter = 0;
            foreach (string dataStr in inputDataArr)
            {
                double data = double.Parse(dataStr);
                builder.OpenElement(++seq, "li");
                builder.OpenElement(++seq, "span");
                builder.AddAttribute(++seq, "class", "legend-dot-"+(counter+1).ToString());
                //builder.AddAttribute(++seq, "style", "background-color:" + colors[counter]);

                builder.CloseElement();

                string labels="";
                if (counter<inputLabelsArr.Length)
                {
                    labels=inputLabelsArr[counter];
                    counter++;
                }

                builder.AddContent(++seq, labels+" "+"("+data.ToString()+")");
                builder.CloseElement();
            }
            builder.CloseElement();
            builder.CloseElement();
            

            builder.CloseElement();
            builder.CloseElement();

   

        }
    }
}
