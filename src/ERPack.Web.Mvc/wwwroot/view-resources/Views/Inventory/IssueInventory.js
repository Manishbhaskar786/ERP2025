(function ($) {
    //let idType = "InventoryIssueId"
    //debugger
    //$.ajax({
    //    url: `/Common/GetIdByPreference?idType=${idType}`,
    //    method: "GET",
    //    dataType: "json",
    //    success: function (data) {
    //        $("#issueCode").val(data.result.id);
    //    },
    //    error: function (xhr, status, error) {
    //        // Handle errors
    //        console.error("Error:", status, error);
    //    }
    //});

    FillIssueCode();
    $("#inventoryRequestDiv").hide();
    $("#divPurchaseIndent").hide();

    $(document).on("change", "#inventoryRequest", function () {
        $("#inventoryRequestDiv").show();
    })

    $(document).on("change", "#Manual", function () {
        $("#inventoryRequestDiv").hide();
    })

    $(document).on("change", "#FromStoreId,#ToStoreId", function () {
        let inputId = $(this)[0].id;
        let storeId = $(this).val();
        let tableRow = $(this).closest('tr');
        let materialId = tableRow.find("#MaterialId").val();

        if (materialId) {
            $.ajax({
                url: `/Materials/GetMaterialInventoryById?materialId=${materialId}&storeId=${storeId}`,
                method: "GET",
                dataType: "json",
                success: function (data) {
                    let materialInventory = data.result.materialInventory;
                    if (inputId == 'FromStoreId') {
                        $("#FromStoreQty").val(materialInventory.quantity);
                    }
                    else if (inputId == 'ToStoreId') {
                        $("#ToStoreQty").val(materialInventory.quantity);
                    }

                },
                error: function (xhr, status, error) {
                    // Handle errors
                    console.error("Error:", status, error);
                }
            });
        }
        else {
            abp.notify.error('Please Select Material ItemCode!');
        }
    });

    $(document).on("click", "#addRow", function () {
        CloneRow();
    });

    $(document).on("click", "#addPIRow", function () {
        ClonePIRow();
    });

    $(document).on("change", "#inventoryRequestId", function () {
        let id = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Inventory/GetInventoryRequest?id=" + id,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                if (response.result.msg == "OK") {
                    let data = JSON.parse(response.result.data)
                    $("#inventoryRequestBy").val(data.RequestedBy);
                    const obj = {};
                    obj.id = "0";
                    obj.value = data.MaterialId;
                    GetMaterialDetails(obj)
                }
            },
            error: function () {
                alert("Error occured!!")
            }
        });
    })

    $("#tblIssueInventory").on("click", ".fa-trash", function (event) {
        $(this).closest("tr").remove();
    });
})(jQuery);

let cloneCount = 1;
function CloneRow() {
    
    if (!ValidateIssueInventoryItems()) {
        return;
    }

    $("#IssueInventoryRow").clone(true)
        .attr('id', 'IssueInventoryRow' + cloneCount, 'class', 'row')
        .insertAfter('[id^=IssueInventoryRow]:last')
        .find("input[type='number'],input[type='hidden'],input[type='text'],select").val("");

    $('#IssueInventoryRow' + cloneCount).find('input,select').each(function () {
        $(this).attr('id', $(this).attr('id').replace(/.$/, cloneCount));
        $(this).attr('name', $(this).attr('name') + cloneCount);
    });
    $('#IssueInventoryRow' + cloneCount).find('.fa-trash').removeAttr('hidden');
    cloneCount++
}

function FillIssueCode() {
    let idType = "InventoryIssueId"
    $.ajax({
        url: `/Common/GetIdByPreference?idType=${idType}`,
        method: "GET",
        dataType: "json",
        success: function (data) {
            $("#issueCode").val(data.result.id);
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error("Error:", status, error);
        }
    });
}

