// Simple trace class.
class UserLog {
    static _configured: boolean = false;
    static _callback: Function[] = [];

    // Attaches a callback-function for trace output.
    static Configure(callback: Function) {
        if (!!callback) {
            let l = UserLog._callback;
            if (0 > l.indexOf(callback)) {
                l.push(callback);
            }
        }
    }

    static Trace(s: string): void {
        if (!UserLog._configured) UserLog.Configure(null);

        console.log(s);

        let l = UserLog._callback;
        for (let j = 0; j < l.length; ++j) {
            try {
                l[j](s);
            }
            catch (e)
            { }
        } 
    }

    static Warning(s: string): void {
        UserLog.Trace("WARNING: " + s);
    }

}
