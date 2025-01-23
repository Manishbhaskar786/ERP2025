(function ($) {
    var _purchaseOrderService = abp.services.app.purchaseOrder,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#PurchaseOrdersTable');

    var _$purchaseOrdersTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _purchaseOrderService.getAll,
            inputFilter: function () {
                return $('#PurchaseOrdersSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => { _$purchaseOrdersTable.ajax.reload(null, false); }
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                className: 'dt-control',
                defaultContent: '',
            },
            {
                targets: 1,
                data: 'poCode'
            },
            {
                targets: 2,
                data: 'vendorName'
            },
            {
                targets: 3,
                data: 'purchaseItem'
            },
            {
                targets: 4,
                data: 'creationTime'
            },
            {
                targets: 5,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-purchaseorder" data-purchaseorder-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-purchaseorder" data-purchaseorder-id="${row.id}" data-purchaseorder-name="${row.poCode}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    // Add event listener for opening and closing details
    _$purchaseOrdersTable.on('click', 'td.dt-control', function (e) {
        let tr = e.target.closest('tr');
        let row = _$purchaseOrdersTable.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        }
        else {
            // Open this row
            format(row);
        }
    });

    // Formatting function for row details - modify as you need
    function format(row) {

        let purchaseorderId = row.data().id;
        let html = `<table id="" class="table table-striped table-bordered nowrap">
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
                <tbody id="tblVal">`;

        let purchaseOrderItems;
        if (purchaseorderId) {
            $.ajax({
                url: `/PurchaseOrders/GetPurchaseOrderItems?purchaseorderId=${purchaseorderId}`,
                method: "GET",
                dataType: "json",
                success: function (response) {
                    purchaseOrderItems = response.result.data;
                    $(purchaseOrderItems).each(function () {
                        html += "<tr>";
                        html += "<td>" + this.itemType + "</td>";
                        html += "<td>" + this.materialCode + "</td>";
                        html += "<td>" + this.materialName + "</td>";
                        html += "<td>" + this.buyingUnit + "</td>";
                        html += "<td>" + this.price + "</td>";
                        html += "<td>" + this.quantity + "</td>";
                        html += "<td>" + this.purchaseDate + "</td>";
                        html += "<td>" + this.amount + "</td>";
                        html += "<td>" + this.cgst + "</td>";
                        html += "<td>" + this.sgst + "</td>";
                        html += "<td>" + this.igst + "</td>";
                        html += "<td>" + this.gstAmount + "</td>";
                        html += "<td>" + this.totalAmount + "</td>";
                        html += "</tr>";
                    });
                    html += "</tbody>";
                    html += "</table>";

                    row.child(html).show();
                },
                error: function (xhr, status, error) {
                    // Handle errors
                    console.error("Error:", status, error);
                }
            });
        }
    }

    $(document).on('click', '.delete-purchaseorder', function () {
        var purchaseorderId = $(this).attr("data-purchaseorder-id");
        var purchaseorderName = $(this).attr('data-purchaseorder-name');

        deletePurchaseOrder(purchaseorderId, purchaseorderName);
    });

    function deletePurchaseOrder(purchaseorderId, purchaseorderName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                purchaseorderName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _purchaseOrderService.delete({
                        id: purchaseorderId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$purchaseOrdersTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-purchaseorder', function (e) {
        var purchaseOrderId = $(this).attr("data-purchaseorder-id");
        e.preventDefault();
        window.location.href = '/PurchaseOrders/EditPurchaseOrder?purchaseOrderId=' + purchaseOrderId;
    });

    $('.btn-search').on('click', (e) => {
        _$purchaseOrdersTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$purchaseOrdersTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
