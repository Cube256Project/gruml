/// <reference path="BindingOperations.ts" />
/// <reference path="Control.ts" />

/// Selects data templates.
class DataTemplateSelector {
    // Selects the data template for a data-type.
    // @param e The container DOM element.
    // @param type The qualified data type name.
    public static findTemplateForType(e: any, type: string): Function {
        // VITIJBLJY2: construct resource tag.
        let sarg = "template:class:" + type;
        return DataTemplateSelector.findTemplate(e, sarg);
    }

    public static findTemplate(e: any, sarg: string): Function
    {
        let result: Function = null;

        // iterate ancestors in search of a dictionary
        while (!result) {
            // still a DOM element?
            if (!e || !e.getAttribute) break;

            // WXQ7XZ4EKL: look for the key of the associated dictionary ...
            var key = e.getAttribute("__dict");
            if (!!key) {
                // get associated dictionary function
                let dict: any = window[key];
                if (dict && typeof dict === "function") {
                    let render: any = dict(sarg);
                    if (!!render) {
                        // this will break the while loop
                        result = render;
                    }
                }
            }

            // continue with parent node
            e = e.parentNode;
        }

        if (!result) {
            UserLog.Trace("warning: template search argument '" + sarg + "' was found.");
        }

        return result;
    }
}
