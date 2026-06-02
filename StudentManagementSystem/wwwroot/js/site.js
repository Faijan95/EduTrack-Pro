const words = [
    "Full Stack Developer",
    "ASP.NET Developer",
    "Frontend Designer",
    "Web Developer"
];

let i = 0;
let j = 0;
let currentWord = "";
let isDeleting = false;



function type() {

    currentWord = words[i];

    if (isDeleting) {

        document.getElementById("typing").textContent =
            currentWord.substring(0, j--);

    } else {

        document.getElementById("typing").textContent =
            currentWord.substring(0, j++);

    }

    if (!isDeleting && j === currentWord.length) {

        isDeleting = true;

        setTimeout(type, 1000);

        return;
    }

    if (isDeleting && j === 0) {

        isDeleting = false;

        i++;

        if (i === words.length) {

            i = 0;
        }
    }

    setTimeout(type, isDeleting ? 60 : 100);
}

type();



// TABLE ko  SHOW or HIDE karwana


function toggleTable() {

    let table =
        document.getElementById("messageTable");

    let button =
        document.getElementById("toggleBtn");

    if (table.style.display === "none") {

        table.style.display = "block";

        button.innerText = "Hide";

    } else {

        table.style.display = "none";

        button.innerText = "Show";
    }
}




function handleRangeChange() {

    const range =
        document.getElementById("rangeSelector").value;

    const customBox =
        document.getElementById("customDateBox");

  

    if (range === "custom") {

        customBox.style.display = "flex";

        return;
    }

   

    customBox.style.display = "none";


    document
        .getElementById("filterForm")
        .submit();
}





function autoSubmitForm() {

    const start =
        document.querySelector(
            'input[name="startDate"]'
        ).value;

    const end =
        document.querySelector(
            'input[name="endDate"]'
        ).value;

   

    if (start !== "" && end !== "") {

        document
            .getElementById("filterForm")
            .submit();
    }
}