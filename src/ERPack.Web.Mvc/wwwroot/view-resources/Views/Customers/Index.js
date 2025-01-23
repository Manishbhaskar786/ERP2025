(function ($) {
    var _customerService = abp.services.app.customer,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#CustomersTable');

    var _$customersTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _customerService.getAll,
            inputFilter: function () {
                return $('#CustomersSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$customersTable.draw(false)
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
                checkboxes: {
                    selectRow: true
                },
                render: (data, type, row, meta) => {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                targets: 1,
                data: 'name'
            },
            {
                targets: 2,
                data: 'industry'
            },
            {
                targets: 3,
                data: 'category.categoryName',
                sortable: false,
            },
            {
                targets: 4,
                data: 'businessCurrency.name',
                sortable: false,
            },
            {
                targets: 5,
                data: 'city',
                sortable: false,
            },
            {
                targets: 6,
                data: 'state.stateName'
            },
            {
                targets: 7,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-customer" data-customer-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-customer" data-customer-id="${row.id}" data-customer-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    $(document).on('click', '.delete-customer', function () {
        var customerId = $(this).attr("data-customer-id");
        var customerName = $(this).attr('data-customer-name');

        deleteCustomer(customerId, customerName);
    });

    function deleteCustomer(customerId, customerName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                customerName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _customerService.delete({
                        id: customerId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$customersTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-customer', function (e) {
        var customerId = $(this).attr("data-customer-id");
        e.preventDefault();
        window.location.href = '/Customers/CreateCustomer?customerId=' + customerId;
    });

    $(document).on('change ', '.selectAll input[type="checkbox"]', function (e) {
        if (this.checked == true) {
            $('#bulkActions').show();
        }
        else if (this.checked == false) {
            $('#bulkActions').hide();
        }
    });

    abp.event.on('user.edited', (data) => {
        _$customersTable.ajax.reload();
    });

    $('.btn-search').on('click', (e) => {
        _$customersTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$customersTable.ajax.reload();
            return false;
        }
    });

    $('#importForm').on('submit', function (e) {
        e.preventDefault(); // Prevent default form submission

        var formData = new FormData(this);

        abp.ui.setBusy($('body'));
        abp.ajax({
            url: '/Customers/ImportCustomer',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                abp.ui.clearBusy($('body'));
                if (!response.isValid) {
                    $("#import-error").text(response.msg)
                    if (response.msg == "") {
                        if (response.noCategoryErrorMessage != undefined) {
                            abp.notify.error(response.noCategoryErrorMessage);
                            $("#import-customer-modal").modal('hide');
                            _$customersTable.ajax.reload();
                        } else {
                            abp.notify.info(response.validationMessage.m_StringValue, {
                                delay: 5000
                            });
                            $("#import-customer-modal").modal('hide');
                            _$customersTable.ajax.reload();
                        }
                    }
                } else {
                    if (response.validationMessage.m_StringValue != "") {
                        abp.notify.info(response.validationMessage.m_StringValue, {
                            delay: 5000
                        });
                        $("#import-customer-modal").modal('hide');
                        _$customersTable.ajax.reload();
                    } else {
                        abp.notify.success('All Data Imported successfully');
                        $("#import-customer-modal").modal('hide');
                        _$customersTable.ajax.reload();
                    }
                }
            },
            error: function (xhr, status, error) {
                $('#import-error').html("An error occurred while processing your request.");
            }
        });
    });
})(jQuery);

function fnCreatePdf() {
    event.preventDefault();
    var tabledata = $("#CustomersTable").html();
    let obj = {};
    obj.Name = "CustomersList";
    obj.Html = tabledata;
    let data = JSON.stringify(obj);
    $.ajax({
        type: "POST",
        url: "/Common/GeneratePdf",
        data: data,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.success == true) {
                window.open(response.result.data, "_blank");
            }
        },
        error: function () {

        }
    });
}

function fnCreateCSV() {
    event.preventDefault();
    var tabledata = $("#CustomersList").html();
    let obj = {};
    obj.Name = "UserList";
    obj.Html = tabledata;
    let data = JSON.stringify(obj);
    $.ajax({
        type: "POST",
        url: "/Common/GenerateExcel",
        data: data,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.success == true) {
                window.open(response.result.data, "_blank");
            }
        },
        error: function () {

        }
    });
}

$(document).on('click', '#import-customer', function () {
    $("#import-customer-modal").modal('show');
    $(".modal-backdrop").hide()
});

$('#upload-customer-file').on('change', function () {
    var fileName = $(this).val().split('\\').pop();
    $(this).next('.custom-file-label').html(fileName);
});
