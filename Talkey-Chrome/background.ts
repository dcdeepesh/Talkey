const WS_PORT = 23267;
enum State { On, Off, Connecting }
let state = State.Off;
let socket: WebSocket;
let tabId: number;

chrome.runtime.onInstalled.addListener(() => setIcon('off'));

chrome.browserAction.onClicked.addListener(() => {
    if (state == State.Off) {
        chrome.tabs.query({active: true, currentWindow: true}, tabs => {
            let tab = tabs[0];
            if (tab.url === undefined || tab.id === undefined)
                return; // TODO error
            
            if (!/^https?:\/\/meet\.google\.com\//i.test(tab.url)) {
                setIcon('error');
                setTimeout(() => setIcon('off'), 500);
                return;
            }
            
            tabId = tab.id;
            setIcon('conn');
            state = State.Connecting;
            initWebsocket();
        });
    } else {
        try { closeWebsocket(); }
        catch (e) {}
        setIcon('off');
        state = State.Off;
    }
});

function initWebsocket() {
    socket = new WebSocket(`ws://localhost:${WS_PORT}/`)
    socket.addEventListener('open', () => {
        setIcon('on');
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
}

function unmute() {
    chrome.tabs.sendMessage(tabId, { "action": "unmute" });
}

function setIcon(iconType: String) {
    chrome.browserAction.setIcon({
        path: {
            "16": `icons/${iconType}/icon16.png`,
            "32": `icons/${iconType}/icon32.png`,
            "48": `icons/${iconType}/icon48.png`,
            "128": `icons/${iconType}/icon128.png`,
        }
    });
}