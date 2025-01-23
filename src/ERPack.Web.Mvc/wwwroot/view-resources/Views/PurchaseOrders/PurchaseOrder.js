(function ($) {    
    let idType = "PurchaseOrderId"
    $.ajax({
        url: `/Common/GetIdByPreference?idType=${idType}`,
        method: "GET",
        dataType: "json",
        success: function (data) {
            $("#poCode").val(data.result.id);
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error("Error:", status, error);
        }
    });

    if ($("#Id").val() != 0) {
        var PoId = $("#Id").val();
        GetOrderItems(PoId);
    }

    $(document).on("click", "#addRow", function () {
        CloneRow();
    });

    $("#tblPurchaseOrder").on("click", ".fa-trash", function (event) {
        $(this).closest("tr").remove();
    });
})(jQuery);

let cloneCount = 1;
function CloneRow() {

    $("#purchaseOrderRow").clone(true)
        .attr('id', 'purchaseOrderRow' + cloneCount, 'class', 'row')
        .insertAfter('[id^=purchaseOrderRow]:last')
        .find("input[type='number'],input[type='hidden'],input[type='text'],select").val("");

    $('#purchaseOrderRow' + cloneCount).find('input,select').each(function () {
        $(this).attr('id', $(this).attr('id').replace(/.$/, cloneCount));
        $(this).attr('name', $(this).attr('name') + cloneCount);
    });
    $('#purchaseOrderRow' + cloneCount).find('.fa-trash').removeAttr('hidden');
    cloneCount++
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
                $("#hdnMaterialId" + lastCharOfId).val(data.Id);
                $("#materialId" + lastCharOfId).val(data.Id);
                $("#materialName" + lastCharOfId).val(data.Id);
                $("#buyingUnitName" + lastCharOfId).val(data.BuyingUnitId);
                $("#buyingPrice" + lastCharOfId).val(data.BuyingPrice);
                $("#cGST" + lastCharOfId).val(data.CGST);
                $("#sGST" + lastCharOfId).val(data.SGST);
                $("#iGST" + lastCharOfId).val(data.IGST);
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function GetMaterialsByType(obj) {
    let selectedId = obj.id;
    let typeId = obj.value;
    let lastCharOfId = selectedId.charAt(selectedId.length - 1);

    $.ajax({
        type: "GET",
        url: "/Materials/GetMaterialByType?typeId=" + typeId,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let materials = response.result.data;
                $('#materialId' + lastCharOfId).find("option").remove();
                $('#materialName' + lastCharOfId).find("option").remove();
                let optionHTML = "";
                optionHTML += `<option value="" selected>Select</option>`;

                for (let i = 0; i < materials.length; i++) {
                    optionHTML += ` <option value="${materials[i].id}">
                                                ${materials[i].itemCode}
                                            </option>`
                }

                $('#materialId' + lastCharOfId).append(optionHTML);

                let optionHTML2 = "";
                optionHTML2 += `<option value="" selected>Select</option>`;

                for (let i = 0; i < materials.length; i++) {
                    optionHTML2 += ` <option value="${materials[i].id}">
                                                ${materials[i].displayName}
                                            </option>`
                }
                $('#materialName' + lastCharOfId).append(optionHTML2);
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function GetOrderItems(id) {
    debugger
    $("#tblTask tr:gt(1)").remove();
    $("#tblTask").find("input[type='number'],input[type='hidden'],input[type='text'],select").val("");
    //cloneCount = 1;
   
    $.ajax({
        type: "GET",
        url: "/PurchaseOrders/GetPurchaseOrderItems?purchaseorderId=" + id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            
            if (response.result.msg == "OK") {
                let data = response.result.data;
                $.each(data, function (index, value) {
                    debugger
                    $("#hdnitemTypeId" + index).val(value.itemTypeId);
                    $("#hdnmaterialId" + index).val(value.materialId);
                    $("#itemTypeId" + index).val(value.itemTypeId);
                    var material = document.getElementById("itemTypeId" + index);
                    if (material.value != null && material1.value != "") {
                        GetMaterialsByType(material);
                    }
                   
                    
                    $("#materialId" + index).val(value.materialId);
                    var material1 = document.getElementById("materialId" + index);
                    material1.value = value.materialId;
                    if (material1.value != null && material1.value != "") {
                        fnFillMaterialInfo(material1);
                    }
                    
                    
                    //$("#hdnmaterialName" + index).val(value.materialId);
                    //$("#materialName" + index).val(value.materialId);
                    //var material2 = document.getElementById("materialName" + index);
                    //fnFillMaterialInfo(material2);
                    
                    //$("#buyingUnitId" + index).val(value.unitId);
                    //$("#buyingUnitName" + index).val(value.unitId);
                    //$("#buyingPrice" + index).val(value.Price);
                    //$("#qtyPurchase" + index).val(value.Quantity);
                    //$("#createdDateTime" + index).val(value.PurchaseDate);
                    //var amount = document.getElementById("amount" + index);
                    //CalculateAmount(amount);
                    //var cGST = document.getElementById("cGST" + index);
                    //CalculateAmount(cGST);
                    //var iGST = document.getElementById("iGST" + index);
                    //CalculateAmount(iGST);
                    //var sGST = document.getElementById("sGST" + index);
                    //CalculateAmount(sGST);
                    
                    
                    var isLastElement = index == data.length - 1;
                    if (!isLastElement) {
                        CloneRow();
                    }
                });
            }
        },
        error: function (response) {
            alert(response.statusText);
        }
    });
}


function CalculateAmount(obj) {

    let selectedId = obj.id;
    let lastCharOfId = selectedId.charAt(selectedId.length - 1);

    let PurchaseQty = $("#qtyPurchase" + lastCharOfId).val();
    let BuyingPrice = $("#buyingPrice" + lastCharOfId).val();

    var amount = BuyingPrice * PurchaseQty;
    $("#amount" + lastCharOfId).val(amount);

    var cgst = $("#cGST" + lastCharOfId).val();
    var igst = $("#sGST" + lastCharOfId).val();
    var sgst = $("#iGST" + lastCharOfId).val();

    if ((cgst > 0 || sgst > 0) && igst > 0) {
        alert("either CGST and SGST or IGST allowed.");
        return;
    }

    gstAmount = (amount / 100 * cgst) + (amount / 100 * igst) + (amount / 100 * sgst);

    $("#amountGST" + lastCharOfId).val(gstAmount);
    let totalAmount = gstAmount + amount;
    $("#totalAmount" + lastCharOfId).val(totalAmount);
}

function validateInput20digitwith2decimal(input) {
    // Remove non-numeric characters except for decimal points
    let value = input.value.replace(/[^0-9.]/g, '');

    // Split the input on the decimal point
    let parts = value.split('.');

    // Limit to two decimal places
    if (parts.length > 2) {
        value = parts[0] + '.' + parts[1];
    }

    if (parts.length === 2) {
        // If there's a decimal part, limit its length to 2
        parts[1] = parts[1].substring(0, 2);
        value = parts[0] + '.' + parts[1];
    }

    // Check for total length (including decimal point)
    if (value.length > 20) {
        value = value.slice(0, 20);
    }

    // Set the input value
    input.value = value;
}

function validateGST(input) {
    // Remove non-numeric characters except for decimal points
    let value = input.value.replace(/[^0-9.]/g, '');

    // Split the input on the decimal point
    let parts = value.split('.');

    // Limit to two decimal places
    if (parts.length > 2) {
        value = parts[0] + '.' + parts[1];
    }

    if (parts.length === 2) {
        // If there's a decimal part, limit its length to 2
        parts[1] = parts[1].substring(0, 2);
        value = parts[0] + '.' + parts[1];
    }

    // Convert to number for range check
    const numberValue = parseFloat(value);

    // Check if the number is between 0 and 100
    if (numberValue < 0 || numberValue > 100) {
        value = ''; // Clear the input if it's out of range
    }

    // Set the input value
    input.value = value;
}

function SavePurchaseOrder() {
    if ($('#vendorId').val() == "") {
        abp.notify.error('VendorId is Required!');
        return;
    }

    var formData = $('#purchaseOrderCreateForm').serializeArray();
    var purchaseOrderItems = $("select[id^='materialId']");
    for (i = 0; i < purchaseOrderItems.length; i++) {
        formData.push({ name: "PurchaseOrderItems[" + i + "].MaterialId", value: $($("select[id^='materialId']")[i]).val() });
        formData.push({ name: "PurchaseOrderItems[" + i + "].ItemTypeId", value: $($("select[id^='itemTypeId']")[i]).val() });
        formData.push({ name: "PurchaseOrderItems[" + i + "].UnitId", value: $($("select[id^='buyingUnitName']")[i]).val() });
        formData.push({ name: "PurchaseOrderItems[" + i + "].Price", value: $($("input[id^='buyingPrice']")[i]).val() });
        formData.push({ name: "PurchaseOrderItems[" + i + "].Quantity", value: $($("input[id^='qtyPurchase']")[i]).val() });
        formData.push({ name: "PurchaseOrderItems[" + i + "].PurchaseDate", value: $($("input[id^='createdDateTime']")[i]).val() });
        formData.push({ name: "PurchaseOrderItems[" + i + "].Amount", value: $($("input[id^='amount']")[i]).val() });
        formData.push({ name: "PurchaseOrderItems[" + i + "].CGST", value: $($("input[id^='cGST']")[i]).val() });
        formData.push({ name: "PurchaseOrderItems[" + i + "].SGST", value: $($("input[id^='sGST']")[i]).val() });
        formData.push({ name: "PurchaseOrderItems[" + i + "].IGST", value: $($("input[id^='iGST']")[i]).val() });
        formData.push({ name: "PurchaseOrderItems[" + i + "].GSTAmount", value: $($("input[id^='amountGST']")[i]).val() });
        formData.push({ name: "PurchaseOrderItems[" + i + "].TotalAmount", value: $($("input[id^='totalAmount']")[i]).val() });
    }

    $.ajax({
        type: "POST",
        data: formData,
        url: '/PurchaseOrders/AddPurchaseOrder',
        success: function (data) {
            if (data.result.msg == "OK") {
                abp.notify.success('SavedSuccessfully');
                $('#purchaseOrderCreateForm')[0].reset();
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