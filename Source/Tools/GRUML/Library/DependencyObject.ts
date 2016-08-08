/// <reference path="Interfaces.ts" />
/// <reference path="Delegate.ts" />

class DependencyObject implements INotifyPropertyChanged {
    private _event: Delegate = null;

    addPropertyChanged(handler: Function): void {
        this._event = Delegate.combine(this._event, handler);
    }

    removePropertyChanged(handler: Function): void {
        this._event = Delegate.remove(this._event, handler);
    }

    triggerPropertyChanged(prop: string) {
        Delegate.invoke(this._event, this, prop);
    }
}
