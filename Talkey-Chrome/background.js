var WS_PORT = 23267;
var State;
(function (State) {
    State[State["On"] = 0] = "On";
    State[State["Off"] = 1] = "Off";
    State[State["Connecting"] = 2] = "Connecting";
})(State || (State = {}));
var state = State.Off;
var socket;
var tabId;
chrome.runtime.onInstalled.addListener(function () {
    chrome.browserAction.setIcon({
        path: {
            "16": "icons/off/icon16.png",
            "32": "icons/off/icon32.png",
            "48": "icons/off/icon48.png",
            "128": "icons/off/icon128.png",
            "256": "icons/off/icon256.png"
        }
    });
});
chrome.browserAction.onClicked.addListener(function () {
    if (state == State.Off) {
        chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
            var id = tabs[0].id;
            if (id === undefined)
                0; // TODO error
            else
                tabId = id;
            console.log(tabId);
        });
        chrome.browserAction.setIcon({
            path: {
                "16": "icons/conn/icon16.png",
                "32": "icons/conn/icon32.png",
                "48": "icons/conn/icon48.png",
                "128": "icons/conn/icon128.png",
                "256": "icons/conn/icon256.png"
            }
        });
        state = State.Connecting;
        initWebsocket();
    }
    else {
        try {
            closeWebsocket();
        }
        catch (e) { }
        chrome.browserAction.setIcon({
            path: {
                "16": "icons/off/icon16.png",
                "32": "icons/off/icon32.png",
                "48": "icons/off/icon48.png",
                "128": "icons/off/icon128.png",
                "256": "icons/off/icon256.png"
            }
        });
        state = State.Off;
    }
});
function initWebsocket() {
    socket = new WebSocket("ws://localhost:" + WS_PORT + "/");
    socket.addEventListener('open', function () {
        chrome.browserAction.setIcon({
            path: {
                "16": "icons/on/icon16.png",
                "32": "icons/on/icon32.png",
                "48": "icons/on/icon48.png",
                "128": "icons/on/icon128.png",
                "256": "icons/on/icon256.png"
            }
        });
        state = State.On;
    });
    socket.addEventListener('message', onWSMessage);
}
function closeWebsocket() {
    socket.close();
}
function onWSMessage(event) {
    var data = event.data.toString();
    if (data === 'MUTE')
        mute();
    else if (data === 'UNMUTE')
        unmute();
}
function mute() {
    chrome.tabs.sendMessage(tabId, { "action": "mute" });
    chrome.browserAction.setBadgeText({ text: "M" });
}
function unmute() {
    chrome.tabs.sendMessage(tabId, { "action": "unmute" });
    chrome.browserAction.setBadgeText({ text: "UM" });
}
