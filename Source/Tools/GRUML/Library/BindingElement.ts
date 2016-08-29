/// <reference path="Interfaces.ts" />
/// <reference path="UserLog.ts" />

// Single element of a path based Binding object.
class BindingElement {

    private _parent: IBindingElementParent;
    private _index: number;
    private _prop: string;
    private _source: any;
    private _value: any;
    private _handler: Function;

    constructor(parent: IBindingElementParent, index: number, prop: string) {
        this._parent = parent;
        this._index = index;
        this._prop = prop;
        this._source = null;
        this._value = null;
        this._handler = null;
    }

    get value() {
        return this._value;
    }

    get source() {
        return this._source;
    }

    // Reads the current value from the source and attaches a 
    // notification handler, if possible.
    // @param source The source object to attach to.
    read(source: any): void {
        let sourcechanged: boolean = false;

        if (source != this._source) {
            // source has changed, remove existing handler
            this.clear();
            sourcechanged = true;
        }

        this._source = source;
        if (!this._source) {
            if (this._index == 0) {
                UserLog.Trace("warning: no source for leftmost binding element.");
            }

            // no source, clear value and bail out.
            this._value = null;
            return;
        }

        // read value
        this._value = this._source[this._prop];

        // UserLog.Trace("[BindingElement] prop " + this._prop + " == " + this._value);

        // is INotifyPropertyChanged supported?
        if (sourcechanged && !!this._source["addPropertyChanged"]) {

            let notify = <INotifyPropertyChanged>this._source;

            // construct notification handler
            let self = this;
            this._handler = function (sender: any, e: string) {
                // if property matches
                if (e == self._prop) {
                    // notify parent to trigger rebind
                    self._parent.notifyValueChanged(self._index);
                }
            };

            // and add it
            notify.addPropertyChanged(this._handler);
        }
    }

    write(value: any): void {
        if (!!this._source) {
            this._source[this._prop] = value;
        }
        else {
            UserLog.Trace("[BindingElement] write value not updated.");
        }
    }

    clear(): void {
        if (!!this._handler) {

            let notify = <INotifyPropertyChanged>this._source;
            notify.removePropertyChanged(this._handler);
            this._handler = null;
        }
        this._value = null;
        this._source = null;
    }
}
