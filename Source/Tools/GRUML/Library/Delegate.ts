class Delegate {
    private _list: Function[] = [];

    constructor(a: Function[]) {
        this._list = a;
    }

    static combine(left: Delegate, right: Function): Delegate {
        if (!left) {
            return new Delegate([ right ]);
        }
        else if (!right) {
            return left;
        }
        else {
            if (0 > left._list.indexOf(right)) {
                left._list.push(right);
            }

            return left;
        }
    }

    static remove(left: Delegate, right: Function): Delegate {
        if (!left) return null;
        var index = left._list.indexOf(right);
        if (index < 0) return left;
        left._list.splice(index, 1);
        if (left._list.length == 0) return null;
        return left;
    }

    static invoke(d: Delegate, sender?: any, e?: any): void {
        if (!!d) {
            var c = d._list.slice();
            for (let j = 0; j < c.length; ++j) {
                c[j](sender, e);
            }
        }
    }
}
