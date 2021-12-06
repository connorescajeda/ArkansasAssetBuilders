// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var list = { "S1": "taxyear1", "S2": "taxyear2", "S3": "taxyear3", "S4": "taxyear4" };
//alert(list);
function validate() {
    var checkedlist = {};
    //check which box has been selected and put that in the list
    for (var i in list) {
        //alert("is checked " + document.getElementById(i).checked);
        if (document.getElementById(i).checked) {
            //alert("adding " + list[i] + " to the list");
            var newkey = i;
            checkedlist[newkey] = list[i];
        }
    }
    //alert("checked list: " + Object.keys(checkedlist).length);
    for (i = 0; i < Object.keys(checkedlist).length; i++) {
        //alert(Object.values(checkedlist)[i]);
        var ddl = document.getElementById(Object.values(checkedlist)[i]);
        if (ddl.value == "0") {
            alert("please select tax year");
        }
    }
}