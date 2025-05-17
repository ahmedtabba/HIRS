//Configuration values

var cookieDomainName = 'mof.gov.ae';//mof.gov.ae
var cookieStylePath = 'InternalSharedWeb/Content/css/themes/';
var cookieImgesPath = 'InternalSharedWeb/Content/images/';


var recaptchaRendered = null;
var recaptchaRenderedFeedback = null;
var chatRefreshed = false;



$(document).ready(function ($) {
    toastr.options.closeButton = true;
    toastr.options.progressBar = true;

    kendo.culture("en-US");
    $.validator.addMethod('date',
        function (value, element) {
            var isValidDate = this.optional(element) || kendo.parseDate(value, "dd/MM/yyyy");
            return isValidDate
        });

    //Turn off Caching for IE
    $.ajaxSetup({
        cache: false
    });




    //Set No English Content
    $('span.Only-Arabic-Content').each(function () {
        $(this).text(' (This Content is available in Arabic Only)');
    });


    $.validator.methods.number = function (value, element) {
        return parseFloat(value).toString() !== "NaN";
    }

    // Print
    $(".print").click(function (e) {
        e.preventDefault();

        window.print();
    });

    $(".floatNumber").keypress(function (event) {
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    })




    // Current Date
    $("#spnSiteDate").text(GetSiteDate());





    jQuery('[data-toggle="tooltip"]').tooltip();




    // ----- Alt KPI for Images
    var title = $(document).find("title").text();
    $('img:not([alt])').attr('alt', title);
    $('img').each(function () {
        if ($(this).attr('title')) {
            $(this).attr('alt', $(this).attr('title'));
        }
    });

    //String.format extension method
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/\{(\d+)\}/g, function (m, n) { return args[n]; });
    };


});

function DocumentDir() {
    return document.dir || document.getElementsByTagName('html')[0].dir;
}

function GetSiteDate() {
    var EngMonth = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    var ArMonth = ['يناير', 'فبراير', 'مارس', 'أبريل', 'مايو', 'يونيو', 'يوليو', 'أغسطس', 'سبتمبر', 'اكتوبر', 'نوفمبر', 'ديسمبر'];

    var EngDay = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']
    var ArDay = ['الاحد', 'الاثنين', 'الثلاثاء', 'الاربعاء', 'الخميس', 'الجمعة', 'السبت']

    var today = new Date();
    var d = today.getDate();
    var dd = today.getDay();
    var mm = today.getMonth(); //January is 0!
    var yyyy = today.getFullYear();

    var retVal;

    if (DocumentDir() == 'rtl') {
        retVal = ArDay[dd] + ' ' + d + ' ' + ArMonth[mm] + ' ' + yyyy;
    } else {
        retVal = EngDay[dd] + ' ' + d + ' ' + EngMonth[mm] + ' ' + yyyy;
    }

    return retVal;
}

//External E-Services

function ConvertJsonDate(JsonDate) {
    var dateString = JsonDate.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();

    var date = day + "/" + month + "/" + year;
    return date;
}

function ConvertJsonDateTime(JsonDateTime) {

    if (JsonDateTime != null) {
        var dateString = JsonDateTime.substr(6);
        var currentTime = new Date(parseInt(dateString));
        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate();
        var year = currentTime.getFullYear();

        var Hour = currentTime.getHours();
        var Minute = currentTime.getMinutes();

        var date = day + "/" + month + "/" + year + " - " + Hour + ":" + Minute;
        return date;
    }
}

function Language(lang) {
    Cookies.set('language', lang, { expires: 365, domain: cookieDomainName });
    window.location.href = window.location.href;
}


function SuccessMsg(Msg) {
    $("#SuccessmyModal .modal-body").html(Msg);

    $('#SuccessmyModal').modal('show');
}

function ErrorMsg(Msg) {

    $("#ErrormyModal .modal-body").html(Msg);
    $('#ErrormyModal').modal('show');
}

