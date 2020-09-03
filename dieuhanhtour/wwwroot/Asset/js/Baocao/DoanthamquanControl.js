var DoanthamquanControl = {
    init: function () {

        DoanthamquanControl.registerEvent();       
    },

    registerEvent: function () {
        $(document).ready(function () {

        });
        $(function () {
            $('.mytable').DataTable({
                'paging': true,
                'lengthChange': false,
                'searching': false,
                'ordering': false,
                'info': false,
                'autoWidth': false
            })
            $('#frmCacdiemtq').validate({
                rules: {
                    tungay: {
                        required: true
                    },
                    denngay: {
                        required: true
                    }
                },
                messages: {
                    tungay: {
                        required: "Vui lòng nhập từ ngày"
                    },
                    denngay: {
                        required: "Vui lòng nhập đến ngày"
                    },
                }
                //,
                //submitHandler: function (form) {
                //    debugger
                //}
            });
        });
        $('#btSearchDoan').on('click', function () {
            if ($('#frmCacdiemtq').valid()) {

                $('#frmCacdiemtq').submit();

            }
        });
        $('#btnExport').on('click', function () {
            var tungay = $('#txtTungay').val();
            var denngay = $('#txtDenngay').val();
            var aweekday = $('#aweekday').val();
            $.ajax({
                url: '/Baocao/DoanthamquanToExcel',
                type: 'GET',
                dataType: 'json',
                data: { tungay: tungay, denngay: denngay, aweekday: aweekday },
                success: function (response) {
                    console.log(response)
                    if (response) {
                        alert('OK');
                    }
                }
            });
        });

    }

};
DoanthamquanControl.init();