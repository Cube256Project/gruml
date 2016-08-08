/// <reference path="Delegate.ts" />

// Abstract baseclass for binding classes.
abstract class BindingBase {

    private _source: any;
    protected _event: Delegate = null;

    constructor() { }

    get source(): any { return this._source; }
    set source(s: any) {
        this._source = s;
        this.onSourceChanged();
    }

    // called when source property changes
    protected onSourceChanged(): void { }

    triggerValueChanged(): void {
        Delegate.invoke(this._event);
    }

    addValueChanged(handler: Function): void {
        this._event = Delegate.combine(this._event, handler);
    }

    removeValueChanged(handler: Function): void {
        this._event = Delegate.remove(this._event, handler);
    }

    public abstract write(value: any): void;
    public abstract read(): any;
}