function ConfirmMsg(Msg, YesFunction) {
    $("#ConfirmModal .modal-body").html(Msg);
    $('#ConfirmModal').modal('show');
    alert("done");
}

function Handel_cross_Popup() {
    $(".modal").on('show.bs.modal', function () {

        $(document.body).addClass('modal-open');

    }).on('hidden.bs.modal', function () {

        if ($('.modal').hasClass('in')) {
            $(document.body).addClass('modal-open');
        }

    });
}
var gridIds = "#grid,#RisksGrid,#TeamMemberGrid,#TaskGrid,#ReasonGrid";

function AddCustomHeader_ToKendoCalls() {
    var grids = [];
    $(".k-grid").each(function (index, item) {
        grids.push(item);
    });

    if (grids) {
        $.each(grids, function (index, item) {
            var grid = $(item).data("kendoGrid");
            if (grid) {
                grid.dataSource.transport.options.read.beforeSend = function (xhr) {
                    xhr.setRequestHeader('is_kendorequest', 'true');
                };
                grid.dataSource.transport.options.create.beforeSend = function (xhr) {
                    xhr.setRequestHeader('is_kendorequest', 'true');
                };
                grid.dataSource.transport.options.update.beforeSend = function (xhr) {
                    xhr.setRequestHeader('is_kendorequest', 'true');
                };
                grid.dataSource.transport.options.destroy.beforeSend = function (xhr) {
                    xhr.setRequestHeader('is_kendorequest', 'true');
                };
            }
        });
    }

    var ddl = $("#GoalId").data("kendoDropDownList");
    if (ddl) {
        ddl.dataSource.transport.options.read.beforeSend = function (xhr) {
            xhr.setRequestHeader('is_kendorequest', 'true');
        };
    }

}

function Ajax_ErrorHandler(args) {
    if (args.error) {
        if (args.message) {
            var errMsgBody = "<div><ul>";
            if (typeof args.message === 'string') {
                errMsgBody += "<li>" + args.message + "</li>";
            }

            errMsgBody += "</div></ul>";

            $("#AlertmyModal .modal-body").html(errMsgBody);
            $("#AlertmyModal").modal();
        }
    }
    else
        return false;
}

function isNumberKey(txt, evt) {

    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 46) {
        //Check if the text already contains the . character
        if (txt.value.indexOf('.') === -1) {
            return true;
        } else {
            return false;
        }
    } else {
        if (charCode > 31
            && (charCode < 48 || charCode > 57))
            return false;
    }
    return true;
}



function KendoGrid_ErrorHandler(args, gridName) {
    
    debugger;
    if (args.errors) {

        var grid = $('#' + gridName).data("kendoGrid");

        if (grid.dataSource._destroyed.length != 0)
            grid.cancelChanges();

        grid.one("dataBinding", function (e) {
            e.preventDefault();// cancel grid rebind if error occurs
        });

        var errMsgBody = "<div><ul>";
        if (typeof args.errors === 'string') {
            errMsgBody += "<li>" + args.errors + "</li>";
        }
        else {
            $.each(args.errors, function (propertyName) {
                debugger;
                if (propertyName)
                    errMsgBody += "<li>" + this.errors + "</li>";
            });
        }
        errMsgBody += "</div></ul>";

        //$("#DangerModal .modal-body").html(errMsgBody);
        //$("#DangerModal").modal();
        toastr.error(errMsgBody, '');
    }
}


function onRequestEnd(e, details) {
    debugger;
    if (e.type === "update" && e.response.Errors === null)
        toastr.success(`${Editing}  ${HasBeenCompletedSuccessfully}!!`, '', { timeOut: 4000 });
    else if (e.type === "create" && e.response.Errors === null)
        toastr.success(`${Adding}  ${HasBeenCompletedSuccessfully}!!`, ' ', { timeOut: 4000 });
    else if (e.type === "destroy" && e.response.Errors === null)
        toastr.success(`${Deleting}  ${HasBeenCompletedSuccessfully}!!`, ' ', { timeOut: 4000 });
}


function onJSError(err) {
    toastr.error(err, '');
}

