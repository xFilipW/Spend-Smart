document.addEventListener("DOMContentLoaded", function () {
    var statusSelect = document.getElementById("statusSelect");
    var prioritySelect = document.getElementById("prioritySelect");
    var categorySelect = document.getElementById("categorySelect");
    var dueDateInput = document.getElementById("paymentDueDateInput");
    var paymentDateInput = document.getElementById("paymentDateInput");
    var dueDateError = document.querySelector("[data-valmsg-for='PaymentDueDate']");
    var paymentDateError = document.querySelector("[data-valmsg-for='PaymentDate']");

    function removePlaceholderOption(selectElement) {
        selectElement.addEventListener("focus", function () {
            let firstOption = selectElement.querySelector("option[value='']");
            if (firstOption) {
                firstOption.style.display = "none";
            }
        });
    }

    removePlaceholderOption(statusSelect);
    removePlaceholderOption(prioritySelect);
    removePlaceholderOption(categorySelect);

    function updateFields() {
        var status = statusSelect.value;
        var today = new Date().toISOString().split("T")[0];

        if (status === "Completed") {
            dueDateInput.value = "";
            dueDateInput.setAttribute("disabled", "disabled");
            dueDateError.style.display = "none";

            paymentDateInput.removeAttribute("disabled");
            paymentDateInput.setAttribute("max", today);
        } else if (status === "Planned") {
            paymentDateInput.value = "";
            paymentDateInput.setAttribute("disabled", "disabled");
            paymentDateError.style.display = "none";

            dueDateInput.removeAttribute("disabled");
            dueDateInput.setAttribute("min", today);
        } else {
            dueDateInput.removeAttribute("disabled");
            paymentDateInput.removeAttribute("disabled");
        }
    }

    function validateForm(event) {
        var status = statusSelect.value;
        var dueDate = dueDateInput.value;
        var paymentDate = paymentDateInput.value;
        var today = new Date().toISOString().split("T")[0];
        var isValid = true;

        if (status === "Planned" && dueDate && dueDate < today) {
            dueDateError.textContent = "The due date cannot be in the past for planned expenses.";
            dueDateError.style.display = "block";
            isValid = false;
        } else {
            dueDateError.style.display = "none";
        }

        if (status === "Completed" && paymentDate && paymentDate > today) {
            paymentDateError.textContent = "The payment date cannot be in the future for completed expenses.";
            paymentDateError.style.display = "block";
            isValid = false;
        } else {
            paymentDateError.style.display = "none";
        }

        if (!isValid) event.preventDefault();
    }

    statusSelect.addEventListener("change", updateFields);
    document.querySelector("form").addEventListener("submit", validateForm);
    updateFields();

    var fileInput = document.getElementById("attachmentInput");
    var selectFileBtn = document.getElementById("selectFileBtn");
    var fileNameDisplay = document.getElementById("fileNameDisplay");
    var removeFileBtn = document.getElementById("removeFileBtn");
    var removeAttachment = document.getElementById("removeAttachment");

    if (selectFileBtn && fileInput) {
        selectFileBtn.addEventListener("click", function () {
            fileInput.click();
        });

        fileInput.addEventListener("change", function () {
            if (fileInput.files.length > 0) {
                fileNameDisplay.value = fileInput.files[0].name;
                removeFileBtn.style.display = "block";
                removeAttachment.value = "false";
            } else {
                fileNameDisplay.value = "File not selected";
                removeFileBtn.style.display = "none";
                removeAttachment.value = "true";
            }
        });
    }

    function removeFile() {
        fileInput.value = "";
        fileNameDisplay.value = "File not selected";
        removeFileBtn.style.display = "none";
        removeAttachment.value = "true";
    }

    if (removeFileBtn) {
        removeFileBtn.addEventListener("click", removeFile);
    }
});
