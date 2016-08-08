/// <reference path="ObjectBinding.ts" />
/// <reference path="DOMBinding.ts" />
/// <reference path="BindingObject.ts" />
/// <reference path="ResourceBinding.ts" />

class BindingOperations {
    static isHTMLElement(o: any): boolean {
        return !!o["nodeType"];
    }
    static setBinding(e: any, prop: string, source: BindingBase): void {
        let target: BindingBase = null;
        if (BindingOperations.isHTMLElement(e)) {
            // UserLog.Trace("binding to HTML element '" + e.nodeName + "' property '" + prop + "' ...");
            if (e.nodeName.toUpperCase() == "INPUT") {
                let kind = e.getAttribute("type");
                if (!kind) kind = "TEXT";
                if (kind.toUpperCase() == "TEXT") {
                    target = new TextInputBinding(e, prop);
                }
            }

            if (!target) {
                target = new DOMBinding(e, prop);
            }
        }
        else {
            target = new ObjectBinding(e, prop);
            target.source = e;
        }

        new BindingObject(target, source);
    }
}

