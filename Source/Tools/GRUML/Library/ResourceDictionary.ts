/// <reference path="UserLog.ts" />

class ResourceDictionary {
    public static install(appdict: string, e: any) {
        // WXQ7XZ4EKL: set applevel dictionary
        e.setAttribute("__dict", appdict);

        UserLog.Trace("default resource dictionary '" + appdict + "'.");
    }
}