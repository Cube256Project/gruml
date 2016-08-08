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
}
