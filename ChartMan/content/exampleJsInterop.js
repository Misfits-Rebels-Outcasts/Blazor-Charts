// This file is to show how a library package may provide JavaScript interop features
// wrapped in a .NET API

window.exampleJsFunctions = {
  showPrompt: function (message) {
    return prompt(message, 'Type anything here');
    }
};

window.JsFunctions = {

    printWorld: function (message) {

        var dcolor = document.getElementById("dcolor");
        var bbox = dcolor.getBoundingClientRect();
        return bbox.width;
    }
};
