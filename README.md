# Talkey

[![Windows badge](https://img.shields.io/badge/windows-v1.1-blue)](https://github.com/dcdeepesh/Talkey/releases/latest)

[![Chrome badge](https://img.shields.io/chrome-web-store/v/ikpllienmchnfkfbfindmciobnhdgjlh)](https://chrome.google.com/webstore/detail/ikpllienmchnfkfbfindmciobnhdgjlh)
[![Edge badge](https://img.shields.io/badge/edge%20addons-v1.0-blue)](https://microsoftedge.microsoft.com/addons/detail/flhikbcahicljnkcehffnefgphedkhbc)

Talkey is a global push to talk utility for Google Meet. Global means the push to talk key can be used anywhere, not just when the Meet tab is open/focused.

Its features include multi-key keybindings to activate push to talk, audio feedback on activate and deactivate and being globally accessible. Talkey achieves the global nature by having two running components communicating with each other; a browser component (i.e. browser extension), which handles the Google Meet controls, and a platform component (e.g. an app for the host OS), which takes care of keybindings.


## Table of contents
- [Download and installation](#Download-and-Installation)
- [Usage](#Usage)
- [Configuring Talkey](#Configuring-Talkey)
  - [Changing the push to talk keybinding](#Changing-the-push-to-talk-keybinding)
  - [Audio feedback](#Audio-feedback)
  - [Run at startup](#Run-at-startup)
- [Uninstallation](#Uninstallation)
- [Frequently Asked Questions (FAQ)](#Frequently-Asked-Questions-(FAQ))


## Download and Installation

Talkey is currently supported on Chrome and Edge (browser component), and 64-bit Windows (platform component).

Latest versions of all components can be found below:
- Chrome: [Chome Web Store](https://chrome.google.com/webstore/detail/ikpllienmchnfkfbfindmciobnhdgjlh)
- Edge: [Edge Add-ons](https://microsoftedge.microsoft.com/addons/detail/flhikbcahicljnkcehffnefgphedkhbc)
- Windows: [latest release](https://github.com/dcdeepesh/Talkey/releases/latest).

> Your browser, OS, antivirus etc. may warn you that Talkey and/or its setup are harmful. This is just because the executables are unsigned, hence those warnings can be ignored.


## Usage

To use Talkey,
1. First make sure the platform component is running. To make this step easier, Talkey runs at start by default.

    On Windows this can be seen in the system tray or notification area. If you unchecked the checkbox to run at start, manually run the application from its desktop icon or start menu entry. If you deleted both, run it from Talkey's installation folder (`C:\Users\<user>\AppData\Local\Programs\Talkey`).

2. Then go to the browser tab with the Google Meet meeting and click Talkey's extension icon to enable it. If you try to enable Talkey on a tab without a Google Meet meeting, it will flash red and do nothing.

3. The extension icon should now (briefly) become yellow, indicating that it is connecting to the platform component.

4. When it connects to the platform component, its icon turns blue, indicating Talkey is now ready to use.
   
You can now use Talkey. Your mic in the meeting will stay unmuted as long as you hold the push to talk key. Pushing it will unmute, releasing will mute again. You get audio feedback on mute and unmute. The default push to talk key is left control. To see how to change it, see [Changing the push to talk keybinding](#Changing-the-push-to-talk-keybinding).


## Configuring Talkey

Talkey can be configured using its settings window, accessible through its notification area icon in Windows.

### Changing the push to talk keybinding

Talkey supports multi-key keybindings to activate push to talk. It is activated only when all keys of the keybinding are pressed (more keys may be pressed, they aren't considered).

> The default keybinding is left control.

To change the keybinding, click 'Change' near the current keybinding. It will now listen for keys. Every key you press will be _added_ to the keybinding. The keys don't need to be held, you can just tap them to add to the new keybinding. Press 'Done' or 'Reset' to set the new keybinding or reset it, respectively. If you want to cancel, press 'Reset' then 'Done'.

### Audio feedback

When push to talk is activated (mic is unmuted; when all keybinding keys are pressed) and deactivated (mic is muted; when at least one keybinding key is released), Talkey gives an audio feedback. This behaviour can be configured in settings, to give feedback only on activate, only on deactivate, both, or neither.

### Run at startup

By default, Talkey runs at startup to save you from the hassle of remembering and starting it every time you want to use it. It is recommended not to disable this option for a better experience. However, if you choose to disable it, make sure you have a desktop icon or the start menu entry, otherwise it will be very inconvenient to go to Talkey's installation directory every time you want use it.

## Uninstallation

When uninstalling Talkey, make sure to uninstall both the browser extension and the platform component.

## Frequently Asked Questions (FAQ)

**What is the default push to talk key?**  
Left control

**Does talkey support multiple keybindings?**  
Not yet. Talkey only supports multiple keys in a single keybinding, but not multiple keybindings.

**Can I use Talkey on 32-bit Windows?**  
Talkey hasn't been tested on 32-bit Windows. So the behaviour is undefined. However, it will most likely not work, yet.

**Can I close Talkey from the system tray/notification area?**  
You can, but make sure to read [Run at startup](#run-at-startup) first.

**What about OSes other than Windows and browsers other than Chrome?**  
Support for them is coming soon.

**Are the feedback sound effects from Discord?**  
They were, but as of version 1.1, sound effects have been customized due to copyright issues🙁 (made possible with [NsiX](https://nsix.itch.io/)'s help).

**I'm doing everything accordingly but Talkey is still not working for me, what do I do?**  
Congratulations! No I mean it's bad news ofcourse, but you've just found an issue! Go ahead and open an issue, mentioning your problem and appropriate system configuration (OS, OS version, 32-bit or 64-bit, browser, browser version etc).

**Talkey works fine for me, but I've found a bug, now what?**  
Open an issue! Mention appropriate system configuration (OS, OS version, 32-bit or 64-bit, browser, browser version etc) and the bug you've encountered. If possible, mention the smallest (and simplest) amount of steps to reproduce the bug.
