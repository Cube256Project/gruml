/// <reference path="UserLog.ts" />
/// <reference path="DependencyObject.ts" />

abstract class Control extends DependencyObject {
    protected _parent: any = null;
    protected _dc: any = null;

    public $type: string = "System.Control";

    constructor() {
        super();
    }

    set parent(a: any) {
        if (this._parent !== a) {
            if (!a) {
                UserLog.Trace("[Control] detached from parent?");
                this._parent = null;
            }
            else {
                // re-render
                this.clear();
                this.render(this._parent = a);
                this.rendercomplete();
            }
        }
    }

    // Returns the parent DOM element, if any.
    get parent(): any {
        return this._parent;
    }

    set dc(v: any) {
        if (this._dc !== v) {
            // new dc: trigger binding updates
            this._dc = v;
            this.triggerPropertyChanged("dc");
        }
    }

    get dc(): any {
        return this._dc;
    }

    protected clear(): void {
        let c = this._parent;
        if (!!c) {
            while (!!c.firstChild) c.removeChild(c.firstChild);
        }
    }

    protected invalidate(): void {
        if (!!this.parent) {
            this.clear();
            this.render(this.parent);
            this.rendercomplete();
        }
    }

    private rendercomplete() {
        let self = this;
        // postpone 'onrendered' ... yield
        window.setTimeout(function () {
            self.onrendered();
        }, 0);
    }

    protected abstract render(e: any): void;

    protected onrendered() { }

    public static SendRoutedCommand(sender: HTMLElement, command: string) {
        let logstring = "[Control.SendRoutedCommand] sender: " + sender.nodeName;

        // extract parameter
        let parameter = sender["data-parameter"];

        if (parameter) {
            logstring += " PARAMETER " + parameter["$type"];
        }

        let handled = false;

        try {

            while (sender) {
                var c0 = sender["__control"];
                if (c0) {
                    let control = c0();
                    let handler = control["ExecuteRoutedCommand"];
                    if (handler) {
                        logstring += " CALLHANDLER " + control["$type"];
                        if (handler.call(control, command, parameter)) {
                            logstring += " ==> HANDLED";
                            handled = true;
                            break;
                        }
                    }
                }

                sender = sender.parentElement;
            }
        }
        catch (err) {
            logstring += " ==> ERROR: " + err;
        }

        if (!handled) {
            logstring += " ==> NOT HANDLED";
        }

        UserLog.Trace(logstring);
    }
}
