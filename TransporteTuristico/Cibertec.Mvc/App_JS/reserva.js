(function (reserva) {
    reserva.success = successReload;
    reserva.pages = 1;
    reserva.rowSize = 10;
  
    /*Atributos para el manejo del Hub*/
    reserva.hub = {};
    reserva.ids = [];
    reserva.recordInUse = false;

    reserva.addCustomer = addCustomerId;
    reserva.removeHubCustomer = removeCustomerId;
    reserva.validate = validate;

    $(function () {
        connectToHub();
        ObtenerValueSelect();
        init(1);
    })

    return reserva;

    function ObtenerValueSelect() {

        $(document).on('change', '#main_body #NewID', function () {
            var valor = $(this[this.selectedIndex]).val();
            $.ajax({
                url: '/Reserva/ListItinerario/' + parseInt(valor),
                type: 'GET',
                dataType: 'json', // added data type
                success: function (res) {
                    $("#Costo").val(res.Costo);
                }
            });
        });
    }
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
        $.get('/Reserva/Count/' + reserva.rowSize,
            function (data) {
                reserva.pages = data;
                $('.pagination').bootpag({
                    total: reserva.pages,
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
                    getCustomers(num);
                });
                getCustomers(numPage);
            });
    }

    function getCustomers(cantPages) {
        var url = '/Reserva/List/' + cantPages + '/' + reserva.rowSize;
        $.get(url, function (data) {
            $('.content').html(data);
            //console.log(data);
        });
    }
    function addCustomerId(id) {
        reserva.hub.server.addCustomerId(id);
    }

    function removeCustomerId(id) {
        reserva.hub.server.removeCustomerId(id);
    }

    function connectToHub() {
        reserva.hub = $.connection.customerHub;
        reserva.hub.client.customerStatus = customerStatus;
    }

    function customerStatus(customerIds) {
        console.log(customerIds);
        reserva.ids = customerIds;
    }

    function validate(id) {
        reserva.recordInUse = (reserva.ids.indexOf(id) > -1);
        if (reserva.recordInUse) {
            $('#inUse').removeClass('hidden');
            $('#btn-save').html("");
        }
    }
})(window.reserva = window.reserva || {});