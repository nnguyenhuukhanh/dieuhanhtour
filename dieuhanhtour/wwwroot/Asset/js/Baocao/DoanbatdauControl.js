var DoanbatdauControl = {
    init: function () {

        DoanbatdauControl.registerEvent();
        DoanbatdauControl.loadCboPhongban();
    },

    registerEvent: function () {
        $(document).ready(function () {

        });
        $(function () {
            //$('.mytable').DataTable({
            //    'paging': true,
            //    'lengthChange': false,
            //    'searching': false,
            //    'ordering': true,
            //    'info': false,
            //    'autoWidth': false
            //})
            $('#frmDoanbatdau').validate({
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
            if ($('#frmDoanbatdau').valid()) {
              
                $('#frmDoanbatdau').submit();
              
            }           
        });
        $('#btnExport').on('click', function () {
            var tungay = $('#txtTungay').val();
            var denngay = $('#txtDenngay').val();
            var thitruong = $('#cboPhongban').val();
            $.ajax({
                url: '/Baocao/DoanbatdautoExcel',
                type: 'GET',
                dataType: 'json',
                data: { tungay: tungay, denngay: denngay, thitruong: thitruong },
                success: function (response) {
                    console.log(response)
                    if (response) {
                        alert('OK');
                    }
                    
                }
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
               // console.log(data);
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
DoanbatdauControl.init();