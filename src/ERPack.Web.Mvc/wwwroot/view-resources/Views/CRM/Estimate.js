//const { debug } = require("console");

(function ($) {
    if ($("#designId").val() != 0) {
        var designId = $("#designId").val();
        GetDesignDetails(designId);
    }
    if ($("#id").val() == '0') {
        let idType = "EstimateId"
        $.ajax({
            url: `/Common/GetIdByPreference?idType=${idType}&name=""`,
            method: "GET",
            dataType: "json",
            success: function (data) {
                $("#estimateId").val(data.result.id);
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error("Error:", status, error);
            }
        });
    }
    $(document).on("click", "#addRow", function () {
        CloneRow();
    });

    let customers = new Bloodhound({
        datumTokenizer: function (d) {
            return Bloodhound.tokenizers.whitespace(d.name);
        },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        limit: 10,
        remote: {
            url: '/Customers/GetCustomersNames?name=%QUERY',
            wildcard: "%QUERY",
            filter: function (response) {
                let data = JSON.parse(response.result.data);

                return $.map(data, function (customer) {
                    return {
                        name: customer.Name,
                        id: customer.Id
                    }
                });
            }
        }
    });

    $('#taCustomers').typeahead(
        {
            hint: false,
            highlight: true,
            minLength: 1
        },
        {
            name: 'customers',
            displayKey: 'name',
            source: customers.ttAdapter(),
            templates: {
                empty: [
                    '<div class="empty-message">',
                    'Sorry no data !',
                    '</div>'
                ].join('\n')
            }
        })
        .bind('typeahead:select', function (ev, suggestion) {
            $('#hdnCustomers').val(suggestion.id);
            GetCustomerDetails(suggestion.id);
        });

    /*$("#designId").on('change', function () {
        let name = this.selectedOptions[0].text;
        let idType = "EstimateId"

        if (name && name.length > 2 && $("#id").val() == '0') {
            $.ajax({
                url: `/Common/GetIdByPreference?idType=${idType}&name=${name}`,
                method: "GET",
                dataType: "json",
                success: function (data) {
                    $("#estimateId").val(data.result.id);
                },
                error: function (xhr, status, error) {
                    // Handle errors
                    console.error("Error:", status, error);
                }
            });
        }
    });*/

    $("#tblTask").on("click", ".fa-trash", function (event) {
        $(this).closest("tr").remove();
        CalculateTotalAmount();
    });
})(jQuery);

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

                $("#materialId" + lastCharOfId).val(data.Id);
                $("#hdnMaterialId" + lastCharOfId).val(data.Id);
                $("#unitId" + lastCharOfId).val(data.SellingUnitId);
                $("#unitName" + lastCharOfId).val(data.SellingUnitId);
                $("#price" + lastCharOfId).val(data.SellingPrice);
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

