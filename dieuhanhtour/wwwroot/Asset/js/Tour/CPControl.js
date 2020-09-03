var CPControl = {
    init: function () {

        CPControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {
            $('input[type="checkbox"].minimal').iCheck({
                checkboxClass: 'icheckbox_minimal-blue'
            });
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

            var operator = $('#txtOperator').val();
            var session = $('#txtSession').val();
            if (operator == session) {
                $('.tfoot').show();
            }
            else {
                $('.tfoot').hide();
            }
        });


        // $(function () {
        //debugger
        //$.validator.addMethod('notEqual', function (value, element, arg) {
        //    return arg != value;
        //}, 'Phải chọn một mục nào đó!');

        //$('#frmAddCP').validate({
        //    rules: {
        //        fromdate: {
        //            required: true
        //        },
        //        todate: {
        //            required: true
        //        },
        //        vatin: {
        //            required: true
        //        },
        //        vatout: {
        //            required: true
        //        }
        //    },
        //    messages: {
        //        fromdate: {
        //            required: "(*)"
        //        },
        //        todate: {
        //            required: "(*)"
        //        },
        //        vatin: {
        //            required: "(*)"
        //        },
        //        vatout: {
        //            required: "(*)"
        //        }
        //    }
        //,
        //submitHandler: function (form) {
        //    debugger
        //}
        //});     

        $('#frmEditCP').validate({
            rules: {
                fromdate: {
                    required: true
                },
                todate: {
                    required: true
                },
                vatin: {
                    required: true
                },
                vatout: {
                    required: true
                }
            },
            messages: {
                fromdate: {
                    required: "(*)"
                },
                todate: {
                    required: "(*)"
                },
                vatin: {
                    required: "(*)"
                },
                vatout: {
                    required: "(*)"
                }
            }
            //,
            //submitHandler: function (form) {
            //    debugger
            //}
        });
        $('#frmAddCP').validate({
            rules: {
                fromdate: {
                    required: true
                },
                todate: {
                    required: true
                },
                vatin: {
                    required: true
                },
                vatout: {
                    required: true
                }
            },
            messages: {
                fromdate: {
                    required: "(*)"
                },
                todate: {
                    required: "(*)"
                },
                vatin: {
                    required: "(*)"
                },
                vatout: {
                    required: "(*)"
                }
            }

        });   
        function addCommas(x) {
            var parts = x.toString().split(".");
            parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            return parts.join(".");
        }
        $('.dongchiphi').on('click', function () {
            $('.showCP').show(500);
            $('#ModelCP').hide(500);
        });
        $("body").on("click", "#tbCP .fAdd", function () {
            var locktour = $("#txtLocktour").val();
            if (locktour.length > 0) {
                alert("Tour này đã khoá, không thể thêm chi phí");
                return;
            }
            else {
                CPControl.AddCP();
            }
        });

        $('#btAddEditCP').on('click', function () {
            debugger
            if ($('#frmEditCP').valid()) {
                var frmEditCP = $('#frmEditCP').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditCP",
                    data: frmEditCP,
                    dataType: "json",
                    success: function () {

                        CPControl.loadChiphikhac();
                    }
                    , error: function (xhr, status, error) {
                        alert("Có lỗi: " + error + " ,xin thông báo cho người quản lý biết!");
                    }
                });
            }
        });

        $('#btAddCP').off('click').on('click', function () {
            if ($('#frmAddCP').valid()) {
                var frmAddCP = $('#frmAddCP').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/AddCP",
                    data: frmAddCP,
                    dataType: "json",
                    success: function () {
                        CPControl.loadChiphikhac();
                    }
                    , error: function (xhr, status, error) {
                        alert("Có lỗi: " + error + " ,xin thông báo cho người quản lý biết!");
                    }
                });
            }
        });

        $('.ViewLogCP').on('click', function () { //khi click nut xem log
            var id = $(this).data('id'); //id chi phi khac        

            $.ajax({
                url: '/Tour/ViewLogCP',
                data: { id: id },
                type: 'GET',
                success: function (data) {

                    $('#ModalViewLogCP').modal('show');
                    $('.DetailLogCP').html(data);
                }
            });

        });

    },
    //Cập nhật ds khách
    EditCP: function (id) {
        $.ajax({
            url: '/Tour/EditCP',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showCP').hide(500);
                $('#ModelCP').show(500);
                $('.NoidungCP').html(data);
            }
        });
    },
    AddCP: function () {
        var sgtcode = $('#utxtId').val();
        $.ajax({
            url: '/Tour/AddCP',
            data: { code: sgtcode },
            type: 'GET',
            success: function (data) {
                $('.showCP').hide(500);
                $('#ModelCP').show(500);
                $('.NoidungCP').html(data);
            }
        });
    }
    ,
    loadChiphikhac: function () {

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
CPControl.init();