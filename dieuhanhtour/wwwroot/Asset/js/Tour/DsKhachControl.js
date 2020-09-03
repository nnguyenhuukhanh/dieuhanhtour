var DsKhachControl = {
    init: function () {

        DsKhachControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {
            var locktour = $("#txtLocktour").val();
            if (locktour.length > 0) {
                $('#btImport').attr('disabled', true).addClass('ui-state-disabled');
                $('#btAddKhach').attr('disabled', true).addClass('ui-state-disabled');            
                $('#btAddEditKhach').attr('disabled', true).addClass('ui-state-disabled');
            }
            $('.select2').select2();
            $(".datepicker").mask("99/99/9999");
            $(".time").mask("99:99");
            $('input.numbers').keyup(function (event) {
                var val = $(this).val();
                if (isNaN(val)) {
                    val = val.replace(/[^0-9\.]/g, '');
                    if (val.split('.').length > 2)
                        val = val.replace(/\.+$/, "");
                }
                $(this).val(val);

                $(this).val(function (index, value) {
                    return addCommas(value)
                        ;
                });

            });

            //$.validator.addMethod('notEqual', function (value, element, arg) {
            //    return arg != value;
            //}, 'Phải chọn một mục nào đó!');
            var operator = $('#txtOperator').val();
            var session = $('#txtSession').val();
            if (operator == session) {
                $('.tfoot').show();
            }
            else {
                $('.tfoot').hide();
            }
        });
        
        $(function () {
            $('#frmEditKhach').validate({
                rules: {
                    hoten: {
                        required: true
                    },
                    ngaysinh: {
                        required: true
                    },
                    loaiphong: {
                        required: true
                       
                    }
                },
                messages: {
                    hoten: {
                        required: "(*)"
                    },
                    ngaysinh: {
                        required: "(*)"
                    },
                    loaiphong: {
                        required: "(*)"                        
                    }
                }
                //,
                //submitHandler: function (form) {
                //    debugger
                //}
            });

            $('#frmAddKhach').validate({
                rules: {
                    hoten: {
                        required: true
                    },
                    ngaysinh: {
                        required: true
                    },
                    loaiphong: {
                        required: true

                    }
                },
                messages: {
                    hoten: {
                        required: "(*)"
                    },
                    ngaysinh: {
                        required: "(*)"
                    },
                    loaiphong: {
                        required: "(*)"
                    }
                }
                //,
                //submitHandler: function (form) {
                //    debugger
                //}
            });
        })
        function addCommas(x) {
            var parts = x.toString().split(".");
            parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            return parts.join(".");
        }

        $('.dongkhach').on('click', function () {
            $('.showYeucaukh').show(500);
            $('#ModelYeucauKh').hide(500);           
        });
        $("body").on("click", "#tbKhachHang .fAdd", function () {
            DsKhachControl.AddKhach();
        });

        $('#btAddEditKhach').off('click').on('click', function () {
            debugger

            var frmEditKhach = $('#frmEditKhach').serialize();
            $.ajax({
                type: "POST",
                url: "/Tour/EditKhach",
                data: frmEditKhach,
                dataType: "json",
                success: function () {

                    DsKhachControl.loadDskhach();
                }
                , error: function (xhr, status, error) {
                    alert("Có lỗi: " + error + " ,xin thông báo cho người quản lý biết!");
                }
            });

        });

        $('#btAddKhach').off('click').on('click', function () {
            debugger

            var frmEditKhach = $('#frmAddKhach').serialize();
            $.ajax({
                type: "POST",
                url: "/Tour/AddKhach",
                data: frmEditKhach,
                dataType: "json",
                success: function (msg) {
                    //alert(msg);
                    DsKhachControl.loadDskhach();
                }
                , error: function (xhr, status, error) {
                    alert("Có lỗi: " + error + " ,xin thông báo cho người quản lý biết!");
                }
            });

        });

        //nhap danh sach khach tu excel
        $('#btImport').on('click', function () {
            debugger

            var input = $('#filedskh');
            var files = input[0].files;
            var formData = new FormData();
            formData.append("sgtcode", $(this).data('id'));
            for (var i = 0; i != files.length; i++) {
                formData.append("files", files[i]);
            }

         
            $.ajax({
                type: "POST",
                url: "/Tour/ImportDSK",
                data: formData,              
                processData: false,
                contentType: false,
                success: function (msg) {
                   // alert(msg);
                    if (msg == "OK") {
                        alert('Đã import danh sách khách thành công!');
                        TourControl.loadDskhach();
                    } else {
                        alert(msg);
                    }
                  
                }
                , error: function (xhr, status, error) {
                    alert("Có lỗi: " + error + " ,xin thông báo cho người quản lý biết!");
                }
            });
        });    

        $('#btSavedata').on('click', function () {

            DsKhachControl.exportList();

        }) 

        $('.ViewLogKT').on('click', function () { //khi click nut xem log
            var id = $(this).data('id'); //idkhach        

            $.ajax({
                url: '/Tour/ViewLogKhach',
                data: { id: id },
                type: 'GET',
                success: function (data) {

                    $('#ModalViewLogKh').modal('show');
                    $('.DetailLogKh').html(data);
                }
            });

        });

        $('.nhapdskhach').on('click', function () { //khi click nut nhap ds khach
            
            var id = $(this).data('id'); //sgtcode        

            $.ajax({
                url: '/Tour/ImportDsKhach',
                data: { id: id },
                type: 'GET',
                success: function (data) {
                    
                    $('.showYeucaukh').hide(500);
                    $('#ModelYeucauKh').show(500);
                    $('.YeucauKh').html(data);
                }
                , error: function (xhr) {
                    
                    alert(xhr.statusText);
                }
            });

        });

    },
    //Cập nhật ds khách
    EditKhach: function (id) {
        $.ajax({
            url: '/Tour/EditKhach',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showYeucaukh').hide(500);
                $('#ModelYeucauKh').show(500);
                $('.YeucauKh').html(data);
            }
        });
    },
    AddKhach: function () {
        var sgtcode = $('#utxtId').val();
        $.ajax({
            url: '/Tour/AddKhach',
            data: { code: sgtcode },
            type: 'GET',
            success: function (data) {
                $('.showYeucaukh').hide(500);
                $('#ModelYeucauKh').show(500);
                $('.YeucauKh').html(data);
            }
        });
    }
    ,
    loadDskhach: function () {
        
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
    exportList: function () {

        var idList = [];
        var b=false ;
        $.each($('.ckId'), function (i, item) {
            if ($(this).prop('checked')) {
                b = true
            }
            else {
                b= false
            }
            idList.push({
                idkhach: $(item).data('idkhach'),
                vmb: b
                });

        });
        //$('#stringId').val(JSON.stringify(idList));
        $.ajax({
            type: "POST",
            url: "/Tour/SaveVemaybay",
            data: { list: JSON.stringify(idList) },
            dataType: "json",
            success: function (msg) {               
                DsKhachControl.loadDskhach();               
            }
            , error: function (xhr, status, error) {
                alert("Có lỗi: " + error + " ,xin thông báo cho người quản lý biết!");
            }
        });

       
    }
};
DsKhachControl.init();