function ShowSuccessfllyMsg(msg) {
    toastr.success(msg, '', { timeOut: 4000 });
}





function KendoDDL_ErrorHandler(args) {
    
    if (args.errors) {
        var errMsgBody = "<div><ul>";
        if (typeof args.errors === 'string') {
            errMsgBody += "<li>" + args.errors + "</li>";
        }
        else {
            $.each(args.errors, function (propertyName) {
                errMsgBody += "<li>" + this.errors + "</li>";
            });
        }
        errMsgBody += "</div></ul>";

     

        toastr.error(errMsgBody, '');
    }
}

function PopupMessage(title, msg) {
    $("#InfoModal .modal-title").html('<div>' + title + '</div>');

    $("#InfoModal .modal-body").html('<div>' + msg + '</div>');
    $("#InfoModal").modal();

}

function getUrlParts(url) {
    var a = document.createElement('a');
    a.href = url;

    return {
        href: a.href,
        host: a.host,
        hostname: a.hostname,
        port: a.port,
        pathname: a.pathname,
        protocol: a.protocol,
        hash: a.hash,
        search: a.search
    };
}

function precise_round(num, decimals) {
    var sign = num >= 0 ? 1 : -1;
    return (Math.round((num * Math.pow(10, decimals)) + (sign * 0.001)) / Math.pow(10, decimals)).toFixed(decimals);
}

function getProgressHtml(status, precent, text) {
    var result = '<div class="progress-group">';
    result += '<div class="progress progress-xs "><div class="progress-bar ' + getProgressStatusClass(status) + '"';
    result += 'role="progressbar" aria-valuenow="' + precent + '" aria-valuemin="0" aria-valuemax="100" style="width: ' + precent + '%">';
    result += '<span class="sr-only">' + precent + '%</span> </div> </div>';
    result += '<span class="progress-text">' + text + '</span>';
    result += '<span class="progress-number"><span class="label ' + getStatusClass(status) + '"><strong>' + precent + '%</strong> </span> </span> </div>';
    return result;
}

function getPercentageHtmlValue(status, percent, text) {
    var result = '';
    var PercentText = '';
    var PercentageMarkSymbol = '%';
    if (percent == null)
        PercentText = '-'
    else
        PercentText = percent;
    if (text == '0')
        PercentageMarkSymbol = '';
    if (status != 6 && PercentText != '-')
        result += '<span class="progress-number"><span class="label ' + getStatusClass(status) + '"><strong>' + PercentText + PercentageMarkSymbol + '</strong> </span> </span>';
    else
        result += PercentText;
    return result;
}
function printVal(val, ispercentage) {
    var res = val;
    if (val == null || val == '') {
        res = ' - ';
    }
    else if (ispercentage == '1')
        res = res + '%';
    return res;
}
function getKPIHtmlValue(status, percent, isPercentage) {
    var result = '';
    var PercentText = '';
    var PercentageMarkSymbol = '%';
    if (percent == null) {
        PercentageMarkSymbol = '';
        PercentText = '-';
    } else
        PercentText = percent;
    if (isPercentage == '0')
        PercentageMarkSymbol = '';
    if (status != null && PercentText != '-')
        result += '<span class="progress-number"><span class="label ' + getStatusClass(status) + '"><strong>' + PercentText + PercentageMarkSymbol + '</strong> </span> </span>';
    else
        result += PercentText + PercentageMarkSymbol;
    return result;
}

function showPercentage(val, isPercentage) {

    if (isPercentage == '0' || isPercentage == false)
        return val;
    else
        return val + '%';

}

function getKPIFinalHtmlValue(status, percent, isPercentage) {
    var result = '';
    var PercentText = '';
    var PercentageMarkSymbol = '%';
    if (percent == null) {
        PercentageMarkSymbol = '';
        PercentText = '-';
    } else
        PercentText = percent;
    if (isPercentage == '0' || isPercentage == false)
        PercentageMarkSymbol = '';
    if (status != null)
        result += '<span class="progress-number"><span class="label ' + getStatusClass(status) + '"><strong>' + PercentText + PercentageMarkSymbol + '</strong> </span> </span>';
    else
        result += PercentText + PercentageMarkSymbol;
    return result;
}
function parseDate(vale) {
    return kendo.toString(kendo.parseDate(vale, 'yyyy-MM-dd'), 'MMM/yyyy');
}

