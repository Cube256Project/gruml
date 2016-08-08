
// Simple trace class.
class UserLog {
    static _callback: Function = null;

    // Attaches the callbackfunction for trace output.
    static Configure(callback: Function) {
        UserLog._callback = callback;
    }

    static Trace(s: string): void {
        console.log(s);
        if (!!UserLog._callback) {
            UserLog._callback(s);
        }
    }

    static Warning(s: string): void {
        console.log(s);
        if (!!UserLog._callback) {
            UserLog._callback(s);
        }
    }

}
