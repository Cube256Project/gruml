/// <reference path="Interfaces.ts" />
/// <reference path="BindingBase.ts" />
/// <reference path="BindingElement.ts" />

// Binding to a 'normal' object, i.e. not a control.
class ObjectBinding extends BindingBase implements IBindingElementParent {

    private _elements: BindingElement[] = [];
    private _converter: IValueConverter = null;

    constructor(source: any, path: string, converter?: IValueConverter) {
        super();
        this._converter = converter;
        this.parsePath(path);

        // UserLog.Trace("Binding(" + source + ", " + path + ", " + this._elements.length + ") ...");

        this.source = source;
    }

    private parsePath(path: string): void {
        if (!!path) {
            let list = path.split(".");
            for (let j = 0; j < list.length; ++j) {
                this._elements[j] = new BindingElement(this, j, list[j]);
            }
        }
    }

    protected onSourceChanged(): void {
        this.bindToSource(0);
    }

    private bindToSource(fromindex: number): void {
        let el = this._elements;
        for (let j = fromindex; j < el.length; ++j) {
            let source = j > 0 ? el[j - 1].value : this.source;
            el[j].read(source);
        }
    }

    // Called when a value in the path changes.
    notifyValueChanged(index: number): void {
        this.bindToSource(index);
        this.triggerValueChanged();
    }

    private convertSource(v: any): any {
        return !!this._converter ? this._converter.convert(v) : v;
    }

    private convertTarget(v: any): any {
        return !!this._converter ? this._converter.convertBack(v) : v;
    }

    // Returns the cached value of the binding.
    read(): any {
        let el = this._elements;
        if (el.length > 0) {
            return this.convertSource(el[el.length - 1].value);
        }
        else {
            return this.convertSource(this.source);
        }
    }

    write(value: any): void {
        let el = this._elements;
        if (el.length < 1) return;
        let e = el[el.length - 1];
        e.write(this.convertTarget(value));
    }
}