function parseShortDate(value) {
    return kendo.toString(kendo.parseDate(value, 'dd-MMM'), 'dd-MMM');
}

function parseLongDate(value) {
    return kendo.toString(kendo.parseDate(value, 'dd-MM-yyyy'), 'dd-MM-yyyy');
}


function getStatusClass(status) {
    var classtext = '';
    if (status == 0)
        classtext = 'achievement-label-Delay';
    else if (status == 1)
        classtext = 'achievement-label-Cautious';
    else if (status == 2)
        classtext = 'achievement-label-InProgress';
    else if (status == 3)
        classtext = 'achievement-label-Finished';
    else if (status == 4)
        classtext = 'achievement-label-Stopped';
    else if (status == 5)
        classtext = 'achievement-label-NotVerified';
    else
        classtext = 'achievement-label-Default';

    return classtext;
}

function getProgressStatusClass(status) {
    var classtext = '';
    if (status == 0)
        classtext = 'progress-bar-delay';
    else if (status == 1)
        classtext = 'progress-bar-cautious';
    else if (status == 2)
        classtext = 'progress-bar-inProgress';
    else if (status == 3)
        classtext = 'progress-bar-finished';
    else if (status == 5)
        classtext = 'progress-bar-NotVerified';

    return classtext;
}


$(document).ready(function () {

    AddCustomHeader_ToKendoCalls();
});

(function ($) {
    $(window).on("load", function () {




    });
})(jQuery);


function StickyNotesload(modelType, modelId, modelTag, Channel) {
    $("#divNotes_" + Channel).load(stickyNoteLoadNotesURL + "?objectType=" + modelType + "&objectId=" + modelId + "&objectTag=" + modelTag + "&Channel=" + Channel);
}



function AddNote(modelType, modelId, modelTag, Channel, ParentId) {
    debugger;
    console.log("ParentId");
    var sendbtn = $('#btn-send-note_' + Channel);
    sendbtn.attr("disabled", "disabled");

    $.ajax({
        type: 'POST',
        url: stickyNoteAddNoteURL,
        data: CollectData(Channel, ParentId),
        dataType: 'json',
        success: function (data) {
            StickyNotesload(modelType, modelId, modelTag, Channel);
            sendbtn.removeAttr("disabled");

            $("#txtNote_" + Channel).val('');
        }
    });
}

function CollectData(Channel, ParentId) {
    var objectType = $('#hdnObjectType_' + Channel).val();
    var objectId = $("#hdnObjectId_" + Channel).val();
    var objectTag = $("#hdnObjectTag_" + Channel).val();
    var channel = $("#hdnChannel_" + Channel).val();
    var noteText = $("#txtNote_" + Channel).val();
    return { objectType: objectType, objectId: objectId, objectTag: objectTag, channel: channel, note: noteText, parantId: ParentId };
}



function datePickerFilter(element) {
    element.kendoDatePicker({
        format: "dd/MM/yyyy"
    });
}
function gridDataBound(e) {
    setTimeout(function () {
        debugger;
        var grid = e.sender;

        var firstItemIndex = (grid.pager.page() - 1) * grid.pager.pageSize() + 1;
        var lastItemIndex = (grid.pager.page() - 1) * grid.pager.pageSize() +
            grid.dataSource.view().length;
        var filteredTotal = grid.dataSource.total();
        var databaseTotal = grid.dataSource._pristineTotal;
        grid.element.find(".k-pager-info").text(
            kendo.format("{0} - {1}  العناصر من {3} الاجمالي",
                firstItemIndex, lastItemIndex, filteredTotal, databaseTotal));
    }, 2);
}