(function ($) {
    let l = abp.localization.getSource('ERPack'),
        _$form = $('#customerCreateForm'),
        _$contactform = $('#contactForm'),
        _$customerTaxationForm = $('#TaxationForm'),
        _$table = $('#ContactsTable'),
        _$modalCategory = $('#CategoryCreateModal'),
        _$taxationTable = $('#CustomerTaxationTable');
    var _customerService = abp.services.app.customer

    /* Customer Start */
    $.validator.addMethod("PanNumber", function (value, element) {
        return this.optional(element) || /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/.test(value);
    }, "");

    _$form.validate({
        rules: {
            CategoryId: "required",
            BusinessCurrencyId: "required",
            StateId: "required",
            CountryId: "required",
            PAN: {
                PanNumber: true
            }
        },
        messages: {
            CategoryId: "Please select a category.",
            BusinessCurrencyId: "Please select a currency.",
            StateId: "Please select a state.",
            CountryId: "Please select a country.",
            PAN: {
                PanNumber: "Please enter valid PAN number."
            }
        }
    });
  
    $('#PAN').keyup(function () {        
        var panNumber = $(this).val();
        var panNumber = panNumber.toUpperCase();
        $(this).val(panNumber);
    })

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        let $frm = $('form');
        let customer = new FormData($frm[0]);
        const maxFileSize = 4 * 1024 * 1024; // 4 MB in bytes
        const allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;
        const fileToValidate = $("#ImageFile")[0].files[0];

        if (fileToValidate != undefined) {
            if (!allowedExtensions.exec(fileToValidate.name)) {
                abp.notify.error(`${fileToValidate.name} must be a valid file type (JPEG, JPG, PNG).`);
                return;
            }

            if (fileToValidate.size > maxFileSize) {
                abp.notify.error(`${fileToValidate.name} size must not exceed 4 MB.`);
                return;
            }

            customer.append("ImageFile", fileToValidate);
        }
        let companyLogo = $("#hdnFile").val();
        if (companyLogo) {
            customer.append("CompanyLogo", companyLogo);
        }
        $.ajax({
            type: "POST",
            url: "/Customers/CreateCustomer",
            data: customer,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success == true) {
                    if (response.result["id"] == 0) {
                        abp.notify.error(l(response.result["msg"]));
                    } else {
                        abp.notify.info(l('SavedSuccessfully'));
                        $('form')[0].reset();
                        if (response.result.returnUrl == "" || response.result.returnUrl == null) {
                            window.location = '/Customers/CreateCustomer?customerId=' + response.result.id;
                        }
                        else {
                            window.location.href = response.result.returnUrl + '?enquiryId=0&customerId=' + response.result.id;
                        }
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("Error: ", status, error);
                abp.notify.error('Error In Saving');
            },
            complete: function () {
                abp.ui.clearBusy(_$form);
            }
        });
    });

    $('#saveCategory').on('click', (e) => {
        let categoryName = $("#CategoryName").val();
        if (categoryName == null || categoryName == '' || categoryName == undefined) {
            $('#category-name').show();
            return;
        }
        else {
            $('#category-name').hide();
        }
        $.ajax({
            type: "POST",
            url: "/Common/AddCategory?name=" + categoryName,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.result.msg == "OK") {
                    let optionHTML = `
                                      <option value="${data.result.id}">
                                                     ${categoryName}
                                      </option>`;
                    $('#CategoryId').append(optionHTML);
                    abp.notify.info(l('Saved Successfully'));
                    $('#CategoryName').val('');
                    _$modalCategory.modal('hide');
                }
                else {
                    abp.notify.error(data.result.msg);
                }
            },
            error: function () {
            }
        });
    });
    /* Customer End */

    /* Contact Start */
    var _$contactTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: false,
        lengthChange: false,
        info: false,
        listAction: {
            ajaxFunction: function (data) {
                var customerId = $('#Id').val();
                data.customerId = customerId;
                return _customerService.getAllContact(data);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$contactTable.draw(false)
            }
        ],
        scrollX: !0,
        select: {
            style: 'multi'
        },
        order: [[0, 'asc']],
        columnDefs: [
            {
                targets: 0,
                defaultContent: '',
                sortable: false,
                render: (data, type, row, meta) => {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                data: 'fullName'
            },
            {
                targets: 2,
                data: 'designation',
                sortable: false,
            },
            {
                targets: 3,
                data: 'emailAddress',
                sortable: false,
            },
            {
                targets: 4,
                data: 'contactNo',
                sortable: false,
            },
            {
                targets: 5,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-contact" id="edit-contact" data-contact-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    $(document).on('click', '#edit-contact', function (e) {
        var contactId = $(this).attr("data-contact-id");
        e.preventDefault();
        $.ajax({
            type: "GET",
            url: "/Customers/CreateContact",
            data: { contactId: contactId },
            dataType: "html",
            success: function (response) {
                $('#contact-body').html('').html(response);
                $('#ContactCreateModal .modal-title').text("Edit Contact Detail");
                $('#ContactCreateModal').modal('show');
            }
        });
    });

    $.validator.addMethod("Mobile", function (value, element) {
        const regex = /^\+91\d{10}$/;
        return this.optional(element) || regex.test(value);
    }, "Please enter a valid phone number in the format +91 0000000000.");

    $.validator.addMethod("Email", function (value, element) {
        const regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        return this.optional(element) || regex.test(value);
    }, "Please enter a valid email.");

    _$contactform.validate({
        rules: {
            FullName: "required",
            ContactNo: {
                required: true,
                Mobile: true
            },
            EmailAddress: {
                required: true,
                Email: true
            },
        },
        messages: {
            FullName: "Full name is required",
            ContactNo: {
                required: "Contact number is required",
                Mobile: "Please enter a valid phone number"
            },
            EmailAddress: {
                required: "Email address is required",
                Email: "Please enter a valid email"
            }
        }
    });

    _$contactform.find('.save-contact-button').on('click', (e) => {
        e.preventDefault();
        if (!_$contactform.valid()) {
            return;
        }
        var AddEditContactModel = {
            FullName: $("#contactForm #FullName").val(),
            Designation: $("#contactForm #Designation").val(),
            EmailAddress: $("#contactForm #EmailAddress").val(),
            ContactNo: $("#contactForm #ContactNo").val(),
            CustomerId: $("#Id").val(),
            Id: $("#contactForm #ContactId").val()
        };
        $.ajax({
            type: "POST",
            url: '/Customers/CreateContact',
            data: { input: AddEditContactModel },
            success: function (data) {
                if (data.result.msg == "OK") {
                    abp.notify.info(l('SavedSuccessfully'));
                    window.location = '/Customers/CreateCustomer?customerId=' + $('#Id').val();
                }
                else if (data.result.msg == "ERROR") {
                    abp.notify.error(`'${AddEditContactModel.EmailAddress}' already taken.`);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error: ", status, error);
                abp.notify.error(xhr.responseJSON.error.message);
            },
            complete: function () {
                abp.ui.clearBusy(_$form);
            }
        });
    });
    /* Contact End */

    /* Taxation Start */
    var _$taxation = _$taxationTable.DataTable({
        paging: true,
        ordering: true,
        searching: false,
        lengthChange: false,
        info: false,
        listAction: {
            ajaxFunction: function (data) {
                var customerId = $('#Id').val();
                data.customerId = customerId;
                return _customerService.getAllTaxation(data);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$taxation.draw(false)
            }
        ],
        scrollX: !0,
        select: {
            style: 'multi'
        },
        order: [[0, 'asc']],
        columnDefs: [
            {
                targets: 0,
                defaultContent: '',
                sortable: false,
                render: (data, type, row, meta) => {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                data: 'addressName'
            },
            {
                targets: 2,
                data: 'address1',
                sortable: false,
            },
            {
                targets: 3,
                data: 'address2',
                sortable: false,
            },
            {
                targets: 4,
                data: 'city',
                sortable: false,
            },
            {
                targets: 5,
                data: 'state.stateName',
                sortable: false,
            },
            {
                targets: 6,
                data: 'country.countryName',
                sortable: false,
            },
            {
                targets: 7,
                data: 'pinCode',
                sortable: false,
            },
            {
                targets: 8,
                data: 'panNumber',
                sortable: false,
            },
            {
                targets: 9,
                data: 'gstNumber',
                sortable: false,
            },
            {
                targets: 10,
                data: 'isDefault',
                render: data => `<input type="checkbox" disabled ${data ? 'checked' : ''}>`,
                sortable: false,
            },
            {
                targets: 11,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-taxation" id="edit-taxation" data-taxation-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    $(document).on('click', '#edit-taxation', function (e) {
        var taxationId = $(this).attr("data-taxation-id");
        e.preventDefault();
        $.ajax({
            type: "GET",
            url: "/Customers/CreateCustomerTaxation",
            data: { taxationId: taxationId },
            dataType: "html",
            success: function (response) {
                $('#taxation-body').html('').html(response);
                $('#ContactCreateModal .modal-title').text("Edit Taxation Detail");
                $('#TaxationCreateModal').modal('show');
            }
        });
    });

    $.validator.addMethod("Pincode", function (value, element) {
        return this.optional(element) || /^\d{6}$/.test(value);
    }, "");

    $.validator.addMethod("validateGST", function (value, element) {
        return this.optional(element) || /^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[A-Z1-9]{1}Z{1}[A-Z0-9]{1}$/.test(value);
    }, "");

    _$customerTaxationForm.validate({
        rules: {
            AddressName: "required",
            Address1: "required",
            City: "required",
            CountryId: "required",
            StateId: "required",
            PinCode: {
                required: true,
                Pincode: true
            },
            PANNumber: {
                required: true,
                PanNumber: true
            },
            GSTNumber: {
                required: true,
                validateGST: true
            },
        },
        messages: {
            AddressName: "Address name is required.",
            Address1: "Address 1 is required.",
            City: "City is required.",
            CountryId: "Please select Country.",
            StateId: "Please select state.",
            PinCode: {
                required: "Pincode is required",
                Pincode: "Please enter a valid 6-digit PIN code."
            },
            PANNumber: {
                required: "PAN number is required",
                PanNumber: "Please enter valid PAN number."
            },
            GSTNumber: {
                required: "GST number is required",
                validateGST: "Please enter a valid GST number. Format: 00XXXXX0000XZX"
            },
        }
    });

    _$customerTaxationForm.find('.save-taxation-button').on('click', (e) => {
        e.preventDefault();
        if (!_$customerTaxationForm.valid()) {
            return;
        }
        var AddEditTaxationInfoModel = {
            AddressName: $("#TaxationForm #AddressName").val(),
            Address1: $("#TaxationForm #Address1").val(),
            Address2: $("#TaxationForm #Address2").val(),
            City: $("#TaxationForm #City").val(),
            StateId: $("#TaxationForm #StateId").val(),
            CountryId: $("#TaxationForm #CountryId").val(),
            CustomerId: $("#Id").val(),
            PANNumber: $("#TaxationForm #PANNumber").val(),
            PinCode: $("#TaxationForm #PinCode").val(),
            GSTNumber: $("#TaxationForm #GSTNumber").val(),
            Id: $("#TaxationForm #TaxationId").val(),
            IsDefault: $("#TaxationForm #IsDefault").prop("checked")
        };
        $.ajax({
            type: "POST",
            url: '/Customers/CreateCustomerTaxation',
            data: { input: AddEditTaxationInfoModel },
            success: function (data) {
                if (data.result.msg == "OK") {
                    abp.notify.info(l('SavedSuccessfully'));
                    window.location = '/Customers/CreateCustomer?customerId=' + $('#Id').val();
                }
                else {
                    abp.notify.error(data.result.msg);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error: ", status, error);
                abp.notify.error('Error In Saving');
            },
            complete: function () {
                abp.ui.clearBusy(_$form);
            }
        });
    });
    /* Taxation End */
    
})(jQuery);

function OpenTaxationModal() {
    $('#taxation-body').load('/Customers/CreateCustomerTaxation', function () {
        $('#TaxationCreateModal').modal('show'); 
    });
}
function OpenContactModal() {
    $('#contact-body').load('/Customers/CreateContact', function () {
        $('#ContactCreateModal').modal('show'); 
    });
}