(function ($) {
    var _purchaseReceiveService = abp.services.app.purchaseReceive,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#PurchaseRecievesTable');

    var _$purchaseRecievesTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _purchaseReceiveService.getAll,
            inputFilter: function () {
                return $('#PurchaseReceivesSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$purchaseRecievesTable.draw(false)
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
                data: 'id'
            },
            {
                targets: 2,
                data: 'purchaseReceiveDate'
            },
            {
                targets: 3,
                data: 'poCode'
            },
            {
                targets: 4,
                data: 'vendorName'
            },
            {
                targets: 5,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-purchasereceive" data-purchasereceive-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-purchasereceive" data-purchasereceive-id="${row.id}" data-purchasereceive-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    // Add event listener for opening and closing details
    _$purchaseRecievesTable.on('click', 'td.dt-control', function (e) {
        let tr = e.target.closest('tr');
        let row = _$purchaseRecievesTable.row(tr);

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

        let purchaseReceiveId = row.data().id;
        let html = `<table id="" class="table table-striped table-bordered nowrap">
                                    <thead>
                                        <tr>
                                            <th>Material</th>
                                            <th>Unit</th>
                                            <th>Qty Ordered</th>
                                            <th>Qty Received</th>
                                            <th>Store</th>
                                        </tr>
                                    </thead>
                <tbody id="tblVal">`;

        let purchaseReceiveItems;
        if (purchaseReceiveId) {
            $.ajax({
                url: `/PurchaseReceives/GetPurchaseReceiveItems?purchaseReceiveId=${purchaseReceiveId}`,
                method: "GET",
                dataType: "json",
                success: function (response) {
                    purchaseReceiveItems = response.result.data;
                    $(purchaseReceiveItems).each(function () {
                        html += "<tr>";
                        html += "<td>" + this.materialName + "</td>";
                        html += "<td>" + this.unit + "</td>";
                        html += "<td>" + this.quantityOrdered + "</td>";
                        html += "<td>" + this.quantityReceived + "</td>";
                        html += "<td>" + this.storeName + "</td>";
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

    $(document).on('click', '.delete-purchasereceive', function () {
        var purchasereceiveId = $(this).attr("data-purchasereceive-id");
        var purchasereceiveName = $(this).attr('data-purchasereceive-name');

        deletePurchaseOrder(purchasereceiveId, purchasereceiveName);
    });

    function deletePurchaseOrder(purchasereceiveId, purchasereceiveName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                purchasereceiveName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _purchaseReceiveService.delete({
                        id: purchasereceiveId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$purchaseRecievesTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-purchasereceive', function (e) {
        var purchaseOrderId = $(this).attr("data-purchasereceive-id");
        e.preventDefault();
        window.location.href = '/PurchaseReceives/AddPurchaseReceive?purchaseReceiveId=' + purchaseReceiveId;
    });

    $('.btn-search').on('click', (e) => {
        _$purchaseRecievesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$purchaseRecievesTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
