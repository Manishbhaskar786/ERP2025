(function ($) {
    let _vendorService = abp.services.app.vendor,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#VendorsTable');

    let _$vendorsTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _vendorService.getAll,
            inputFilter: function () {
                return $('#VendorsSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$vendorsTable.draw(false)
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
                sortable: false,
                defaultContent: '',
                checkboxes: {
                    selectRow: true
                }
            },
            {
                targets: 1,
                data: 'vendorName'
            },
            {
                targets: 2,
                data: 'vendorCode'
            },
            {
                targets: 3,
                data: 'address1'
            },
            {
                targets: 4,
                data: 'address2'
            },
            {
                targets: 5,
                data: 'city'
            },
            {
                targets: 6,
                data: 'state'
            },
            {
                targets:7,
                data: 'pinCode'
            },
            {
                targets: 8,
                data: 'country'
            },
            {
                targets: 9,
                data: 'contactPerson'
            },
            {
                targets: 10,
                data: 'designation'
            },
            {
                targets: 11,
                data: 'gst'
            },
            {
                targets: 12,
                data: 'bankName'
            },
            {
                targets: 13,
                data: 'accountNumber'
            },
            {
                targets: 14,
                data: 'branch'
            },
            {
                targets: 15,
                data: 'ifsc'
            },
            {
                targets: 16,
                data: 'panCard'
            },
            {
                targets: 17,
                data: 'email'
            },
            {
                targets: 18,
                data: 'phoneNo'
            },
            {
                targets: 19,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-vendor" data-vendor-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-vendor" data-vendor-id="${row.id}" data-vendor-name="${row.vendorName}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    $(document).on('click', '.delete-vendor', function () {
        let vendorId = $(this).attr("data-vendor-id");
        let vendorName = $(this).attr('data-vendor-name');

        deleteVendor(vendorId, vendorName);
    });

    function deleteVendor(vendorId, vendorName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                vendorName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _vendorService.delete({
                        id: vendorId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$vendorsTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-vendor', function (e) {
        let vendorId = $(this).attr("data-vendor-id");
        e.preventDefault();
        window.location.href = '/Vendors/AddVendor?vendorId=' + vendorId;
    });

    $('.btn-search').on('click', (e) => {
        _$vendorsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$vendorsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);

function fnCreatePdf() {
    event.preventDefault();
    var tabledata = $("#VendorsTable").html();
    let obj = {};
    obj.Name = "VendorList";
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
    var tabledata = $("#VendorsTable").html();
    let obj = {};
    obj.Name = "VendorList";
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
