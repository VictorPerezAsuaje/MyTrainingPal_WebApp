function openNav() {
    document.getElementById("mySidebar").classList.toggle("open");
    document.getElementById("openFiltersBtn").classList.toggle("open");
}

function MarkCompleted(completedRow, idSetToHide, idSetToShow) {
    let finished = document.getElementById("exerciseFinishedCongrats");
    let elementToHide = document.getElementById(idSetToHide);
    let elementToShow = document.getElementById(idSetToShow);
    const video = document.getElementById("mainVideo");

    completedRow.classList.toggle("completed");
    elementToHide.classList.add("d-none");
    if (elementToShow !== null && elementToShow !== undefined)
        elementToShow.classList.remove("d-none");
    else
        finished.classList.remove("d-none");

    video.src = elementToShow.children[0].src == undefined ? "https://cdn.pixabay.com/photo/2017/04/22/10/15/woman-2250970_960_720.jpg" : elementToShow.children[0].src;
}