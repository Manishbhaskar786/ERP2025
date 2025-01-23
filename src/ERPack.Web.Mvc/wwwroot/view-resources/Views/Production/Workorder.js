(function ($) {
    if ($("#estimateId").val() != 0) {
        var estimateId = $("#estimateId").val();
        GetEstimateTasks(estimateId);
    }
    else {
        GetPerferenceId("WorkorderId");
    }

    $(document).on("click", "#addRow", function () {
        CloneRow();
    });

    $("#tblTask").on("click", ".fa-trash", function (event) {
        $(this).closest("tr").remove();
        CalculateTotalAmount();
    });
    setInterval(fadeit, 2000);
})(jQuery);

function fadeit() {
    $(".modal-backdrop").hide();
}
function fnFillMaterialInfo(obj) {

    let selectedId = obj.id;
    let lastCharOfId = selectedId.charAt(selectedId.length - 1);

    $.ajax({
        type: "GET",
        url: "/Materials/GetMaterialById?id=" + obj.value,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let data = JSON.parse(response.result.data)
                $("#materialId" + lastCharOfId).val(data.ItemCode);
                $("#hdnMaterialId" + lastCharOfId).val(data.Id);
                $("#unitId" + lastCharOfId).val(data.SellingUnitId);
                $("#unitName" + lastCharOfId).val(data.SellingUnitId);

                GetDepartmentUsers(data.DepartmentId, lastCharOfId);
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

let departmentIds = [];

function GetEstimateTasks(id) {
    $("#tblTask tr:gt(1)").remove();
    $("#tblTask").find("input[type='number'],input[type='hidden'],input[type='text'],select").val("");
    //cloneCount = 1;
    $("#tblSubTask tr:gt(1)").remove();

    $.ajax({
        type: "GET",
        url: "/CRM/GetEstimateTasks?estimateId=" + id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let data = response.result.data;
                $.each(data, function (index, value) {
                    $("#materialId" + index).val(value.itemCode);
                    $("#hdnMaterialId" + index).val(value.materialId);
                    $("#materialName" + index).val(value.materialId);
                    $("#unitId" + index).val(value.unitId);
                    $("#unitName" + index).val(value.unitId);
                    $("#quantity" + index).val(value.qty);
                    $("#departmentId" + index).val(value.departmentId);

                    GetDepartmentUsers(value.departmentId, index);                  
                    departmentIds.push(value.departmentId);
                    
                    var isLastElement = index == data.length - 1;
                    if (!isLastElement) {
                        CloneRow();
                    }
                });
                GetPerferenceId("WorkorderTaskId");
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function GetDepartmentUsers(id, index) {

    $.ajax({
        type: "GET",
        url: "/Users/GetUsersByDepartment?departmentId=" + id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let users = response.result.users;
                $('#departmentUsers' + index).find("option").remove();
                let optionHTML = "";
                optionHTML += `<option value="" selected>Select User</option>`;
                for (let i = 0; i < users.length; i++) {
                    optionHTML += ` <option value="${users[i].id}">
                                                ${users[i].name}
                                            </option>`
                }
                $('#departmentUsers' + index).append(optionHTML);  
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function validateInput20digitwith2decimal(input) {
    
    const value = input.value.replace(/[^0-9.]/g, '');
    const parts = value.split('.');

    // Prevent multiple decimal points
    if (parts.length > 2) {
        input.value = parts[0] + '.' + parts[1]; // Keep only the first decimal point
    }

    // Limit decimal places to 2
    if (parts.length === 2) {
        parts[1] = parts[1].substring(0, 2); // Limit to 2 decimal places
    }

    // Remove leading zeros from integer part
    parts[0] = parts[0].replace(/^0+(?=\d)/, '');

    // Reassemble value and enforce max length
    let newValue = parts[0];
    if (parts.length === 2) {
        newValue += '.' + parts[1];
    }

    // Limit to 20 digits before the decimal
    if (newValue.length > 22) { // 20 digits + 2 decimals + 1 decimal point
        newValue = newValue.slice(0, 22);
    }

    input.value = newValue; // Update the input value
}

function AddSubTask(subWorkorderId) {
    let subTaskRowCount = 1;
    let index = 0;
    $("#divSubTasks").removeAttr('style');
    
    let departments = [];

    $.ajax({
        type: "GET",
        url: "/Common/GetDepartments",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                departments = response.result.data;

                var uniqueDepartmentIds = departmentIds.filter(function (item, i, departmentIds) {
                    return i == departmentIds.indexOf(item);
                });
                for (let i = 0; i < uniqueDepartmentIds.length; i++) {
                    const department = departments.filter(v => v.id === uniqueDepartmentIds[i]);

                    $("#hdnDepartmentId" + index).val(uniqueDepartmentIds[i]);
                    $("#departmentName" + index).val(department[0].deptName);
                    if (i == 0) {
                        $("#subWorkorderId" + i).val(parseInt(subWorkorderId));
                    }
                    else {
                        let serialNumber = parseInt(subWorkorderId.replace(/[^0-9]/gi, ''));
                        let nextSerialNumber = parseInt(serialNumber, 10) + i;
                        let subWorkorderIdString = subWorkorderId.slice(0, "-" + serialNumber.toString().length);
                        let nextSubWorkorderId = subWorkorderIdString + nextSerialNumber;
                        $("#subWorkorderId" + i).val(nextSerialNumber);
                    }

                    var isLastElement = index == uniqueDepartmentIds.length - 1;

                    if (!isLastElement) {

                        $("#subTaskRow").clone(true)
                            .attr('id', 'subTaskRow' + subTaskRowCount, 'class', 'row')
                            .insertAfter('[id^=subTaskRow]:last')
                            .find("input,select").val("");

                        $('#subTaskRow' + subTaskRowCount).find('input,select').each(function () {
                            $(this).attr('id', $(this).attr('id').replace(/.$/, subTaskRowCount));
                            $(this).attr('name', $(this).attr('name') + subTaskRowCount);
                        });
                    }

                    index++;
                    subTaskRowCount++;
                }
            }
        }
    });
}

function GetPerferenceId(IdType) {
    let idString;
    $.ajax({
        url: `/Common/GetIdByPreference?idType=${IdType}`,
        method: "GET",
        dataType: "json",
        success: function (data) {
            idString = data.result.id;
        },
        complete: function (data) {
            if (IdType == "WorkorderId") {
                $("#workorderId").val(idString);
            }
            if (IdType == "WorkorderTaskId") {
                let subWorkorderId = idString;
                AddSubTask(subWorkorderId);
            }
            return idString;
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error("Error:", status, error);
        }
    });
}

function SaveWorkorder() {
    
    if ($('#isHighPriority').is(":checked")) {
        // it is checked
        $('#hdnHighPriority').val(true);
    }
    else {
        $('#hdnHighPriority').val(false);
    }

    var formData = $('#workorderCreateForm').serializeArray();
    var tasks = $("input[id^='materialId']");
    for (i = 0; i < tasks.length; i++) {
        formData.push({ name: "WorkorderTasks[" + i + "].MaterialId", value: $($("input[id^='hdnMaterialId']")[i]).val() });
        formData.push({ name: "WorkorderTasks[" + i + "].UnitId", value: $($("input[id^='unitId']")[i]).val() });
        formData.push({ name: "WorkorderTasks[" + i + "].UserId", value: $($("select[id^='departmentUsers']")[i]).val() });
        formData.push({ name: "WorkorderTasks[" + i + "].DepartmentId", value: $($("input[id^='departmentId']")[i]).val() });
        formData.push({ name: "WorkorderTasks[" + i + "].Quantity", value: $($("input[id^='quantity']")[i]).val() });
        formData.push({ name: "WorkorderTasks[" + i + "].TaskIssueDate", value: $($("input[id^='taskIssueDate']")[i]).val() });
        formData.push({ name: "WorkorderTasks[" + i + "].TaskIssueCompleteDate", value: $($("input[id^='taskToBeCompleted']")[i]).val() });
        formData.push({ name: "WorkorderTasks[" + i + "].TaskIssueActualCompleteDate", value: $($("input[id^='taskActualCompletionDate']")[i]).val() });
    }

    let departmentIds = $("input[id^='hdnDepartmentId']");
    for (i = 0; i < departmentIds.length; i++) {
        formData.push({ name: "WorkorderSubTasks[" + i + "].WorkOrderSubTaskId", value: $("#subWorkorderId" + [i]).val() });
        formData.push({ name: "WorkorderSubTasks[" + i + "].DepartmentId", value: $("#hdnDepartmentId" + [i]).val() });
    }

    abp.ui.setBusy();
    $.ajax({
        type: "POST",
        data: formData,
        url: '/Production/SaveWorkorder',
        success: function (data) {
            if (data.result.msg == "OK") {
                $("#id").val(data.result.id);
                abp.notify.success('SavedSuccessfully');
                $('#workorderCreateForm')[0].reset();
                window.location = '/Production/WorkorderList';
            }
            else {
                abp.ui.clearBusy();
                abp.notify.error('Error In Saving');
            }
        },
        failure: function (response) {
            abp.ui.clearBusy();
            abp.notify.error('Error In Saving');
        },
        error: function (response) {
            abp.ui.clearBusy();
            abp.notify.error('Error In Saving');
        }
    });
}

let cloneCount = 1;

function CloneRow() {

    $("#taskRow").clone(true)
        .attr('id', 'taskRow' + cloneCount, 'class', 'row')
        .insertAfter('[id^=taskRow]:last')
        .find("input[type='number'],input[type='hidden'],input[type='text'],select").val("");

    $('#taskRow' + cloneCount).find('input,select').each(function () {
        $(this).attr('id', $(this).attr('id').replace(/.$/, cloneCount));
        $(this).attr('name', $(this).attr('name') + cloneCount);
    });
    $('#taskRow' + cloneCount).find('.fa-trash').removeAttr('hidden');
    cloneCount++
}

function ShowPreview() {
    $("#divPrevWorkorderId").text("Workorder Id : " + $("#workorderId").val());
    $("#divPrevSubject").text("Sub : " + $("#description").val());

    var tableRow = "";
    var icount = 1;
    var tasks = $("input[id^='materialId']");
    for (i = 0; i < tasks.length; i++) {      
        var MatName = $($("select[id^='materialName'] :selected")[i]).text();
        var MatUnit = $($("select[id^='unitName'] :selected")[i]).text();
        var MatQty = $($("input[id^='quantity']")[i]).val();
        var MatTaskIssue = $($("input[id^='taskIssueDate']")[i]).val();
        var MatTaskComplete = $($("input[id^='taskToBeCompleted']")[i]).val();
        var MatTaskActualComplete = $($("input[id^='taskActualCompletionDate']")[i]).val();
        var UserId = $($("input[id^='departmentUserId']")[i]).val();
        if (UserId != "") {
            var User = $($("select[id^='departmentUsers'] :selected")[i]).text();
        }
        else {
            var User = "";
        }

        tableRow += "<tr><th scope=\"row\">" + icount + "</th><td>" + MatName + "</td><td>" + MatUnit + "</td><td>" + MatQty +
            "</td><td>" + MatTaskIssue + "</td><td>" + MatTaskComplete + "</td><td>" + MatTaskActualComplete + "</td><td>" + User + "</td></tr>";
        icount++;
    }
    $("#tblPrevBody").html(tableRow);
    $("#WorkorderPreviewModal").modal("show");
}

function GeneratePdf() {
    if ($('#id').val() == 0) {
        abp.notify.info("Please save workorder first to generate pdf !");
        return;
    }

    var workorderId = $('#id').val();
    if (workorderId != null) {
        var url = '/Production/PdfWorkorder?workorderId=' + workorderId;
        window.open(url, "_blank");
    }
    else {
        abp.notify.info("Please enter a valid WorkorderId.!");
    }
}