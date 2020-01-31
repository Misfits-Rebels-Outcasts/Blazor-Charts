using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebAssemblyMan.SVGRender;

namespace WebAssemblyMan.SVGRender
{
  public class BlazorRenderer
  {
        private string content = null;
        public void Draw(int k, RenderTreeBuilder builder, SVG svg)
        {
            builder.OpenElement(++k, svg.type);
            
            foreach (string attribute in svg.GetAttributes())
            {
                string[] splitContent = attribute.Split('=');
                builder.AddAttribute(++k, splitContent[0], splitContent[1]);
            }
            
            foreach (SVG child in svg.GetChildren())
            {
                if (child.type == "g")
                    Draw(k, builder, child);
                else
                {
                    builder.OpenElement(++k, child.type);
                    foreach (string attribute in child.GetAttributes())
                    {
                        string[] splitContent = attribute.Split('=');
                        if (splitContent[0] == "content")
                        {
                            content = splitContent[1];
                        }
                        else
                            builder.AddAttribute(++k, splitContent[0], splitContent[1]);
                    }
                    if (content != null)
                    {
                        builder.AddContent(++k, content);
                        content = null;
                    }
                    builder.CloseElement();
                }
            }
            builder.CloseElement();
        }


    }
}
