document.addEventListener("DOMContentLoaded", function () {
    const links = document.querySelectorAll(".sidebar .nav-link");

    function setActiveLink() {
        const currentPath = window.location.pathname;

        links.forEach(link => {
            const linkPath = link.getAttribute("href");

            if (currentPath === linkPath || (linkPath === "/Home/CreateEditExpense" && currentPath.startsWith("/Home/CreateEditExpense"))) {
                link.classList.add("active");
            } else {
                link.classList.remove("active");
            }
        });
    }

    setActiveLink();

    links.forEach(link => {
        link.addEventListener("click", function () {
            links.forEach(l => l.classList.remove("active"));
            this.classList.add("active");
        });
    });
});