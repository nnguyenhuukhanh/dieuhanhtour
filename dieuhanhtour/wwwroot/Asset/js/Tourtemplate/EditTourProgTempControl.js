var EditTourProgTempControl = {
    init: function () {

        EditTourProgTempControl.registerEvent();
    },

    registerEvent: function () {
        $(document).ready(function () {
            $('.select2').select2();
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
        });
        function addCommas(x) {
            var parts = x.toString().split(".");
            parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            return parts.join(".");
        }
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
        $('.DelTourProgTemp').on('click', function () {
            var id = $(this).data('id');
            var srvtype = $(this).data('srvtype');
            if (confirm("Xoá dịch vụ " + srvtype + " này?")) {
                $.ajax({
                    type: "POST",
                    url: "/TourTemplate/DelTourSrvProgTemp",
                    data: { id: id }
                });
                $('#row_' + id).remove();
            }
           
        });

        $('#btCapnhatTourProgTemp').on('click', function () {
            if ($('#frmEditTourProgTem').valid()) {
                var id = $('#txthidenCode').val();
                var frmEditTourProgTem = $('#frmEditTourProgTem').serialize();
                $.ajax({
                    type: "POST",
                    url: "/TourTemplate/EditTourProgTemp",
                    data: frmEditTourProgTem,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            $('#ModalTourProgTemp').show(500);
                            EditTourProgTempControl.loadTourProgTemp(id);
                        }
                    }
                });
            }
        });

        $('.EditProgTemp').on('click', function () {
            var id = $(this).data('id');
            var srvtype = $(this).data('srvtyp');
            switch (srvtype) {
                case 'LUN':
                case 'DIN':
                case 'BRK':
                    EditTourProgTempControl.EditLunchProgTemp(id);
                    break;
                case 'OVR':
                case 'WPU':
                case 'TRS':
                    EditTourProgTempControl.EditOverseaProgTemp(id);
                    break;
                case 'PAC':
                    EditTourProgTempControl.EditPacProgTemp(id);
                    break;
                case 'HTL':
                case "CRU":
                    EditTourProgTempControl.EditHotelProgTemp(id);
                    break;
                case 'OTH':
                    EditTourProgTempControl.EditOtherProgTemp(id);
                    break;
                case 'TXT':
                case 'GUI':
                    EditTourProgTempControl.EditTextProgTemp(id);
                    break;
                case 'CAR':
                case 'CAG':
                    EditTourProgTempControl.EditCarProgTemp(id);
                    break;
                case 'AIR':
                    EditTourProgTempControl.EditAirProgTemp(id);
                    break;
                case 'TRA':
                    EditTourProgTempControl.EditTrainProgTemp(id);
                    break;
                case 'SHP':
                    EditTourProgTempControl.EditShopProgTemp(id);
                    break;
                case 'SSE':
                    EditTourProgTempControl.EditSightseeingTemp(id);
                    break;
                case 'ITI':
                    EditTourProgTempControl.EditItineraryTempProg(id);
                    break;
                default:
                    //EditTourProgTempControl.loadTourProgTemp();
                    EditTourProgTempControl.EditOverseaProgTemp(id);
                    break;
            }
        });       
        $("body").on("click", "#tbTourProgTemp .fAdd", function () {
           EditTourProgTempControl.AddTourSrvProgTemp();
        });
        $('#btThemdvTourProgTemp').on('click', function () {
            if ($('#frmAddSrvTourProg').valid()) {
                var id = $('#txtAddSrvCode').val();
                var srv = $('#ddlAddSrv').val();
                var frmAddSrvTourProg = $('#frmAddSrvTourProg').serialize();
                $.ajax({
                    type: "POST",
                    url: "/TourTemplate/AddTourSrvProgTemp",
                    data: frmAddSrvTourProg,
                    dataType: "json",
                    success: function (response) {
                        //if (response) {
                        //    //$('#ModalTourProgTemp').show(500);
                        //    EditTourProgTempControl.loadTourProgTemp(id);
                        //}
                        if (response) {
                            debugger;
                            var data = JSON.parse(response);
                            console.log(data);
                            var id = data.Id;
                            switch (data.srvtype) {
                                case 'PAC':
                                    EditTourProgTempControl.EditPacProgTemp(id);
                                    break;
                                case 'CAR':
                                case 'CAG':
                                    EditTourProgTempControl.EditCarProgTemp(id);
                                    break;
                                case 'AIR':
                                    EditTourProgTempControl.EditAirProgTemp(id);
                                    break;
                                case 'SHP':
                                    EditTourProgTempControl.EditShopProgTemp(id);
                                    break;
                                case 'LUN':
                                case 'BRK':
                                case 'DIN':
                                    EditTourProgTempControl.EditLunchProgTemp(id);
                                    break;
                                case 'TXT':
                                    EditTourProgTempControl.EditTextProgTemp(id);
                                    break;
                                case 'OTH':
                                    EditTourProgTempControl.EditOtherProgTemp(id);
                                    break;
                                case 'HTL':
                                    EditTourProgTempControl.EditHotelProgTemp(id);
                                    break;
                                case 'SSE':
                                    EditTourProgTempControl.EditSightseeingTemp(id);
                                    break;
                                case 'ITI':
                                    EditTourProgTempControl.loadTourProgTemp();
                                    break;
                                //case 'SHW':
                                //case 'MUS':
                                //case 'OVR':
                                //case 'SHW':
                                //    EditTourProgControl.EditOverseaProg(id);
                                //    break;
                                default:
                                    EditTourProgTempControl.EditOverseaProgTemp(id);
                                    break;

                            }

                        }
                    }
                });
            }
        });
        $('#btThemTextTourProgTemp').on('click', function () {            
            var frmEditTextProgTemp = $('#frmEditTextProgTemp').serialize();
            $.ajax({
                type: "POST",
                url: "/TourTemplate/EditTextProgTemp",
                data: frmEditTextProgTemp,
                dataType: "json",
                success: function (response) {
                    if (response) {
                        EditTourProgTempControl.loadTourProgTemp();
                    }
                }
            });

        });
        $('#btCapnhatHanhtrinhTourProgTemp').on('click', function () {
            var frmEditHanhtrinhProgTemp = $('#frmEditHanhtrinhProgTemp').serialize();
            $.ajax({
                type: "POST",
                url: "/TourTemplate/EditItineraryProg",
                data: frmEditHanhtrinhProgTemp,
                dataType: "json",
                success: function (response) {
                    if (response) {
                        EditTourProgTempControl.loadTourProgTemp();
                    }
                }
            });

        });
        $('#btCapnhatCarTourProgTemp').on('click', function () {
            var frmEditCarProgTemp = $('#frmEditCarProgTemp').serialize();
            $.ajax({
                type: "POST",
                url: "/TourTemplate/EditCarProgTemp",
                data: frmEditCarProgTemp,
                dataType: "json",
                success: function (response) {
                    if (response) {
                        EditTourProgTempControl.loadTourProgTemp();
                    }
                }
            });
        });
        $('#btCapnhatAirProgTemp').on('click', function () {
            if ($('#frmEditAirProgTemp').valid()) {
                var frmEditAirProgTemp = $('#frmEditAirProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/TourTemplate/EditAirProgTemp",
                    data: frmEditAirProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgTempControl.loadTourProgTemp();
                        }
                    }
                });
            };
        });
        $('#btCapnhatTrainProgTemp').on('click', function () {
            if ($('#frmEditTrainProgTemp').valid()) {
                var frmEditTrainProgTemp = $('#frmEditTrainProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/TourTemplate/EditTrainProgTemp",
                    data: frmEditTrainProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgTempControl.loadTourProgTemp();
                        }
                    }
                });
            };
        });
        $('#btCapnhatLunchProgTemp').on('click', function () {
            if ($('#frmEditLunchProgTemp').valid()){
                var frmEditLunchProgTemp = $('#frmEditLunchProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/TourTemplate/EditLunchProgTemp",
                    data: frmEditLunchProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgTempControl.loadTourProgTemp();
                        }
                    }
                });
            };
        });
        $('#btCapnhatOverseaProgTemp').on('click', function () {
            if ($('#frmEditOverseaProgTemp').valid()) {
                var frmEditOverseaProgTemp = $('#frmEditOverseaProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/TourTemplate/EditOverseaProgTemp",
                    data: frmEditOverseaProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgTempControl.loadTourProgTemp();
                        }
                    }
                });
            };
        });
        $('#btCapnhatPacProgTemp').on('click', function () {
            if ($('#frmEditPacProgTemp').valid()) {
                var frmEditPacProgTemp = $('#frmEditPacProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/TourTemplate/EditPacProgTemp",
                    data: frmEditPacProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgTempControl.loadTourProgTemp();
                        }
                    }
                });
            };
        });
        $('#btCapnhatShopProgTemp').on('click', function () {
            if ($('#frmEditShopProgTemp').valid()) {
                var frmEditShopProgTemp = $('#frmEditShopProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/TourTemplate/EditShopProgTemp",
                    data: frmEditShopProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgTempControl.loadTourProgTemp();
                        }
                    }
                });
            };
        });
        $('#btCapnhatHotelProgTemp').on('click', function () {
            if ($('#frmEditHotelProgTemp').valid()) {
                var frmEditHotelProgTemp = $('#frmEditHotelProgTemp').serialize();
                $.ajax({
                    type: "POST",
                    url: "/TourTemplate/EditHotelProgTemp",
                    data: frmEditHotelProgTemp,
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            EditTourProgTempControl.loadTourProgTemp();
                        }
                    }
                });
            }
        });

        //them dich vu tham quan -----------------------------------------------------------
        $('#frmEditSightseeingTemp').validate({
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

        $('#btThemSightseeingTemp').on('click', function () {
            var frmEditSightseeingTemp = $('#frmEditSightseeingTemp').serialize();
            $.ajax({
                type: "POST",
                url: "/TourTemplate/EditSightseeingProgTemp",
                data: frmEditSightseeingTemp,
                dataType: "json",
                success: function (response) {
                    debugger
                    if (response) {
                        EditTourProgTempControl.loadTourProgTemp();
                    }
                }
            });

        });

        $("body").on("click", "#tbSeetmp .fAdd", function () {
            var row = $(this).closest('tr');
            $('td', row).each(function () {
                var input = $(this).find('input');
                input.show();

                if ($(this).find(".select2").length > 0) {
                    debugger
                    var span = $(this).find('span');
                    span.show();

                    $(this).find(".select2").select2().next().show();
                }

            });
            row.find('.fSeeTempUpdate').show();
            row.find('.fSeeCancel').show();

            $(this).hide();
        });



        $("body").on("click", "#tbSeetmp .Edit", function () {

            var row = $(this).closest("tr");
            $("td", row).each(function () {
                if ($(this).find("input").length > 0) {

                    var input = $(this).find('input');
                    var span = $(this).find('span');
                    span.hide();
                    input.show();

                }

                if ($(this).find(".select2").length > 0) {
                    debugger
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

        $("body").on("click", "#tbSeetmp .Cancel", function () {
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

        $("#tbSeetmp .Delete").on("click", function () {
            var id = $(this).data('id');
            if (confirm("Xoá dịch vụ này?")) {
                $.ajax({
                    type: "POST",
                    url: "/TourTemplate/DelSeeTemp",
                    data: { id: id }
                });
                $('#rowseetemp_' + id).remove();
            }
        });


        // Huy them khach san tour program template
        $("body").on("click", "#tbSeetmp .fSeeCancel", function () {
            var row = $(this).closest('tr');
            $('td', row).each(function () {
                var input = $(this).find('input');
                input.hide();

                if ($(this).find(".select2").length > 0) {

                    $(this).find(".select2").select2().next().hide();
                }

            });
            row.find('.fSeeTempUpdate').hide();
            row.find('.fAdd').show();

            $(this).hide();
        });

        //them moi
        $('.fSeeTempUpdate').on('click', function () {
            debugger
            var seetemp = {};
            seetemp.Code = $("#hidCode").val();
            seetemp.stt = $("#hidStt").val();
            seetemp.Codedtq = $("#dldtqadd").val();
           
            seetemp.Serial = $(".txtserial").val();
            seetemp.PaxPrice = $(".txtPaxprice").val();
            seetemp.ChildernPrice = $(".txtChildernprice").val();
            seetemp.Vatin = $(".txtvatin").val();
            seetemp.Vatout = $(".txtvatout").val();
            seetemp.httt = $("#dlhtttadd").val();
            $.ajax({
                type: "POST",
                url: "/TourTemplate/AddSeetemp",
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
            row.find(".fSeeTempUpdate").hide();
            row.find(".fSeeCancel").show();

            $(this).hide();
            var id = $('#txtIdSee').val();//Id Tourprogtemp           
            EditTourProgTempControl.EditSightseeingTemp(id);//hien lai chi tiet dich vu tham quan
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
                url: "/TourTemplate/EditHotelTemp",
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
                    url: "/TourTemplate/DelHotelTemp",
                    data: { id: id }                  
                });
                $('#rowhoteltemp_' + id).remove();
            }
        });

        
        $('.fHotelTempUpdate').on('click', function () {
           
            var hoteltemp = {};
            hoteltemp.Code = $(".txtcode").val();
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
                url: "/TourTemplate/AddHotelTemp",                        
                data: { entity: hoteltemp }            
            });
            var id = $('#txtIdHotelProgTemp').val();
            EditTourProgTempControl.EditHotelProgTemp(id);
        });

        $('.dong').on('click', function () {
            $('#ModalTourProgTemp').hide(500);
            $('.showcttour').show(500);
        });
        $(function () {
            $('.select2').select2();
            $(".timeinput").inputmask("99:99");
        });
        $('.timSupplier').on('click', function () {
            $('.cboSupplier_').toggle(500);
            EditTourProgTempControl.loadCboSupplier();
        });
        $('#cboSupplier').on('change', function () {
            $('.txtNhacc').val($('#cboSupplier').val());
            $('.cboSupplier_').toggle(500);
        });
        $('.timSrvCode').on('click', function () {
            $('.cboSrvCode_').toggle(500);
            EditTourProgTempControl.loadCboSrvCode();
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
    loadTourProgTemp: function () {
        var id = $('#utxtId').val();
        $.ajax({
            url: '/TourTemplate/listTourProgTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('#ModalTourProgTemp').hide(500);
                $('.showcttour').show(500);
                $('#cttour').html(data);
            }
        });
    },
    //Cập nhật nhà hàng cho chương trình tour
    EditLunchProgTemp: function (id) {
        //var id = $('#utxtId').val();
        $.ajax({
            url: '/TourTemplate/EditLunchProgTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
    //  //Cập nhật nối tour nước ngoài cho chương trình tour
    EditOverseaProgTemp: function (id) {
        $.ajax({
            url: '/TourTemplate/EditOverSeaProgTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
     //Cập nhật tour trọn gói cho chương trình tour
    EditPacProgTemp: function (id) {
        $.ajax({
            url: '/TourTemplate/EditPacProgTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
    //Cập nhật shopping cho chương trình tour
    EditShopProgTemp: function (id) {
        $.ajax({
            url: '/TourTemplate/EditShopProgTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
    //Cập nhật air cho chương trình tour
    EditAirProgTemp: function (id) {
        $.ajax({
            url: '/TourTemplate/EditAirProgTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
    EditTrainProgTemp: function (id) {
        $.ajax({
            url: '/TourTemplate/EditTrainProgTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
    //Cập nhật dịch vụ khách sạn cho chương trình tour
    EditHotelProgTemp: function (id) {
        $.ajax({
            url: '/TourTemplate/EditHotelProgTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
    //Cập nhật dịch vụ khác cho chương trình tour
    EditOtherProgTemp: function (id) {
        $.ajax({
            url: '/TourTemplate/EditOtherProgTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
    //Cập nhật text cho chương trình tour
    EditTextProgTemp: function (id) {
        //var id = $('#utxtId').val();
        $.ajax({
            url: '/TourTemplate/EditTextProgTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
    EditItineraryTempProg: function (id) {
        //var id = $('#utxtId').val();
        $.ajax({
            url: '/TourTemplate/EditItineraryProg',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
      //Cập nhật dich vu tham quan cho chương trình tour
     EditSightseeingTemp: function (id) {
       debugger
        $.ajax({
            url: '/TourTemplate/EditSightseeingTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
    EditCarProgTemp: function (id) {
        //var id = $('#utxtId').val();
        $.ajax({
            url: '/TourTemplate/EditCarProgTemp',
            data: { id: id },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
    // Tạo service cho chương trình tour
    AddTourSrvProgTemp: function () {
        var code = $('#utxtId').val();
        $.ajax({
            url: '/TourTemplate/AddTourSrvProgTemp',
            data: { id: code },
            type: 'GET',
            success: function (data) {
                $('.showcttour').hide(500);
                $('#ModalTourProgTemp').show(500);
                $('.TourProgTemp').html(data);
            }
        });
    },
    //deleteTourProgTemp: function (id) {
    //    $.ajax({
    //        url: '/TourTemplate/DelTourSrvProgTemp',
    //        data: {id:id },
    //        type: 'POST',
    //        success: function (response) {
    //            if (response.status) {
    //                bootbox.alert({
    //                    size: "small",
    //                    title: "THÔNG BÁO",
    //                    message: "Đã xóa thành công.",
    //                    callback: function () {
    //                    }
    //                })
    //            }
    //            else {
    //                bootbox.alert({
    //                    size: "small",
    //                    title: "DELETE INFO",
    //                    message: "Xoá dịch vụ lỗi."
    //                })
    //            }
    //        },
    //        error: function (err) {
    //            console.log(err);
    //        }
    //    });
    //},
};
EditTourProgTempControl.init();