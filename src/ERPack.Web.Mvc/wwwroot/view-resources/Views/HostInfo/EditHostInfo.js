(function ($) {
    var _HostTaxationInfo = abp.services.app.hostTaxationInfo,
        l = abp.localization.getSource('ERPack'),
        _$form = $('#EditHostForm'),
        _$table = $('#taxationDetailsTable');

    var _$taxationDetailsTable = _$table.DataTable({
        paging: true,
        ordering: true,
        listAction: {
            ajaxFunction: _HostTaxationInfo.getAll,
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$taxationDetailsTable.draw(false)
            }
        ],
        scrollX: !0,
        select: {
            style: 'multi'
        },
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                defaultContent: '',
                sortable: false,
                checkboxes: {
                    selectRow: true
                },
                render: (data, type, row, meta) => {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                targets: 1,
                data: 'addressName'
            },
            {
                targets: 2,
                data: 'address1'
            },
            {
                targets: 3,
                data: 'address2',
            },
            {
                targets: 4,
                data: 'city',
            },
            {
                targets: 5,
                data: 'state',
            },
            {
                targets: 6,
                data: 'country'
            },
            {
                targets: 7,
                data: 'pinCode'
            },
            {
                targets: 8,
                data: 'panNumber'
            },
            {
                targets: 9,
                data: 'isDefault',
                sortable: false,
                render: (data, type, row, meta) => {
                    return `<input type="checkbox" class="is-default-checkbox change-default-details" ${data ? 'checked disabled' : ''} data-details-id="${row.id}">`;
                }
            },
            {
                targets: 10,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary" onclick="openAddEditModal(${row.id})">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    _$form.validate({
        rules: {
            CompanyName: "required",
            CurrencyId: "required"
        },
        messages: {
            CompanyName: "Please enter company name",
            CurrencyId: "Please select a currency"
        }
    });

    _$form.find('.save-button').on('click', (e) => {
        _$form.validate({
            rules: {
                CompanyName: "required",
                CurrencyId: "required"
            },
            messages: {
                CompanyName: "Please enter company name",
                CurrencyId: "Please select a currency"
            }
        });
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }

        const logoFile = $("#logoFile")[0].files[0];
        const documentsLogoFile = $("#documentsLogoFile")[0].files[0];
        const maxFileSize = 4 * 1024 * 1024;
        const allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;

        if (logoFile && logoFile.size > maxFileSize) {
            abp.notify.error(`${logoFile.name} size must not exceed 4 MB.`);
            return;
        }
        if (logoFile && !allowedExtensions.exec(logoFile.name)) {
            abp.notify.error(`${logoFile.name} must be a valid file type (JPEG, JPG, PNG).`);
            return;
        }

        if (documentsLogoFile && documentsLogoFile.size > maxFileSize) {
            abp.notify.error(`${documentsLogoFile.name} file size must be less than 4 MB.`);
            return;
        }
        if (documentsLogoFile && !allowedExtensions.exec(documentsLogoFile.name)) {
            abp.notify.error(`${documentsLogoFile.name} must be a valid file type (JPEG, JPG, PNG).`);
            return;
        }

        let hostInfo = new FormData($('form')[0]);
        hostInfo.append("DocumentsLogoFile", documentsLogoFile);
        hostInfo.append("LogoFile", logoFile);

        hostInfo.append("DocumentsLogo", $("#hdnDocumentsLogoFile").val());
        hostInfo.append("Logo", $("#hdnLogoFile").val());

        abp.ui.setBusy(_$form);
        $.ajax({
            type: "POST",
            url: "/Tenants/Update",
            data: hostInfo,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success == true) {
                    abp.ui.clearBusy(_$form);
                    abp.notify.info(l('SavedSuccessfully'));
                    window.location = '/Tenants/Edit';
                }
            },
            error: function () {

            }
        });

    });

    $(document).on('click', '#submit-hostTaxationForm', (e) => {
        _$hostInfoDetailForm = $("#hosttaxationinfoform");
        _$hostInfoDetailForm.validate({
            rules: {
                addressName: "required",
                address1: "required"
            },
            messages: {
                addressName: "Address Name is required",
                address1: "Address 1 is required"
            }
        });

        e.preventDefault();
        if (!_$hostInfoDetailForm.valid()) {
            return;
        }
        if (!validatePAN()) {
            return false;
        }

        if (!validateGST()) {
            return false;
        }
        if (!validatePinCode()) {
            return false;
        }

        let hostTaxationInfo = new FormData(_$hostInfoDetailForm[0]);
        hostTaxationInfo.append("IsDefault", $('#isDefault').is(':checked'));
        abp.ui.setBusy($('body'));
        $.ajax({
            type: "POST",
            url: "/Tenants/AddEditCompanyTaxationDetails",
            data: hostTaxationInfo,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.result.success == true) {
                    $("#AddEditHostTaxationModal").hide()
                    abp.ui.clearBusy($('body'));
                    abp.notify.info(l('SavedSuccessfully'));
                    window.location = '/Tenants/Edit';
                } else {
                    abp.ui.clearBusy($('body'));
                    abp.notify.error(response.result.msg);
                }
            },
            error: function () {

            }
        });
    });

    $(document).on('change', '.change-default-details', function () {
        var detailsId = $(this).attr("data-details-id");

        ChangeDefaultDetails(detailsId);
    });
    function ChangeDefaultDetails(id) {
        abp.message.confirm(
            abp.utils.formatString(
                'Are you sure you want to change the default Company Taxation/Address Details details?'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _HostTaxationInfo.changeDefaultDetails({
                        id: id
                    }).done(() => {
                        abp.notify.info('Successfully Changed Default Details');
                        _$taxationDetailsTable.ajax.reload();
                    });
                } else {
                    $(`.is-default-checkbox[data-details-id=${id}]`).prop('checked', false)
                }
            }
        );
    }
})(jQuery);

