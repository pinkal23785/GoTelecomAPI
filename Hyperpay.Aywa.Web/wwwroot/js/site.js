// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




$(document).ready(function () {
    var $loading = $('#loader').hide();
    $(document)
        .ajaxStart(function () {
            $loading.show();
        })
        .ajaxStop(function () {
            $loading.hide();
        });;

});

function Print() {

    var divContents = document.getElementById("invoice").innerHTML;
    var a = window.open('', '', 'fullscreen="yes"');
    a.document.write('<html><head>');
    a.document.write('<link rel="stylesheet" href="/css/site.css" type="text/css" />');
    a.document.write('<link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css" type="text/css" />');
    a.document.write('</head><body style="font-family:normal,gemedium" onload="window.print();">');
    a.document.write('<div class="d-flex justify-content-center mb-3">');
    //a.document.write('<div style="width:100%;">');

    a.document.write(divContents);
    a.document.write('</div>');
    a.document.write('<style>.involice-tbl{width:90% !important;}</style>');
    //a.document.write('</div>');

    a.document.write('</body></html>');
    a.document.close();
    //a.print();

}