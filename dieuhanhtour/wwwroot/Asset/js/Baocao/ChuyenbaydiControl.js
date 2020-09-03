var ChuyenbaydiControl = {
    init: function () {

        ChuyenbaydiControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {

        });
        $(function () {
            $('.mytable').DataTable({
                'paging': true,
                'lengthChange': false,
                'searching': false,
                'ordering': true,
                'info': false,
                'autoWidth': false
            })
            $('#frmChuyenbaydi').validate({
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
            if ($('#frmChuyenbaydi').valid()) {

                $('#frmChuyenbaydi').submit();

            }
        });

    }
};
ChuyenbaydiControl.init();