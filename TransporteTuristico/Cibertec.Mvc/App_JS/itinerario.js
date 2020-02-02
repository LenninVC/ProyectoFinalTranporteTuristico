(function (itinerario) {
    itinerario.success = successReload;
    itinerario.pages = 1;
    itinerario.rowSize = 10;
    /*Atributos para el manejo del Hub*/
    itinerario.hub = {};
    itinerario.ids = [];
    itinerario.recordInUse = false;

    itinerario.addItinerario = addItinerarioId;
    itinerario.removeHubItinerario = removeItinerarioId;
    itinerario.validate = validate;

    $(function () {
        connectToHub();
        init(1);
    })

    return itinerario;

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
        $.get('/Itinerario/Count/' + itinerario.rowSize,
            function (data) {
                itinerario.pages = data;
                $('.pagination').bootpag({
                    total: itinerario.pages,
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
                    getItinerarios(num);
                });
                getItinerarios(numPage);
            });
    }

    function getItinerarios(cantPages) {
        var url = '/Itinerario/List/' + cantPages + '/' + itinerario.rowSize;
        $.get(url, function (data) {
            $('.content').html(data);
            //console.log(data);
        });
    }

    function addItinerarioId(id) {
        itinerario.hub.server.addItinerarioId(id);
    }

    function removeItinerarioId(id) {
        itinerario.hub.server.removeItinerarioId(id);
    }

    function connectToHub() {
        itinerario.hub = $.connection.itinerarioHub;
        itinerario.hub.client.itinerarioStatus = itinerarioStatus;
    }

    function itinerarioStatus(itinerarioIds) {
        console.log(itinerarioIds);
        itinerario.ids = itinerarioIds;
    }

    function validate(id) {
        itinerario.recordInUse = (itinerario.ids.indexOf(id) > -1);
        if (itinerario.recordInUse) {
            $('#inUse').removeClass('hidden');
            $('#btn-save').html("");
        }
    }
})(window.itinerario = window.itinerario || {});