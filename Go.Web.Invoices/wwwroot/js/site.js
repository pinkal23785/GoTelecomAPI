// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function Print() {
    
    var divContents = document.getElementById("invoice").innerHTML;
    var a = window.open('', '', 'height=500, width=500');
    a.document.write('<html><head>');
    a.document.write('<link rel="stylesheet" href="/css/site.css" type="text/css" />');
    a.document.write('<link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css" type="text/css" />');
    a.document.write('</head><body onload="window.print();">');
    a.document.write('<div class="d-flex justify-content-center mb-3">');
    a.document.write('<div style="width:80%;">');

    a.document.write(divContents);
    a.document.write('</div>');
    a.document.write('</div>');

    a.document.write('</body></html>');
    a.document.close();
    //a.print();
    
}