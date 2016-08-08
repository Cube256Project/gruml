/// <reference path="Interfaces.ts" />
/// <reference path="BindingBase.ts" />
/// <reference path="BindingElement.ts" />

class ResourceBinding extends BindingBase {

    private _e: any;
    private _key: string;

    constructor(e: any, key: string) {
        super();
        this._e = e;
        this._key = key;
    }

    public write(value: any) {
        UserLog.Warning("[ResourceBinding] binding is read-only, write ignored.");
    }

    public read(): any {
        return DataTemplateSelector.findTemplate(this._e, "template:name:" + this._key);
    }

}