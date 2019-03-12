const minIntervalValue = 5;
const maxIntervalValue = 45;
const maxLengthToPlay = 60;
const mediaFile = "sounds/starship.mp3"

let playIntervalId;
let playLengthId;
let isMediaPlaying;


function logInfo(message, important) {
    console.log(message);

    logElement = document.getElementById("player-log");
    newLine = document.createElement("p");
    if (important)
        newLine.classList.add("bold-red-text");

    currentDate = Date.now();
    dateFormat = {
        year: 'numeric', month: 'numeric', day: 'numeric',
        hour: 'numeric', minute: 'numeric', second: 'numeric'
    }
    newLine.innerText = new Intl.DateTimeFormat('ro-RO', dateFormat).format(currentDate) + " : " + message;
    logElement.insertBefore(newLine, logElement.firstChild);

    // also save log to db via web API
}

function readConfiguration() {
    minIntervalValue = parseInt(document.getElementById("min-value").value);
    maxIntervalValue = parseInt(document.getElementById("max-value").value);
    maxLengthToPlay = parseInt(document.getElementById("max-length").value);
    mediaFile = document.getElementById("media-file").value;
    if (Number.isInteger(minIntervalValue) && Number.isInteger(maxIntervalValue) && Number.isInteger(maxLengthToPlay)) {
        return true;
    }
}

async function playSound() {
    audioElem = document.getElementById("audio-elem");
    try {
        await audioElem.play();
        logInfo("Playing sound file!");
        playLengthId = window.setTimeout(stopPlay, maxLengthToPlay * 1000);
    } catch (err) {
        logInfo("Can not play the file.");
    }
}

function stopPlay() {
    audioElem = document.getElementById("audio-elem");
    audioElem.pause();
    audioElem.currentTime = 0;
    logInfo("Max play length reached.");
    scheduleNextPlay();
}

function getNextPlayTime(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min; //The maximum is inclusive and the minimum is inclusive 
}

function scheduleNextPlay() {
    nextPlayTime = getNextPlayTime(minIntervalValue, maxIntervalValue);  // time in minutes
    logInfo("Next play in: " + nextPlayTime + " minutes");
    playIntervalId = window.setTimeout(playSound, nextPlayTime * 60000); // time in seconds
}


/* event handlers */
function playButtonClick() {
    if (readConfiguration()) {
        logInfo("AudioPlayer started.", true);
        scheduleNextPlay();
    }
    else {
        logInfo("Configurion is invalid.");
    }
}

function stopButtonClick() {
    window.clearTimeout(playIntervalId);
    window.clearTimeout(playLengthId);
    audioElem = document.getElementById("audio-elem");
    audioElem.pause();
    audioElem.currentTime = 0;
    isMediaPlaying = false;
    logInfo("AudioPlayer stopped.", true);
}

function audioEnded() {
    isMediaPlaying = false;
    logInfo("Playback ended.");
    scheduleNextPlay();
}


/* attach events */
document.getElementById("play-button").addEventListener("click", playButtonClick);
document.getElementById("stop-button").addEventListener("click", stopButtonClick);
document.getElementById('audio-elem').addEventListener("ended", audioEnded);