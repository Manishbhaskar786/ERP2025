(function ($) {
    var _inventoryService = abp.services.app.inventory,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#InventoryTable');

    var _$inventoryTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _inventoryService.getAll,
            inputFilter: function () {
                return $('#InventorySearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$inventoryTable.draw(false)
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
                sortable: false
            },
            {
                targets: 1,
                data: 'issueCode'
            },
            {
                targets: 2,
                data: 'creationTime'
            },
            {
                targets: 3,
                data: 'issuedBy'
            },
            {
                targets: 4,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-inventory" data-inventory-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-inventory" data-inventory-id="${row.id}" data-inventory-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    // Add event listener for opening and closing details
    _$inventoryTable.on('click', 'td.dt-control', function (e) {
        let tr = e.target.closest('tr');
        let row = _$inventoryTable.row(tr);
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

        let inventoryId = row.data().id;
        let html = `<table id="EstimateTasksTable" class="table table-striped table-bordered nowrap">
                                    <thead>
                                        <tr>
                                            <th>Type</th>
                                            <th>Item Code</th>
                                            <th>Item Description</th>
                                            <th>FromStore Qty</th>
                                            <th>Qty Transfered</th>
                                            <th>Store Name</th>
                                            <th>Department</th>
                                            <th>Issued By</th>
                                        </tr>
                                    </thead>
                <tbody id="tblVal">`;
        let inventoryItems;
        if (inventoryId) {
            $.ajax({
                url: `/Inventory/GetInventoryItems?inventoryId=${inventoryId}`,
                method: "GET",
                dataType: "json",
                success: function (response) {
                    inventoryItems = response.result.data;
                    $(inventoryItems).each(function () {
                        html += "<tr>";
                        html += "<td>" + this.itemType + "</td>";
                        html += "<td>" + this.materialCode + "</td>";
                        html += "<td>" + this.materialName + "</td>";
                        html += "<td>" + this.superStoreQty + "</td>";
                        html += "<td>" + this.qtyTransferred + "</td>";
                        html += "<td>" + this.toStoreName + "</td>";
                        html += "<td>" + this.departmentName + "</td>";
                        html += "<td>" + this.issuedBy + "</td>";
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

    $('.btn-search').on('click', (e) => {
        _$estimatesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$estimatesTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
