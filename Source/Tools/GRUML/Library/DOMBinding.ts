/// <reference path="UserLog.ts" />
/// <reference path="BindingBase.ts" />

// A binding to an HTML control, i.e. a DOM element.
class DOMBinding extends BindingBase {
    protected _prop: string;

    constructor(control: any, prop: string) {
        super();
        this._prop = prop;
        this.source = control;

        this.source.addEventListener("change", function () {
            UserLog.Trace("DOMBinding.change!");
        });
    }

    read(): any {
        return this.source[this._prop];
    }

    write(value: any): void {
        let e = this.source;
        let p = this._prop;
        let handled = false;

        let logstring: string = "[DOM] " + e.tagName + "." + p + ": ";

        try {

            let c: any = null;
            if (!!e['__wrapper']) {
                // OFNREJVN7V: extract control

                logstring += "WRAPPER ";

                c = e.__wrapper();

                if (!c) {
                    UserLog.Warning("warning: wrapper found did not return control.");
                }
                else {
                    logstring += "CONTROL [" + getTypeID(c) + "] ";
                }
            }

            if (!!c) {
                let f = true;
                if (f) {
                    // UserLog.Trace("  control [" + c['$type'] + "] is associated and property '" + p + "' is present ...");

                    logstring += "PROPERTY; ";

                    // assign control property and 
                    c[p] = value;

                    handled = true;

                    // bail out
                    return;
                }
                else {
                    logstring += "property is NOT present; ";
                }
            }

            logstring += "ATTRIBUTE; ";

            // at this point, control properties are ruled out
            // so it's either a 'special property' or an attribute.
            if (p == "text") {

                // special case: text property
                while (!!e.firstChild) e.removeChild(e.firstChild);
                e.appendChild(document.createTextNode(value));
                handled = true;
            }
            else if (p == "enabled") {
                if (!!value) {
                    e.removeAttribute("disabled");
                }
                else {
                    e.setAttribute("disabled", !value);
                }
                handled = true;
            }
            else if (p == "visibility") {
                if (!!value) {
                    e.style.display = "block";
                }
                else {
                    e.style.display = "none";
                }
                handled = true;
            }
            else {
                e.setAttribute(p, value);
                handled = true;
            }
        }
        finally {
            logstring += handled ? " ==> OK" : " --- ignored";
            UserLog.Trace(logstring);
        }
    }
}

class TextInputBinding extends DOMBinding {
    private _value: string;

    read(): any {
        if (this._prop == "value") {
            return this._value;
        }
        else {
            return super.read();
        }
    }

    constructor(control: any, prop: string) {
        super(control, prop);
        this._value = control.value;
        let self = this;
        this.source.addEventListener("keyup", function (e) {
            self._value = control.value;
            self.triggerValueChanged();
        });
    }
}
