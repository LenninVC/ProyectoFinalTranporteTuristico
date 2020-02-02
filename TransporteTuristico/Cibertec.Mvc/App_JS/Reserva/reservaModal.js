(function (reservaModal) {
    
    reservaModal.reservaModal = successReload;
    reservaModal.Data = '';
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
    $(function () {

        //ObtenerValueSelect();
        BuscarCliente();
    })

    return reservaModal;

   

    function ObtenerValueSelect() {

        $(document).on('change', '#main_body #NewID', function () {


            var valor = $(this[this.selectedIndex]).val();


            $.get('/Reserva/ListItinerario/' + parseInt(valor),
                function (data) {
                    $("#Costo").val(data.Costo);
         
                });

        });


        //$('#NewID').change(function () {
        //    var _myNames = JSON.stringify('@Html.Raw(Json.Encode(ViewData["myNames"]))');
        //    var valor = $(this[this.selectedIndex]).val();
            


        //    if (valor== -1) {
        //        $("#Costo").val(23);

        //    } else if (valor == 1) {

        //        $("#Costo").val(1236);

        //    } else {
        //        $("#Costo").val(222);
        //    }
        //});
    }


    function BuscarCliente() {
        $("#btnBuscar").click(function () {
             $.get('/Customer/BuscarCliente/' + parseInt($("#NumeroDni").val()),
                function (data) {
                    $("#Nombre").val(data.Nombres);
                    $("#Apellidos").val(data.Apellidos);
                    $("#IdCliente").val(data.IdCliente);
                    
                });
        });
    }
})(window.reservaModal = window.reservaModal || {});