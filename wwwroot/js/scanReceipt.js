document.addEventListener("DOMContentLoaded", function () {
    var fileInput = document.getElementById("attachmentInput");
    var selectFileBtn = document.getElementById("fileBtn");
    var fileNameDisplay = document.getElementById("fileNameDisplay");
    var removeFileBtn = document.getElementById("removeFileBtn");

    removeFileBtn.style.display = "none";

    if (selectFileBtn && fileInput) {
        selectFileBtn.addEventListener("click", function () {
            fileInput.click();
        });

        fileInput.addEventListener("change", function () {
            if (fileInput.files.length > 0) {
                fileNameDisplay.value = fileInput.files[0].name;
                removeFileBtn.style.display = "block";
            } else {
                fileNameDisplay.value = "File not selected";
                removeFileBtn.style.display = "none";
            }
        });
    }

    function removeFile() {
        fileInput.value = "";
        fileNameDisplay.value = "File not selected";
        removeFileBtn.style.display = "none"; 
    }

    if (removeFileBtn) {
        removeFileBtn.addEventListener("click", removeFile);
    }

    fileNameDisplay.addEventListener("focus", function () {
        setTimeout(function () {
            if (fileInput.files.length > 0) {
                removeFileBtn.style.display = "block";
            }
        }, 0);
    });
});
