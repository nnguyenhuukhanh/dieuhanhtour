var SupplierControl = {
    init: function () {

        SupplierControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {
            $("#txtSeachSupplier").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#tbBodySupplier tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
            $('#tbSupplier').off('click').on('click', 'tbody tr', function (event) {
                $(this).addClass('highlight').siblings().removeClass('highlight');
                //var matinh = $(this).data('matinh');
                //$('#txtMatinh').val(matinh);
               // var url = "/Thanhpho/ListThanhphoByTinh";
                //$.get(url, { matinh: matinh }, function (res) {
                //    if (res) {
                //        $('#ThanhphoByTinh').html(res);
                //    };
                //});
            });
        });
        $(function () {
            if ($.fn.dataTable.isDataTable('#tbSupplier')) {
                table = $('#tbSupplier').DataTable();
            }
            else {
                table = $('#tbSupplier').DataTable({
                    'paging': false,
                    'lengthChange': false,
                    'searching': false,
                    'ordering': true,
                    'info': true,
                    'autoWidth': false
                });
            }
        });
        $('.dongtp').off('click').on('click', function () {

            $('.showThanhpho').show(500);
            $('#ModalEditThanhpho').hide(500);
        });
        $('#frmEditThanhpho').validate({
            rules: {
                Tentp: {
                    required: true
                }
            },
            messages: {
                Tentp: {
                    required: "Vui lòng nhập tên thành phố"
                }
            }
        });
        $('#frmAddThanhpho').validate({
            rules: {
                Tentp: {
                    required: true
                }
            },
            messages: {
                Tentp: {
                    required: "Vui lòng nhập tên thành phố"
                }
            }
        });
        $('#btUpdateThanhpho').off('click').on('click', function () {
            if ($('#frmEditThanhpho').valid()) {
                var matinh = $('#txtMatinh').val();
                var frmEditThanhpho = $('#frmEditThanhpho').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Thanhpho/EditThanhpho",
                    data: frmEditThanhpho,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            ThanhphoControl.LoadThanhpho(matinh);
                        }
                    }
                });
            }
        });
        $('#btAddThanhpho').off('click').on('click', function () {
            if ($('#frmAddThanhpho').valid()) {
                var matinh = $('#txtMatinh').val();
                var frmAddThanhpho = $('#frmAddThanhpho').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Thanhpho/ThemThanhpho",
                    data: frmAddThanhpho,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            ThanhphoControl.LoadThanhpho(matinh);
                        }
                    }
                });
            }
        });
        $("body").off('click').on("click", "#tbThanhpho .fAdd", function () {
            var matinh = $('#txtMatinh').val();
            ThanhphoControl.AddThanhpho(matinh);
        });
    },
    LoadThanhpho: function (matinh) {
        $.ajax({
            url: '/thanhpho/listThanhphoByTinh',
            data: { matinh: matinh },
            type: 'GET',
            success: function (data) {
                $('.showThanhpho').show(500);
                $('.showThanhpho').html(data);
                $('#ModalEditThanhpho').hide(500);
            }
        });
    },
    EditThanhpho: function (matp) {
        $.ajax({
            url: '/Thanhpho/EditThanpho',
            data: { matp: matp },
            type: 'GET',
            success: function (data) {
                $('.showThanhpho').hide(500);
                $('#ModalEditThanhpho').show(500);
                $('.EditThanhpho').html(data);
            }
        });
    },
    AddThanhpho: function (matinh) {
        $.ajax({
            url: '/Thanhpho/ThemThanhpho',
            data: { matinh: matinh },
            type: 'GET',
            success: function (data) {
                $('.showThanhpho').hide(500);
                $('#ModalEditThanhpho').show(500);
                $('.EditThanhpho').html(data);
            }
        });
    },
};
SupplierControl.init();