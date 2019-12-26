var _globalObject;
$("button.btn-account-open-search").on('click', function (e) {
    debugger;

    e.stopImmediatePropagation();
    _globalObject = $(this).closest("div.account-number-div").find(".account-aumber");
    var filterAccount = _globalObject.attr("accountFilter")
    var accountType =_globalObject.attr("accountType")
    var accountNumber = $(this).closest('div.account-number-div').find('.account-id').val();
    $.ajax({
        type: 'GET',
        url: "/Teller/AccountNumberSearch",
        data: {
            accountNumber: accountNumber,
            filterAccount: filterAccount,
            accountType: accountType
        },
        success: function (result) {
            $('#account-pop-up-div').html(result).modal({
                'show': true,
                'backdrop': 'static'
            });
        },
        error: function () {

        }
    });
});
$(".account-aumber").on('change', function (e) {
    if ($(this).val() == "") {
        return;
    }
    e.stopImmediatePropagation();
    _globalObject = $(this).closest("div.account-number-div").find(".account-aumber").attr("id");

    var accountNumber = $(this).val();
    var data = $(this).attr('showwith');
    var filterAccount = $(this).attr('accountFilter');
    var accountType = $(this).attr('accounttype');
    var viewDetailsBtn = $("button#btn-account-view-details-" + _globalObject)
    $.ajax({
        type: 'GET',
        url: "/Teller/GetAccountNumber",
        data: {
            accountNumber: accountNumber,

        },
        success: function (result) {
            var count = result.length;
            if (count == 1) {

                $(".account-aumber#" + _globalObject).val(result[0].Accno);
                $(".account-id#" + _globalObject).val(result[0].IAccno);
                if (data != "NoDisplay") {
                    $('#account-details-show-div').addClass('hidden');
                    $(viewDetailsBtn).find('.fa-toggle-off').addClass('hidden');
                    $(viewDetailsBtn).find('.fa-toggle-on').removeClass('hidden');
                    $(viewDetailsBtn).attr('Action', 'Show')

                    $(viewDetailsBtn).removeClass('hidden');
                }
            } else {
                $.ajax({
                    type: 'GET',
                    url: "/Teller/AccountNumberSearch",
                    data: {
                        accountNumber: accountNumber,
                        filterAccount: filterAccount,
                        accountType: accountType
                    },
                    success: function (result) {
                        $('#account-pop-up-div').html(result).modal({
                            'show': true,
                            'backdrop': 'static'
                        });
                    },
                    error: function () {

                    }
                });
            }


        },
        error: function () {

        }
    });
});
$(".account-aumber").on('keyup', function (e) {

    e.stopImmediatePropagation();
    _globalObject = $(this).closest("div.account-number-div").find(".account-aumber").attr("id");
    var accountNumber = $(this).val();
    var data = $(this).attr('showwith');
    var filterAccount = $(this).attr('accountFilter');
    var accountType = $(this).attr('accounttype');
    var viewDetailsBtn = $("button#btn-account-view-details-" + _globalObject)
    if (e.keyCode == 8) {
        $(".account-aumber#" + _globalObject).val("");
        $(".account-id#" + _globalObject).val("");
    }
    if (e.keyCode == 13) {


        $.ajax({
            type: 'GET',
            url: "/Teller/GetAccountNumber",
            data: { accountNumber: accountNumber },
            success: function (result) {
                var count = result.length;
                if (count == 1) {
                    $(".account-aumber#" + _globalObject).val(result[0].Accno);
                    $(".account-id#" + _globalObject).val(result[0].IAccno);
                    if (data != "NoDisplay") {
                        $('#account-details-show-div').addClass('hidden');
                        $(viewDetailsBtn).find('.fa-toggle-off').addClass('hidden');
                        $(viewDetailsBtn).find('.fa-toggle-on').removeClass('hidden');
                        $(viewDetailsBtn).attr('Action', 'Show')

                        $(viewDetailsBtn).removeClass('hidden');
                    }
                } else {
                    $.ajax({
                        type: 'GET',
                        url: "/Teller/AccountNumberSearch",
                        data: {
                            accountNumber: accountNumber,
                            filterAccount: filterAccount,
                            accountType: accountType
                        },
                        success: function (result) {
                            $('#account-pop-up-div').html(result).modal({
                                'show': true,
                                'backdrop': 'static'
                            });
                        },
                        error: function () {

                        }
                    });
                }


            },
            error: function () {

            }
        });
    }
});
$(document).on('click', '.table-click-account-number-search table tr', function (e) {
 
    e.stopImmediatePropagation();
    var accountNumber = $(this).closest('tr').attr('id');
    var parent = $(this).parents();
    var idAttribute = _globalObject;
    var data = $(parent).find(".account-number-div").find(".account-aumber#" + idAttribute).attr('showwith');
    var viewDetailsBtn = $(parent).find(".account-number-div").find("button#btn-account-view-details-" + idAttribute);
    $.ajax({
        type: 'GET',
        url: '/Teller/GetSelectAccountNumber',
        data: {
            accountNumber: accountNumber
        },
        success: function (result) {
            debugger;
            $(parent).find(".account-number-div").find(".account-aumber#" + idAttribute).val(result.AccountNumber);
            $(parent).find(".account-number-div").find(".account-id#" + idAttribute).val(result.AccountId);

            if (data != "NoDisplay") {


                $(viewDetailsBtn).find('.fa-toggle-off').addClass('hidden');
                $(viewDetailsBtn).find('.fa-toggle-on').removeClass('hidden');
                $(viewDetailsBtn).attr('Action', 'Show')
                $(viewDetailsBtn).removeClass('hidden');
                $('#account-details-show-div').addClass('hidden');
                $(parent).find(".account-number-div").find("button#btn-account-view-details-" + idAttribute).removeClass('hidden');

                if (data == "ChargableAccounts") {
                    $.ajax({
                        type: 'GET',
                        url: '/Teller/GetFixedAccountsByAccountId',
                        data: {
                            Iaccno: result.AccountId,
                        },
                        success: function (Isfixed) {

                            if (Isfixed) {
                                $.MessageBox({
                                    buttonDone: "OK",
                                    message: "Fixed Account Cannot be Chargeable"
                                }).done(function () {

                                }).fail(function () {
                                    //return false;
                                })
                            }

                        },
                    });

                }
                if (data == "ChequeTransHistory")
                {
                   
                    $.ajax({
                        type: 'GET',
                        url: '/Information/StatusChequeList',
                        data: {
                            iAccNo: result.AccountId,
                        },
                        success: function (chequeTrans) {
                            $("#cheque-transaction-status-change").html(chequeTrans);
                          
                        },
                    });
                }
                if (data == "WithdrawWithIntPay") {
                    $.ajax({
                        type: 'GET',
                        url: '/Teller/InterestPayable',
                        data: {
                            accountId: result.AccountId,
                        },
                        success: function (intPayable) {

                            if (intPayable.isNominee == true) {
                                if (intPayable.Amount != 0) {

                                    $('a.intPayable').removeClass('hidden')
                                    $.MessageBox({
                                        buttonDone: "OK",
                                        message: "Available Interest Payable Amount=NRS." + intPayable.Amount
                                    }).done(function () {

                                    }).fail(function () {
                                        //return false;
                                    })

                                    $('.payableamount').html("NRS. " + result.Amount);
                                }
                            } else {
                                $('a.intPayable').addClass('hidden')
                            }
                        },
                    });
                }
            }
            $($(parent).find("#account-pop-up-div")).modal('toggle');

        },
    });
});

$('.btn-account-view-details-button').on('click', function () {

    var attrShowHide = $(this).attr('Action');
    if (attrShowHide == 'Show') {
        $('.fa-toggle-off').removeClass('hidden');
        $('.fa-toggle-on').addClass('hidden');
        $(this).attr('Action', 'Hide')
    } else {
        $('.fa-toggle-off').addClass('hidden');
        $('.fa-toggle-on').removeClass('hidden');
        $(this).attr('Action', 'Show')
        $('#account-details-show-div').addClass('hidden');
        return;
    }


    var idAttribute = _globalObject;
    var showType = $(".account-number-div").find(".account-aumber#" + idAttribute).attr('showwith');
    var AccountId = $(".account-number-div").find(".account-id#" + idAttribute).val();
    if (AccountId == 0 || AccountId == "") {
        return;
    }
    $.ajax({
        type: 'GET',
        url: '/Teller/_AcountDetailsViewShow',
        data: {
            accountId: AccountId,
            showType: showType
        },
        success: function (accountDetailsShow) {
            $('#account-details-show-div').removeClass('hidden');
            $('#account-details-show-div').html(accountDetailsShow);
        }
    })
});
