function openNav() {
    document.getElementById("mySidebar").classList.toggle("open");
    document.getElementById("openFiltersBtn").classList.toggle("open");
}

function MarkCompleted(completedRow, idSetToHide, idSetToShow) {
    let elementToHide = document.getElementById(idSetToHide);
    let elementToShow = document.getElementById(idSetToShow);

    completedRow.classList.toggle("completed");
    elementToHide.classList.add("d-none");
    elementToShow.classList.remove("d-none");

    
}