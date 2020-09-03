var BookedControl = {
    init: function () {

        BookedControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {
            $('#tbBooking').on('click', 'tbody tr', function (event) {
                $(this).addClass('highlight').siblings().removeClass('highlight');
                var supplierid = $(this).data('code');             
                var sgtcode = $(this).data('sgtcode');
                var name = $(this).data('name');
                $('#Email').html($(this).data('email'));
                var url = "/Tour/getBookingByCode";
               
                $.get(url, { sgtcode: sgtcode, supplierid: supplierid }, function (response) {
                    var data = JSON.parse(response);
                    if (data != null) {
                        $('.txtSgtcode').val(sgtcode);
                        $('#txtSupplierId').val(supplierid);
                        $('#txtSupplierId_').val(supplierid);
                        $('#txtTimes').val(data.Times);
                        $('#txtBooking').val(data.Booking);
                        $('#txtNote').val(data.Note);
                        //$('#txtNote_').val(data.Note);
                        $('#txtProfile').val(data.Profile);
                        $('#txtName').val(name);    
                        $('#txtLogfile').val(data.Logfile);
                        $('#txtLogfile_').val(data.Logfile);
                        $('#Email').val(data.Email);
                    } else {
                        $('#txtSgtcode').val(sgtcode);
                        $('#txtSupplierId').val(supplierid);
                        $('#txtTimes').val('0');
                        $('#txtBooking').val('');
                        $('#txtProfile').val('');
                        $('#txtNote').val('');
                        $('#txtName').val(name);
                    }
                });
            })
            
        });
        
        
        //$('.exportBookingToWord').off('click').on('click', function () {
        //    alert('a');
        //});
        $('#btWord').off('click').on('click', function () {
            var supplierid = $("#txtSupplierId").val();
            var sgtcode = $('#txtSgtcode').val();
            if (supplierid.length == 0) {
                alert("Vui lòng chọn nhà cung cấp");
                return;
            }
            else {
                $('#txtNote_').val($('#txtNote').val());
                $('#frmWord').submit();               
            }
            
        });
        $('#btSaveBooking').off('click').on('click', function () {
            var supplierid = $("#txtSupplierId").val();
            if (supplierid.length == 0) {
                alert("Vui lòng chọn nhà cung cấp");
                return;
            }
            else {

                $('#frmBooking').submit();
            }

        });
    }
};
BookedControl.init();