const fileInputs = document.querySelectorAll('.custom-file-input');
fileInputs.forEach(input => {
    input.addEventListener('change', function () {
        updateFileLabel(input);
    });
});

function updateFileLabel(input) {
    const label = document.querySelector(`label[for="${input.id}"]`);
    const files = Array.from(input.files);
    const fileNames = files.map(file => file.name).join(', ');
    label.textContent = fileNames || 'Choose Documents';
}

function isValidPAN(pan) {
    const panPattern = /^[A-Z]{5}[0-9]{4}[A-Z]$/;
    return panPattern.test(pan);
}

function isValidGST(gst) {
    // Regular expression for validating GST number
    const gstRegex = /^[0-9]{2}[A-Z]{4}[0-9]{4}[A-Z][0-9][Z][0-9]$/;
    return gstRegex.test(gst);
}

function validatePAN() {
    const panInput = document.getElementById('panNumber').value;
    const panRegex = /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
    const messageDiv = document.getElementById('panError');

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
        messageDiv.textContent = 'Please Enter PAN number';
        return false;
    }

}

function validateGST() {    
    const gstInput = document.getElementById('GSTNumber').value;    
    const gstRegex = /^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[A-Z1-9]{1}Z{1}[A-Z0-9]{1}$/;

    const messageDiv = document.getElementById('gstError');

    if (gstInput != "") {
        if (gstRegex.test(gstInput)) {
            messageDiv.textContent = '';
            return true;
        } else {
            messageDiv.textContent = 'Please enter a valid GST number. Format: 00XXXXX0000X0ZX0';
            messageDiv.style.color = 'red';
            return false;
        }
    } else {
        messageDiv.textContent = 'Please Enter GST number';
        return false;
    }

}

function validatePinCode() {
    const pinCodeInput = document.getElementById('pincode').value;
    const pinCodeRegex = /^[0-9]{6}$/; // Regex for a 6-digit pin code
    const messageDiv = document.getElementById('pinCodeError');

    if (pinCodeInput !== "") {
        if (pinCodeRegex.test(pinCodeInput)) {
            messageDiv.textContent = ''; // Clear any previous error message
            return true; // Valid pin code
        } else {
            messageDiv.textContent = 'Please enter a valid 6-digit pin code.';
            messageDiv.style.color = 'red';
            return false; // Invalid pin code
        }
    } else {
        messageDiv.textContent = 'Please enter a pin code.';
        messageDiv.style.color = 'red'; // Ensure the error message is red
        return false; // No input
    }
}