function GetDesignDetails(id) {

    $.ajax({
        type: "GET",
        url: "/Production/GetDesignById?id=" + id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let data = JSON.parse(response.result.data)
                $('#enquiryId').val(data.EnquiryId);
                GetCustomerDetails(data.CustomerId);
                if ($("#id").val() == 0) {
                    GetDesignMaterialDetails(id);
                }
                else {
                    let estimateId = $("#id").val();
                    GetEstimateTasks(estimateId);
                }
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function GetDesignMaterialDetails(id) {
    $.ajax({
        type: "GET",
        url: "/Production/GetDesignMaterials?id=" + id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let data = JSON.parse(response.result.data)
                $.each(data, function (index, value) {
                    $("#materialId" + index).val(value.ItemCode);
                    $("#hdnMaterialId" + index).val(value.MaterialId);
                    $("#unitId" + index).val(value.SellingUnitId);
                    $("#unitName" + index).val(value.SellingUnitId);
                    $("#price" + index).val(value.SellingPrice);
                    $("#cGST" + index).val(value.CGST);
                    $("#sGST" + index).val(value.SGST);
                    $("#iGST" + index).val(value.IGST);
                    $("#materialName" + index).val(value.MaterialId);
                    $("#quantity" + index).val(value.Quantity);
                    $("#discount" + index).val(0);
                    $("#amount" + index).val(value.SellingPrice * value.Quantity);

                    var isLastElement = index == data.length - 1;
                    if (!isLastElement) {
                        CloneRow();
                    }
                });
                CalculateTotalAmount();
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function GetEstimateTasks(id) {
    $.ajax({
        type: "GET",
        url: "/CRM/GetEstimateTasks?estimateId=" + id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let data = response.result.data;
                $.each(data, function (index, value) {
                    $("#hdnEstimateTaskId" + index).val(value.id);
                    $("#materialId" + index).val(value.itemCode);
                    $("#hdnMaterialId" + index).val(value.materialId);
                    $("#unitId" + index).val(value.unitId);
                    $("#unitName" + index).val(value.unitId);
                    $("#quantity" + index).val(value.qty);
                    $("#price" + index).val(value.price);
                    $("#cGST" + index).val(value.cgst);
                    $("#sGST" + index).val(value.sgst);
                    $("#iGST" + index).val(value.igst);
                    $("#materialName" + index).val(value.materialId);
                    $("#discount" + index).val(value.discountPercentage);
                    $("#amount" + index).val(value.amount);

                    var isLastElement = index == data.length - 1;
                    if (!isLastElement) {
                        CloneRow();
                    }
                });
                CalculateTotalAmount();
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function GetCustomerDetails(id) {

    $.ajax({
        type: "GET",
        url: "/Customers/GetCustomerById?id=" + id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let data = JSON.parse(response.result.data)
                $("#inpCustomerId").val(data.Id);
                $("#customerId").val(data.CustomerId);
                $("#taCustomers").val(data.Name);
                $("#customerAddress").val(data.Address1 + "," + data.Address2 + "," + data.City + "," + data.State + "," + data.PinCode + "," + data.Country);
                $("#customerPan").val(data.PAN);
                $("#customerGst").val(data.GSTNo);
                $("#customerEmail").val(data.EmailAddress);
                $("#customerPhone").val(data.Mobile);
                $("#customerWebsite").val(data.Website);
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}
function fadeit() {
    $(".modal-backdrop").hide();
}


function validatePAN() {
    const panInput = document.getElementById('customerPan').value;
    const panRegex = /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
    const messageDiv = document.getElementById('customerPanerr');

    if (panInput != "") {
        if (panRegex.test(panInput)) {
            messageDiv.textContent = '';
            return true;
        } else {
            messageDiv.textContent = 'Invalid PAN format. Please enter a valid PAN.';
            messageDiv.style.color = 'red';
            return false;
        }
    } else {
        messageDiv.textContent = '';
        return true;
    }

}
function validateGST() {
    const gstInput = document.getElementById('customerGst').value;
    const gstRegex = /^(?:[0-9]{2})(?:[A-Z]{4})(?:\d{4})(?:[A-Z]{1})(?:Z)(?:[0-9A-Z]{1})$/;
    const messageDiv = document.getElementById('Gsterr');

    if (gstInput != "") {
        if (gstRegex.test(gstInput)) {
            messageDiv.textContent = '';
            return true;
        } else {
            messageDiv.textContent = 'Invalid GST Number format. Please enter a valid GST.';
            messageDiv.style.color = 'red';
            return false;
        }
    } else {
        messageDiv.textContent = '';
        return true;
    }

}


function Validate20digitWith2decimal(Valueamt, messagespan) {

    const Regex_Validate20digitWith2decimal = /^\d{1,20}(\.\d{1,2})?$/;
    if (Valueamt != undefined && Valueamt.length > 20) {
        messagespan.textContent = 'Max lenth should be 20 digit.';
    } else {
        messagespan.textContent = '';
    }

    if (Valueamt != "") {
        if (Regex_Validate20digitWith2decimal.test(Valueamt)) {
            messagespan.textContent = '';
        } else {
            messagespan.textContent = 'Invalid Quantity format.';
            messagespan.style.color = 'red';
        }
    } else {
        messagespan.textContent = '';
    }
}

function CGSTValidation(cgstValue, messageDiv, msg) {
    // Clear previous messages
    messageDiv.textContent = '';

    // Regular expression for validating the input
    const regex = /^(100(\.00?)?|[0-9]{1,2}(\.[0-9]{1,2})?)$/;

    // Check if the value matches the regex
    if (cgstValue != "") {
        if (!regex.test(cgstValue)) {
            messageDiv.textContent = msg
            messageDiv.style.color = 'red';
        } else {
            messageDiv.textContent = '';
        }
    }

}

function PreventNaNValue(objVal, id) {
    if (isNaN(objVal)) {
        $(id).text(0);
    } else {
        $(id).text(objVal);
    }
}

function formatAmount(amount) {
    if (!isNaN(amount)) {
        return parseFloat(amount).toFixed(2);
    } else {
        return 0;
    }
}

let cloneCount = 1;

function CloneRow() {

    $("#taskRow").clone(true)
        .attr('id', 'taskRow' + cloneCount, 'class', 'row')
        .insertAfter('[id^=taskRow]:last')
        .find('input,select,span').val("");

    $('#taskRow' + cloneCount).find('input,select,span').each(function () {
        $(this).attr('id', $(this).attr('id').replace(/.$/, cloneCount));
        $(this).attr('name', $(this).attr('name') + cloneCount);
    });
    $('#taskRow' + cloneCount).find('.fa-trash').removeAttr('hidden');
    cloneCount++
}

function CalculateTotalAmount(isChanged = 'false') {
    let rowCount = $('#tblTask tr[id*="taskRow"]').length;
    let totalAmount = 0;
    let netAmount = 0;
    let iCGSTPercentage = 0;
    let iSGSTPercentage = 0;
    let iIGSTPercentage = 0;
    let iCGSTAmount = 0;
    let iSGSTAmount = 0;
    let iIGSTAmount = 0;
    let iDiscountPercentage = 0;

    $('#tblTask tr[id*="taskRow"]').each(function (index) { // Using each to dynamically get current index
        const qtyInput = $(this).find('input[id^="quantity"]').val();
        const qtyRegex = /^\d{1,20}(\.\d{1,2})?$/;
        const messageDiv = $(this).find('span[id^="quantityerr"]');
        messageDiv.css('color', 'red');

        if (qtyInput && qtyInput.length > 20) {
            messageDiv.text('Max length should be 20 digits.');
        } else {
            messageDiv.text('');
        }

        if (qtyInput != "") {
            messageDiv.text(qtyRegex.test(qtyInput) ? '' : 'Invalid Quantity format.');
        } else {
            messageDiv.text(isChanged === 'true' ? 'Field is required.' : '');
        }

        const priceInput = $(this).find('input[id^="price"]').val();
        const PriceRegex = /^\d{1,20}(\.\d{1,2})?$/;
        const PricemessageDiv = $(this).find('span[id^="priceerr"]');
        PricemessageDiv.css('color', 'red');

        if (priceInput && priceInput.length > 20) {
            PricemessageDiv.text('Max length should be 20 digits.');
        } else {
            PricemessageDiv.text('');
        }

        if (priceInput != "") {
            PricemessageDiv.text(PriceRegex.test(priceInput) ? '' : 'Invalid Quantity format.');
        } else {
            PricemessageDiv.text(isChanged === 'true' ? 'Field is required.' : '');
        }

        const discountInput = $(this).find('input[id^="discount"]').val();
        const discounterror = $(this).find('span[id^="discounterr"]');
        let discount = discountInput ? parseInt(discountInput) : 0;
        if (!(discount > 0))
            $(this).find('input[id^="discount"]').val(0);
        discounterror.css('color', 'red')
        discounterror.text(discount < 0 || discount > 100 ? 'Enter value between 0 to 100.' : '');

        const cGstInput = $(this).find('input[id^="cGST"]').val();
        const dcGstErr = $(this).find('span[id^="cGSTerr"]')[0];
        if (cGstInput == '')
            $(this).find('input[id^="cGST"]').val(0);
        CGSTValidation(cGstInput, dcGstErr, "Enter value between 0 to 100.");

        const sGstInput = $(this).find('input[id^="sGST"]').val();
        const dcSGstErr = $(this).find('span[id^="sGSTerr"]')[0];
        if (sGstInput == '')
            $(this).find('input[id^="sGST"]').val(0);
        CGSTValidation(sGstInput, dcSGstErr, "Enter value between 0 to 100.");

        const iGSTInput = $(this).find('input[id^="iGST"]').val();
        const dciGSTerr = $(this).find('span[id^="iGSTerr"]')[0];
        if (iGSTInput == '')
            $(this).find('input[id^="iGST"]').val(0);
        CGSTValidation(iGSTInput, dciGSTerr, "Enter value between 0 to 100.");

        const amountInput = $(this).find('input[id^="amount"]').val();
        const amounterr = $(this).find('span[id^="amounterr"]')[0];
        Validate20digitWith2decimal(amountInput, amounterr);

        const iPrice = parseInt($(this).find('input[id^="price"]').val()) || 0;
        const iQty = parseInt($(this).find('input[id^="quantity"]').val()) || 0;
        iDiscountPercentage = parseInt($(this).find('input[id^="discount"]').val()) || 0;

        iCGSTPercentage = Math.max(iCGSTPercentage, parseInt($(this).find('input[id^="cGST"]').val()) || 0);
        iSGSTPercentage = Math.max(iSGSTPercentage, parseInt($(this).find('input[id^="sGST"]').val()) || 0);
        iIGSTPercentage = Math.max(iIGSTPercentage, parseInt($(this).find('input[id^="iGST"]').val()) || 0);

        if ((iCGSTPercentage > 0 || iSGSTPercentage > 0) && iIGSTPercentage > 0) {
            alert("Either CGST and SGST or IGST allowed.");
            return;
        }

        var iAmount = iPrice * iQty;
        var iDiscountAmount = (iAmount * iDiscountPercentage) / 100;
        var iAmountAfterDiscount = iAmount - iDiscountAmount;

        $(this).find('input[id^="amount"]').val(iAmountAfterDiscount);
        totalAmount += iAmountAfterDiscount;
    });

    iCGSTAmount = iCGSTPercentage > 0 ? (totalAmount * iCGSTPercentage) / 100 : 0;
    iSGSTAmount = iSGSTPercentage > 0 ? (totalAmount * iSGSTPercentage) / 100 : 0;
    iIGSTAmount = iIGSTPercentage > 0 ? (totalAmount * iIGSTPercentage) / 100 : 0;
    netAmount = totalAmount + iCGSTAmount + iSGSTAmount + iIGSTAmount;

    PreventNaNValue(totalAmount, "#divGrossAmount");
    //if (totalAmount == "NaN") {
    //    $("#divGrossAmount").text(0);
    //}els{
    //    $("#divGrossAmount").text(totalAmount);
    //}
    PreventNaNValue(formatAmount(iCGSTAmount), "#divCGST");
    PreventNaNValue(formatAmount(iSGSTAmount), "#divSGST");
    PreventNaNValue(formatAmount(iIGSTAmount), "#divIGST");
    PreventNaNValue(formatAmount(netAmount), "#divTotalAmount");
    // $("#divCGST").text(iCGSTAmount);
    //  $("#divSGST").text(iSGSTAmount);
    // $("#divIGST").text(iIGSTAmount);
    //$("#divTotalAmount").text(netAmount);

}

function SaveEstimate() {

    if ($("#description").val() == "" || $("#customerAddress").val() == "") {
        if ($("#description").val() == "")
            alert("Description is required.");
        if ($("#customerAddress").val() == "")
            alert("Customer Address is required.");
        return;
    }


    var formData = $('#estimateCreateForm').serializeArray();
    var tasks = $("input[id^='materialId']");
    formData.customerId = $('#inpCustomerId').val();

    /*if (!validatePAN()) {
        return false;
    }
    if (!validateGST()) {
        return false;
    }*/

    // Dynamically add tasks details based on visible rows
    $("#tblTask tr[id^='taskRow']").each(function (index, row) {
        const $row = $(row);

        if ($("#id").val() !== "0") {
            formData.push({
                name: `EstimateTasks[${index}].Id`,
                value: $row.find("input[id^='hdnEstimateTaskId']").val()
            });
        }

        formData.push(
            { name: `EstimateTasks[${index}].MaterialId`, value: $row.find("input[id^='hdnMaterialId']").val() },
            { name: `EstimateTasks[${index}].UnitId`, value: $row.find("input[id^='unitId']").val() },
            { name: `EstimateTasks[${index}].Qty`, value: $row.find("input[id^='quantity']").val() },
            { name: `EstimateTasks[${index}].Price`, value: $row.find("input[id^='price']").val() },
            { name: `EstimateTasks[${index}].DiscountPercentage`, value: $row.find("input[id^='discount']").val() },
            { name: `EstimateTasks[${index}].CGST`, value: $row.find("input[id^='cGST']").val() },
            { name: `EstimateTasks[${index}].SGST`, value: $row.find("input[id^='sGST']").val() },
            { name: `EstimateTasks[${index}].IGST`, value: $row.find("input[id^='iGST']").val() },
            { name: `EstimateTasks[${index}].Amount`, value: $row.find("input[id^='amount']").val() }
        );
    });

    // Adding calculated amounts to formData
    formData.push(
        { name: "TotalAmount", value: $("#divTotalAmount").text() },
        { name: "CGSTAmount", value: $("#divCGST").text() },
        { name: "SGSTAmount", value: $("#divSGST").text() },
        { name: "IGSTAmount", value: $("#divIGST").text() },
        { name: "GrossAmount", value: $("#divGrossAmount").text() }
    );
    $.ajax({
        type: "POST",
        data: formData,
        url: '/CRM/SaveEstimate',
        success: function (data) {
            if (data.result.msg == "OK") {
                $("#id").val(data.result.id);
                abp.notify.success('SavedSuccessfully');
                $('#estimateCreateForm')[0].reset();
                window.location = '/CRM/EstimatesList';
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

function ShowPreview() {

    if ($("#description").val() == "" || $("#customerAddress").val() == "") {
        if ($("#description").val() == "")
            alert("Description is required.");
        if ($("#customerAddress").val() == "")
            alert("Customer Address is required.");
        return;
    }

    $("#divPrevEstimateId").text("Estimate Id : " + $("#estimateId").val());
    $("#divPrevSubject").text("Sub : " + $("#description").val());

    $("#divPrevName").text($("#taCustomers").val());
    $("#divPrevAddress").text($("#customerAddress").val());
    $("#divPrevPhone").text("Phone :" + $("#customerPhone").val());
    $("#divPrevEmail").text("Email :" + $("#customerEmail").val());

    $("#divPrevGrossAmount").text($("#divGrossAmount").text());
    $("#divPrevCGST").text($("#divCGST").text());
    $("#divPrevSGST").text($("#divSGST").text());
    $("#divPrevIGST").text($("#divIGST").text());
    $("#divPrevTotalAmount").text($("#divTotalAmount").text());

    var tableRow = "";
    var icount = 1;
    var tasks = $("input[id^='materialId']");
    for (i = 0; i < tasks.length; i++) {
        var MatName = $($("select[id^='materialName'] :selected")[i]).text();
        var MatUnit = $($("select[id^='unitName'] :selected")[i]).text();
        var MatQty = $($("input[id^='quantity']")[i]).val();
        var MatPrice = $($("input[id^='price']")[i]).val();
        var MatDiscount = $($("input[id^='discount']")[i]).val();
        var MatAmount = $($("input[id^='amount']")[i]).val();

        tableRow += "<tr><th scope=\"row\">" + icount + "</th><td style='word -break: break-all;'>" + MatName + "</td><td style='word -break: break-all;'>" + MatUnit + "</td><td>" + MatQty +
            "</td><td>" + MatPrice + "</td><td>" + MatDiscount + "</td><td>" + MatAmount + "</td></tr>";
        icount++;
    }
    $("#tblPrevBody").html(tableRow);
    $("#EstimatePreviewModal").modal("show");
    setInterval(fadeit, 2000);
}

function GeneratePdf() {
    if ($('#id').val() == 0) {
        abp.notify.info("Please save estimate first to generate pdf !");
        return;
    }

    var estimateId = $('#id').val();
    if (estimateId != null) {
        var url = '/CRM/PdfEstimate?estimateId=' + estimateId;
        window.open(url, "_blank");
    }
    else {
        abp.notify.info("Please enter a valid EstimateId.!");
    }
}

function SendApprovalEmail() {
    let estimate = $('#estimateId').val();
    let customerId = $('#inpCustomerId').val();
    let enquiryId = $('#designId').val();


    if (isNaN(estimate) || estimate == "" || isNaN(customerId) || customerId == "" || isNaN(enquiryId) || enquiryId == "") {
        return;
    }

    $.ajax({
        type: "GET",
        url: "/CRM/EstimateApprovalEmail?customerId=" + customerId + "&enquiryId=" + enquiryId + "&estimate='" + estimate + "'",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                abp.notify.info("Estimate Sent for Approval !");
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}