function ValidateIssueInventoryItems() {
    var alertmsg = "";
    var validatecnt = (cloneCount - 1);
    
    var itemTypeId = $("#itemTypeId" + validatecnt).val(); 
    var materialId = $("#materialId" + validatecnt).val(); 
    var fromStoreId = $("#fromStoreId" + validatecnt).val(); 
    var toStoreId = $("#toStoreId" + validatecnt).val(); 
    var toStoreQty = $("#toStoreQty" + validatecnt).val(); 
    var qtyTransferred = $("#qtyTransferred" + validatecnt).val();
    var departmentId = $("#departmentId" + validatecnt).val();
    var personIssuedId = $("#personIssuedId" + validatecnt).val();

    if (!checkValue(itemTypeId)) {
        alertmsg = "Please select Type";
    }else if (!checkValue(materialId)) {
        alertmsg = "Please select Item Code";
    } else if (!checkValue(fromStoreId)) {
        alertmsg = "Please select From store";
    } else if (!checkValue(toStoreId)) {
        alertmsg = "Please select To store";
    } else if (!checkValue(toStoreQty)) {
        alertmsg = "Please select To Store which have Qty";
    } else if (!checkValue(qtyTransferred)) {
        alertmsg = "Please select Qty Transfered";
    } else if (!checkValue(departmentId)) {
        alertmsg = "Please select Issued Department";
    } else if (!checkValue(personIssuedId)) {
        alertmsg = "Please select Person Issued";
    }

    if (alertmsg != "") {
        alert(alertmsg + " before add new row.");
        return false;
    }
    return true;
}

function checkValue(data) {
    if (data !== null && data !== "" && data !== undefined) {
        return true
    } else {
        return false;
    }
}

let clonePICount = 1;
function ClonePIRow() {

    $("#PurchaseIndentRow").clone(true)
        .attr('id', 'PurchaseIndentRow' + cloneCount, 'class', 'row')
        .insertAfter('[id^=PurchaseIndentRow]:last')
        .find("input[type='number'],input[type='hidden'],input[type='text'],select").val("");

    $('#PurchaseIndentRow' + cloneCount).find('input,select').each(function () {
        $(this).attr('id', $(this).attr('id').replace(/.$/, cloneCount));
        $(this).attr('name', $(this).attr('name') + cloneCount);
    });
    $('#PurchaseIndentRow' + cloneCount).find('.fa-trash').removeAttr('hidden');
    cloneCount++
}

function IssueInventory() {
    
    if (!ValidateIssueInventoryItems()) {
        return;
    }

    var formData = $('#inventoryIssueForm').serializeArray();
    var inventoryItems = $("input[id^='materialName']");
    for (i = 0; i < inventoryItems.length; i++) {
        formData.push({ name: "InventoryItems[" + i + "].MaterialId", value: $($("select[id^='materialId']")[i]).val() });
        formData.push({ name: "InventoryItems[" + i + "].ItemTypeId", value: $($("select[id^='itemTypeId']")[i]).val() });
        formData.push({ name: "InventoryItems[" + i + "].FromStoreId", value: $($("select[id^='fromStoreId']")[i]).val() });
        formData.push({ name: "InventoryItems[" + i + "].SuperStoreQty", value: $($("input[id^='fromStoreQty']")[i]).val() });
        formData.push({ name: "InventoryItems[" + i + "].ToStoreId", value: $($("select[id^='toStoreId']")[i]).val() });
        formData.push({ name: "InventoryItems[" + i + "].ToStoreQty", value: $($("input[id^='toStoreQty']")[i]).val() });
        formData.push({ name: "InventoryItems[" + i + "].QtyTransferred", value: $($("input[id^='qtyTransferred']")[i]).val() });
        formData.push({ name: "InventoryItems[" + i + "].IssuedDepartmentId", value: $($("select[id^='departmentId']")[i]).val() });
        formData.push({ name: "InventoryItems[" + i + "].PersonIssuedId", value: $($("select[id^='personIssuedId']")[i]).val() });
    }

    $.ajax({
        type: "POST",
        data: formData,
        url: '/Inventory/IssueInventory',
        success: function (data) {
            if (data.result.msg == "OK") {
                abp.notify.success('SavedSuccessfully');
                $('#inventoryIssueForm')[0].reset();
                FillIssueCode();
            }
            else {
                abp.notify.error('Error In Saving');
            }
        },
        failure: function (response) {

        },
        error: function (response) {
            alert("error");
        }
    });
}

