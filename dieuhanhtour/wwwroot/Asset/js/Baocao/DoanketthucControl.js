var DoanketthucControl = {
    init: function () {

        DoanketthucControl.registerEvent();
        DoanketthucControl.loadCboPhongban();
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
            $('#frmDoanketthuc').validate({
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
       
    },
    //Load cbo thị trường
    loadCboPhongban: function () {
        $('#cboPhongban').html('');
        var option = '';

        $.ajax({
            url: '/Baocao/listPhongban',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = JSON.parse(response.data)
                var option = '';
                option = option + '<option value="' + "" + '">' + "Tất cả thị trường" + '</option>';
                $.each(data, function (i, item) {
                    option = option + '<option value="' + item.maphong + '">' + item.tenphong + '</option>';
                });
                $('#cboPhongban').html(option);

            }
        });
    }

};
DoanketthucControl.init();