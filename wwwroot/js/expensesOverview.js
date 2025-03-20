function sortExpenses() {
    let table = document.getElementById("expensesTable");
    let rows = Array.from(table.getElementsByTagName("tr")).slice(1);
    let sortOrder = document.getElementById("sortOrder").value;

    rows.sort((a, b) => {
        if (sortOrder === "id-asc" || sortOrder === "id-desc") {
            let idA = parseInt(a.cells[0].textContent.trim());
            let idB = parseInt(b.cells[0].textContent.trim());
            return sortOrder === "id-asc" ? idA - idB : idB - idA;
        }
        else if (sortOrder === "lowest" || sortOrder === "highest") {
            let valA = parseFloat(a.cells[1].getAttribute("data-value"));
            let valB = parseFloat(b.cells[1].getAttribute("data-value"));
            return sortOrder === "lowest" ? valA - valB : valB - valA;
        }
        else if (sortOrder === "priority-high-low" || sortOrder === "priority-low-high") {
            let priorityOrder = { "High": 3, "Medium": 2, "Low": 1 };
            let priorityA = priorityOrder[a.cells[7].textContent.trim()] || 0;
            let priorityB = priorityOrder[b.cells[7].textContent.trim()] || 0;
            return sortOrder === "priority-high-low" ? priorityB - priorityA : priorityA - priorityB;
        }
        else if (["planned", "completed"].includes(sortOrder)) {
            let statusOrder = {
                "Planned": sortOrder === "planned" ? 1 : 2,
                "Completed": sortOrder === "completed" ? 1 : 2
            };
            let statusA = statusOrder[a.cells[4].textContent.trim()] || 3;
            let statusB = statusOrder[b.cells[4].textContent.trim()] || 3;
            return statusA - statusB;
        }
    });

    let tbody = table.getElementsByTagName("tbody")[0];
    tbody.innerHTML = "";
    rows.forEach(row => tbody.appendChild(row));
}

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("sortOrder").value = "id-asc";
    sortExpenses();
});

function showDeleteModal(expenseId) {
    document.getElementById("expenseIdToDelete").value = expenseId;
    document.getElementById("deleteModal").style.display = "flex";
}

function closeDeleteModal() {
    document.getElementById("deleteModal").style.display = "none";
}

function deleteExpense() {
    let expenseId = document.getElementById("expenseIdToDelete").value;

    fetch(`/Home/DeleteExpense/${expenseId}`, { method: "POST" })
        .then(response => {
            if (response.ok) {
                location.reload();
            } else {
                alert("Error deleting expense.");
            }
        })
        .catch(error => console.error("Error:", error));

    closeDeleteModal();
}
