/// <reference path="BindingBase.ts" />

// Combines two bindings.
class BindingObject {
    private _target: BindingBase;
    private _source: BindingBase;
    private _updatingsource: boolean = false;

    constructor(target: BindingBase, source: BindingBase) {
        this._target = target;
        this._source = source;

        this.updateTarget();

        let self = this;
        this._source.addValueChanged(function () { self.updateTarget(); });
        this._target.addValueChanged(function () { self.updateSource(); });
    }

    updateTarget(): void {
        if (!this._updatingsource) {
            let value = this._source.read();
            this._target.write(value);
        }
    }

    updateSource(): void {
        try {
            this._updatingsource = true;
            this._source.write(this._target.read());
        }
        finally {
            this._updatingsource = false;
        }
    }
}
