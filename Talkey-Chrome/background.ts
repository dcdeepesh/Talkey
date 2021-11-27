const WS_PORT = 23267;
enum State { On, Off, Connecting }
let state = State.Off;
let socket: WebSocket;
let tabId: number;

chrome.runtime.onInstalled.addListener(() => {
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

chrome.browserAction.onClicked.addListener(() => {
    if (state == State.Off) {
        chrome.tabs.query({active: true, currentWindow: true}, tabs => {
            let id = tabs[0].id;
            if (id === undefined)
                0; // TODO error
            else tabId = id;
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
    } else {
        try { closeWebsocket(); }
        catch (e) {}
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
    socket = new WebSocket(`ws://localhost:${WS_PORT}/`)
    socket.addEventListener('open', () => {
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

function onWSMessage(event: MessageEvent<any>) {
    const data = event.data.toString();
    if (data === 'MUTE')
        mute();
    else if (data === 'UNMUTE')
        unmute();
}

function mute() {
    chrome.tabs.sendMessage(tabId, { "action": "mute" });
    chrome.browserAction.setBadgeText({text: "M"});
}

function unmute() {
    chrome.tabs.sendMessage(tabId, { "action": "unmute" });
    chrome.browserAction.setBadgeText({text: "UM"});
}