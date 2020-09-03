

var EditTourProgControl = {
    init: function () {

        EditTourProgControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {
            var select = $('#txtSelect').val();
            if (select > 0) {
                var selector = "#tbTourProg tr:eq(" + select + ")";
                $(selector).addClass('highlight')
            }
            $('[data-toggle="tooltip"]').tooltip();
                
            $('.select2').select2();
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
        function addCommas(x) {
            var parts = x.toString().split(".");
            parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            return parts.join(".");
        }
        $('#tbTourProg').on('click', 'tbody tr', function (event) {
            $(this).addClass('highlight').siblings().removeClass('highlight');
            var select = $(this).data('stt');
            $('#txtSelect').val(select);
        });
        $('#frmEditLunchProgTemp').validate({
            rules: {
                vatin: {
                    required: true
                },
                vatout: {
                    required: true
                },
                
                unitpricea: {
                    required: true
                },
                unitpricec: {
                    required: true
                },
                amount: {
                    required: true
                }
            },
            messages: {
                vatin: {
                    required: "(*)"
                },
                vatout: {
                    required: "(*)"
                },
               
                unitpricea: {
                    required: "(*)"
                },
                unitpricec: {
                    required: "(*)"
                },
                amount: {
                    required: "(*)"
                }
            }
        });
        $('#frmEditHotelProgTemp').validate({
            rules: {
                vatin: {
                    required: true
                },
                vatout: {
                    required: true
                },
                
                foc: {
                    required: true
                }
            },
            messages: {
                vatin: {
                    required: "(*)"
                },
                vatout: {
                    required: "(*)"
                },
                
                foc: {
                    required: "(*)"
                }
            }
        });
        $('#frmEditOverseaProgTemp').validate({
            rules: {
                vatin: {
                    required: true
                },
                vatout: {
                    required: true
                },
                
                unitpricea: {
                    required: true
                },
                unitpricec: {
                    required: true
                },
                amount: {
                    required: true
                }
            },
            messages: {
                vatin: {
                    required: "(*)"
                },
                vatout: {
                    required: "(*)"
                },
                
                unitpricea: {
                    required: "(*)"
                },
                unitpricec: {
                    required: "(*)"
                },
                amount: {
                    required: "(*)"
                }
            }
        });
        $('#frmEditShowProgTemp').validate({
            rules: {
                vatin: {
                    required: true
                },
                vatout: {
                    required: true
                },

                unitpricea: {
                    required: true
                },
                unitpricec: {
                    required: true
                },
                amount: {
                    required: true
                }
            },
            messages: {
                vatin: {
                    required: "(*)"
                },
                vatout: {
                    required: "(*)"
                },

                unitpricea: {
                    required: "(*)"
                },
                unitpricec: {
                    required: "(*)"
                },
                amount: {
                    required: "(*)"
                }
            }
        });
        $('#frmEditPacProgTemp').validate({
            rules: {
                vatin: {
                    required: true
                },
                vatout: {
                    required: true
                },
                
                unitpricea: {
                    required: true
                },
                unitpricec: {
                    required: true
                },
                amount: {
                    required: true
                }
            },
            messages: {
                vatin: {
                    required: "(*)"
                },
                vatout: {
                    required: "(*)"
                },
                
                unitpricea: {
                    required: "(*)"
                },
                unitpricec: {
                    required: "(*)"
                },
                amount: {
                    required: "(*)"
                }
            }
        });
        $('#frmEditShopProgTemp').validate({
            rules: {
                vatin: {
                    required: true
                },
                vatout: {
                    required: true
                },
                
                unitpricea: {
                    required: true
                },
                unitpricec: {
                    required: true
                },
                amount: {
                    required: true
                }
            },
            messages: {
                vatin: {
                    required: "(*)"
                },
                vatout: {
                    required: "(*)"
                },
                
                unitpricea: {
                    required: "(*)"
                },
                unitpricec: {
                    required: "(*)"
                },
                amount: {
                    required: "(*)"
                }
            }
        });
        $('#frmAddSrvTourProg').validate({
            rules: {
                date: {
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
                date: {
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
        $('#frmInsertSrvTourProg').validate({
            rules: {
                date: {
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
                date: {
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
        $('#frmEditAirProgTemp').validate({
            rules: {
                vatin: {
                    required: true
                },
                vatout: {
                    required: true
                },
                
                unitpricea: {
                    required: true
                },
                unitpricec: {
                    required: true
                }
            },
            messages: {
                vatin: {
                    required: "(*)"
                },
                vatout: {
                    required: "(*)"
                },
                
                unitpricea: {
                    required: "(*)"
                },
                unitpricec: {
                    required: "(*)"
                }
            }
        });
        $('#frmEditTrainProgTemp').validate({
            rules: {
                vatin: {
                    required: true
                },
                vatout: {
                    required: true
                },

                unitpricea: {
                    required: true
                },
                unitpricec: {
                    required: true
                }
            },
            messages: {
                vatin: {
                    required: "(*)"
                },
                vatout: {
                    required: "(*)"
                },

                unitpricea: {
                    required: "(*)"
                },
                unitpricec: {
                    required: "(*)"
                }
            }
        });
        $('#frmXoadichvu').validate({
            rules: {

                lydohuydv: {
                    required: true
                }
            },
            messages: {

                lydohuydv: {
                    required: "Nhập lý do huỷ"
                }
            }
        });
        $('#frmEditCarProgTemp').validate({
            rules: {
               
            },
            messages: {
                
            }
        });
        //them dich vu tham quan -----------------------------------------------------------
        $('#frmEditSightseeing').validate({
            rules: {
                vatin: {
                    required: true
                },
                vatout: {
                    required: true
                }
            },
            messages: {
                vatin: {
                    required: "(*)"
                },
                vatout: {
                    required: "(*)"
                }
            }
        });
        $('#frmEditTextProgTemp').validate({
            rules: {
               
               
            },
            messages: {
                
                
            }
        });
        
        $('#btCapnhatTourProg').on('click', function () {
            if ($('#frmEditTourProg').valid()) {
                var id = $('#txthidenCode').val();
                var frmEditTourProgTem = $('#frmEditTourProg').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditTourProg",
                    data: frmEditTourProgTem,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            $('#ModalTourProg').show(500);
                            EditTourProgControl.loadTourProgTemp(id);
                        }
                    }
                });
            }
        });      

        // hiển thị thông tin trước khi xoá dịch vụ TourProg

        $('.DelTourProg_').on('click', function () {
            var id = $(this).data('id');
            var locktour = $("#txtLocktour").val();
            if (locktour.length > 0) {
                alert("Tour đã bị khoá, không thể xoá");
                return;
            }
            else {
                var url = '/Tour/DelTourProg';
                $.get(url, { id: id }, function (data) {
                    $("#ModalDelTourProg").modal('show');
                    $(".DelTourProg").html(data);
                });
            }
        });

       

        $('.ViewLog').on('click', function () { //khi click nut xem log
            var id = $(this).data('id'); //sgtcode
            var cn = $(this).data('cn');
            var session = $(this).data('session');
            // if (session != cn) {
            //    alert("Bạn không có quyền xem chương trình này.")
            // }
            //else {
            var srvtype = $(this).data('srvtyp');

            $.ajax({
                url: '/Tour/ViewLog',
                data: { id: id },
                type: 'GET',
                success: function (data) {
                    // $('.showcttour').hide(500);
                    $('#ModalViewLog').modal('show');
                    $('.DetailLog').html(data);
                }
            });

            // }
        });
        $("body").on("click", "#tbTourProg .fAdd", function () {
            var locktour = $("#txtLocktour").val();
            if (locktour.length > 0) {
                $('.fAdd').removeClass('fAdd');
                alert('tour này đã đóng, không thể thêm dịch vụ')              
                return;
            }
            else {
                EditTourProgControl.AddTourSrvProg();
            }
        });
        //khi click nut them dich vu cua tour
        $('#btThemdvTourProg').on('click', function () {
            if ($('#frmAddSrvTourProg').valid()) {
                var id = $('#txtAddSrvCode').val();//id=sgtcode
                var srv = $('#ddlAddSrv').val();
                var frmAddSrvTourProg = $('#frmAddSrvTourProg').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/AddTourSrvProg",
                    data: frmAddSrvTourProg,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            var data = JSON.parse(response);
                            var id = data.Id;
                            switch (data.srvtype) {
                                case 'PAC':
                                    EditTourProgControl.EditPacProg(id);
                                    break;
                                case 'CAR':
                                case 'CAG':
                                    EditTourProgControl.EditCarProg(id);
                                    break;
                                case 'AIR':
                                    EditTourProgControl.EditAirProg(id);
                                    break;
                                case 'TRA':
                                    EditTourProgControl.EditTrainProg(id);
                                    break;
                                case 'SHP':
                                    EditTourProgControl.EditShopProg(id);
                                    break;
                                case 'LUN':
                                case 'BRK':
                                case 'DIN':
                                    EditTourProgControl.EditLunchProg(id);
                                    break;
                                case 'TXT':
                                    EditTourProgControl.loadTourProg();
                                    break;
                                case 'OTH':
                                    EditTourProgControl.EditOtherProg(id);
                                    break;
                                case 'HTL':
                                case 'CRU':
                                    EditTourProgControl.EditHotelProg(id);
                                    break;
                                case 'SSE':
                                    EditTourProgControl.EditSightseeing(id);
                                    break;
                                case 'SHW':
                                case 'MUS':
                                case 'WPU':
                                    EditTourProgControl.EditShowProg(id);
                                    break;
                                case 'ITI':
                                    EditTourProgControl.loadTourProg();
                                    break;
                                //case 'SHW':
                                //case 'MUS':
                                //case 'OVR':
                                //case 'SHW':
                                //    EditTourProgControl.EditOverseaProg(id);
                                //    break;
                                default:
                                    EditTourProgControl.EditOverseaProg(id);
                                    break;

                             }
                           
                        }
                    }
                });
            }
        });
        // Chèn dòng
        $('#btChendvTourProg').on('click', function () {
            if ($('#frmInsertSrvTourProg').valid()) {
                //var id = $('#txtAddSrvCode').val();//id=sgtcode
                //var srv = $('#ddlAddSrv').val();
                var frmInsertSrvTourProg = $('#frmInsertSrvTourProg').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/InsertTourSrvProg",
                    data: frmInsertSrvTourProg,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            var data = JSON.parse(response);
                            var id = data.Id;
                            switch (data.srvtype) {
                                case 'PAC':
                                    EditTourProgControl.EditPacProg(id);
                                    break;
                                case 'CAR':
                                case 'CAG':
                                    EditTourProgControl.EditCarProg(id);
                                    break;
                                case 'AIR':
                                    EditTourProgControl.EditAirProg(id);
                                    break;
                                case 'SHP':
                                    EditTourProgControl.EditShopProg(id);
                                    break;
                                case 'LUN':
                                case 'BRK':
                                case 'DIN':
                                    EditTourProgControl.EditLunchProg(id);
                                    break;
                                case 'TXT':
                                    EditTourProgControl.loadTourProg();
                                    break;
                                case 'OTH':
                                    EditTourProgControl.EditOtherProg(id);
                                    break;
                                case 'HTL':
                                    EditTourProgControl.EditHotelProg(id);
                                    break;
                                case 'SSE':
                                    EditTourProgControl.EditSightseeing(id);
                                    break;
                                //case 'SHW':
                                //case 'MUS':
                                //case 'OVR':
                                //case 'SHW':
                                //    EditTourProgControl.EditOverseaProg(id);
                                //    break;
                                default:
                                    EditTourProgControl.EditOverseaProg(id);
                                    break;

                            }

                        }
                    }
                });
            }
        });
        $('#btThemTextTourProgTemp').off('click').on('click', function () {
            if ($('#frmEditTextProgTemp').valid()) {
                var frmEditTextProgTemp = $('#frmEditTextProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditTextProg",
                    data: frmEditTextProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
            }
        });
        $('#btCapnhatHanhtrinhTourProgTemp').off('click').on('click', function () {
            var frmEditHanhtrinhProgTemp = $('#frmEditHanhtrinhProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditItineraryProg",
                    data: frmEditHanhtrinhProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
        });
        $('#btCapnhatCarTourProgTemp').off('click').on('click', function () {
            if ($('#frmEditCarProgTemp').valid()) {
                var frmEditCarProgTemp = $('#frmEditCarProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditCarProg",
                    data: frmEditCarProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
            }
           
        });
        $('#btCapnhatAirProgTemp').off('click').on('click', function () {
            if ($('#frmEditAirProgTemp').valid()) {
                var frmEditAirProgTemp = $('#frmEditAirProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditAirProg",
                    data: frmEditAirProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
            };
        });

        $('#btCapnhatTrainProgTemp').off('click').on('click', function () {
            debugger;
            if ($('#frmEditTrainProgTemp').valid()) {
                var frmEditTrainProgTemp = $('#frmEditTrainProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditTrainProg",
                    data: frmEditTrainProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
            };
        });

        $('#btCapnhatLunchProgTemp').off('click').on('click', function () {
            if ($('#frmEditLunchProgTemp').valid()) {
                var frmEditLunchProgTemp = $('#frmEditLunchProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditLunchProg",
                    data: frmEditLunchProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
            };
        });
        $('#btCapnhatOverseaProgTemp').off('click').on('click', function () {
            if ($('#frmEditOverseaProgTemp').valid()) {
                var frmEditOverseaProgTemp = $('#frmEditOverseaProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditOverSeaProg",
                    data: frmEditOverseaProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
            };
        });
        $('#btCapnhatShowProgTemp').off('click').on('click', function () {
            if ($('#frmEditShowProgTemp').valid()) {
                var frmEditShowProgTemp = $('#frmEditShowProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditShowProg",
                    data: frmEditShowProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
            };
        });
        $('#btCapnhatPacProgTemp').off('click').on('click', function () {
           
            if ($('#frmEditPacProgTemp').valid()) {
                var frmEditPacProgTemp = $('#frmEditPacProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditPacProg",
                    data: frmEditPacProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
            };
        });
        $('#btCapnhatShopProgTemp').off('click').on('click', function () {
            if ($('#frmEditShopProgTemp').valid()) {
                var frmEditShopProgTemp = $('#frmEditShopProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditShopProg",
                    data: frmEditShopProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
            };
        });
        $('#btCapnhatHotelProgTemp').off('click').on('click', function () {
            if ($('#frmEditHotelProgTemp').valid()) {
                var frmEditHotelProgTemp = $('#frmEditHotelProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditHotelProg",
                    data: frmEditHotelProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
            }
        });

       

        $('#btThemSightseeing').off('click').on('click', function () {
            if ($('#frmEditSightseeing').valid()) {
                var frmEditSightseeingTemp = $('#frmEditSightseeing').serialize();
                $.ajax({
                    type: "POST",
                    url: "/Tour/EditSightseeingProg",
                    data: frmEditSightseeingTemp,
                    dataType: "json",
                    success: function (response) {

                        if (response) {
                            EditTourProgControl.loadTourProg();
                        }
                    }
                });
            }
            });
   
        $("body").on("click", "#tbSee .fAdd", function () {
            var row = $(this).closest('tr');
            $('td', row).each(function () {
                var input = $(this).find('input');
                input.show();

                if ($(this).find(".select2").length > 0) {
                    
                    var span = $(this).find('span');
                    span.show();

                    $(this).find(".select2").select2().next().show();
                }

            });
            row.find('.fSeeUpdate').show();
            row.find('.fSeeCancel').show();

            $(this).hide();
        });



        $("body").on("click", "#tbSee .Edit", function () {

            var row = $(this).closest("tr");
            $("td", row).each(function () {
                if ($(this).find("input").length > 0) {

                    var input = $(this).find('input');
                    var span = $(this).find('span');
                    span.hide();
                    input.show();

                }

                if ($(this).find(".select2").length > 0) {
                    
                    var span = $(this).find('span');
                    span.hide();
                    // $('.select2').show();                   
                    $(this).find(".select2").select2().next().show();
                }

            });

            row.find(".Update").show();
            row.find(".Cancel").show();
            row.find(".Delete").hide();

            $(this).hide();
        });

        $("body").on("click", "#tbSee .Cancel", function () {
            var row = $(this).closest("tr");
            $("td", row).each(function () {
                if ($(this).find("input").length > 0) {
                    var span = $(this).find("span");
                    var input = $(this).find("input");
                    span.show();
                    input.hide();
                }

                if ($(this).find(".select2").length > 0) {
                    
                    var span = $(this).find('span');
                    span.show();
                    // $('.select2').show();                   
                    $(this).find(".select2").select2().next().hide();
                }

            });
            row.find(".Edit").show();
            row.find(".Update").hide();
            row.find(".Cancel").hide();
            row.find(".Delete").show();

            $(this).hide();
        });

        $("#tbSee .Delete").on("click", function () {
            var id = $(this).data('id');
            if (confirm("Xoá dịch vụ này?")) {
                $.ajax({
                    type: "POST",
                    url: "/Tour/DelSee",
                    data: { id: id }
                });
                $('#rowseetemp_' + id).remove();
            }
        });


        // khi click nut cancel
        $("body").on("click", "#tbSee .fSeeCancel", function () {
            var row = $(this).closest('tr');
            $('td', row).each(function () {
                var input = $(this).find('input');
                input.hide();

                if ($(this).find(".select2").length > 0) {

                    $(this).find(".select2").select2().next().hide();
                }

            });
            row.find('.fSeeUpdate').hide();
            row.find('.fAdd').show();

            $(this).hide();
        });

        //them moi
        $('.fSeeUpdate').on('click', function () {
            debugger
            var seetemp = {};
            seetemp.sgtcode = $("#hidCode").val();
            seetemp.stt = $("#hidStt").val();
            seetemp.Codedtq = $("#dldtqadd").val();
            seetemp.Serial = $(".txtserial").val();
            seetemp.Debit = $("#dldebitadd").val();
            seetemp.Pax = $(".txtpaxadd").val();
            seetemp.PaxPrice = $(".txtpaxpriceadd").val();
            seetemp.Childern = $(".txtchildernadd").val();
            seetemp.ChildernPrice = $(".txtchildernpriceadd").val();
            seetemp.Vatin = $(".txtvatin").val();
            seetemp.Vatout = $(".txtvatout").val();
            seetemp.httt = $("#dlhtttadd").val();
            $.ajax({
                type: "POST",
                url: "/Tour/AddSee",
                data: { entity: seetemp }
            });

            var row = $(this).closest("tr");
            $("td", row).each(function () {
                if ($(this).find("input").length > 0) {

                    var input = $(this).find('input');
                    var span = $(this).find('span');
                    span.show();
                    input.hide();

                }

                if ($(this).find(".select2").length > 0) {

                    var span = $(this).find('span');
                    span.show();
                    $(this).find(".select2").select2().next().hide();
                }

            });

            row.find(".fAdd").show();
            row.find(".fSeeUpdate").hide();
            row.find(".fSeeCancel").show();

            $(this).hide();
            var id = $('#txtIdSee').val();//Id Tourprogtemp           
            EditTourProgControl.EditSightseeing(id);//hien lai chi tiet dich vu tham quan
        });      

        //end dich vu tham quan ---------------------------------------------------------------------------------------

        // show input tbKsProgTemp
        $("body").on("click", "#tbKsProgTemp .Edit", function () {
            var row = $(this).closest("tr");
            $("td", row).each(function () {
                if ($(this).find("input").length > 0) {                  
                    var input = row.find('input');
                    var span = row.find('span');    
                    span.hide();
                    input.show();
                }
            });
            row.find(".Update").show();
            row.find(".Cancel").show();
            row.find(".Delete").hide();
            $(this).hide();
        });
        // Cancel Edit tbKsProgTemp
        $("body").on("click", "#tbKsProgTemp .Cancel", function () {
            var row = $(this).closest("tr");
            $("td", row).each(function () {
                var span = $(this).find("span");
                var input = $(this).find("input");             
                span.show();
                input.hide();
            });
            row.find(".Edit").show();
            row.find(".Delete").show();
            row.find(".Update").hide();
            $(this).hide();
        });
       //Cap nhat khach san
        $("body").on("click", "#tbKsProgTemp .Update", function () {
            var row = $(this).closest("tr");
            $("td", row).each(function () {
                if ($(this).find("input").length > 0) {
                    var span = $(this).find("span");
                    var input = $(this).find("input");                    
                    span.html(input.val());
                    span.show();                    
                    input.hide(); 
                }
            });
            row.find(".Edit").show();
            row.find(".Delete").show();
            row.find(".Cancel").hide();
            $(this).hide();
            var hoteltemp = {};
            hoteltemp.Id = row.find(".Id").find("span").html();
            hoteltemp.currency = row.find(".currency").find('span').html();
            hoteltemp.note = row.find(".note").find("span").html();
            hoteltemp.sgl = row.find(".sgl").find("span").html();
            hoteltemp.sglpax = row.find(".sglpax").find("span").html();
            hoteltemp.sglcost = row.find(".sglcost").find("span").html();
            hoteltemp.sglfoc = row.find(".sglfoc").find("span").html();
            hoteltemp.extsgl = row.find(".extsgl").find("span").html();
            hoteltemp.extsglcost = row.find(".extsglcost").find("span").html();

            hoteltemp.dbl = row.find(".dbl").find("span").html();
            hoteltemp.dblpax = row.find(".dblpax").find("span").html();
            hoteltemp.dblcost = row.find(".dblcost").find("span").html();
            hoteltemp.dblfoc = row.find(".dblfoc").find("span").html();
            hoteltemp.extdbl = row.find(".extdbl").find("span").html();
            hoteltemp.extdblcost = row.find(".extdblcost").find("span").html();

            hoteltemp.twn = row.find(".twn").find("span").html();
            hoteltemp.twnpax = row.find(".twnpax").find("span").html();
            hoteltemp.twncost = row.find(".twncost").find("span").html();
            hoteltemp.twnfoc = row.find(".twnfoc").find("span").html();
            hoteltemp.twndbl = row.find(".twndbl").find("span").html();
            hoteltemp.extwncost = row.find(".extwncost").find("span").html();

            hoteltemp.tpl = row.find(".tpl").find("span").html();
            hoteltemp.tplpax = row.find(".tplpax").find("span").html();
            hoteltemp.tplcost = row.find(".tplcost").find("span").html();
            hoteltemp.tplfoc = row.find(".tplfoc").find("span").html();
            hoteltemp.exttpl = row.find(".exttpl").find("span").html();
            hoteltemp.extplcost = row.find(".extplcost").find("span").html();

            hoteltemp.oth = row.find(".oth").find("span").html();
            hoteltemp.othpax = row.find(".othpax").find("span").html();
            hoteltemp.othcost = row.find(".othcost").find("span").html();
            hoteltemp.othtype = row.find(".othtype").find("span").html();

            //console.log(hoteltemp);

            $.ajax({
                type: "POST",
                url: "/Tour/EditHotel",
                //data: {entity: JSON.stringify(hoteltemp) },
                data: {entity: hoteltemp  }
                //contentType: "application/json; charset=utf-8",
                //dataType: "json"
            });
        });
        // Thêm khách sạn tour program template
        $("body").on("click", "#tbKsProgTemp .fAdd", function () {
            var row = $(this).closest('tr');
            $('td', row).each(function () {
                var input = $(this).find('input');
                input.show();
            });
            row.find('.fHotelTempUpdate').show();
            row.find('.fCancel').show();
            $(this).hide();
        });
        // Huy them khach san tour program template
        $("body").on("click", "#tbKsProgTemp .fCancel", function () {
            var row = $(this).closest('tr');
            $('td', row).each(function () {
                var input = $(this).find('input');
                input.hide();
            });
            row.find('.fHotelTempUpdate').hide();
            row.find('.fAdd').show();
            $(this).hide();
        });
        //$("body").on("click", "#tbKsProgTemp .Delete", function () {
        $("#tbKsProgTemp .Delete").on("click", function () {
            var id = $(this).data('id');
            if (confirm("Xoá dịch vụ này?")) {
                $.ajax({
                    type: "POST",
                    url: "/Tour/DelHotel",
                    data: { id: id }                  
                });
                $('#rowhotel_' + id).remove();
            }
        });

        //them moi hotel
        $('.fHotelTempUpdate').on('click', function () {
           
            var hoteltemp = {};
            hoteltemp.sgtcode = $(".txtcode").val();//sgtcode
            hoteltemp.stt = $(".txtstt").val();
            hoteltemp.currency =$(".txtcurrency").val();
            hoteltemp.note = $(".txtnote").val();
            hoteltemp.sgl = $(".txtsgl").val();
            hoteltemp.sglpax = $(".txtsglpax").val();
            hoteltemp.sglcost = $(".txtsglcost").val();
          //  hoteltemp.sglfoc = $(".txtsglfoc").val();
            hoteltemp.extsgl = $(".txtextsgl").val();
            hoteltemp.extsglcost = $(".txtextsglcost").val();

            hoteltemp.dbl = $(".txtdbl").val();
            hoteltemp.dblpax = $(".txtdblpax").val();
            hoteltemp.dblcost = $(".txtdblcost").val();
           // hoteltemp.dblfoc = $(".txtdblfoc").val();
            hoteltemp.extdbl = $(".txtextdbl").val();
            hoteltemp.extdblcost = $(".txtextdblcost").val();

            hoteltemp.twn = $(".txttwn").val();
            hoteltemp.twnpax = $(".txttwnpax").val();
            hoteltemp.twncost = $(".txttwncost").val();
           // hoteltemp.twnfoc = $(".txttwnfoc").val();
            hoteltemp.exttwn = $(".txtexttwn").val();
            hoteltemp.extwncost =$(".txtextwncost").val();

            hoteltemp.tpl = $(".txttpl").val();
            hoteltemp.tplpax = $(".txttplpax").val();
            hoteltemp.tplcost = $(".txttplcost").val();
           // hoteltemp.tplfoc = $(".txttplfoc").val();
            hoteltemp.exttpl = $(".txtexttpl").val();
            hoteltemp.extplcost = $(".txtextplcost").val();

            hoteltemp.oth = $(".txtoth").val();
            hoteltemp.othpax = $(".txtothpax").val();
            hoteltemp.othcost = $(".txtothcost").val();
            hoteltemp.othtype = $(".txtothtype").val();
                 $.ajax({
                type: "POST",
                url: "/Tour/AddHotel",                        
                data: { entity: hoteltemp }            
            });
            var id = $('#txtIdHotelProgTemp').val();
            EditTourProgControl.EditHotelProg(id);
        });

        $('.dong').on('click', function () {
            $('#ModalTourProg').hide(500);
            $('.showcttour').show(500);
        });
        $(function () {
            $('.select2').select2();
            $(".timeinput").inputmask("99:99");
        });
        $('.timSupplier').on('click', function () {
            $('.cboSupplier_').toggle(500);
            EditTourProgControl.loadCboSupplier();
        });
        $('#cboSupplier').on('change', function () {
            $('.txtNhacc').val($('#cboSupplier').val());
            $('.cboSupplier_').toggle(500);
        });
        $('.timSrvCode').on('click', function () {
            $('.cboSrvCode_').toggle(500);
            EditTourProgControl.loadCboSrvCode();
        });
        $('#cboSrvCode').on('change', function () {
            $('.txtSrvCode').val($('#cboSrvCode').val());
            $('.cboSrvCode_').toggle(500);
        });
    },
    loadCboSupplier: function () {
        $('#cboSupplier').html('');
        var option = '';
        $.ajax({
            url: '/Supplier/dsSupplier',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                //console.log(response.data);
                //var data = JSON.parse(response.data);
                var data = JSON.parse(JSON.stringify(response.data));
                $.each(data, function (i, item) {
                    option = option + '<option value="' + item.code + '">' + item.tengiaodich + '</option>';
                });
                $('#cboSupplier').html(option);
            }
        });
    },
    loadCboSrvCode: function () {
        $('#cboSrvCode').html('');
        var option = '';
        $.ajax({
            url: '/Supplier/dsSupplier',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                //console.log(response.data);
                //var data = JSON.parse(response.data);
                var data = JSON.parse(JSON.stringify(response.data));
                $.each(data, function (i, item) {
                    option = option + '<option value="' + item.code + '">' + item.tengiaodich + '</option>';
                });
                $('#cboSrvCode').html(option);
            }
        });
    },
    //Load chương trình tour
    loadTourProg: function () {
        var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/listTourProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#ModalTourProg').hide(500);
                $('.showcttour').show(500);
                $('#cttour').html(data);
            }
        });
    },
    //Cập nhật nhà hàng cho chương trình tour
    EditLunchProg: function (id) {
        //var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/EditLunchProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    //  //Cập nhật nối tour nước ngoài cho chương trình tour
    EditOverseaProg: function (id) {
        $.ajax({
            url: '/Tour/EditOverSeaProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
     //Cập nhật tour trọn gói cho chương trình tour
    EditPacProg: function (id) {
        $.ajax({
            url: '/Tour/EditPacProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    //Cập nhật shopping cho chương trình tour
    EditShopProg: function (id) {
        $.ajax({
            url: '/Tour/EditShopProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    //Cập nhật air cho chương trình tour
    EditAirProg: function (id) {
        $.ajax({
            url: '/Tour/EditAirProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    //Cập nhật Train cho chương trình tour
    EditTrainProg: function (id) {
        $.ajax({
            url: '/Tour/EditTrainProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    //Cập nhật Shows cho chương trình tour
    EditShowProg: function (id) {
        $.ajax({
            url: '/Tour/EditShowProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    //Cập nhật dịch vụ khách sạn cho chương trình tour
    EditHotelProg: function (id) {
        $.ajax({
            url: '/Tour/EditHotelProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    //Cập nhật dịch vụ khác cho chương trình tour
    EditOtherProg: function (id) {
        $.ajax({
            url: '/Tour/EditOtherProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    //Cập nhật text cho chương trình tour
    EditTextProg: function (id) {
        //var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/EditTextProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
      //Cập nhật dich vu tham quan cho chương trình tour
     EditSightseeing: function (id) {
        $.ajax({
            url: '/Tour/EditSightseeing',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    EditCarProg: function (id) {
        //var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/EditCarProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    EditItineraryProg: function (id) {
        //var id = $('#utxtId').val();
        $.ajax({
            url: '/Tour/EditItineraryProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    // Tạo service cho chương trình tour
    AddTourSrvProg: function () {
        var sgtcode = $('#utxtId').val();
        $.ajax({
            url: '/Tour/AddTourSrvProg',
            data: { id: sgtcode },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProg').show(500);
                $('.TourProg').html(data);
            }
        });
    },
    
};
EditTourProgControl.init();