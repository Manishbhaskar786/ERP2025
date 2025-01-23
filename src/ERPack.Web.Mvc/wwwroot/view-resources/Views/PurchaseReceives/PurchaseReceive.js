let _$form = $('#purchaseReceiveCreateForm');

function GetPurchaseOrderItems(obj) {
    $("#tblPurchaseReceive tr:gt(1)").remove();
    $("#tblPurchaseReceive").find("input[type='number'],input[type='hidden'],input[type='text'],select").val("");
    $.ajax({
        type: "GET",
        url: "/PurchaseOrders/GetPurchaseOrderItems?purchaseorderId=" + obj.value,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                data = response.result.data;
                $.each(data, function (index, value) {
                    $("#materialId" + index).val(value.materialId);
                    $("#materialName" + index).text(value.materialName);
                    $("#qtyOrderd" + index).text(value.quantity);
                    $("#purchaseOrderItemId" + index).val(value.id);
                    $("#unit" + index).text(value.buyingUnit);

                    var isLastElement = index == data.length - 1;
                    if (!isLastElement) {
                        CloneRow();
                    }
                });
                $("#vendor").val(data[0].vendorName);
                $("#vendorId").val(data[0].vendorId);
                $("#purchaseOrderId").val(data[0].purchaseOrderId);
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

let cloneCount = 1;

function CloneRow() {
    $("#purchaseReceiveRow").clone(true)
        .attr('id', 'purchaseReceiveRow' + cloneCount, 'class', 'row')
        .insertAfter('[id^=purchaseReceiveRow]:last')
        .find('input,select,label').val("");

    $('#purchaseReceiveRow' + cloneCount).find('input,select,label').each(function () {
        $(this).attr('id', $(this).attr('id').replace(/.$/, cloneCount));
        $(this).attr('name', $(this).attr('name') + cloneCount);
    });
    cloneCount++
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

function SavePurchaseReceive() {

    if (!_$form.valid()) {
        return;
    }
    var formData = $('#purchaseReceiveCreateForm').serializeArray();
    var purchaseReceiveItems = $("input[id^='materialId']");
    for (i = 0; i < purchaseReceiveItems.length; i++) {
        formData.push({ name: "purchaseReceiveItems[" + i + "].MaterialId", value: $($("input[id^='materialId']")[i]).val() });
        formData.push({ name: "purchaseReceiveItems[" + i + "].QuantityOrdered", value: $($("label[id^='qtyOrderd']")[i]).text() });
        formData.push({ name: "purchaseReceiveItems[" + i + "].PurchaseOrderItemId", value: $($("input[id^='purchaseOrderItemId']")[i]).val() });
        formData.push({ name: "purchaseReceiveItems[" + i + "].QuantityReceived", value: $($("input[id^='qtyReceived']")[i]).val() });
        formData.push({ name: "purchaseReceiveItems[" + i + "].StoreId", value: $($("select[id^='storeId']")[i]).val() });
    }

    $.ajax({
        type: "POST",
        data: formData,
        url: '/PurchaseReceives/AddPurchaseReceive',
        success: function (data) {
            if (data.result.msg == "OK") {
                abp.notify.success('SavedSuccessfully');
                $('#purchaseReceiveCreateForm')[0].reset();
                window.location = '/PurchaseReceives';
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