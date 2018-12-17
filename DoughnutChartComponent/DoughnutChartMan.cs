using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoughnutChartComponent
{
    public class DoughnutChartMan :BlazorComponent
    {

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var seq = 0;
            /*
            builder.OpenElement(seq, "figure");
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "doughnut-main");

            builder.OpenElement(++seq, "svg");
            builder.AddAttribute(++seq, "width", "100%");
            builder.AddAttribute(++seq, "height", "100%");
            builder.AddAttribute(++seq, "viewBox", "0 0 42 42");

            builder.OpenElement(++seq, "rect");
            builder.AddAttribute(++seq, "width", "100%");
            builder.AddAttribute(++seq, "height", "100%");
            builder.AddAttribute(++seq, "fill", "cyan");
            builder.CloseElement();
            builder.CloseElement();
            builder.CloseElement();
            builder.CloseElement();
            */
            
            builder.OpenElement(seq, "figure");
            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", "doughnut-main");

            builder.OpenElement(++seq, "svg");
            builder.AddAttribute(++seq, "width", "100%");
            builder.AddAttribute(++seq, "height", "100%");
            builder.AddAttribute(++seq, "viewBox", "0 0 42 42");

            builder.OpenElement(++seq, "rect");
            builder.AddAttribute(++seq, "width", "100%");
            builder.AddAttribute(++seq, "height", "100%");
            builder.AddAttribute(++seq, "fill", "white");
            builder.CloseElement();

            builder.OpenElement(++seq, "circle");
            builder.AddAttribute(++seq, "class", "hole");
            builder.AddAttribute(++seq, "cx", "21");
            builder.AddAttribute(++seq, "cy", "21");
            builder.AddAttribute(++seq, "r", "15.91549430918954");
            builder.AddAttribute(++seq, "fill", "#fff");
            builder.CloseElement();

            builder.OpenElement(++seq, "circle");
            builder.AddAttribute(++seq, "cx", "21");
            builder.AddAttribute(++seq, "cy", "21");
            builder.AddAttribute(++seq, "r", "15.91549430918954");
            builder.AddAttribute(++seq, "fill", "transparent");
            builder.AddAttribute(++seq, "stroke", "#d2d3d4");
            builder.AddAttribute(++seq, "stroke-width", "3");
            builder.CloseElement();

            builder.OpenElement(++seq, "circle");
                builder.AddAttribute(++seq, "cx", "21");
                builder.AddAttribute(++seq, "cy", "21");
                builder.AddAttribute(++seq, "r", "15.91549430918954");
                builder.AddAttribute(++seq, "fill", "transparent");
                builder.AddAttribute(++seq, "stroke", "#ce4b99");
                builder.AddAttribute(++seq, "stroke-width", "5");
                builder.AddAttribute(++seq, "stroke-dasharray", "40 60");
                builder.AddAttribute(++seq, "stroke-dashoffset", "25");
                builder.OpenElement(++seq, "title");
                    builder.AddContent(++seq, "App Store");
                builder.CloseElement();
                builder.OpenElement(++seq, "desc");
                    builder.AddContent(++seq, "40% (40 out of 100)");
                builder.CloseElement();
            builder.CloseElement();

            builder.OpenElement(++seq, "circle");
            builder.AddAttribute(++seq, "cx", "21");
            builder.AddAttribute(++seq, "cy", "21");
            builder.AddAttribute(++seq, "r", "15.91549430918954");
            builder.AddAttribute(++seq, "fill", "transparent");
            builder.AddAttribute(++seq, "stroke", "#27A844");
            builder.AddAttribute(++seq, "stroke-width", "5");
            builder.AddAttribute(++seq, "stroke-dasharray", "20 80");
            builder.AddAttribute(++seq, "stroke-dashoffset", "85");
            builder.OpenElement(++seq, "title");
            builder.AddContent(++seq, "Website");
            builder.CloseElement();
            builder.OpenElement(++seq, "desc");
            builder.AddContent(++seq, "20% (20 out of 100)");
            builder.CloseElement();
            builder.CloseElement();

            builder.OpenElement(++seq, "circle");
            builder.AddAttribute(++seq, "cx", "21");
            builder.AddAttribute(++seq, "cy", "21");
            builder.AddAttribute(++seq, "r", "15.91549430918954");
            builder.AddAttribute(++seq, "fill", "transparent");
            builder.AddAttribute(++seq, "stroke", "#377bbc");
            builder.AddAttribute(++seq, "stroke-width", "5");
            builder.AddAttribute(++seq, "stroke-dasharray", "40 60");
            builder.AddAttribute(++seq, "stroke-dashoffset", "65");
            builder.OpenElement(++seq, "title");
            builder.AddContent(++seq, "Partners");
            builder.CloseElement();
            builder.OpenElement(++seq, "desc");
            builder.AddContent(++seq, "40% (40 out of 100)");
            builder.CloseElement();
            builder.CloseElement();

            builder.OpenElement(++seq, "g");
            builder.AddAttribute(++seq, "class", "doughnut-text");
            builder.OpenElement(++seq, "text");
                builder.AddAttribute(++seq, "x", "50%");
                builder.AddAttribute(++seq, "y", "50%");
                builder.AddAttribute(++seq, "class", "doughnut-number");
                builder.AddContent(++seq, "100");
            builder.CloseElement();
            builder.OpenElement(++seq, "text");
            builder.AddAttribute(++seq, "x", "50%");
            builder.AddAttribute(++seq, "y", "50%");
            builder.AddAttribute(++seq, "class", "doughnut-label");
            builder.AddContent(++seq, "Sales");
            builder.CloseElement();
            builder.CloseElement();

            builder.CloseElement();
            builder.CloseElement();

            builder.OpenElement(++seq, "figcaption");
            builder.AddAttribute(++seq, "class", "doughnut-key");
            builder.OpenElement(++seq, "ul");
            builder.AddAttribute(++seq, "class", "doughnut-key-list");
            builder.AddAttribute(++seq, "aria-hidden", "true");
            builder.AddAttribute(++seq, "style", "list-style-type: none;");

            builder.OpenElement(++seq, "li");
            builder.OpenElement(++seq, "span");
            builder.AddAttribute(++seq, "class", "round-dot dot-red");
            builder.CloseElement();
            builder.AddContent(++seq, "App Store (40)");
            builder.CloseElement();

            builder.OpenElement(++seq, "li");
            builder.OpenElement(++seq, "span");
            builder.AddAttribute(++seq, "class", "round-dot dot-green");
            builder.CloseElement();
            builder.AddContent(++seq, "Website (20)");
            builder.CloseElement();

            builder.OpenElement(++seq, "li");
            builder.OpenElement(++seq, "span");
            builder.AddAttribute(++seq, "class", "round-dot dot-blue");
            builder.CloseElement();
            builder.AddContent(++seq, "Partners (40)");
            builder.CloseElement();

            builder.CloseElement();
            builder.CloseElement();

            builder.CloseElement();
            

        }
    }
}
