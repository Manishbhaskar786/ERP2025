// Define the default image path
const DEFAULT_IMAGE_PATH = "/img/default-image.jpg";
(function ($) {

    var l = abp.localization.getSource('ERPack'),
        _$form = $('#enquiryCreateForm');
   
    $("#MaterialRow").on("click", ".fa-trash", function (event) {
        let row = $(this).closest("tr");
        let enquiryMaterialId = row.find("input[id^='hdnEnquiryMaterialId']").val();
        let enquiryMaterialName = row.find("select[id^='materialName']").find(':selected').text();

        if (enquiryMaterialId) {
            abp.message.confirm(
                abp.utils.formatString(
                    ('Are You Sure Want To Delete Material ' + enquiryMaterialName)),
                null,
                (isConfirmed) => {
                    if (isConfirmed) {
                        $.ajax({
                            type: "POST",
                            url: "/CRM/DeleteEnquiryMaterial",
                            data: { enquiryMaterialId: enquiryMaterialId },
                            success: function (response) {
                                if (response.success) {
                                    row.remove();
                                    abp.notify.info("Enquiry Material deleted Successfully");
                                } else {
                                    abp.notify.error("Error Deleting Enquiry Material");
                                }
                            },
                            error: function (xhr, status, error) {
                                abp.notify.error('Error calling delete function: ' + error);
                            }
                        });
                    }
                }
            );
        } else { row.remove(); }
    });

    $("#downloadLink").click(function (e) {
        e.preventDefault();
        app.DownloadImage("icon-buttons");
        
    });

    $("#fileUpload").change(function () {
        const file = this.files[0]; // Get the selected file

        if (file) {
            const maxSize = 10 * 1024 * 1024; // 10 MB in bytes
            if (file.size > maxSize) {
                abp.notify.error("File size must be less than 10 MB.");
                $(this).val(''); // Clear the input
            }
        }
        app.UploadImage('filePreview','icon-buttons');
    });

   
    _$form.find('.save-button').on('click', function (e) {
        e.preventDefault();
        saveEnquiry();
    });

})(jQuery);

