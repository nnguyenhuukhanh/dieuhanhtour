var TourtemplateControl = {
    init: function () {

        TourtemplateControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {
            //var id = $('#utxtId').val();
            TourtemplateControl.loadTourProgTemp();
        });
        $(document).ajaxStart(function () {
            Pace.restart()
        });
        $(function () {
            $('.mytable').DataTable({
                'paging': false,
                'lengthChange': false,
                'searching': false,
                'ordering': true,
                'info': false,
                'autoWidth': false
            });
            $('.ddlnoborder').attr('disabled', true);
        })
        $('.cttour_').click(function () {
            TourtemplateControl.loadTourProgTemp();
        });
        $('.ghichu_').click(function () {
            TourtemplateControl.loadTourTempNote();
        });
        $('#btCapnhatEditTourtempNote').off('click').on('click', function () {
            var frmEditTourtempNote = $('#frmEditTourtempNote').serialize();
            $.ajax({
                type: "POST",
                url: "/TourTemplate/AddEditTourTempNote",
                data: frmEditTourtempNote,
                dataType: "json",
                success: function (response) {
                    if (response) {
                        alert("Cập nhật ghi chú thành công")
                        TourtemplateControl.loadTourTempNote();
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == "200") {
                        location.reload();
                        // listdichvu(idtour);//load lai danh sach dich vu
                    } else {
                        alert(xhr.status);
                    }
                }
            });
        });
       
       
    },
    loadTourProgTemp: function () {
        var id = $('#utxtId').val();
        $.ajax({
            url:'/TourTemplate/listTourProgTemp',
            data:{ id: id },
            type: 'GET',
            success: function (data) {
                $('#cttour').html(data);
            }
        });
    },
    loadTourTempNote: function () {
        var id = $('#utxtId').val();
        $.ajax({
            url: '/TourTemplate/AddEditTourTempNote',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#dsGhichu').html(data);
            }
        });
    }  //,  
    //loadTourSrvProgTemp: function () {
    //    var id = $('#utxtId').val();
    //    $.ajax({
    //        url: '/TourTemplate/AddTourSrvProgTemp',
    //        data: { id: id },
    //        type: 'GET',
    //        success: function (data) {
    //            $('#dsGhichu').html(data);
    //        }
    //    });
    //}
};
TourtemplateControl.init();