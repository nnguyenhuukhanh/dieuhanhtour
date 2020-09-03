var UserControl = {
    init: function () {

        UserControl.registerEvent();
    },

    registerEvent: function () {
        $(function () {
            UserControl.loadListAppByUser($('#txtUsername').val());
            //$('.select2').select2();
         
        });

        $("body").on("click", "#tbAppByUser .fAdd", function () {
            var row = $(this).closest("tr");
            var input_group = row.find(".cbo");
            input_group.show();
            var username = $('#txtUsername').val();
            UserControl.loadCboApp(username);

            row.find(".fUpdate").show();
            row.find(".fCancel").show();
            $(this).hide();
        });
        $("body").on("click", "#tbAppByUser .fCancel", function () {
            var row = $(this).closest("tr");

            var input_group = row.find(".cbo");
            input_group.hide();
            row.find(".fUpdate").hide();
            row.find(".fAdd").show();
            $(this).hide();
        });
        $("body").on("click", "#tbAppByUser .fUpdate", function () {
            var applicationUser = {};
            applicationUser.Username = $("#ftxtUserName").val();
            applicationUser.Mact = $('#cboApp option:selected').val();           
            $.ajax({
                type: "POST",
                url: "/Applications/insertAppUser",
                data: { Username: applicationUser.Username, Mact: applicationUser.Mact },
                dataType: "json"
            });
            UserControl.loadListAppByUser($("#ftxtUserName").val());

        });
        $('body').on('click', "#AppUser_ .refresh", function () {
            var id = $('#txtUsername').val();
            UserControl.loadListAppByUser(id);
        });
        $("body").on("click", "#tbAppByUser .Delete", function () {

            if (confirm("Bạn muốn xóa chương trình này?")) {
                var row = $(this).closest("tr");
                var Username = $(this).data('username');
                var Mact = $(this).data('mact')
                $.ajax({
                    type: "POST",
                    url: "/Applications/delAppUser",
                    data: { Username: Username, Mact: Mact },
                    dataType: "json"
                });
                row.remove();
            }
        });
    },
    loadListAppByUser: function (username) {
        var url = "/Nhanvien/ListAppByUser";
        $.get(url, { username: username }, function (data) {
            $('#AppUser_').html(data);
        })
    },
    loadCboApp: function (username) {
        $('#cboApp').html('');
        var option = '';
        $.ajax({
            url: '/Nhanvien/ListAppNotInUser',
            type: 'GET',
            data: { username: username },
            dataType: 'json',
            success: function (response) {
                var data = JSON.parse(response.data);
                //console.log(data);
                $.each(data, function (i, item) {
                    option = option + '<option value="' + item.Mact + '">' + item.Mota + '</option>';
                });
                $('#cboApp').html(option);
            }
        });
    },
   
};
UserControl.init();