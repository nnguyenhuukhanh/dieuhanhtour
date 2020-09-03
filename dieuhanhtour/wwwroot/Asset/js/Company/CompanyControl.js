var CompanyControl = {
    init: function () {

        CompanyControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {
            $('.select2').select2();
            $(".datepicker").mask("99/99/9999");
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

            $('#frmCompany').validate({
                rules: {
                    'Company.name': {
                        required: true
                    },
                    'Company.fullname': {
                        required: true
                    },
                    'Company.nation': {
                        required: true
                    }
                },
                messages: {
                    'Company.name': {
                        required: 'Phải nhập tên đối tác'
                    },
                    'Company.fullname': {
                        required: 'Phải nhập tên đối tác'
                    },
                    'Company.nation': {
                        required: 'Phải nhập quốc gia'
                    }
                }
            });    

        });
        function addCommas(x) {
            var parts = x.toString().split(".");
            parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            return parts.join(".");
        }     
        //dropdownlist valid
        $.validator.addMethod('notEqual', function (value, element, arg) {
            return arg != value;
        }, 'Phải chọn một mục nào đó!');

       
    }
};
CompanyControl.init();