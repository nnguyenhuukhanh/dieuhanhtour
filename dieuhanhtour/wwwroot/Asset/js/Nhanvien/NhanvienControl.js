var NhanvienControl = {
    init: function () {

        NhanvienControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {           
            $("#txtSearchNhanvien").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#tbBodyNhanvien tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
            $('#tbNhanvien').off('click').on('click', 'tbody tr', function (event) {
                $(this).addClass('highlight').siblings().removeClass('highlight');
                var username = $(this).data('username');
                var url = "/Nhanvien/Edit";
                $.get(url, { id: username }, function (res) {
                    if (res) {
                        $('#ModalEditNhanvien').show();
                        $('.AddEditNhanvien').html(res);
                    };
                });
            });
        });
        $(function () {
            $('#tbNhanvien').DataTable({
                'paging': false,
                'lengthChange': false,
                'searching': false,
                'ordering': true,
                'info': true,
                'autoWidth': false
            })
        });
        $('#btThemnv').on('click', function () {
            var url = "/Nhanvien/Create";
            $.get(url,  function (res) {
                if (res) {
                    $('#ModalEditNhanvien').show();
                    $('.AddEditNhanvien').html(res);
                };
            });
        });
    }
};
NhanvienControl.init();