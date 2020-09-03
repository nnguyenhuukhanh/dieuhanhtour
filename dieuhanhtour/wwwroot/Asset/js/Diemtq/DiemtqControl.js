var DiemtqControl = {
    init: function () {

        DiemtqControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {
            
        });
        
        $('.ddlTinh').off('change').on('change', function () {
            var matinh = $('.ddlTinh option:selected').val();
            var url = "/Diemtq/newCode";
            var url1 = "GetThanhphoByTinh";
            $.get(url, { matinh: matinh }, function (data) {
                if (data != null) {
                    $('#txtCodeDiemtq').val(data);
                }
            });

            var ddlThanhpho = $('.ddlThanhpho');
            ddlThanhpho.empty();
            $.getJSON(url1, { matinh: matinh }, function (thanhphos) {
                //console.log(thanhphos);
                //debugger;
                $.each(thanhphos, function (index, thanhpho) {
                    ddlThanhpho.append($('<option/>', {
                        value: thanhpho.matp,
                        text: thanhpho.tentp
                    }));
                });
            });

        });

    }
};
DiemtqControl.init();