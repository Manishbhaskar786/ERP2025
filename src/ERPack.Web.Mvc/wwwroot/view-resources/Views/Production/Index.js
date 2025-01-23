(function ($) {
    var _workorderService = abp.services.app.workorder,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#tblTasksList'),
        _$materialTable = $('#tblMaterialList');

    var _$tasksListTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _workorderService.getWorkorderTasksByUser,
            inputFilter: function () {
                return $('#TasksListSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => { _$tasksListTable.ajax.reload(null, false); }
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                data: 'id',
                defaultContent: '',
                sortable: false
            },
            {
                targets: 1,
                data: 'workorderCode'
            },
            {
                targets: 2,
                data: 'workorderSubTaskId'
            },
            {
                targets: 3,
                data: 'taskIssueCompleteDate'
            }
        ]
    });
})(jQuery);

$('#tblTasksList').on("click", "tbody tr", function () {
    $("#divMaterialList").removeAttr('style');
    let workorderTaskId = $(this).find(".control").text();
    $("#hdnTaskId").val(workorderTaskId);
    if (workorderTaskId != undefined) {
        $.ajax({
            type: "GET",
            url: "/Production/GetWorkorderTask?workorderTaskId=" + workorderTaskId,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.result.msg == "OK") {
                    $('#tblMaterialData').empty();
                    var jdata = JSON.parse(data.result.data);
                    var tr;
                    var srn = 1;
                    //Append each row to html table
                    for (var i = 0; i < jdata.length; i++) {
                        tr = $('<tr/>');
                        tr.append("<td>" + srn + "</td>");
                        tr.append("<td class='MaterialId' style='display:none'>" + jdata[i].MaterialId + "</td>");
                        tr.append("<td class='TypeId' style='display:none'>" + jdata[i].ItemTypeId + "</td>");
                        tr.append("<td>" + jdata[i].ItemCode + "</td>");
                        tr.append("<td>" + jdata[i].MaterialName + "</td>");
                        tr.append("<td class='Qty'>" + jdata[i].Quantity + "</td>");
                        $('#tblMaterialData').append(tr);
                    }
                }
            },
            error: function () {
                alert("Error occured!!")
            }
        });
    }
});

function GetCompletedTasks() {
    $.ajax({
        type: "GET",
        url: "/Production/GetCompletedWorkorderTasks",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.msg == "OK") {
                var jdata = JSON.parse(data.data);
                if (jdata.length > 0) {
                    $("#divCompletedTasksList").removeAttr('style');
                    $('#tblCompletedTasksData').empty();
                    var tr;
                    var srn = 1;
                    //Append each row to html table
                    for (var i = 0; i < jdata.length; i++) {
                        tr = $('<tr/>');
                        tr.append("<td>" + srn + "</td>");
                        tr.append("<td style='display:none'>" + jdata[i].MaterialId + "</td>");
                        tr.append("<td style='display:none'>" + jdata[i].SubTaskId + "</td>");
                        tr.append("<td>" + jdata[i].TaskId + "</td>");
                        tr.append("<td>" + jdata[i].SubTaskSId + "</td>");
                        tr.append("<td>" + jdata[i].CompletionBy + "</td>");
                        $('#tblCompletedTasksData').append(tr);
                    }
                }
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function SetStatus(e) {
    let id = $("#hdnTaskId").val();
    let status;
    if (e.id == "chkClarification") {
        status = "Clarification";
    }
    if (e.id == "chkOpenChecklist") {
        status = "Checklist";
    }
    if (e.id == "btnSetStock") {
        status = "StockRequested";
    }
    if (e.id == "btnTaskComplete") {
        status = "Completed";
    }
    if (e.id == "btnStockReceived") {
        status = "StockReceived";
    }

    $.ajax({
        type: "GET",
        url: `/Production/SetSubTaskStatus?workorderTaskId=${id}&status=${status}`,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.result.msg == "OK") {
                abp.notify.info('Status Updated!');
                if (e.id == "btnTaskComplete") {
                    window.location = '/Production';
                }
            }
        },
        error: function () {
            abp.notify.error('Error updating status!');
        }
    });
}

function RequestStock() {
    let taskId = $("#hdnTaskId").val();
    var array = [];

    $('#tblMaterialData tr').each(function (index, value) {
        var materialId = $('.MaterialId', value).text();
        var qty = $('.Qty', value).text();
        array.push({ "MaterialId": materialId, "ReqQty": qty, "workorderTaskId": taskId });

    });

    let data = JSON.stringify(array);
    $.ajax({
        type: "POST",
        url: "/Production/RequestStock",
        data: data,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.result.msg == "OK") {
                abp.notify.info('Stock Requested!');
            }
        },
        error: function () {
            abp.notify.error('Error updating status!');
        }
    });
}
