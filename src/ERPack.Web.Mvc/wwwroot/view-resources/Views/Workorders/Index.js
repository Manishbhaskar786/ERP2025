(function ($) {
    var _workorderService = abp.services.app.workorder,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#WorkordersTable');

    var _$workordersTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _workorderService.getAll,
            inputFilter: function () {
                return $('#WorkorderSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$workordersTable.draw(false)
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'dt-control',
                defaultContent: '',
            },
            {
                targets: 1,
                data: 'poCode',
                sortable: false
            },
            {
                targets: 2,
                data: 'purchaseItem',
                sortable: false
            },
            {
                targets: 3,
                data: 'documentUrl',
                sortable: false
            },
            {
                targets: 4,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-workorder" data-workorder-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-workorder" data-workorder-id="${row.id}" data-workorder-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    // Add event listener for opening and closing details
    _$workordersTable.on('click', 'td.dt-control', function (e) {
        let tr = e.target.closest('tr');
        let row = _$workordersTable.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        }
        else {
            // Open this row
            row.child(format(row.data().id)).show();
        }
    });

    function format(workorderId) {
        if (workorderId) {
            $.ajax({
                url: `/Workorders/GetWorkorderItems?workorderId=${workorderId}`,
                method: "GET",
                dataType: "json",
                success: function (response) {
                    response.result.data;
                },
                error: function (xhr, status, error) {
                    // Handle errors
                    console.error("Error:", status, error);
                }
            });
        }

        // `d` is the original data object for the row
        return (`<table id="" class="table table-striped table-bordered nowrap">
                                    <thead>
                                        <tr>
                                            <th>Type</th>
                                            <th>Item Code</th>
                                            <th>Item Description</th>
                                            <th>Buying Unit</th>
                                            <th>Buying Price</th>
                                            <th>Quantity Pruchase</th>
                                            <th>Date Of Purchase</th>
                                            <th>Amount</th>
                                            <th>CGST</th>
                                            <th>SGST</th>
                                            <th>IGST</th>
                                            <th>GST Amount</th>
                                            <th>Total Amount</th>
                                        </tr>
                                    </thead>
                <tbody id="tblVal">

                </tbody>
            </table>`
        );
    }

    $(document).on('click', '.delete-workorder', function () {
        var workorderId = $(this).attr("data-workorder-id");
        var workorderName = $(this).attr('data-workorder-name');

        deleteWorkorder(workorderId, workorderName);
    });

    function deleteWorkorder(workorderId, workorderName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                workorderName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _workorderService.delete({
                        id: c
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$workordersTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-workorder', function (e) {
        var workorderId = $(this).attr("data-workorder-id");
        e.preventDefault();
        window.location.href = '/PurchaseOrders/AddWorkorder?workorder=' + workorder;
    });

    $('.btn-search').on('click', (e) => {
        _$workordersTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$workordersTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
