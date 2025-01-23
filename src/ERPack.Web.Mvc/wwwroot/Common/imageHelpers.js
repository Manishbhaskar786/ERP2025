var app = app || {};
(function () {
    app.UploadImage = function (parentDivId, iconDivId) {
        var parentDiv = $(`#${parentDivId}`);
        var iconDiv = $(`#${iconDivId}`);
        var [file] = parentDiv.find('[data-role="file-upload"]').prop('files');

        if (file) {
            if (file.type.startsWith('image/')) {
                const imageUrl = URL.createObjectURL(file);
                parentDiv.find('[data-role="other-image"]').attr('src', imageUrl).show();
                parentDiv.find('[data-role="pdf-image"]').hide();
                var dl = iconDiv.find('[data-role="download-link"]');
                dl.attr({
                    'href': imageUrl,
                    'download': file.name,
                    'class': 'btn btn-outline-primary'
                });
                iconDiv.find('[data-role="magnify"]').removeAttr('disabled');

                abp.notify.info('Image Uploaded Successfully');
            } else if (file.type === 'application/pdf') {
                const fileUrl = URL.createObjectURL(file);
                parentDiv.find('[data-role="other-image"]').hide();
                parentDiv.find('[data-role="pdf-image"]').attr('src', fileUrl).show();
                iconDiv.find('[data-role="download-link"]').attr({
                    'href': fileUrl,
                    'download': file.name,
                    'class': 'btn btn-outline-primary'
                });
                iconDiv.find('[data-role="magnify"]').removeAttr('disabled');

                abp.notify.info('PDF Uploaded Successfully');
            } else {
                abp.notify.info('Unsupported file type');
            }
        }
    };
    app.DownloadImage = function (parentDivId) {
        var parentDiv = $(`#${parentDivId}`);
        var downloadLink = parentDiv.find('[data-role="download-link"]');
        var fileUrl = downloadLink.attr('href');
        var fileName = downloadLink.attr('download');

        var xhr = new XMLHttpRequest();
        xhr.open('GET', fileUrl, true);
        xhr.responseType = 'blob';

        xhr.onload = function () {
            var blob = new Blob([xhr.response], { type: xhr.getResponseHeader('Content-Type') });
            var tempLink = document.createElement('a');
            tempLink.href = window.URL.createObjectURL(blob);
            tempLink.download = fileName;
            tempLink.click();
        };
        xhr.send();
    };
    app.ZoomoutImage = function (parentDivId, modalDivId) {
        var modalParent = $(`#${modalDivId}`);
        var parentDiv = $(`#${parentDivId}`);
        var DEFAULT_IMAGE_PATH = "/img/default-image.jpg";

        const modal = modalParent.find('[data-role="modal"]').get(0);
        const modalImg = modalParent.find('[data-role="modal-image"]').get(0);
        const pdfModalImg = modalParent.find('[data-role="modal-pdf"]').get(0);
        const img = parentDiv.find('[data-role="other-image"]').get(0);
        const pdfImg = parentDiv.find('[data-role="pdf-image"]').get(0);
        var fileInput = null;

        if (parentDiv.find('[data-role="file-upload"]').length > 0) {
            fileInput = parentDiv.find('[data-role="file-upload"]').prop('files')[0];
        }

        const isDefaultImage = (src) => src.includes(DEFAULT_IMAGE_PATH);
        const isPdf = (src) => src.includes(".pdf");

        const showImageInModal = (imgSrc) => {
            modalImg.src = imgSrc;
            modalImg.style.display = "block";
            pdfModalImg.style.display = "none";
            modal.style.display = "flex";
        };

        const showPdfInModal = (pdfSrc) => {
            pdfModalImg.src = pdfSrc;
            pdfModalImg.style.display = "block";
            modalImg.style.display = "none";
            modal.style.display = "flex";
        };

        if (fileInput) {
            if (fileInput.type.startsWith('image/')) {
                if (!isDefaultImage(img.src)) {
                    showImageInModal(img.src);
                }
            } else if (!isDefaultImage(pdfImg.src)) {
                showPdfInModal(pdfImg.src);
            }
        } else {
            if (!isDefaultImage(img.src) && !isPdf(img.src)) {
                showImageInModal(img.src);
            } else if (!isDefaultImage(pdfImg.src)) {
                showPdfInModal(pdfImg.src);
            }
        }
    };
})();