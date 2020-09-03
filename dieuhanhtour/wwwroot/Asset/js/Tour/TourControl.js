
var TourControl = {
    init: function () {

        TourControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {      
         
            //TourControl.loadTourProg();
            $('tr.selected').each(function () {
                var dongtour = $(this).data('dongtour');
                var cancel = $(this).data('cancel');
                if (dongtour.length > 0) {
                    $(this).css("background-color", "#E6B8B8");
                }
                if (cancel.length > 0) {
                    $(this).css("background-color", "#C0C0C0");
                }
            }) 
            //var selector = "#tbTourInf tr:eq(" + 1 + ")";
            //debugger;
            //$(selector).addClass('highlight')
        });
        //$(document).ajaxStart(function () {
        //    Pace.restart()
        //});

        $(function () {
            $(".datepicker").mask("99/99/9999");
        });
        $('#btsearchDate').off('click').on('click', function () {
            $('#searchDate').toggle(500);
        });
        $('#btsearchGuest').off('click').on('click', function () {
            var url='/Tour/searchKhach'
            $.get(url, {}, function (data) {
                $('#ModalSearchGuest').modal('show');
                $('.DetailGuest').html(data);
            })         
        });
        $('#btSearchGuest_').off('click').on('click', function () {
            var search = $('#txtSearchGuest').val();
            var url = '/Tour/searchKhach'
            $.get(url, {search:search}, function (data) {
                $('.DetailGuest').html(data);
            })    
        });

        $('.ViewLogTour').on('click', function () {
            var code = $(this).data('code');
            var url = '/Tour/ViewLogTour';
            $.get(url, { code: code }, function (data) {
                $('#ModalViewLog').modal('show');
                $('.DetailLog').html(data);
            });
        });

        $('.dongtour').off('click').on('click', function () {
            var cancel = $(this).data('cancel');
            if (cancel.length > 0) {
                alert("Tour này đã huỷ.");
                return;
            }
            else {
                var id = $(this).data('id');
                var dongtour = $(this).data('locktour');
                var message = "";
                if (dongtour.length > 0) {
                    message = "Bạn muốn mở tour này?";
                }
                else {
                    message = "Bạn muốn khoá tour này?";
                }
                if (confirm(message)) {
                    $.ajax({
                        type: "POST",
                        url: "/Tour/Dongtour",
                        data: { id: id },
                        dataType: "json",
                        success: function (response) {
                            if (response) {
                                window.location.reload();
                            }
                        }
                    });
                }
            }
        });

        $('.cttour_').click(function () {
            TourControl.loadTourProg();
        });
        $('.ghichu_').click(function () {
            TourControl.loadTourNote();
        });
        $('.dskhach_').click(function () {
            TourControl.loadDskhach();
        });
        $('.chiphikhac_').click(function () {
            TourControl.loadChiphikhac();
        });
        $('.ycxe_').click(function () {
            TourControl.loadYeucauxe();
        });
        $('.huongdan_').click(function () {
            TourControl.loadHuongdan();
        });
        $('#btCapnhatEditTourtempNote').off('click').on('click', function () {
            var frmEditTourtempNote = $('#frmEditTourtempNote').serialize();
            $.ajax({
                type: "POST",
                url: "/Tour/AddEditTourNote",
                data: frmEditTourtempNote,
                dataType: "json",
                success: function (response) {
                    if (response) {
                        alert("Cập nhật ghi chú thành công")
                        TourControl.loadTourNote();
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
        $('#frmCreateTour').validate({
            rules: {
                arr: {
                    required: true
                },
                dep: {
                    required: true
                },
                pax: {
                    required: true
                },
                childern: {
                    required: true
                },
                revenue: {
                    required: true
                },
                rate: {
                    required: true
                }
            },
            messages: {
                arr: {
                    required: "Nhập từ ngày"
                },
                dep: {
                    required: "Nhập đến ngày"
                },
                pax: {
                    required: "(*)"
                },
                childern: {
                    required: "(*)"
                },
                revenue: {
                    required: "(*)"
                },
                rate: {
                    required: "(*)"
                }
            }
        });
        $('#frmHuytour').validate({
            rules: {
                cancelnote: {
                    required: true
                }
            },
            messages: {
                cancelnote: {
                    required: "Vui lòng nhập lý do"
                }
            }
        });
        $("#ddTourtemp").off('change').on('change', function () {
            var code = $(this).val();
            var url = "/TourTemplate/getTourTempByCode";
            $.get(url, { code: code }, function (d) {
                $("#ddRouting").val(d.tuyentq);
                $('#txtReference').val(d.chudetour);
                $('#ddTourkind').val(d.tourkind);
            })
        })
        $('#ddlPhongdh').off('change').on('change', function () {
            var maphong = $(this).val();
            var url = "/Tour/getNguoiDHByMaPhong";
            
            var ddlNguoidh = $('#ddlNguoidh');
            ddlNguoidh.empty();
            $('#txtOperator').val('');
            if (maphong != null && maphong != '') {
                $.getJSON(url, { maphong: maphong }, function (nguoidhs) {
                    if (nguoidhs != null && !jQuery.isEmptyObject(nguoidhs)) {                      
                        $.each(nguoidhs, function (index, nguoidh) {

                            ddlNguoidh.append($('<option/>', {
                                value: nguoidh.username,
                                text: nguoidh.hoten
                            })).trigger('change');
                        });
                    }
                    else {
                        ddlNguoidh.empty();
                       
                    };
                });
            }
            $('#txtDepartOperator').val($('#ddlPhongdh').val());
        });

        $('#ddlNguoidh').off('change').on("change", function () {
            $('#txtOperator').val($('#ddlNguoidh').val());
        });

        $('.Huytour').off('click').on('click', function () {
           
            var cancel = $(this).data('cancel');    
            var id = $(this).data('id');
            var url = "/Tour/Delete";

            $.get(url, { id: id }, function (data) {
                $("#ModalDongtour").modal('show');
                $(".Cancelnote").html(data);
            });

            //if (cancel.length>0) {               
            //    alert('tour da huy');
            //}
            //else {
            //    var url = "/Tour/Delete";
            //    $.get(url, { id: id }, function (data) {
            //        $("#ModalDongtour").modal('show');
            //        $(".Cancelnote").html(data);
            //    });
            //}
        });
        $('#btHuytour').on('click', function () {           
            if ($('#frmHuytour').valid()) {
                $('#frmHuytour').submit();               
           }
        });

    },
    loadTourProg: function () {
        var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/listTourProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#cttour').html(data);
            }
        });
    },
    loadTourNote: function () {
        var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/AddEditTourNote',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#dsGhichu').html(data);
            }
        });
    },
    loadYeucauxe: function () {
       // debugger
        var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/ListYeucauxe',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#dsYcxe').html(data);
            }
        });
    },
    loadDskhach: function () {
      //  debugger
         
        var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/ListDsKhach',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#dsKhach').html(data);
            }
        });
    },
    loadHuongdan: function () {
       // debugger
       
        var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/ListHuongdan',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#dsHuongdan').html(data);
            }
        });
    },
    loadChiphikhac: function () {
      //  debugger
        var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/ListCPKhac',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#dsChiphikhac').html(data);
            }
        });
    } 
};
TourControl.init();