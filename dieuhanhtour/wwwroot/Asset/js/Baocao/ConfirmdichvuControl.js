var ConfirmdichvuControl = {
    init: function () {

        ConfirmdichvuControl.registerEvent();
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
            $('#frmConfirmdichvu').validate({
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
               
            });
        });

    }
};
ConfirmdichvuControl.init();