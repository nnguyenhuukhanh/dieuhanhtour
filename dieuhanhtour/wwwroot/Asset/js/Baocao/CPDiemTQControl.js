var CPDiemTQControl = {
    init: function () {

        CPDiemTQControl.registerEvent();
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
            $('#frmCPDiemTq').validate({
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
            if ($('#frmCPDiemTq').valid()) {

                $('#frmCPDiemTq').submit();

            }
        });
        $('#btnExport').on('click', function () {
            var tungay = $('#txtTungay').val();
            var denngay = $('#txtDenngay').val();

            $.ajax({
                url: '/Baocao/CPDTQToExcel',
                type: 'GET',
                dataType: 'json',
                data: { tungay: tungay, denngay: denngay },
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
CPDiemTQControl.init();