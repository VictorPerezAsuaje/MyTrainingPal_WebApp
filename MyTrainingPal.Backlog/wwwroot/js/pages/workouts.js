const ExerciseTypeReps = (setNumber) => `
<div class="form-outline mb-4">
    <label class="form-label">Number of reps:</label>
    <input name="numberOfReps${setNumber}" type="number" min="1" class="form-control form-control-lg" placeholder="Number of reps" data-val-required="You have to specify the number of reps." required />
    <span class="text-danger" data-valmsg-for="numberOfReps${setNumber}"></span>
</div>`;

const ExerciseTypeTime = (setNumber) => `
<div class="form-outline mb-4">
    <label class="form-label">Minutes</label>
    <input name="numberOfMinutes${setNumber}" type="number" min="0" class="form-control form-control-lg" placeholder="Number of minutes" data-val-required="You have to specify the number of minutes" required />
    <span class="text-danger" data-valmsg-for="numberOfMinutes${setNumber}"></span>
    <br><label class="form-label">Seconds</label>
    <input name="numberOfSeconds${setNumber}" type="number" min="0" class="form-control form-control-lg" placeholder="Number of seconds" data-val-required="You have to specify the number of seconds" required />
    <span class="text-danger" data-valmsg-for="numberOfSeconds${setNumber}"></span>
</div>`;

const ExerciseSet = (exercises, setNumber, setType) => `
<div id="set${setNumber}">
    <div class="form-outline mb-4">
        <label class="form-label">Exercise</label>
        <select class="form-select">
            ${[...exercises].map(x => `<option value='${x.Id}'>${x.Name}</option>`)}
        </select>
    </div>
    ${setType === "ByTime" ? ExerciseTypeTime(setNumber) : ExerciseTypeReps(setNumber)}
</div><hr />
`

const ChangeAvailableSets = (exercises) => {
    const selectSetType = document.getElementById("WorkoutSetType");
    const selectedType = selectSetType.options[selectSetType.selectedIndex].innerText;

    const inputNumberSets = document.getElementById("NumberOfSetsInput");
    const numberOfSets = inputNumberSets.value;

    const exerciseContainer = document.getElementById("ExerciseContainer");
    exerciseContainer.innerHTML = "";

    let generatedHtml = "";
    for (let i = 0; i < numberOfSets; i++) {
        generatedHtml += ExerciseSet(exercises, i, selectedType);
    }

    exerciseContainer.innerHTML = generatedHtml;
}

const ValidateForm = (formId) => {
    let form = document.getElementById(formId);

    let isValid = true;
    for (var i = 0; i < form.elements.length; i++) {
        if (form.elements[i].value === '' && form.elements[i].hasAttribute('required')) {
            let itemName = form.elements[i].name
            let valRequiredMessage = form.elements[i].getAttribute("data-val-required");
            let span = document.querySelectorAll(`[data-valmsg-for='${itemName}']`)[0];
            span.innerText = valRequiredMessage;
            isValid = false;
        }
    }

    return isValid;
}

const SubmitNewWorkoutHandler = (e) => {
    e.preventDefault();
    if (!ValidateForm('workoutCreateEditForm'))
        return;

    const workoutId = document.getElementById("workoutId").value;
    const workoutName = document.getElementById("workoutName").value;
    const workoutType = document.getElementById("workoutType").value;
    const numberOfSets = document.getElementById("NumberOfSetsInput").value;
    const setType = document.getElementById("WorkoutSetType").value;

    const exerciseContainer = document.getElementById("ExerciseContainer");
    const setContainers = [...exerciseContainer.children].filter(x => x.localName !== "hr");

    const setPostsDTOs = [];
    setContainers.map(x => {
        let exerciseSelect = x.getElementsByTagName("select");
        const exerciseId = exerciseSelect[0].options[exerciseSelect[0].selectedIndex].value;
        const inputs = x.getElementsByTagName("input");

        let setPostDTO;
        if (setType === "ByTime") {
            const minutes = inputs[0].value;
            const seconds = inputs[1].value;
            setPostDTO = new SetPostDTO(setType, exerciseId, null, minutes, seconds, null);
        }
        else {
            const reps = inputs[0].value;
            setPostDTO = new SetPostDTO(setType, exerciseId, null, null, null, reps);
        }

        setPostsDTOs.push(setPostDTO);
    })

    const workoutPutDTO = new WorkoutPutDTO(workoutId === "" ? null : workoutId, workoutName, numberOfSets, setPostsDTOs, setType, workoutType)

    $.ajax({
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: "/Workout/CreateOrEdit",
        data: JSON.stringify(workoutPutDTO),
        success: function (result) {
            $('#workoutCreateEditModal').modal('hide');
            Swal.fire({
                icon: 'success',
                title: 'Done!',
                text: result.responseText,
            })
        },
        error: function (result) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: result.responseText,
            });
        }
    });

    document.getElementById("filterWorkoutsButton").click();
}

class WorkoutPutDTO {
    constructor(id, name, numberOfSets, setPostsDTOs, setType, workoutType) {
        this.Id = id;
        this.Name = name;
        this.NumberOfSets = numberOfSets;
        this.SetPostDTOs = setPostsDTOs;
        this.SelectedSetType = setType;
        this.SelectedWorkoutType = workoutType;
    }
}

class SetPostDTO {
    constructor(setType, exerciseId, hours, minutes, seconds, reps) {
        this.SelectedSetType = setType;
        this.ExerciseId = exerciseId;
        this.Hours = hours;
        this.Minutes = minutes;
        this.Seconds = seconds;
        this.Repetitions = reps;
    }
}