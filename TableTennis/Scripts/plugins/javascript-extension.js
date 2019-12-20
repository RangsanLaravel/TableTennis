/*!
* JavaScript Extension Library
* for N/A
* 
* Copyright © 2016 - N/A, Bangkok Life Assurance PCL
* Author: TooN DinDarkDevil
* Version: 2.0.0.0
*/

//#region " String "
/* trimming space from both side of the string */
String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, "");
}

/* trimming space from left side of the string */
String.prototype.ltrim = function () {
    return this.replace(/^\s+/, "");
}

/* trimming space from right side of the string */
String.prototype.rtrim = function () {
    return this.replace(/\s+$/, "");
}

/* pads left */
String.prototype.lpad = function (padString, length) {
    var str = this;
    while (str.length < length)
        str = padString + str;
    return str;
}

/* pads right */
String.prototype.rpad = function (padString, length) {
    var str = this;
    while (str.length < length)
        str = str + padString;
    return str;
}
//#endregion

//#region " Array "
/* remove by properties */
Array.prototype.remove = function () {
    var w, a = arguments, l = a.length, ax;
    while (l && this.length) {
        w = a[--l];
        while ((ax = this.indexOf(w)) !== -1) {
            this.splice(ax, 1);
        }
    }
    return this;
};
//#endregion