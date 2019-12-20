
// var mySpreadsheet = 'https://docs.google.com/spreadsheets/d/1dnRICwIZ_gIE5ERpZdc8KWH1RuPlUWfIW0Zp0gnmC70/edit#gid=1621997474';
//var mySpreadsheet = 'https://docs.google.com/spreadsheets/d/1qT1LyvoAcb0HTsi2rHBltBVpUBumAUzT__rhMvrz5Rk/edit#gid=0';
var mySpreadsheet = 'https://docs.google.com/spreadsheets/d/1VrnSBdmOrfzzDNW5dye54csqbNUKLZtEykiINRnagK0/edit#gid=473017385';

//$('#switch-hitters').sheetrock({
//    url: mySpreadsheet,
//    query: "select A,B,C,D,E,F,G,H",
//    fetchSize: 200000
//});


$("#btnForm").fancybox({
    autoSize: false,
    width: '500px',
    height: '450px'
});


function getdata() {
    //mySpreadsheet = document.getElementById("linksheet").value;
    bindExcel(mySpreadsheet);
}
 

function bindExcel(link) {
    //if (link === undefined||link ==="" ) {
    //    link = 'https://docs.google.com/spreadsheets/d/1dnRICwIZ_gIE5ERpZdc8KWH1RuPlUWfIW0Zp0gnmC70/edit#gid=1621997474';
    //}
    $('#switch-hitters').sheetrock({
        url: mySpreadsheet,
        query: "select A,B,C,D,E,F,G,H",
        fetchSize: 200000
    });
}