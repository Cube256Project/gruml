/// <reference path="BindingOperations.ts" />
/// <reference path="Control.ts" />
/// <reference path="DataTemplateSelector.ts" />

// Presents a model using a data template.
class ContentPresenter extends Control {

    private _content: any = null;

    public $type: string = "System.ContentPresenter";

    constructor() {
        super();
    }

    set content(value: any) {
        this._content = value;
        this.invalidate();
    }


    protected render(e: any): void {
        var v = this._content;

        if (!v) {
            // content is not set
            return;
        }

        if (typeof v === "function") {
            UserLog.Trace("[ContentPresenter] render static resource ...");
            v(this.dc, this.parent);
            return;
        }

        //var t = v["$type"];
        var t = getTypeID(v);

        if (!!t) {
            UserLog.Trace("[ContentPresenter] search template for [" + t + "] ...");
            var render = DataTemplateSelector.findTemplateForType(e, t);
            if (!render) {
                // template not found
                this.setError(e, "template for [" + t + "] was not found.");
            }
            else {
                render(v, e);
            }
        }
        else if (typeof v === 'string' || v instanceof String) {
            this.parent.appendChild(document.createTextNode(v));
        }
        else {
            // datatype not found
            this.setError(e, "missing data type specification " + v);
        }
    }

    private setError(e: any, m: string) {
        UserLog.Warning("[ContentPresenter.render] " + m);

        var span = document.createElement("span");
        span.setAttribute("class", "gruml-error");
        span.appendChild(document.createTextNode(m));
        e.appendChild(span);
    }
}