function validateNumberofUps(maxlength,obj) {
    //const inputValue = $('#NumberofUps').val();
    const inputValue = obj.value;
    if (inputValue.length > (maxlength-1)) {
        return false;
    } 
}
$(document).ready(function () {
    const enquiryId = $('#Id').val();
    if (enquiryId == 0) {
        generateEnquiryId();
    } else if (enquiryId > 0) {
        fetchEnquiryMaterials(enquiryId);
    }
    $("#magnify").on("click", function () {
        app.ZoomoutImage('filePreview','filePreview');
    });
    // Handle image loading and default state
    const updateImageState = () => {
        if ($('#designImage').attr('src') !== DEFAULT_IMAGE_PATH) {
            $('#downloadLink').attr('class', 'btn btn-outline-primary');
            $('#magnify').removeAttr('disabled');
        }
    };

    $('#designImage, #pdfPreview').each(function () {
        $(this).on('load', updateImageState);
        if (this.complete) {
            $(this).trigger('load');
        }
    });


    // Image scaling
    const modal = $('#myModal');
    const modalContent = $('.modal-content');
  
    $('#zoomSlider').on('input', function () {
        modalContent.css('transform', `scale(${this.value})`);

   });
    $('.close').on('click', function (event) {
        event.stopPropagation();
        modal.hide();
        $('#zoomSlider').val(1);
        // Reset the transform scale to 1
        modalContent.css('transform', 'scale(1)');
    });
    modalContent.on('click', function (event) {
        event.stopPropagation();
    });

    // Set status for checkboxes
    $('input[type=checkbox]').each(function () {
        const controlClass = $(this).data('control-class');
        if (controlClass) {
            setStatus(this.checked, controlClass);
        }
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
        });

    let designs = new Bloodhound({
        datumTokenizer: function (d) {
            return Bloodhound.tokenizers.whitespace(d.name);
        },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        limit: 10,
        remote: {
            url: '/CRM/GetDesignNames?query=%QUERY',
            wildcard: "%QUERY",
            filter: function (response) {
                let data = JSON.parse(response.result.data);
                return $.map(data, function (design) {
                    return {
                        name: design.DesignName,
                        number: design.DesignNumber
                    }
                });
            }
        }
    });

    $('#taDesignName').typeahead(
        {
            hint: false,
            highlight: true,
            minLength: 1
        },
        {
            name: 'designNames',
            displayKey: 'name',
            source: designs.ttAdapter(),
            templates: {
                empty: [
                    '<div class="empty-message">',
                    'Sorry no data !',
                    '</div>'
                ].join('\n')
            }
        }).bind('typeahead:select', function (ev, suggestion) {
            $('#taDesignNumber').val(suggestion.number);
        });

    $('#taDesignNumber').typeahead(
        {
            hint: false,
            highlight: true,
            minLength: 1
        },
        {
            name: 'designNumbers',
            displayKey: 'number',
            source: designs.ttAdapter(),
            templates: {
                empty: [
                    '<div class="empty-message">',
                    'Sorry no data !',
                    '</div>'
                ].join('\n')
            }
        }).bind('typeahead:select', function (ev, suggestion) {
            $('#taDesignName').val(suggestion.name);
        });

    let boardTypes = new Bloodhound({
        datumTokenizer: function (d) {
            return Bloodhound.tokenizers.whitespace(d.name);
        },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        limit: 5,
        remote: {
            url: '/CRM/GetBoardTypes?query=%QUERY',
            wildcard: "%QUERY",
            filter: function (response) {
                let data = JSON.parse(response.result.data);
                return $.map(data, function (boardType) {
                    return {
                        name: boardType.BoardTypeName,
                        id: boardType.Id
                    }
                });
            }
        }
    });

    $('#taBoardType').typeahead(
        {
            hint: false,
            highlight: true,
            minLength: 1
        },
        {
            name: 'boardTypes',
            displayKey: 'name',
            source: boardTypes,
            templates: {
                empty: [
                    '<div class="empty-message">',
                    'Sorry no data !',
                    '</div>'
                ].join('\n')
            }
        })
        .bind('typeahead:select', function (ev, suggestion) {
            $('#hdnBoardType').val(suggestion.id);
        });
    $('#taBoardType').on('input', function () {
        if ($(this).val() === '') {
            $('#hdnBoardType').val('');
        }
    });
    function populateDropdown(dropdownId, ajaxUrl, selectedValue, defaultOptionText, valueKey, displayKey) {
        $.ajax({
            url: ajaxUrl,
            method: "GET",
            dataType: "json",
            success: function (response) {
                if (response.result.msg == "OK") {
                    let items = response.result.data;
                    let optionHTML = `<option value="" disabled selected>${defaultOptionText}</option>`;

                    for (let i = 0; i < items.length; i++) {
                        let isSelected = (selectedValue && items[i][valueKey] == selectedValue) ? 'selected' : '';
                        optionHTML += ` <option value="${items[i][valueKey]}" ${isSelected}>
                                        ${items[i][displayKey]}
                                    </option>`;
                    }

                    $(`#${dropdownId}`).empty().append(optionHTML);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error:", status, error);
            }
        });
    }


    let designUserId = $('#hdnDesignUserId').val();
    populateDropdown('DesignUsers', '/Users/GetDesignUsers', designUserId, 'Select User', 'id', 'name');

    let toolConfigurationId = $('#hdnToolConfigurationId').val();
    populateDropdown('toolConfiguration', '/CRM/GetToolConfiguraions', toolConfigurationId, 'Select Tool Configuration', 'id', 'toolConfigurationName');

    let toolTypeId = $('#hdnToolTypeId').val();
    populateDropdown('toolTypes', '/CRM/GetToolTypes', toolTypeId, 'Select Tool Type', 'id', 'itemTypeName');
  
});

function saveEnquiry() {
    // Check form validity
    if (!$('#enquiryCreateForm').valid()) {
        $('#enquiryCreateForm').find('input[required], select[required]').each(function () {
            this.setCustomValidity("");
        });
        return;
    }

    // Prepare form data
    let enquiryData = new FormData(document.querySelector('#enquiryCreateForm'));

    // Gather material data
    const rows = $('#tblMaterials tbody tr');
    const validMaterials = $(`[id^="hdnMaterialId"]`);
    let flag = !Array.from(validMaterials).some(el => el.value === "0");

    if (flag) {
        const enquiryId = $('#Id').val();
        rows.each(function (index) {
            const row = $(this);
            // Added logic to fetch the existing material index
            const id = row.find('[id^="hdnMaterialId"]').attr('id');
            const match = id.match(/\d+$/);
            if (match) {
                const materialindex = parseInt(match[0], 10);

                const hdnEnquiryMaterial = $(`#hdnEnquiryMaterialId${materialindex}`).val();
                if (enquiryId != 0 && hdnEnquiryMaterial) {
                    enquiryData.append(`EnquiryMaterials[${index}].Id`, hdnEnquiryMaterial);
                }
                enquiryData.append(`EnquiryMaterials[${index}].EnquiryId`, enquiryId);
                enquiryData.append(`EnquiryMaterials[${index}].MaterialId`, $(`#hdnMaterialId${materialindex}`).val());
                enquiryData.append(`EnquiryMaterials[${index}].SortOrder`, materialindex);
            }
        });
    }

    // Set additional fields
    $('#hdnHighPriority').val($('#IsHighPriority').is(":checked"));
    $('#hdnBraile').val($('#IsBraile').is(":checked"));
    $('#hdnEmboss').val($('#IsEmboss').is(":checked"));

    // Send data to server
    $.ajax({
        type: "POST",
        url: "/CRM/SaveEnquiry",
        data: enquiryData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                abp.notify.info('Saved Successfully');
                window.location.href = '/CRM/EnquiryList';
            } else {
                abp.notify.info('Error Saving Enquiry');
            }
        },
        error: function (e) {
            console.log(e);
            abp.notify.error('Error calling Save function');
        }
    });
}

