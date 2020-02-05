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
    public class DonutChart : ComponentBase
    {
        [Parameter]
        public string InputData { get; set; }

        /*
        bool firstTime = true;
        protected async override Task OnAfterRenderAsync()
        {
            //System.Diagnostics.Debug.WriteLine("OnAfterRenderAsync");
            //string hello = await ChartMan.ExampleJsInterop.Display("hello");
            //System.Diagnostics.Debug.WriteLine("::" + hello);
 
        }
        */

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {

            var seq = 0;
            builder.OpenElement(seq, "figure");
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "doughnut-main");

            SVG svg = new SVG() { { "width", "100%" }, { "height", "100%" }, { "viewBox", "0 0 42 42" } };
            Rectangle rect = new Rectangle() { { "width", "100%" }, { "height", "100%" }, { "fill", "white" } };
            Circle hole = new Circle() { { "cx", "21" }, { "cy", "21" }, { "r", "15.915" }, { "fill", "#fff" } };
            Circle ring = new Circle() { { "cx", "21" }, { "cy", "21" }, { "r", "15.915" }, { "fill", "transparent" }, { "stroke-width", "3" }, { "stroke", "#d2d3d4" }};


            int[] inputData = { 30, 30, 40 };
            //string[] colors = { "#ce4b99", "#27A844", "#377bbc" };
            //string[] colors = { "#ff0000", "#ffff00", "#0000ff", "#fccc1a", "#fefe33", "#b2d732", "#66b032", "#347c98", "#0247fe", "#4424d6","#8601af","#c21460" };
            //string[] colors = { "#fe2712", "#fc600a", "#fb9902","#fccc1a", "#fefe33", "#b2d732", "#66b032", "#347c98", "#0247fe", "#4424d6","#8601af","#c21460" };
            string[] colors = { "#ce4b99", "#27A844", "#377bbc","#fe2712", "#fc600a", "#fb9902","#fccc1a", "#fefe33", "#b2d732", "#66b032", "#347c98", "#0247fe", "#4424d6","#8601af","#c21460" };
            
            //string[] colors = {"#fe2712", "#fefe33","#0247fe","#fc600a","#b2d732","#4424d6","#fb9902","#66b032","#8601af","#fccc1a","#347c98","#c21460"};
            string[] labels = { "App Store", "Website", "Partners", "App Store", "Website", "Partners", "App Store", "Website", "Partners", "App Store", "Website", "Partners" };

            double counterClockwiseDefaultOffset = 25;
            double preceedingTotalPercent = 0;
            double offset = counterClockwiseDefaultOffset;
            List<Circle> segments = new List<Circle>();
            int colorCounter = 0;
            string[] inputDataArr = InputData.Split(',');
            foreach (string dataStr in inputDataArr)
            {
                double data = double.Parse(dataStr);
                double percent = data;
                double reversePercent = 100 - percent;
                offset = 100 - preceedingTotalPercent + counterClockwiseDefaultOffset;
                preceedingTotalPercent = preceedingTotalPercent + percent;
                Circle segment = new Circle() { { "cx", "21" }, { "cy", "21" }, { "r", "15.915" }, { "fill", "transparent" }, { "stroke-width", "5" }, { "stroke", colors[colorCounter++] }, { "stroke-dasharray", percent + " " + reversePercent }, { "stroke-dashoffset", offset.ToString() } };
                segments.Add(segment);
            }


            Text numberText = new Text() { { "x", "50%" }, { "y", "50%" }, { "class", "doughnut-number" }, { "content", "100" } };
            Text labelText = new Text() { { "x", "50%" }, { "y", "50%" }, { "id", "dcolor" }, { "class", "doughnut-label" }, { "content", "Sales" } };
            Group grp = new Group() { { "class", "doughnut-text" } };
            grp.AddItems(numberText,labelText);

            svg.AddItems(rect, hole, ring);
            foreach (Circle segment in segments)
                svg.AddItems(segment);
            svg.AddItems(grp);

            BlazorRenderer blazorRenderer = new BlazorRenderer();
            blazorRenderer.Draw(seq, builder, svg);
            
            builder.OpenElement(++seq, "figcaption");
            builder.AddAttribute(++seq, "class", "doughnut-key");
            builder.OpenElement(++seq, "ul");
            builder.AddAttribute(++seq, "class", "doughnut-key-list");
            builder.AddAttribute(++seq, "aria-hidden", "true");
            builder.AddAttribute(++seq, "style", "list-style-type: none;");

            int counter = 0;
            foreach (string dataStr in inputDataArr)
            {
                double data = double.Parse(dataStr);
                builder.OpenElement(++seq, "li");
                builder.OpenElement(++seq, "span");
                builder.AddAttribute(++seq, "class", "round-dot");
                builder.AddAttribute(++seq, "style", "background-color:" + colors[counter]);

                builder.CloseElement();
                builder.AddContent(++seq, labels[counter++]+" "+"("+data.ToString()+")");
                builder.CloseElement();
            }
            builder.CloseElement();
            builder.CloseElement();
            

            builder.CloseElement();
            builder.CloseElement();

   

        }
    }
}
