/// <reference path="UserLog.ts" />
/// <reference path="Delegate.ts" />
/// <reference path="Interfaces.ts" />

class ObservableCollection<T> extends Array<T> implements INotifyCollectionChanged {
    private _event: Delegate;

    addCollectionChanged(handler: Function): void {
        this._event = Delegate.combine(this._event, handler);
    }

    removeCollectionChanged(handler: Function): void {
        this._event = Delegate.remove(this._event, handler);
    }

    add(e: T) {
        let n = new NotifyCollectionChangedEventArgs();
        n.action = NotifyCollectionChangedAction.add;
        n.newStartingIndex = this.length;
        n.newItems = [e];

        this.push(e);

        Delegate.invoke(this._event, this, n);
    }

    removeAt(index: number) {
        let n = new NotifyCollectionChangedEventArgs();
        n.action = NotifyCollectionChangedAction.remove;
        n.oldStartingIndex = index;
        n.oldItems = [this[index]];

        this.splice(index, 1);

        Delegate.invoke(this._event, this, n);
    }

    remove(e: T) {
        let index = this.indexOf(e);
        if (0 <= index) {
            this.removeAt(index);
        }
    }

    clear() {
        let n = new NotifyCollectionChangedEventArgs();
        n.action = NotifyCollectionChangedAction.reset;

        this.splice(0, this.length);

        Delegate.invoke(this._event, this, n);

    }

}