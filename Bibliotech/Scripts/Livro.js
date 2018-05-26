function inserirExemplar() {
    var qtd = $("#qtdExemplares").val();

    var divHtml =
        '<input id="Exemplares_' + qtd + '__Id" name="Exemplares[' + qtd + '].Id" type="hidden" value>' +
        '<div class="form-row" id="divExemplares_' + qtd + '">' +
            '<div class="col-3">' +
                '<input class="form-control" id="Exemplares_' + qtd + '__Codigo" name="Exemplares[' + qtd + '].Codigo" type="text" value="">' +
            '</div>' +
            '<div class="col-3 text-center">' +
                '<input id="Exemplares_' + qtd + '__ExclusivoBiblioteca" name="Exemplares[' + qtd + '].ExclusivoBiblioteca" type="checkbox" value="true"><input name="Exemplares[' + qtd + '].ExclusivoBiblioteca" type="hidden" value="false">' +
            '</div>' +
            '<div class="col-6">' +
                '<button type="button" class="btn btn-danger btn-sm" onclick="removerExemplar(' + qtd + ');">' +
                    '<i class="fa fa-trash"></i>' +
                '</button>' +
            '</div>' +
        '</div>';

    $("#divExemplares_n").append(divHtml);
    $("#qtdExemplares").val(+qtd + +1);
    load();
}

function removerExemplar(i) {
    document.getElementById("divExemplares_" + i).remove();
}

function validarSalvarExemplares() {
    var qtd = $("#qtdExemplares").val();

    for (var i = 0; i < qtdControleFinanceiroSocioAcoes; i++) {
        var codigo = document.getElementById('Exemplares_' + i + '__Codigo');

        if (codigo != null && codigo.value == "") {
            bootbox.alert("O campo Código é obrigatório.");
            return false;
        }
    }
}