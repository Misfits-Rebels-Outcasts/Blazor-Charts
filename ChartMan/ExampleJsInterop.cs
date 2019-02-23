using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ChartMan
{
    public class ExampleJsInterop
    {
        public static Task<string> Prompt(string message)
        {
            // Implemented in exampleJsInterop.js
            return JSRuntime.Current.InvokeAsync<string>(
                "exampleJsFunctions.showPrompt",
                message);

        }
        public static Task<string> Display(string message)
        {
            return JSRuntime.Current.InvokeAsync<string>(
                "JsFunctions.printWorld",
                message);

        }
   
    }
}
