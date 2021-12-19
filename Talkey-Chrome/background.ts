const WS_PORT = 23267;
enum State { On, Off, Connecting }
let state = State.Off;
let socket: WebSocket;
let tabId: number;

const URL_REGEX = /^https?:\/\/meet\.google\.com\//i;

chrome.runtime.onInstalled.addListener(() => setIcon('off'));

chrome.browserAction.onClicked.addListener(() => {
    if (state == State.Off) {
        chrome.tabs.query({active: true, currentWindow: true}, tabs => {
            const tab = tabs[0];
            if (tab.url === undefined
                || tab.id === undefined
                || (!URL_REGEX.test(tab.url))) {
                setIcon('error');
                setTimeout(() => setIcon('off'), 500);
                return;
            }
            
            tabId = tab.id;
            setIcon('conn');
            state = State.Connecting;
            initWebsocket();
        });
    } else closeWebsocket();
});

chrome.tabs.onRemoved.addListener((closedTabId: number, _) => {
    if (closedTabId == tabId)
        closeWebsocket();
});

chrome.tabs.onUpdated.addListener((_, changeInfo: chrome.tabs.TabChangeInfo) => {
    if (changeInfo.url && !URL_REGEX.test(changeInfo.url))
        closeWebsocket();
});

function initWebsocket() {
    socket = new WebSocket(`ws://localhost:${WS_PORT}/`)
    socket.addEventListener('open', () => {
        setIcon('on');
        state = State.On;
    });
    socket.addEventListener('message', (event: MessageEvent<any>) => {
        const data = event.data.toString();
        if (data === 'MUTE')
            mute();
        else if (data === 'UNMUTE')
            unmute();
    });
    socket.addEventListener('close', closeWebsocket);
}

function closeWebsocket() {
    socket.close();
    setIcon('off');
    state = State.Off;
}

function mute() {
    chrome.tabs.sendMessage(tabId, { "action": "mute" });
}

function unmute() {
    chrome.tabs.sendMessage(tabId, { "action": "unmute" });
}

function setIcon(iconType: string) {
    chrome.browserAction.setIcon({
        path: {
            "16": `icons/${iconType}/icon16.png`,
            "32": `icons/${iconType}/icon32.png`,
            "48": `icons/${iconType}/icon48.png`,
            "128": `icons/${iconType}/icon128.png`,
        }
    });
}