// File operations
function UploadFile() {
    $("#fileUpload").click();
}


// Checkbox status change
function setStatus(isChecked, controlClass) {
    const elements = document.querySelectorAll('.' + controlClass);
    const hdControl = $('#hdn' + controlClass);

    elements.forEach(e => {
        if (isChecked) {
            e.removeAttribute("disabled");
            e.setAttribute("enabled", true);
            e.style.cursor = "auto";
        } else {
            e.setAttribute("disabled", true);
            e.removeAttribute("enabled");
            e.value = '';
            e.style.cursor = "not-allowed";
        }
    });

    hdControl.val(isChecked);
}

//populates Material Table data
function fetchEnquiryMaterials(enquiryId) {
    $.ajax({
        type: "GET",
        url: `/CRM/GetEnquiryMaterials?enquiryId=${enquiryId}`,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg === "OK") {
                const data = JSON.parse(response.result.data);
                data.forEach((value, index) => {
                    $("#hdnEnquiryMaterialId" + index).val(value.Id);
                    $("#itemCode" + index).val(value.ItemCode);
                    $("#hdnMaterialId" + index).val(value.MaterialId);
                    $("#materialName" + index).val(value.MaterialName);
                    console.log(value.MaterialName);
                    $("#materialName" + index + " option").each(function () {
                        if ($(this).val() == value.MaterialId) {
                            $(this).prop('selected', true);
                        } else {
                            $(this).prop('selected', false);
                        }
                    });

                    if (index < data.length - 1) {
                        CloneRow();
                    }                
                });
            }
        },
        error: function () {
            abp.notify.error("Error occurred while fetching enquiry materials.");
        }
    });
}

function generateEnquiryId() {
    let name = $('#taCustomers').val();
    let idType = "EnquiryId"
    if (($("#Id").val() == '' || $("#Id").val() == '0')) {
        $.ajax({
            url: `/Common/GetIdByPreference?idType=${idType}&name=${name}`,
            method: "GET",
            dataType: "json",
            success: function (data) {
                $("#EnquiryId").val(data.result.id);
            },
            error: function (xhr, status, error) {
                console.error("Error:", status, error);
            }
        });
    }
}

