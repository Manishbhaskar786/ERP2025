(function ($) {

    var _enquiryService = abp.services.app.enquiry,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#EnquiryTable');
    function formatDate(date) {
        var d = new Date(date);
        var day = ("0" + d.getDate()).slice(-2);
        var month = d.toLocaleString('default', { month: 'short' });
        var year = d.getFullYear();
        var hours = ("0" + d.getHours()).slice(-2);
        var minutes = ("0" + d.getMinutes()).slice(-2);

        return `${day}-${month}-${year} ${hours}:${minutes}`;
    }

    var _$enquiriesTable = _$table.DataTable({
        paging: true,
        ordering: true,
        searching: true,
        listAction: {
            ajaxFunction: _enquiryService.getAll,
            inputFilter: function () {
                return $('#EnquirySearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => {
                    _$enquiriesTable.ajax.reload(null, false); // reload the table without resetting pagination
                }
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
                data: 'enquiryId',
                sortable: false
            },
            {
                targets: 2,
                data: 'creationTime',
                type: 'datetime',
                render: function (data, type, row, meta) {
                    return `<span data-date="${data}">${formatDate(data)}</span>`;
                }
            },
            {
                targets: 3,
                data: 'customerName'
            },
            {
                targets: 4,
                data: 'comments',
                render: function (data, type, row, meta) {
                    if (data && data.length > 20) {
                        return `<span data-toggle="tooltip" class="d-inline-block" title="${data}">${data.substring(0, 50)}...</span>`;
                    }
                    else { return data; }
                }
            },
            {
                targets: 5,
                data: 'status',
                render: (data, type, row, meta) => {
                    if (data !== "Sent For Approval") {
                        return data;
                    }
                    else {
                        return `<select id="approveEstimate-${row.id}" data-enquiry-id="${row.id}" class="form-control approve-estimate" aria-label="">
                                    <option value = "NULL" Selected > Approval Awaited</option>
                                    <option value="1">Approve</option>
                                    <option value="0">Reject</option>
                                </select>`;
                    }
                }
            },
            {
                targets: 6,
                data: 'statusDatetime',
                type: 'datetime',
                render: function (data, type, row, meta) {
                    return `<span data-date="${data}">${formatDate(data)}</span>`;
                }
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
                        `   <button type="button" class="btn btn-sm bg-secondary edit-enquiry" data-enquiry-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i> `,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-enquiry" data-enquiry-id="${row.id}" data-enquiry-name="${row.enquiryId}">`,
                        `       <i class="fas fa-trash"></i> `,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    _$enquiriesTable.on('click', 'td.dt-control', function (e) {
        let tr = e.target.closest('tr');
        let row = _$enquiriesTable.row(tr);
        if (row.child.isShown()) {
            row.child.hide();
        }
        else {
            format(row);
        }
    });

    function format(row) {
        let enquiryId = row.data().id;
        let html = `<table id="EnquiryDetailsTable" class="table table-striped table-bordered nowrap">
                    <thead>
                        <tr>
                            <th>Design Name</th>
                            <th>Design Number</th>
                            <th>Box Length</th>
                            <th>Box Width</th>
                            <th>Box Height</th>
                            <th>Board TypeName</th>
                            <th>Tool TypeId</th>
                        </tr>
                    </thead>
                <tbody id="tblVal">`;
        if (enquiryId) {
            $.ajax({
                url: "/CRM/GetEnquiryById?id=" + enquiryId,
                method: "GET",
                dataType: "json",
                success: function (response) {
                    var enquiryDetails = JSON.parse(response.result.data);
                    html += "<tr>";
                    html += "<td>" + enquiryDetails.DesignName + "</td>";
                    html += "<td>" + enquiryDetails.DesignNumber + "</td>";
                    html += "<td>" + enquiryDetails.BoxLength + "</td>";
                    html += "<td>" + enquiryDetails.BoxWidth + "</td>";
                    html += "<td>" + enquiryDetails.BoxHeight + "</td>";
                    html += "<td>" + enquiryDetails.BoardTypeName + "</td>";
                    html += "<td>" + enquiryDetails.ToolTypeId + "</td>";
                    html += "</tr>";
                    html += "</tbody>";
                    html += "</table>";

                    row.child(html).show();
                },
                error: function (xhr, status, error) {
                    console.error("Error:", status, error);
                }
            });
        }

    }
    $(document).on('click', '.delete-enquiry', function () {
        var enquiryId = $(this).attr("data-enquiry-id");
        var enquiryName = $(this).attr('data-enquiry-name');

        deleteEnquiry(enquiryId, enquiryName);
    });

    function deleteEnquiry(enquiryId, enquiryName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                enquiryName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _enquiryService.delete({
                        id: enquiryId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$enquiriesTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-enquiry', function (e) {
        var enquiryId = $(this).attr("data-enquiry-id");
        e.preventDefault();
        window.location.href = '/CRM/NewEnquiry?enquiryId=' + enquiryId;
    });

    $('.btn-search').on('click', (e) => {
        _$enquiriesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$enquiriesTable.ajax.reload();
            return false;
        }
    });

    $(document).on('change', '.approve-estimate', function () {
        var IsApproved;
        var enquiryId = $(this).attr("data-enquiry-id");
        if ($(this).val() == 1) {
            IsApproved = true;
        }
        else if ($(this).val() == 0) {
            IsApproved = false;
        }
        else {
            IsApproved = null
        }

        var $row = $(this).closest('tr');

        let rowindex = $row.index();
        if ($(this).val() == 1) {
            _$enquiriesTable.cell({ row: rowindex, column: 4 }).data(IsApproved);
            _$enquiriesTable.cell({ row: rowindex, column: 4 }).data("Approved");
        }

        $.ajax({
            type: "GET",
            url: `/CRM/ApproveEstimate?enquiryId=${enquiryId}&isApproved=${IsApproved}`,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                if (response.result.msg == "OK") {
                    abp.notify.info('Status Updated!');
                    location.reload();
                }
            },
            error: function () {
                alert("Error occured!!")
            }
        });
    });
})(jQuery);
