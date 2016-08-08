/// <reference path="BindingOperations.ts" />
/// <reference path="Control.ts" />
/// <reference path="ContentPresenter.ts" />

class ItemsControl extends Control {
    private _list: any[] = [];

    public $type: string = "System.ItemsControl";

    constructor() {
        super();
    }

    get items(): any[] {
        return this._list;
    }

    set items(list: any[]) {
        if (!!list) {
            this._list = list;
            let q: any = list;
            let self = this;

            if (!!q["addCollectionChanged"]) {
                q.addCollectionChanged(function (sender: any, n: NotifyCollectionChangedEventArgs) {
                    //UserLog.Trace("[ListView] collection changed: " + e.action + ", " + e.);
                    self.onCollectionChanged(n);
                });
            }
        }
        else {
            this._list = [];
        }

        this.invalidate();
    }

    private onCollectionChanged(n: NotifyCollectionChangedEventArgs): void {
        if (n.action == NotifyCollectionChangedAction.add) {
            for (let j = 0; j < n.newItems.length; ++j) {
                this.createContainer(n.newStartingIndex + j, n.newItems[j]);
            }
        }
        else if (n.action == NotifyCollectionChangedAction.remove) {
            for (let j = 0; j < n.oldItems.length; ++j) {
                this.removeContainer(n.oldStartingIndex);
            }
        }
        else {
            UserLog.Warning("warning: [ItemsControl.onCollectionChanged] action code " + n.action + " not handled, invalidating.");
            this.invalidate();
        }
    }

    private createContainer(index: number, item: any) {
        let c = this._parent;
        let e = document.createElement("div");
        let r = c.children[index];
        c.insertBefore(e, r);

        let u = new ContentPresenter();
        u.content = item;
        u.parent = e;
        u.dc = item;
    }

    private removeContainer(index: number) {
        let c = this._parent;
        let n = c.children[index];
        if (n) c.removeChild(n);
    }

    protected render(e: any): void {
        let c = this._parent;
        let items = this._list;
        if (!!items) {
            for (let j = 0; j < items.length; ++j) {
                let e = document.createElement("div");
                c.appendChild(e);
                let u = new ContentPresenter();
                u.content = items[j];
                u.parent = e;
                u.dc = items[j];
            }
        }
    }
}
