var DoantheodichvukhacControl = {
    init: function () {

        DoantheodichvukhacControl.registerEvent();
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
            $('input[type="radio"].minimal').iCheck({
                // checkboxClass: 'icheckbox_minimal-blue'
                radioClass: 'iradio_minimal-blue'
            });
            $('#frmDoantheodichvukhac').validate({
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

    }
};
DoantheodichvukhacControl.init();