chrome.runtime.onMessage.addListener((msg, sender, sendResponse) => {
    if (msg.action && msg.action === "mute")
        mute();
    else if (msg.action && msg.action === "unmute")
        unmute();
});

function mute() {
    let button = document.querySelector("button[aria-label^=\"Turn off microphone\"]");
    if (button)
        button.click();
}

function unmute() {
    let button = document.querySelector("button[aria-label^=\"Turn on microphone\"]");
    if (button)
        button.click();
}