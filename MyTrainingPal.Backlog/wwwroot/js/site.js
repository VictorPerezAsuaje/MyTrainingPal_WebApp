function openNav() {
    document.getElementById("mySidebar").classList.toggle("open");
    document.getElementById("openFiltersBtn").classList.toggle("open");
}

function MarkCompleted(completedRow) {
    completedRow.classList.toggle("completed");
}