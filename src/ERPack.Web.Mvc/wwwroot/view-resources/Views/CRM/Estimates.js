(function ($) {
    var _estimateService = abp.services.app.estimate,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#EstimatesTable');

    var _$estimatesTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _estimateService.getAll,
            inputFilter: function () {
                return $('#EstimatesSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => {
                    _$estimatesTable.ajax.reload(null, false); // reload the table without resetting pagination
                }
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
                data: 'estimateId'
            },
            {
                targets: 2,
                data: 'customerName'
            },
            {
                targets: 3,
                data: 'totalAmount'
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
                        `   <button type="button" class="btn btn-sm bg-secondary edit-estimate" data-estimate-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-estimate" data-estimate-id="${row.id}" data-estimate-name="estimate">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    // Add event listener for opening and closing details
    _$estimatesTable.on('click', 'td.dt-control', function (e) {
        let tr = e.target.closest('tr');
        let row = _$estimatesTable.row(tr);
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
        let estimateId = row.data().id;
        let html = `<table id="EstimateTasksTable" class="table table-striped table-bordered nowrap">
                                    <thead>
                                        <tr>
                                            <th>Material</th>
                                            <th>Quantity</th>
                                            <th>Unit</th>
                                            <th>Price</th>
                                            <th>Discount</th>
                                            <th>CGST</th>
                                            <th>SGST</th>
                                            <th>IGST</th>
                                            <th>Amount</th>
                                        </tr>
                                    </thead>
                <tbody id="tblVal">`;
        let estimateTasks;
        if (estimateId) {
            $.ajax({
                url: `/CRM/GetEstimateTasks?estimateId=${estimateId}`,
                method: "GET",
                dataType: "json",
                success: function (response) {
                    estimateTasks = response.result.data;
                    $(estimateTasks).each(function () {
                        html += "<tr>";
                        html += "<td>" + this.materialName + "</td>";
                        html += "<td>" + this.qty + "</td>";
                        html += "<td>" + this.sellingUnitName + "</td>";
                        html += "<td>" + this.price + "</td>";
                        html += "<td>" + this.discountPercentage + "</td>";
                        html += "<td>" + this.cgst + "</td>";
                        html += "<td>" + this.sgst + "</td>";
                        html += "<td>" + this.igst + "</td>";
                        html += "<td>" + this.amount + "</td>";
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

    $(document).on('click', '.delete-estimate', function () {
        var estimateId = $(this).attr("data-estimate-id");
        var estimateName = $(this).attr('data-estimate-name');

        deleteEstimate(estimateId, estimateName);
    });

    function deleteEstimate(estimateId, estimateName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                estimateName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _estimateService.delete({
                        id: estimateId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$estimatesTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-estimate', function (e) {
        var estimateId = $(this).attr("data-estimate-id");
        e.preventDefault();
        window.location.href = '/CRM/Estimate?estimateId=' + estimateId;
    });

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
