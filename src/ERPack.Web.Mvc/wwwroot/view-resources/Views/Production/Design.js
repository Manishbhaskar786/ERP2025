const DEFAULT_IMAGE_PATH = "/img/default-image.jpg";
const DEFAULT_Null_Data = "No Data";
(function ($) {
    var _designService = abp.services.app.design,
        _enquiryService = abp.services.app.enquiry,
        l = abp.localization.getSource('ERPack'),
        _$table = $('#EnquiriesTable'),
        _$completedDesignstable = $('#CompletedDesignsTable');

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
        searching: true,
        listAction: {
            ajaxFunction: _enquiryService.getAllDesignReady,
            inputFilter: function () {
                return $('#DesignSearchForm').serializeFormToObject(true);
            }
        },
        pageLength: 10,
        buttons: [
            {
                name: 'refresh',
                text: '<i id="btnRefresh" class="fas fa-redo-alt"></i>',
                action: () => _$enquiriesTable.draw(false)
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
                className: 'control',
                orderable: false,
                searchable: false,
                defaultContent: '<input type="checkbox" class="select-checkbox">'
            },
            {
                targets: 1,
                data: 'enquiryId',
                sortable: false
            },
            {
                targets: 2,
                data: 'designNumber',
                sortable: false
            },
            {
                targets: 3,
                data: 'designName',
                sortable: false
            },
            {
                targets: 4,
                data: 'boxLength',
                sortable: false
            },
            {
                targets: 5,
                data: 'boxWidth',
                sortable: false
            },
            {
                targets: 6,
                data: 'boxHeight',
                sortable: false
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
                data: 'creationTime',
                render: function (data, type, row, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 9,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm btn-outline-primary toggleButton view-jobcard" data-enquiry-id="${row.id}">`,
                        `       <i class="fa fa-eye-slash"></i> `,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm btn-outline-primary print-jobcard" data-enquiry-id="${row.id}" data-enquiry-name="${row.enquiryId}" onclick="PrintJobCard(this)">`,
                        `       <i class="fa fa-print"></i> `,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    $('#btnRefresh').on('click', function () {
        _$enquiriesTable.ajax.reload(); // Reload data from the server
    });

    $('#EnquiriesTable').on('click', '.select-checkbox', function () {
        var allCheckboxes = $('#EnquiriesTable .select-checkbox');
        var checkedCheckboxes = allCheckboxes.filter(':checked').length;

        if (!this.checked) {
            $('#select-all').prop('checked', false);
        } else if (checkedCheckboxes === allCheckboxes.length) {
            $('#select-all').prop('checked', true);
        }


        toggleBulkActions(checkedCheckboxes, '');
    });
    $('#select-all').on('click', function () {
        // Get all checkboxes in the table
        var checkboxes = $('#EnquiriesTable .select-checkbox');
        // Check/uncheck all checkboxes based on the state of the "Select all" checkbox
        checkboxes.prop('checked', this.checked);
        toggleBulkActions(checkboxes.filter(':checked').length, '');

    });
    $(document).on('change', '.select-checkbox input[type="checkbox"]', function () {
        var checkedCount = $('#EnquiriesTable .select-checkbox:checked').length;
        toggleBulkActions(checkedCount, '');
    });
    function toggleBulkActions(selectedCount, id) {
        if (selectedCount >= 1) {
            $('#' + id + 'bulkActions').show();
        } else {
            $('#' + id + 'bulkActions').hide();
        }
    }
    let prevbutton = null;

    $(document).on('click', '.toggleButton', function (e) {       
        e.preventDefault();
        const $button = $(this);
        const enquiryId = $button.attr("data-enquiry-id");
        const $section = $("#toggleSection");

        const selectors = {
            image: '#designImagemodal, #designImage',
            pdf: '#pdfPreviewmodal, #pdfPreview',
            reportImage: '#designImagemodal1, #designImage1',
            reportPdf: '#pdfPreviewmodal1, #pdfPreview1',
            magnify: '#magnify , #magnify1',
            downloadLink: '#downloadLink, #downloadLink1'
        };

        const resetElements = () => {
            $("#divDesign").removeAttr('style');
            $("#designCreateForm").trigger('reset');
            $(selectors.image).attr('src', DEFAULT_IMAGE_PATH);
            $(selectors.reportImage).attr('src', DEFAULT_IMAGE_PATH);
            $(selectors.pdf).attr('src', DEFAULT_IMAGE_PATH);
            $(selectors.reportPdf).attr('src', DEFAULT_IMAGE_PATH);
            $(selectors.downloadLink).attr('class', 'btn btn-outline-secondary');
            $(selectors.magnify).attr('disabled', 'disabled');
            $(selectors.downloadLink1).attr('disabled', 'disabled');
        };
        const handleDesignImage = (designImage) => {
            if (designImage) {
                const isPdf = designImage.includes('.pdf');
                $(isPdf ? selectors.pdf : selectors.image).attr('src', designImage).show();
                $(isPdf ? selectors.image : selectors.pdf).hide();
                $('.icon-buttons a').attr('href', designImage).attr('download', designImage);
            } else {
                $(selectors.image).show();
            }
        };

        const handleReportDoc = (reportDoc) => {
            if (reportDoc) {
                const isPDFReport = reportDoc.includes('.pdf');
                $(isPDFReport ? selectors.reportPdf : selectors.reportImage).attr('src', reportDoc).show();
                $(isPDFReport ? selectors.reportImage : selectors.reportPdf).hide();
                $('#report-icons a').attr('href', reportDoc).attr('download', reportDoc);
            } else {
                $(selectors.reportImage).show();
            }
        };

        if ($section.hasClass('visible') && enquiryId === $('#enquiryId').val()) {
            $section.removeClass('visible').addClass('hidden');
            $button.find('i').removeClass('fa-eye').addClass('fa-eye-slash');
        } else {
            if ($section.hasClass('visible') && enquiryId !== $('#enquiryId').val()) {
                if (prevbutton) {
                    prevbutton.find('i').removeClass('fa-eye').addClass('fa-eye-slash');
                }
                $button.find('i').removeClass('fa-eye-slash').addClass('fa-eye');
                prevbutton = $button;
            } else {
                $section.removeClass('hidden').addClass('visible');
                $button.find('i').removeClass('fa-eye-slash').addClass('fa-eye');
                prevbutton = $button;
            }
            resetElements();

            if (enquiryId) {
                $.ajax({
                    type: "GET",
                    url: `/CRM/GetEnquiryById?id=${enquiryId}`,
                    dataType: "json",
                    contentType: 'application/json; charset=utf-8',
                    success: function (response) {
                        if (response.result.msg === "OK") {
                            const jdata = JSON.parse(response.result.data);
                            const fields = [
                                { id: '#enquiryId', value: jdata.Id },
                                { id: '#enquiryNumber', value: jdata.EnquiryId },
                                { id: '#taDesignNumber', value: jdata.DesignNumber },
                                { id: '#taDesignName', value: jdata.DesignName },
                                { id: '#width', value: jdata.BoxWidth },
                                { id: '#height', value: jdata.BoxHeight },
                                { id: '#length', value: jdata.BoxLength },
                                { id: '#sheetSizeLength', value: jdata.SheetSizeLength },
                                { id: '#sheetSizeWidth', value: jdata.SheetSizeWidth },
                                { id: '#taBoardType', value: jdata.BoardTypeName },
                                { id: '#brailleWidth', value: jdata.BraileWidth },
                                { id: '#brailleLength', value: jdata.BraileLength },
                                { id: '#embossWidth', value: jdata.EmbossWidth },
                                { id: '#embossLength', value: jdata.EmbossLength },
                                { id: '#numberOfUps', value: jdata.NumberOfUps },
                                { id: '#comments', value: jdata.Comments }
                            ];

                            fields.forEach(field => {
                                $(field.id).val(field.value).attr('placeholder', DEFAULT_Null_Data);
                            });

                            handleDesignImage(jdata.DesignImage);
                            handleReportDoc(jdata.ReportDoc);
                        }
                    },
                    error: function () {
                        alert("Error occurred while fetching enquiry details!");
                    }
                });

                GetEnquiryMaterialDetails(enquiryId);
            }
        }
    });

    $(document).on('click', '.edit-design', function (e) {
        e.preventDefault();
        const designId = $(this).attr("data-design-id");

        const $section = $("#toggleSection");
        const selectors = {
            image: '#designImagemodal, #designImage',
            pdf: '#pdfPreviewmodal, #pdfPreview',
            reportImage: '#designImagemodal1, #designImage1',
            reportPdf: '#pdfPreviewmodal1, #pdfPreview1',
            magnify: '#magnify , #magnify1',
            downloadLink: '#downloadLink, #downloadLink1'
        };

        const resetElements = () => {
            $("#divDesign").removeAttr('style');
            $("#designCreateForm").trigger('reset');
            $(selectors.image).attr('src', DEFAULT_IMAGE_PATH);
            $(selectors.reportImage).attr('src', DEFAULT_IMAGE_PATH);
            $(selectors.pdf).attr('src', DEFAULT_IMAGE_PATH);
            $(selectors.reportPdf).attr('src', DEFAULT_IMAGE_PATH);
            $(selectors.downloadLink).attr('class', 'btn btn-outline-secondary');
            $(selectors.magnify).attr('disabled', 'disabled');
        };

        const handleDesignImage = (designImage) => {
            $(selectors.image).hide();
            $(selectors.pdf).hide();
            if (designImage) {
                const isPdf = designImage.includes('.pdf');
                $(isPdf ? selectors.pdf : selectors.image).attr('src', designImage).show()
                    .siblings(isPdf ? selectors.image : selectors.pdf).hide();
                $('.icon-buttons a').attr('href', designImage).attr('download', designImage);
            } else {
                $(selectors.image).show();
            }
        };

        const handleReportDoc = (reportDoc) => {
            $(selectors.reportImage).hide();
            $(selectors.reportPdf).hide();
            if (reportDoc) {
                const isPDFReport = reportDoc.includes('.pdf');
                $(isPDFReport ? selectors.reportPdf : selectors.reportImage).attr('src', reportDoc).show()
                    .siblings(isPDFReport ? selectors.reportImage : selectors.reportPdf).hide();
                $('#report-icons a').attr('href', reportDoc).attr('download', reportDoc);
            } else {
                $(selectors.reportImage).show();
            }
        };

        if ($section.hasClass('visible') && designId == $('#designId').val()) {
            $section.removeClass('visible').addClass('hidden');
        } else {
            $section.removeClass('hidden').addClass('visible');
            resetElements();
        }

        $.ajax({
            type: "GET",
            url: `/Production/GetDesignById?id=${designId}`,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                if (response.result.msg === "OK") {
                    const jdata = JSON.parse(response.result.data);
                    const fieldsWithPlaceholders = [
                        { id: '#id', value: jdata.Id, placeholder: '0' },
                        { id: '#designId', value: jdata.DesignId },
                        { id: '#enquiryId', value: jdata.EnquiryId },
                        { id: '#enquiryNumber', value: jdata.EnquiryNumber },
                        { id: '#taDesignNumber', value: jdata.DesignNumber },
                        { id: '#taDesignName', value: jdata.DesignName },
                        { id: '#width', value: jdata.BoxWidth },
                        { id: '#height', value: jdata.BoxHeight },
                        { id: '#length', value: jdata.BoxLength },
                        { id: '#sheetSizeLength', value: jdata.SheetSizeLength },
                        { id: '#sheetSizeWidth', value: jdata.SheetSizeWidth },
                        { id: '#taBoardType', value: jdata.BoardTypeName },
                        { id: '#brailleWidth', value: jdata.BraileWidth },
                        { id: '#brailleLength', value: jdata.BraileLength },
                        { id: '#embossWidth', value: jdata.EmbossWidth },
                        { id: '#embossLength', value: jdata.EmbossLength },
                        { id: '#numberOfUps', value: jdata.NumberOfUps },
                        { id: '#comments', value: jdata.Comments }
                    ];

                    fieldsWithPlaceholders.forEach(field => {
                        const placeholder = field.id === '#id' ? field.placeholder : DEFAULT_Null_Data;
                        $(field.id).val(field.value).attr('placeholder', placeholder);
                    });

                    handleDesignImage(jdata.DesignImage);
                    handleReportDoc(jdata.ReportDoc);
                }
            },
            error: function () {
                alert("Error occurred!!");
            }
        });

        GetDesignMaterialDetails(designId);
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
    $("#magnify").on("click", function () {
        app.ZoomoutImage('filePreview', 'filePreview');
    });

    $("#magnify1").on("click", function () {
        app.ZoomoutImage('filePreview1', 'filePreview');
    });

    const updateImageState = () => {
        // Check the state for the first group of images
        const group1Image = $('#designImage');
        const group1ImgHasNonDefault = group1Image.filter(function () {
            const src = $(this).attr('src');
            return src && src !== DEFAULT_IMAGE_PATH;
        }).length > 0;

        const group1Pdf = $('#pdfPreview');
        const group1PdfHasNonDefault = group1Pdf.filter(function () {
            const src = $(this).attr('src');
            return src && src !== DEFAULT_IMAGE_PATH;
        }).length > 0;

        // Check the state for the second group of images
        const group2Image = $('#designImage1');
        const group2ImgHasNonDefault = group2Image.filter(function () {
            const src = $(this).attr('src');
            return src && src !== DEFAULT_IMAGE_PATH;
        }).length > 0;

        const group2Pdf = $('#pdfPreview1');
        const group2PdfHasNonDefault = group2Pdf.filter(function () {
            const src = $(this).attr('src');
            return src && src !== DEFAULT_IMAGE_PATH;
        }).length > 0;

        // Update the state for the first group of images
        //if (group1ImgHasNonDefault || group1PdfHasNonDefault) {
        //    $('#downloadLink').attr('class', 'btn btn-outline-primary');
        //    $('#magnify').removeAttr('disabled');
        //    $('#downloadLink1').removeAttr('disabled');
        //} else {
        //    $('#downloadLink').attr('class', 'btn btn-outline-secondary');
        //    $('#magnify').attr('disabled', 'disabled');
        //    $('#downloadLink1').attr('disabled', 'disabled');
        //}

        if ((group1ImgHasNonDefault || group1PdfHasNonDefault) && (group2ImgHasNonDefault || group2PdfHasNonDefault)) {
            $('#downloadLink1').removeClass('disabled-link').addClass('btn btn-outline-primary');
            $('#magnify1').removeAttr('disabled');
            $('#downloadLink1').removeAttr('disabled');
        } else {
            $('#downloadLink1').removeClass('btn btn-outline-primary').addClass('btn btn-outline-secondary');
            $('#magnify1').attr('disabled', 'disabled');
            $('#downloadLink1').attr('disabled', 'disabled');
        }

        // Update the state for the second group of images
        if (group2ImgHasNonDefault || group2PdfHasNonDefault) {
            $('#downloadLink1').attr('class', 'btn btn-outline-primary');
            $('#magnify1').removeAttr('disabled');
            $('#downloadLink1').removeAttr('disabled');
        } else {
            $('#downloadLink1').attr('class', 'btn btn-outline-secondary');
            $('#magnify1').attr('disabled', 'disabled');
            $('#downloadLink1').attr('disabled', 'disabled');
        }
    };

    $('#designImage, #pdfPreview, #designImage1, #pdfPreview1').on('load', updateImageState);

    $('#designImage, #pdfPreview, #designImage1, #pdfPreview1').each(function () {
        if (this.complete) {
            $(this).trigger('load');
        }
    });
    // Image scaling
    const modal = $('#myModal');
    const modalContent = $('.modal-content');

    $('#zoomSlider').on('input', function () {
        modalContent.css('transform', `scale(${this.value})`);

    });
    $('.close').on('click', function (event) {
        event.stopPropagation();
        modal.hide();
        $('#zoomSlider').val(1);
        // Reset the transform scale to 1
        modalContent.css('transform', 'scale(1)');
    });
    modalContent.on('click', function (event) {
        event.stopPropagation();
    });


    var _$completedDesignsTable = _$completedDesignstable.DataTable({
        paging: true,
        searching: true,
        listAction: {
            ajaxFunction: _designService.getCompletedDesigns,
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i id="btnCDRefresh" class="fas fa-redo-alt"></i>',
                action: () => _$completedDesignsTable.draw(false)
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
                className: 'control',
                orderable: false,
                searchable: false,
                defaultContent: '<input type="checkbox" class="select-checkbox">'
            },
            {
                targets: 1,
                data: 'enquiryNumber',
                sortable: false
            },
            {
                targets: 2,
                data: 'designId',
                sortable: false
            },
            {
                targets: 3,
                data: 'completionDatetime',
                render: function (data, type, row, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 4,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-design" data-design-id="${row.id}">`,
                        `       <i class="fas fa-pencil-alt"></i>`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });
    $('#btnCDRefresh').on('click', function () {
        _$completedDesignsTable.ajax.reload(); // Reload data from the server
    });

    $('#CompletedDesignsTable').on('click', '.select-checkbox', function () {
        var allCheckboxes = $('#CompletedDesignsTable .select-checkbox');
        var checkedCheckboxes = allCheckboxes.filter(':checked').length;

        if (!this.checked) {
            $('#CompletedDesignsTable #select-all-cd').prop('checked', false);
        } else if (checkedCheckboxes === allCheckboxes.length) {
            $('#CompletedDesignsTable #select-all-cd').prop('checked', true);
        }


        toggleBulkActions(checkedCheckboxes, 'CompletedDesign');
    });
    $('#CompletedDesignsTable #select-all-cd').on('click', function () {
        // Get all checkboxes in the table
        var checkboxes = $('#CompletedDesignsTable .select-checkbox');
        // Check/uncheck all checkboxes based on the state of the "Select all" checkbox
        checkboxes.prop('checked', this.checked);
        toggleBulkActions(checkboxes.filter(':checked').length, 'CompletedDesign');

    });

    $(document).on('change', '#CompletedDesignsTable .select-checkbox input[type="checkbox"]', function () {
        var checkedCount = $('#CompletedDesignsTable .select-checkbox:checked').length;
        toggleBulkActions(checkedCount, 'CompletedDesign');
    });



    $(document).on("click", "#addRow", function () {
        CloneRow();
    });
    $("#MaterialRow").on("click", ".fa-trash", function (event) {


        let row = $(this).closest("tr");
        let designId = $('#id').val();
        if (designId) {
            let designMaterialId = row.find("input[id^='hdnDesignMaterialId']").val();
            let designMaterialName = row.find("select[id^='materialName']").find(':selected').text();

            if (designMaterialId) {
                abp.message.confirm(
                    abp.utils.formatString(
                        ('Are You Sure Want To Delete Material ' + designMaterialName)),
                    null,
                    (isConfirmed) => {
                        if (isConfirmed) {
                            $.ajax({
                                type: "POST",
                                url: "/Production/DeleteDesignMaterial",
                                data: { designMaterialId: designMaterialId },
                                success: function (response) {
                                    if (response.success) {
                                        row.remove();
                                        abp.notify.info("Enquiry Material deleted Successfully");
                                    } else {
                                        abp.notify.error("Error Deleting Enquiry Material");
                                    }
                                },
                                error: function (xhr, status, error) {
                                    abp.notify.error('Error calling delete function: ' + error);
                                }
                            });
                        }
                    }
                );
            } else { row.remove(); }
        }
        else {
            row.remove();
        }
    });


    $("#pop").on("click", function () {
        $('#imagepreview').attr('src', $('#designImage').attr('src'));
        $('#imagemodal').modal('show');
    });
})(jQuery);

let cloneCount = 1;
function CloneRow() {
    var row = $("#MaterialRow").clone(true)
        .attr('id', 'MaterialRow' + cloneCount, 'class', 'row')        
        .insertAfter('[id^=MaterialRow]:last')
        .find('input,select').val("");


    $('#MaterialRow' + cloneCount).find('input,select').each(function () {
        $(this).attr('id', $(this).attr('id').replace(/.$/, cloneCount));
        $(this).attr('name', $(this).attr('name') + cloneCount);
        //$(this).attr("onkeypress", "return validateLimit()")
    });
    $('#MaterialRow' + cloneCount).find('.fa-trash').removeAttr('hidden');
    cloneCount++
}

function fnFillMaterialInfo(obj) {

    let selectedId = obj.id;
    let lastCharOfId = selectedId.charAt(selectedId.length - 1);

    $.ajax({
        type: "GET",
        url: "/Materials/GetMaterialById?id=" + obj.value,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let data = JSON.parse(response.result.data)
                if (!isNaN(lastCharOfId)) {
                    $("#itemCode" + lastCharOfId).val(data.ItemCode);
                    $("#hdnMaterialId" + lastCharOfId).val(data.Id);
                }
                else {
                    $("#itemCode").val(data.ItemCode);
                    $("#hdnMaterialId").val(data.Id);
                }
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function GetDesignMaterialDetails(id) {

    $.ajax({
        type: "GET",
        url: "/Production/GetDesignMaterials?id=" + id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let data = JSON.parse(response.result.data)

                cloneCount = 1;
                var rowcount = $("input[id^='hdnDesignMaterialId']");
                if (rowcount.length > 1) {
                    for (let j = 1; j < rowcount.length; j++) {
                        $(`#MaterialRow${j}`).remove();
                    }
                }


                $.each(data, function (index, value) {
                    if (index != 0) {

                        CloneRow();
                    }
                    $("#hdnDesignMaterialId" + index).val(value.Id);
                    $("#itemCode" + index).val(value.ItemCode);
                    $("#hdnMaterialId" + index).val(value.MaterialId);
                    $("#materialName" + index).val(value.MaterialId);
                    $("#quantity" + index).val(value.Quantity);


                });
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}

function GetEnquiryMaterialDetails(enquiryId) {

    $.ajax({
        type: "GET",
        url: `/CRM/GetEnquiryMaterials?enquiryId=${enquiryId}`,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.result.msg == "OK") {
                let data = JSON.parse(response.result.data)

                cloneCount = 1;
                var rowcount = $("input[id^='hdnDesignMaterialId']");
                if (rowcount.length > 1) {
                    for (let j = 1; j < rowcount.length; j++) {
                        $(`#MaterialRow${j}`).remove();
                    }
                }
                $.each(data, function (index, value) {
                    if (index != 0) {

                        CloneRow();
                    }
                    $("#hdnDesignMaterialId" + index).val(value.Id);
                    $("#itemCode" + index).val(value.ItemCode);
                    $("#hdnMaterialId" + index).val(value.MaterialId);
                    $("#materialName" + index).val(value.MaterialId);
                    $("#quantity" + index).val();
                });
            }
        },
        error: function () {
            alert("Error occured!!")
        }
    });
}
function SaveDesign() {
    var arrayData = $('#designCreateForm').serializeArray();

    const rows = $('#tblMaterials tbody tr');
    const validMaterials = $(`[id^="hdnMaterialId"]`);
    let flag = !Array.from(validMaterials).some(el => el.value === "0");
    if (flag) {
        const designId = $('#id').val();

        rows.each(function (index) {
            const row = $(this);
            const id = row.find('[id^="hdnMaterialId"]').attr('id');
            const match = id.match(/\d+$/);
            if (match) {
                const materialindex = parseInt(match[0], 10);

                const hdnDesignMaterial = $(`#hdnDesignMaterialId${materialindex}`).val();
                if (designId != 0 && hdnDesignMaterial) {
                    arrayData.push({ name: "DesignMaterials[" + index + "].Id", value: $($("input[id^='hdnDesignMaterialId']")[index]).val() });
                }
                arrayData.push({ name: "DesignMaterials[" + index + "].MaterialId", value: $($("input[id^='hdnMaterialId']")[index]).val() });
                arrayData.push({ name: "DesignMaterials[" + index + "].Quantity", value: $($("input[id^='quantity']")[index]).val() });
            }
        });
    }


    var formData = new FormData(); // Use FormData to handle both files and other data


    $.each(arrayData, function (index, field) {
        formData.append(field.name, field.value);
    });


    // Append the file input manually to formData
    var fileInput = $('#fileUpload')[0];
    if (fileInput.files.length > 0) {
        formData.append('DesignImageDoc', fileInput.files[0]);
    }
    abp.ui.setBusy();
    $.ajax({
        type: "POST",
        data: formData,
        url: '/Production/SaveDesign',
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.result.msg == "OK") {
                //SaveReport(data.result.designId);
                abp.notify.success('SavedSuccessfully');
                $('#designCreateForm')[0].reset();
                location.reload();
            }
            else {
                abp.notify.error('Error In Saving');
            }
        },
        failure: function (response) {

        },
        error: function (response) {
            alert("error");
        }
    });
    abp.ui.clearBusy();
}
function getSelectedRows(table) {
    var selectedRows = $("#" + table + "Table tbody tr").has('input[type="checkbox"]:checked');

    // Create a new table to store the selected rows
    var tabledata = $("<table></table>").append($("#" + table + "Table thead").clone());

    // Append each selected row to the new table
    selectedRows.each(function () {
        tabledata.append($(this).clone());
    });
    return tabledata;
}
function fnCreatePdf(event, tablename) {
    event.preventDefault();

    var tabledata = getSelectedRows(tablename);
    let obj = {};
    obj.Name = tablename + "List";
    obj.Html = tabledata.html();
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

function fnCreateCSV(event, tablename) {
    event.preventDefault();

    var tabledata = getSelectedRows(tablename);
    let obj = {};
    obj.Name = tablename + "List";

    obj.Html = tabledata.html();
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

function PrintJobCard(obj) {
    var designId = $(obj).attr("data-enquiry-id");
    if (designId != null) {
        var url = '/CRM/PdfDesignJob?designId=' + designId;
        window.open(url, "_blank");
    }
    else {
        abp.notify.info("Please enter a valid EstimateId.!");
    }
}

$(function () {
    $("#downloadLink").click(function (e) {
        e.preventDefault();
        app.DownloadImage('icon-buttons');
    });
    $("#downloadLink1").click(function (e) {
        const selectedFileName = document.getElementById('fileUpload');
        const file = selectedFileName.files[0]; // Get the selected file

        if (file) {
            e.preventDefault();
            app.DownloadImage("report-icons");
        }
        else {
            //abp.notify.error("Errors.");
        }
    });
});
$("#fileUpload").change(function () {
    const file = this.files[0]; // Get the selected file

    if (file) {
        const maxSize = 10 * 1024 * 1024; // 10 MB in bytes
        if (file.size > maxSize) {
            abp.notify.error("File size must be less than 10 MB.");
            $(this).val(''); // Clear the input            
        }
    }
    app.UploadImage('filePreview1', 'report-icons'); 
});
function UploadReport() {
    document.getElementById('fileUploadReport').click();
}
function validateLimit(obj, maxlength)
{
    const inputValue = obj.value;   
    if (inputValue.length > (maxlength - 1)) {
        //abp.notify.error("Input exceeds 20 digits limit.");
        return false;
    }
}