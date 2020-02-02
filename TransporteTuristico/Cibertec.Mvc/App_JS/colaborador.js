(function (colaborador) {
    colaborador.success = successReload;
    colaborador.pages = 1;
    colaborador.rowSize = 10;
    /*Atributos para el manejo del Hub*/
    colaborador.hub = {};
    colaborador.ids = [];
    colaborador.recordInUse = false;

    colaborador.addColaborador = addColaboradorId;
    colaborador.removeHubColaborador = removeColaboradorId;
    colaborador.validate = validate;

    $(function () {
        connectToHub();
        init(1);
    })

    return colaborador;

    function successReload(option) {
        cibertec.closeModal(option);
        elements = document.getElementsByClassName('active');
        activePage = elements[1].children;
        console.log(activePage[0].text);

        lstTableRows = $('.table >tbody >tr').length - 1;
        console.log(lstTableRows);

        if (option === "delete" && lstTableRows === 1) {
            cant = activePage[0].text;
            init(cant - 1);
        }
        else 
            init(activePage[0].text); 
    }

    function init(numPage) {
        $.get('/Colaborador/Count/' + colaborador.rowSize,
            function (data) {
                colaborador.pages = data;
                $('.pagination').bootpag({
                    total: colaborador.pages,
                    page: numPage,
                    maxVisible: 5,
                    leaps: true,
                    firstLastUse: true,
                    first: '← ',
                    last: '→ ',
                    wrapClass: 'pagination',
                    activeClass: 'active',
                    disabledClass: 'disabled',
                    nextClass: 'next',
                    prevClass: 'prev',
                    lastClass: 'last',
                    firstClass: 'first'
                }).on('page', function (event, num) {
                    getColaboradores(num);
                });
                getColaboradores(numPage);
            });
    }

    function getColaboradores(cantPages) {
        var url = '/Colaborador/List/' + cantPages + '/' + colaborador.rowSize;
        $.get(url, function (data) {
            $('.content').html(data);
            //console.log(data);
        });
    }

    function addColaboradorId(id) {
        colaborador.hub.server.addColaboradorId(id);
    }

    function removeColaboradorId(id) {
        colaborador.hub.server.removeColaboradorId(id);
    }

    function connectToHub() {
        colaborador.hub = $.connection.colaboradorHub;
        colaborador.hub.client.colaboradorStatus = colaboradorStatus;
    }

    function colaboradorStatus(colaboradorIds) {
        console.log(colaboradorIds);
        colaborador.ids = colaboradorIds;
    }

    function validate(id) {
        colaborador.recordInUse = (colaborador.ids.indexOf(id) > -1);
        if (colaborador.recordInUse) {
            $('#inUse').removeClass('hidden');
            $('#btn-save').html("");
        }
    }
})(window.colaborador = window.colaborador || {});