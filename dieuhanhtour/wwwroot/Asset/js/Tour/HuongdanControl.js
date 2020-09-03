var HuongdanControl = {
    init: function () {

        HuongdanControl.registerEvent();
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

        $(function () {
            $('#frmAddHuongDan').validate({
                rules: {
                    Batdau: {
                        required: true
                    },
                    Ketthuc: {
                        required: true
                    }
                },
                messages: {
                    Batdau: {
                        required: "(*)"
                    },
                    Ketthuc: {
                        required: "(*)"
                    }
                }

            });
        })
        
        function addCommas(x) {
            var parts = x.toString().split(".");
            parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            return parts.join(".");
        }
        $('.donghd').on('click', function () {
            $('.showYeucauhd').show(500);
            $('#ModelYeucauhd').hide(500);
        });
        $("body").on("click", "#tbHuongdan .fAdd", function () {
            HuongdanControl.AddHuongdan();
        });

        $('#btAddHuongdan').on('click', function () {
            
            var frmAddHd = $('#frmAddHuongDan').serialize();
            $.ajax({
                type: "POST",
                url: "/Tour/AddHuongdan",
                data: frmAddHd,
                dataType: "json",
                success: function () {

                    HuongdanControl.loadHuongdan();

                }
                , error: function (xhr, status, error) {
                    alert("Có lỗi: " + error + " ,xin thông báo cho người quản lý biết!");
                }
            });

        });

        $('#btEditHuongdan').on('click', function () {
            debugger

            var frmAddHd = $('#frmEditHuongdan').serialize();
            $.ajax({
                type: "POST",
                url: "/Tour/EditHuongdan",
                data: frmAddHd,
                dataType: "json",
                success: function () {

                    HuongdanControl.loadHuongdan();

                }
                , error: function (xhr, status, error) {
                    alert("Có lỗi: " + error + " ,xin thông báo cho người quản lý biết!");
                }
            });

        });

        $('.ViewLogHD').on('click', function () { //khi click nut xem log
            var id = $(this).data('id'); //sgtcode        

            $.ajax({
                url: '/Tour/ViewLogHuongDan',
                data: { id: id },
                type: 'GET',
                success: function (data) {

                    $('#ModalViewLogHd').modal('show');
                    $('.DetailLogHd').html(data);
                }
            });
        });
    },
    //Cập nhật yêu cầu xe
    EditHuongdan: function (id) {
        $.ajax({
            url: '/Tour/EditHuongdan',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showYeucauhd').hide(500);
                $('#ModelYeucauhd').show(500);
                $('.Yeucauhd').html(data);
            }
        });
    },
    AddHuongdan: function () {
        var sgtcode = $('#utxtId').val();
        $.ajax({
            url: '/Tour/AddHuongdan',
            data: { id: sgtcode },
            type: 'GET',
            success: function (data) {
                $('.showYeucauhd').hide(500);
                $('#ModelYeucauhd').show(500);
                $('.Yeucauhd').html(data);
            }
        });
    },
    loadHuongdan: function () {
        
        var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/ListHuongdan',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#dsHuongdan').html(data);
            }
        });
    }
};
HuongdanControl.init();