var display_datetime = {
    display_elementid: '',
    display_set: function () {
        setTimeout(this.display_show, 1000);
    },
    display_show: function () {
        var x = new Date();
        var dd = x.getDate().toString();
        var MM = (x.getMonth() + 1).toString();
        var yyyy = (x.getFullYear() + 543).toString();
        var HH = x.getHours().toString();
        var mm = x.getMinutes().toString();
        var SS = x.getSeconds().toString();

        dd = dd[1] ? dd : "0" + dd[0];
        MM = MM[1] ? MM : "0" + MM[0];
        HH = HH[1] ? HH : "0" + HH[0];
        mm = mm[1] ? mm : "0" + mm[0];
        SS = SS[1] ? SS : "0" + SS[0];

        var x1 = dd + "/" + MM + "/" + yyyy;
        x1 = x1 + "  " + HH + ":" + mm + ":" + SS;
        try {
            document.getElementById(display_datetime.display_elementid).innerHTML = x1;
            display_datetime.display_set();
        }
        catch (ex) {
            console.log(ex.message);
        }
    },
    display_begin: function () {
        this.display_elementid = arguments[0];
        this.display_show();
    }
}

$(document).ready(display_datetime.display_begin("display_datetime"));