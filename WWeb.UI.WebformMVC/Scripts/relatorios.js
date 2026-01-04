$(document).ready(function () {

    $(".data").datepicker({
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior'
    });

    $("input").blur(function () {
        if ($(this).val() == "") {
            $(this).css({ "border": "1px solid #F00", "padding": "2px" });
        }
    });

    $("#submit").click(function () {
        var cont = 0;
        $("#form input").each(function () {
            if ($(this).val() == "") {
                $(this).css({ "border": "1px solid #F00", "padding": "2px" });
                cont++;
            }
        });

        if (cont == 0) {
            $("#form").submit();
        }
    });
});