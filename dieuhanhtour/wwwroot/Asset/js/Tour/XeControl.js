var XeControl = {
    init: function () {

        XeControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {

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

            $.validator.addMethod('notEqual', function (value, element, arg) {
                return arg != value;
            }, 'Phải chọn một mục nào đó!');

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
            $('#frmEditXe').validate({
                rules: {
                    Ngaydon: {
                        required: true
                    },
                    Denngay: {
                        required: true
                    },
                    Sokhach: {
                        required: true,
                        number: true
                    },
                    Giodon: {
                        required: true
                    },
                    Diemdon: {
                        required: true
                    }
                    ,
                    Loaixe: {
                        notEqual: ""
                    }
                },
                messages: {
                    Ngaydon: {
                        required: "(*)"
                    },
                    Denngay: {
                        required: "(*)"
                    },
                    Sokhach: {
                        required: "(*)",
                        number: "Nhập kiểu số nhé!"
                    },
                    Giodon: {
                        required: "(*)"
                    },
                    Diemdon: {
                        required: "(*)"
                    }
                    ,
                    Loaixe: {
                        notEqual: "(*)"
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

        $('.dongxe').on('click', function () {

            $('.showYeucauxe').show(500);
            $('#ModelYeucauxe').hide(500);
        });
        $("body").on("click", "#tbXe .fAdd", function () {
            XeControl.AddXe();
        });

        $('#btAddEditXe').on('click', function () {
            var frmAddXe = $('#frmAddXe').serialize();
            $.ajax({
                type: "POST",
                url: "/Tour/AddXe",
                data: frmAddXe,
                dataType: "json",
                success: function () {

                    XeControl.loadYeucauxe();

                }
                , error: function (xhr, status, error) {
                    alert("Có lỗi: " + error + " ,xin thông báo cho người quản lý biết!");
                }
            });

        });

        $('#btEditXe').on('click', function () {


            var frmEditXe = $('#frmEditXe').serialize();

            $.ajax({
                type: "POST",
                url: "/Tour/EditXe",
                data: frmEditXe,
                dataType: "json",
                success: function () {

                    XeControl.loadYeucauxe();
                }
                , error: function (xhr, status, error) {
                    alert("Có lỗi: " + error + " ,xin thông báo cho người quản lý biết!");
                }
            });

        });

        $('.ViewLogDx').on('click', function () { //khi click nut xem log
            var id = $(this).data('id'); //sgtcode        

            $.ajax({
                url: '/Tour/ViewLogDieuXe',
                data: { id: id },
                type: 'GET',
                success: function (data) {

                    $('#ModalViewLogDx').modal('show');
                    $('.DetailLogDx').html(data);
                }
            });

        });

    },
    //Cập nhật yêu cầu xe
    EditXe: function (id) {
        $.ajax({
            url: '/Tour/EditXe',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showYeucauxe').hide(500);
                $('#ModelYeucauxe').show(500);
                $('.Yeucauxe').html(data);
            }
        });
    },
    AddXe: function () {
        var sgtcode = $('#utxtId').val();
        $.ajax({
            url: '/Tour/AddXe',
            data: { code: sgtcode },
            type: 'GET',
            success: function (data) {
                $('.showYeucauxe').hide(500);
                $('#ModelYeucauxe').show(500);
                $('.Yeucauxe').html(data);
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
    }
};
XeControl.init();