var ApplicationControl = {
    init: function () {

        ApplicationControl.registerEvent();
    },

    registerEvent: function () {
        $(function () {
            ApplicationControl.loadListUserByApp($('#txtmact').val());
            $('.select2').select2();
            $('#tbUserByApp').DataTable()
            //{
            //    'paging': true,
            //    'lengthChange': false,
            //    'searching': true,
            //    'ordering': true,
            //    'info': true,
            //    'autoWidth': false
            //})
           
        });

        $("body").on("click", "#tbUserByApp .fAdd", function () {
            var row = $(this).closest("tr");
            var input_group = row.find(".cbo");
            input_group.show();  
            var mact = $('#txtmact').val();
            ApplicationControl.loadCboUser(mact);

            row.find(".fUpdate").show();
            row.find(".fCancel").show();
            $(this).hide();
        });
        $("body").on("click", "#tbUserByApp .fCancel", function () {
            var row = $(this).closest("tr");
          
            var input_group = row.find(".cbo");
            input_group.hide(); 
            row.find(".fUpdate").hide();
            row.find(".fAdd").show();
            $(this).hide();
        });
        $("body").on("click", "#tbUserByApp .fUpdate", function () {
            var applicationUser = {};
            applicationUser.Username = $('#cboUser option:selected').val();
            applicationUser.Mact = $("#ftxtMact").val();     
            $.ajax({
                type: "POST",
                url: "/Applications/insertAppUser",
                data: {Username:  applicationUser.Username , Mact: applicationUser.Mact  },
                dataType: "json"              
            });
            ApplicationControl.loadListUserByApp(applicationUser.Mact);
            
        });
        $('body').on('click',"#UserApp_ .refresh",function () {
            var id = $('#txtmact').val();
            ApplicationControl.loadListUserByApp(id);
        });
        $("body").on("click", "#tbUserByApp .Delete", function () {
            
            if (confirm("Bạn muốn xóa user này?")) {
                var row = $(this).closest("tr");
                var Username = $(this).data('username');
                var Mact = $(this).data('mact')
                $.ajax({
                    type: "POST",
                    url: "/Applications/delAppUser",
                    data: { Username: Username, Mact: Mact },
                    dataType: "json"
                    //success: function (response) {
                       
                    //}
                    
                });
                row.remove();
            }
        });
    },
    loadListUserByApp: function (id) {
        var url = "/Applications/listUserByApp";
        $.get(url, { id: id }, function (data) {
            $('#UserApp_').html(data);
        })
    },
    loadCboUser: function (mact) {
        $('#cboUser').html('');
        var option = '';
        $.ajax({
            url: '/Nhanvien/ListUserNotInApp',
            type: 'GET',
            data: { mact: mact },
            dataType: 'json',
            success: function (response) {
                var data = JSON.parse(response.data);
                //console.log(data);
                $.each(data, function (i, item) {
                    option = option + '<option value="' + item.Username + '">' + item.Username + '</option>';
                });
                $('#cboUser').html(option);
            }
        });
    },
    //deletekhachsan: function (id) {
    //    $.ajax({
    //        url: '/Khachsan/xoaKhachsan',
    //        data: { code: id },
    //        type: 'POST',
    //        success: function (response) {
    //            if (response.status) {
    //                bootbox.alert({
    //                    size: "small",
    //                    title: "THÔNG BÁO",
    //                    message: "Đã xóa khách sạn code: " + id + " thành công.",
    //                    callback: function () {

    //                    }

    //                })
    //            }
    //            else {
    //                bootbox.alert({
    //                    size: "small",
    //                    title: "DELETE INFO",
    //                    message: response.message
    //                })
    //            }
    //        },
    //        error: function (err) {
    //            console.log(err);
    //        }
    //    });
    //},

};
ApplicationControl.init();