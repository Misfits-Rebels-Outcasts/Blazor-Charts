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

        [Parameter]
        public string InputLabels { get; set; }

        [Parameter]
        public string PrimaryText { get; set; }

        [Parameter]
        public string SecondaryText { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {

            var seq = 0;
            builder.OpenElement(seq, "figure");
            builder.AddAttribute(++seq, "class", "donut-chart");
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "main");

            SVG svg = new SVG() { { "width", "80%" }, { "height", "80%" }, { "viewBox", "0 0 42 42" } };
            Rectangle rect = new Rectangle() { { "class", "background-rect" }};
            Circle hole = new Circle() { { "class", "hole" }, { "cx", "21" }, { "cy", "21" }, { "r", "15.915" } };
            Circle ring = new Circle() { { "class", "ring" }, { "cx", "21" }, { "cy", "21" }, { "r", "15.915" }};
            
            double counterClockwiseDefaultOffset = 25;
            double preceedingTotalPercent = 0;
            double offset = counterClockwiseDefaultOffset;
            List<Circle> segments = new List<Circle>();
            string[] inputDataArr = InputData.Split(',');
            string[] inputLabelsArr = InputLabels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int counter = 0;            
            foreach (string dataStr in inputDataArr)
            {
                double data = 0;
                bool isDouble2=double.TryParse(dataStr,out data);

                double percent = data;
                double reversePercent = 100 - percent;
                offset = 100 - preceedingTotalPercent + counterClockwiseDefaultOffset;
                preceedingTotalPercent = preceedingTotalPercent + percent;
                Circle segment = new Circle() { {"class", "segment-"+(1+counter++).ToString()}, { "cx", "21" }, { "cy", "21" }, { "r", "15.915" }, { "stroke-dasharray", percent + " " + reversePercent }, { "stroke-dashoffset", offset.ToString() } };
                segments.Add(segment);
            }

            Text numberText = new Text() { { "class", "donut-number" }, { "content", PrimaryText } };
            Text labelText = new Text() { { "class", "donut-label" }, { "content", SecondaryText } };

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

                builder.CloseElement();

                string labels="";
                if (counter<inputLabelsArr.Length)
                {
                    labels=inputLabelsArr[counter];
                    counter++;
                }

                builder.AddContent(++seq, labels+" "+"("+data.ToString()+"%)");
                builder.CloseElement();
            }
            builder.CloseElement();
            builder.CloseElement();
            

            builder.CloseElement();
            builder.CloseElement();

   

        }
    }
}
