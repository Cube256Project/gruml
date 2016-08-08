
interface INotifyPropertyChanged {
    addPropertyChanged(handler: Function): void;
    removePropertyChanged(handler: Function): void;
}

enum NotifyCollectionChangedAction {
    add,
    remove,
    reset
}

class NotifyCollectionChangedEventArgs {
    public action: NotifyCollectionChangedAction = null;
    public newItems: any[] = null;
    public newStartingIndex: number;
    public oldItems: any[] = null;
    public oldStartingIndex: number;
}

interface INotifyCollectionChanged {
    addCollectionChanged(handler: Function): void;
    removeCollectionChanged(handler: Function): void;
}

interface IValueConverter {
    convert(v: any): any;
    convertBack(v: any): any;
}

// Interface provided by objects containing BindingElements.
interface IBindingElementParent {
    notifyValueChanged(index: number): void;
}