function GetDepartmentUsers(obj) {
    let selectedId = obj.id;
    let departmentId = obj.value;
    let lastCharOfId = selectedId.charAt(selectedId.length - 1);

    $.ajax({
        type: "GET",
        url: "/Users/GetUsersByDepartment?departmentId=" + departmentId,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let users = response.result.users;
                $('#personIssuedId' + lastCharOfId).find("option").remove();
                let optionHTML = "";
                optionHTML += `<option value="" selected>Select User</option>`;
                for (let i = 0; i < users.length; i++) {
                    optionHTML += ` <option value="${users[i].id}">
                                                ${users[i].name}
                                            </option>`
                }
                $('#personIssuedId' + lastCharOfId).append(optionHTML);
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function GetMaterialDetails(obj) {
    let selectedId = obj.id;
    let materialId = obj.value;
    let lastCharOfId = selectedId.charAt(selectedId.length - 1);
    $.ajax({
        type: "GET",
        url: "/Materials/GetMaterialById?id=" + materialId,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let data = JSON.parse(response.result.data)
                if (selectedId.startsWith('piMaterialId') || selectedId.startsWith('piMaterialName')) {
                    $('#piMaterialId' + lastCharOfId).val(data.Id);
                    $('#piMaterialName' + lastCharOfId).val(data.Id);
                }
                else {
                    $('#materialName' + lastCharOfId).val(data.DisplayName);
                    $('#materialId' + lastCharOfId).val(data.Id);
                    $('#itemTypeId' + lastCharOfId).val(data.ItemTypeId);
                }
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function GetStoreInventory(obj) {

    let selectedId = obj.id;
    let storeId = obj.value;
    let lastCharOfId = selectedId.charAt(selectedId.length - 1);
    let materialId = $("#materialId" + lastCharOfId).val();

    if (materialId) {
        $.ajax({
            url: `/Materials/GetMaterialInventoryById?materialId=${materialId}&storeId=${storeId}`,
            method: "GET",
            dataType: "json",
            success: function (response) {
                let materialInventory = response.result.data;

                if (materialInventory == "") {
                    abp.notify.info('No Inventory found for selected material!');
                }
                else {
                    if (selectedId.startsWith('fromStoreId')) {
                        $("#fromStoreQty" + lastCharOfId).val(materialInventory.quantity);
                    }
                    else if (selectedId.startsWith('toStoreId')) {
                        $("#toStoreQty" + lastCharOfId).val(materialInventory.quantity);
                    }
                }
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error("Error:", status, error);
            }
        });
    }
    else {
        abp.notify.error('Please Select Material ItemCode!');
    }
}

function CreatePurchaseIndent() {
    $("#divPurchaseIndent").show();
}

function SavePurchaseIndent() {
    var purchaseIndentItems = $("select[id^='piMaterialId']");

    purchaseIndentsArray = [];
    for (i = 0; i < purchaseIndentItems.length; i++) {
        var purchaseIndent = {}
        purchaseIndent.ItemTypeId = $($("select[id^='piItemTypeId']")[i]).val();
        purchaseIndent.MaterialId = $($("select[id^='piMaterialId']")[i]).val();
        purchaseIndent.Quantity = $($("input[id^='piQuantity']")[i]).val();
        purchaseIndent.RequiredBy = $($("input[id^='piRequireBy']")[i]).val();
        purchaseIndent.Remark = $($("input[id^='piRemarks']")[i]).val();

        purchaseIndentsArray.push(purchaseIndent);
    }

    purchaseIndents = JSON.stringify(purchaseIndentsArray);

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: '/PurchaseIndents/CreatePurchaseIndents',
        data: purchaseIndents,
        success: function (data) {
            if (data.result.msg == "OK") {
                abp.notify.success('Purchase Indent Created!');
                $('#inventoryIssueForm')[0].reset();
                window.location = "/PurchaseIndents";
            }
            else {
                abp.notify.error('Error In Saving');
            }
        },
        failure: function (response) {
            
        }
    });
}

