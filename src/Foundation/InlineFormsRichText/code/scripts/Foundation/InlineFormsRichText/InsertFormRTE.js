Telerik.Web.UI.Editor.CommandList["InsertForm"] = function (commandName, editor, args) {
    var element = editor.getSelectedElement();
    var formId = extractFormGuid(element);
    editor.showExternalDialog(
        "/sitecore/shell/default.aspx?xmlcontrol=RichText.InsertForm&la=" + scLanguage + "&formID=" + formId,
        null, //argument
        800, //Height
        550, //Width
        function (sender, returnValue) {
            if (returnValue && returnValue.media !== "") {
                if (formId != null) {
                    element.innerHTML = returnValue.media;
                } else {
                    editor.pasteHtml(returnValue.openTag + returnValue.media + returnValue.closeTag);
                }
            }
        }, //callback
        null, // callback args
        "Insert Form",
        true, //modal
        Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Resize , // behaviors
        false, //showStatusBar
        false //showTitleBar
    );
}

function extractFormGuid(element) {
    if (element == undefined || element.innerText.indexOf('[form') === -1) {
        return null;
    }
    var re = /[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}/i;
    return re.exec(element.innerText);
}

