
abstract class DomCondition {
    public previous: DomCondition;
    public abstract match(e: any[]): boolean;
}

class DomConditionIntersection extends DomCondition {
    public elements: DomCondition[] = [];
    public match(s: any[]): boolean {
        let r = true;
        for (let j = 0; j < this.elements.length; ++j) {
            if (!this.elements[j].match(s)) {
                r = false;
                break;
            }
        }
        return r;
    }
}

class DomNodeTagNameCondition extends DomCondition {
    private _tag: string;
    constructor(c: string) {
        super();
        this._tag = c.toLowerCase();
    }

    match(s: HTMLElement[]) {
        let last = s[0];
        return last.nodeName.toLowerCase() == this._tag;
    }
}

class DomNodeClassCondition extends DomCondition {
    private _class: string;
    constructor(c: string) {
        super();
        this._class = c;
    }

    match(s: HTMLElement[]) {
        let last = s[0];
        let css = last.getAttribute("class");
        if (null != css && css.length > 0) {
            let classes = css.split(" ");
            for (let j = 0; j < classes.length; ++j) {
                if (classes[j] == this._class) return true;
            }
        }
        return false;
    }
}

class DomNodeHandler {
    _c: DomCondition;
    _f: Function;

    constructor(c: DomCondition, f: Function) {
        this._c = c;
        this._f = f;
    }
}

class DomConditionParser {
    // Converts a selector string into a condition object.
    public static parse(t: string): DomCondition {
        let l0 = t.split(' ');
        let r: DomCondition = null;
        let radd: DomCondition = null;
        while (l0.length) {
            var q = l0[0];
            l0.shift();
            let l1 = q.split('.');
            if (l1.length == 2) {
                let z = new DomConditionIntersection();
                z.elements.push(new DomNodeTagNameCondition(l1[0]));
                z.elements.push(new DomNodeClassCondition(l1[1]));
                radd = z;
            } else if (l1.length == 1) {
                let p: string = l1[0];
                if (p[0] == ".") {
                    p = p.substring(1);
                    radd = new DomNodeClassCondition(p);
                }
                else {
                    radd = new DomNodeTagNameCondition(p);
                }
            }

            if (!!r) radd.previous = r;
            r = radd;
        }

        return r;
    }
}

// Decorates the HTMLDOM according to a CSS/jquery-like selector specification.
class DomNodeEnumerator {
    private _list: DomNodeHandler[] = [];
    public addHandler(c: DomCondition, f: Function) {
        this._list.push(new DomNodeHandler(c, f));
    }

    public enumerate(e?: any) {
        if (!e) e = document.body;

        let s: HTMLElement[] = [e];
        this.enumerateInner(s);
    }

    private enumerateInner(s: HTMLElement[]) {
        let e = s[0];
        for (let j = 0; j < e.children.length; ++j) {
            let u = e.children[j];
            if (u.nodeType == 1) {
                s.unshift(u as HTMLElement);
                this.enumerateInner(s);
                s.shift();
            }
        }

        this.callback(s);
    }

    private callback(s: HTMLElement[]) {
        let h = this._list;
        let e = s[0];

        let text = e.nodeName;
        let css = e.getAttribute("class");
        if (!!css && css.length > 0) text += "." + css;

        for (let j = 0; j < h.length; ++j) {
            let dh = h[j];
            if (dh._c.match(s)) {
                dh._f(e);
            }
        }
    }
}

function $dom(parent: HTMLElement, pattern: any, f: Function) {
    let x = new DomNodeEnumerator();
    if (typeof pattern === "string") {
        pattern = DomConditionParser.parse(pattern);
    }

    x.addHandler(pattern, f);
    x.enumerate(parent);
}


function testDomNodeEnumerator() {
    let x = new DomNodeEnumerator();
    x.addHandler(DomConditionParser.parse("P.log"), LogItemDecorator);
    x.enumerate();
}