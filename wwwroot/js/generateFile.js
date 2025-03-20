document.addEventListener("DOMContentLoaded", function () {
    const errorModal = document.getElementById("errorModal");
    const errorTitle = document.getElementById("errorTitle");
    const errorMessage = document.getElementById("errorMessage");
    const closeErrorModalBtn = document.getElementById("closeErrorModal");
    const generateFileBtn = document.getElementById("generateFileBtn");
    const fileForm = document.getElementById("fileForm");
    const selectAllBtn = document.getElementById("selectAllBtn");
    let allSelected = false;

    function showErrorModal(title, message) {
        errorTitle.textContent = title;
        errorMessage.textContent = message;
        errorModal.style.display = "flex";
    }

    function closeErrorModal() {
        errorModal.style.display = "none";
    }

    closeErrorModalBtn.addEventListener("click", closeErrorModal);

    generateFileBtn.addEventListener("click", function (event) {
        event.preventDefault();

        const selectedExpenses = document.querySelectorAll('input[name="selectedExpenses"]:checked');
        const fileType = document.getElementById("fileType").value;

        if (selectedExpenses.length === 0) {
            showErrorModal("No expenses selected", "Please select at least one expense.");
            return;
        }

        if (!fileType) {
            showErrorModal("File type not selected", "Please select a file type before generating.");
            return;
        }

        fileForm.submit();
    });

    selectAllBtn.addEventListener("click", function () {
        const checkboxes = document.querySelectorAll('input[name="selectedExpenses"]');
        allSelected = !allSelected;

        checkboxes.forEach(checkbox => {
            checkbox.checked = allSelected;
        });

        updateSelectAllButton();
    });

    function updateSelectAllButton() {
        const checkboxes = document.querySelectorAll('input[name="selectedExpenses"]');
        const checkedCount = document.querySelectorAll('input[name="selectedExpenses"]:checked').length;

        allSelected = checkedCount === checkboxes.length;
        selectAllBtn.textContent = allSelected ? "Deselect all" : "Select all";
    }

    document.querySelectorAll('input[name="selectedExpenses"]').forEach(checkbox => {
        checkbox.addEventListener("change", updateSelectAllButton);
    });
});
