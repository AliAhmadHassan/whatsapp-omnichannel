function CriarDivAgenda() {
    try {

        var iDiv = document.createElement('div');
        iDiv.id = 'div-agenda';
        iDiv.setAttribute('style', 'padding:10px;position:absolute;height:450px; width:550px; float:right; bottom:5px; right:580px; border:1px solid #efefef;background-color:#fff;overflow:scroll;');

        var body = document.getElementsByTagName('body');
        body[0].appendChild(iDiv);

    } catch (e) {
        alert(e);
    }
}

function LoadAgenda() {
    try {

        var protocolo = window.location.protocol;
        var root_site = window.location.host;
        var uri = protocolo + '//' + root_site;

        $.ajax({
            url: uri + "/Agenda/Index/",
            dataType: 'html',
            type: 'GET',
            contentType: 'text/html',
            cache: false,
            async: true,
            context: document.body,
            success: function (data) {
                try {
                    $('#div-agenda').html(data);
                } catch (e) {
                    alert('Entra no catch');
                }
            },
            error: function (request, status, err) {
                alert('Entra no error');
            }
        });

    } catch (e) {
        alert('Entra no error');
    }
    finally {

    }
}

$(document).ready(function () {

    $.support.cors = true;

    $.ajaxSetup({ cache: false });

    $.ajaxSettings.cache = false;

    CriarDivAgenda();

    LoadAgenda();
});
