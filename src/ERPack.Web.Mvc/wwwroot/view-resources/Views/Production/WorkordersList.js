(function ($) {
    var _workorderService = abp.services.app.workorder,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#WorkordersTable');

    var _$workordersTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _workorderService.getAll,
            inputFilter: function () {
                return $('#WorkordersSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => { _$workordersTable.ajax.reload(null, false); }
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        order: [[2, 'asc']],
        columnDefs: [
            {
                targets: 0,
                className: 'dt-control',
                defaultContent: '',
                sortable: false
            },
            {
                targets: 1,
                data: 'workorderId'
            },
            {
                targets: 2,
                data: 'enquiryCode'
            },
            {
                targets: 3,
                data: 'taskIssueDate'
            },
            {
                targets: 4,
                data: 'taskIssueCompleteDate'
            },
            {
                targets: 5,
                data: 'taskIssueActualCompleteDate'
            },
            {
                targets: 6,
                data: 'status'
            },
            {
                targets: 7,
                data: 'isHighPriority',
                render: function (data) {
                    if (data === true) {
                        return '<p class="blink_text">High Priority</p>';
                    }
                    else {
                        return '';
                    }
                }
            },
            {
                targets: 8,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-workorder" data-workorder-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-workorder" data-workorder-id="${row.id}" data-workorder-name="${row.workorderId}">`,
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
            format(row);
        }
    });

    // Formatting function for row details - modify as you need
    function format(row) {
        let workorderId = row.data().id;
        let html = `<table id="EstimateTasksTable" class="table table-striped table-bordered nowrap">
                                    <thead>
                                        <tr>
                                            <th>Sub Task Id</th>
                                            <th>Allocation Time</th>
                                            <th>Expected Completion Time</th>
                                            <th>Completion Time</th>
                                            <th>Department</th>
                                            <th>User</th>
                                            <th>Status</th>
                                        </tr>
                                    </thead>
                <tbody id="tblVal">`;
        let workorderTasks;
        if (workorderId) {
            $.ajax({
                url: `/Production/GetWorkorderTasks?workorderId=${workorderId}`,
                method: "GET",
                dataType: "json",
                success: function (response) {
                    workorderTasks = response.result.data;
                    $(workorderTasks).each(function () {
                        html += "<tr>";
                        html += "<td>" + (this.workorderSubTaskCode !== null && this.workorderSubTaskCode !== undefined ?  this.workorderSubTaskCode : "") + "</td>";
                        html += "<td>" + (this.taskIssueDate !== null && this.taskIssueDate !== undefined ? this.taskIssueDate : "") + "</td>";
                        html += "<td>" + (this.taskIssueCompleteDate !== null && this.taskIssueCompleteDate !== undefined ? this.taskIssueCompleteDate : "") + "</td>";
                        html += "<td>" + (this.taskIssueActualCompleteDate !== null && this.taskIssueActualCompleteDate !== undefined ? this.taskIssueActualCompleteDate : "") + "</td>";
                        html += "<td>" + (this.departmentName !== null && this.departmentName !== undefined ? this.departmentName : "") + "</td>";
                        html += "<td>" + (this.userName !== null && this.userName !== undefined ? this.userName : "") + "</td>";
                        html += "<td>" + (this.status !== null && this.status !== undefined ? this.status : "") + "</td>";
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
                        id: workorderId
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
        window.location.href = '/Production/AddWorkorder?workorderId=' + workorderId;
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
