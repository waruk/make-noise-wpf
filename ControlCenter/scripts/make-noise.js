var isPlayerStarted = false;

showPlayerControls("play");

function showPlayerControls(buttonToShow) {
    playButton = document.querySelector('#play-button');
    stopButton = document.querySelector('#stop-button');

    if (buttonToShow === "play") {
        playButton.className = "show-player-button";
        stopButton.className = "hide-player-button";
    }
    else {
        playButton.className = "hide-player-button";
        stopButton.className = "show-player-button";
    }

}

function setPlayerStatus(status) {
    playerStatusElement = document.querySelector('#player-status');
    playerStatusElement.textContent = "Status " + status;
}

function writeToLog(message) {
    logElement = document.querySelector("#plaplayer-log")
    

}

/* attach events */

document.querySelector("#player-controls").onclick = function () {
    if (isPlayerStarted) {
        setPlayerStatus("OFF");
        showPlayerControls("play");
        isPlayerStarted = false;
    }
    else {
        setPlayerStatus("ON");
        showPlayerControls("stop");
        isPlayerStarted = true;
    }
}