function GetMaterialsByType(obj) {
    const typeId = obj.value;
    const urlname = typeId == 3
        ? "/Materials/GetAllMaterials"
        : `/Materials/GetMaterialByType?typeId=${typeId}`;

    $.ajax({
        type: "GET",
        url: urlname,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg === "OK") {
                const materials = response.result.data;
                const rowcount = $('[id^="MaterialRow"]').length;

                for (let j = 0; j < rowcount; j++) {
                    const materialDropdown = $(`#materialName${j}`);

                    // Clear existing values
                    $(`#itemCode${j}`).val('');
                    $(`#hdnMaterialId${j}`).val(0);
                    $(`#hdnMaterialName${j}`).val('');
                    materialDropdown.find("option").remove();

                    // Create default and material options
                    let optionHTML = '<option value="" selected>Select Material</option>';
                    materials.forEach(material => {
                        optionHTML += `<option value="${material.id}">${material.displayName}</option>`;
                    });

                    materialDropdown.append(optionHTML);
                }

                // Remove extra rows if more than one
                if (rowcount > 1) {
                    for (let j = 1; j < rowcount; j++) {
                        $(`#MaterialRow${j}`).remove();
                    }
                }
            } else {
                clearMaterialDropdowns();
            }
        },
        error: function () {
            abp.notify.error("Error occurred while fetching materials.");
        }
    });
}

function clearMaterialDropdowns() {
    const rows = $('[id^="MaterialRow"]');
    const rowcount = rows.length;

    rows.each(function (index) {
        const materialDropdown = $(`#materialName${index}`);

        // Clear values and options
        $(`#itemCode${index}`).val('');
        $(`#hdnMaterialId${index}`).val(0);
        $(`#hdnMaterialName${index}`).val('');
        materialDropdown.empty();

        // Add default option
        materialDropdown.append('<option value="" selected>Nothing to Select</option>');
    });

    // Remove extra rows if more than one
    if (rowcount > 1) {
        rows.slice(1).remove(); // Removes all rows except the first
    }
}


let cloneCount = 1;
function CloneRow() {
     //$("#materialName" + cloneCount - 1).val();
    var materialselection = document.getElementById('materialName' + (cloneCount - 1)).value;
    const errmsg = document.getElementById('erraddRow');
    if (materialselection == "" || materialselection == undefined) {
        errmsg.textContent = "Please select materialName before add new row";
        errmsg.style.color = 'red';
        return;
    } else {
        errmsg.textContent = "";
    }

    var row = $("#MaterialRow").clone(true)
        .attr('id', 'MaterialRow' + cloneCount, 'class', 'row')
        .insertAfter('[id^=MaterialRow]:last')
        .find('input,select').val("");

    $('#MaterialRow' + cloneCount).find('input,select').each(function () {
        $(this).attr('id', $(this).attr('id').replace(/.$/, cloneCount));
        $(this).attr('name', $(this).attr('name') + cloneCount);
    });
    $('#MaterialRow' + cloneCount).find('.fa-trash').removeAttr('hidden');
    cloneCount++
}


////Update ItemCode based on Material name Change
function fnFillMaterialInfo(obj) {
    const selectedId = obj.id;
    const lastCharOfId = selectedId.slice(-1); // Get the last character of the ID
    
    const url = `/Materials/GetMaterialById?id=${obj.value}`;
    const isSuffixNumeric = !isNaN(lastCharOfId);
    const suffix = isSuffixNumeric ? lastCharOfId : '';

    // Cache jQuery selectors
    const $itemCode = $(`#itemCode${suffix}`);
    const $hdnMaterialId = $(`#hdnMaterialId${suffix}`);

    if (obj.value == '') {
        $itemCode.val('');
        return;
    }

    $.ajax({
        type: "GET",
        url: url,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg === "OK") {
                const data = JSON.parse(response.result.data);

                // Update fields
                $itemCode.val(data.ItemCode);
                $hdnMaterialId.val(data.Id);
                const errmsg = document.getElementById('erraddRow');
                errmsg.textContent = "";
            }
        },
        error: function () {
            abp.notify.error("Error occurred while fetching material information.");
        }
    });
}
