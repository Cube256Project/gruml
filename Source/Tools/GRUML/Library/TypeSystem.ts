class TypeSystem {
}

function getTypeID(a: any): string {
    var rx = /function (.{1,})\(/;
    var results = (rx).exec((a).constructor.toString());
    return (results && results.length > 1) ? results[1] : null;
}