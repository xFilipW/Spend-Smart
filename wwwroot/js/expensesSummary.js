document.addEventListener("DOMContentLoaded", function () {
    var ctx = document.getElementById('expenseChart').getContext('2d');
    var chartType = document.getElementById('chartType');

    function fetchExpensesData() {
        fetch('/Home/GetExpensesData')
            .then(response => response.json())
            .then(data => {
                var labels = data.map(expense => expense.category);
                var values = data.map(expense => expense.totalAmount);

                updateChart(labels, values);
            })
            .catch(error => console.error('Error fetching data:', error));
    }

    var expenseChart;
    function updateChart(labels, values) {
        if (expenseChart) {
            expenseChart.destroy();
        }

        expenseChart = new Chart(ctx, {
            type: chartType.value,
            data: {
                labels: labels,
                datasets: [{
                    label: "Expenses",
                    data: values,
                    backgroundColor: ["#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF"]
                }]
            },
            options: { responsive: true }
        });
    }

    chartType.addEventListener("change", fetchExpensesData);

    fetchExpensesData();
});
