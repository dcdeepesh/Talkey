# Talkey

Talkey is a global push to talk utility for Google Meet. Global means the push to talk key can be used anywhere, not just when the Meet tab is open/focused.

Its features include multi-key keybindings to activate push to talk, audio feedback on activate and deactivate and being globally accessible. Talkey achieves the global nature by having two running components communicating with each other; a browser component (i.e. browser extension), which handles the Google Meet controls, and a platform component (e.g. an app for the host OS), which takes care of keybindings.

> The platform component should be run before running the browser component. To make it convenient, the platform component starts at boot by default.


## Table of contents
- [Download and installation](#Download-and-Installation)
- [Usage](#Usage)
- [Configuring Talkey](#Configuring-Talkey)
  - [Changing the push to talk keybinding](#Changing-the-push-to-talk-keybinding)
  - [Audio feedback](#Audio-feedback)
  - [Other settings](#Other-settings)
- [Uninstallation](#Uninstallation)
- [Frequently Asked Questions (FAQ)](#Frequently-Asked-Questions-(FAQ))


## Download and Installation

> Talkey is currently supported only on Chrome (the browser component) and 64-bit Windows (the platform component).

The talkey installer automatically adds it as a startup app.  
TODO


## Usage

To use Talkey,
1. First make sure the platform component is running.

    On Windows this can be seen in the system tray or notification area. Because Talkey automatically starts at boot, it will be visible in the system tray unless closed manually (not recommended); if that's the case, run the application in Talkey's installation folder.

2. Then go to the browser tab with the Google Meet meeting, and click the Talkey extension button to enable it.
    
    > If you try to enable Talkey on a tab without a Google Meet meeting, it will flash red and do nothing.

3. The extension icon should now become yellow, indicating that it is connecting to the platform component. Note that the extension might become yellow for a very small duration (sometimes even unnoticable).

4. When the extension connects to the platform component, it's icon turns blue, indicating that Talkey is now connected and enabled.
   
You can now use Talkey. Your mic in the meeting will stay unmuted as long as you hold the PTT key. Pushing it will unmute, releasing will mute again. You get audio feedback on mute and unmute. **The default push to talk key is left control.** To see how to change it, see [Changing the push to talk keybinding](#Changing-the-push-to-talk-keybinding).


## Configuring Talkey

Talkey can be configured using it's configuration window, accessible through the system tray or notification area icon in Windows.

### Changing the push to talk keybinding

Talkey supports multi-key keybindings to activate push to talk. It is activated only when all keys of the keybinding are pressed (more keys may be pressed, they aren't considered).

> The default keybinding is left control.

To change the keybinding, click 'Change' near the current keybinding. It will now listen for keys. Every key you press will be _added_ to the keybinding. The keys don't need to be held, you can just tap them to add to the new keybinding. Press Done(TODO) to set the new keybinding, or Cancel(TODO) to reset.

### Audio feedback

When push to talk is activated (mic is unmuted; when all keybinding keys are pressed) and deactivated (mic is muted; when at least one keybinding key is released), Talkey gives an audio feedback. This behaviour can be configured in settings, to give feedback only on activate, only on deactivate, both, or neither.

### Other settings

You can find various other settings in the configuration window. They are pretty much self-explanatory.

## Uninstallation

When you want to remove Talkey, make sure to uninstall both the browser extension and the platform component. On Windows the uninstaller for the platform component can be run from Programs and Features, as just like any other app.

## Frequently Asked Questions (FAQ)

**What is the default push to talk key?**  
Left control

**Does talkey support multiple keybindings?**  
Not yet. Talkey only supports multiple keys in a single keybinding, but not multiple keybindings.

**Can I use Talkey on 32-bit Windows?**  
Talkey hasn't been tested on 32-bit Windows. So the behaviour is undefined. However, it will most likely not work, yet.

**Can I close Talkey from the system tray/notification area?**  
You can, but don't. Otherwise you'll have to go through the hassle of starting it again from its installation folder. If the icon annoys you so much that you're ready to go through the hassle, sure, do it.

**What about OSes other than Windows and browsers other than Chrome?**  
Support for them is coming soon.

**Is the feedback sound effect from Discord?**  
No. Ofcourse not. Or is it? ;)

**I'm doing everything accordingly but Talkey is still not working for me, what do I do?**  
Congratulations! No I mean it's bad news ofcourse, but you've just found an issue! Go ahead and open an issue, mentioning your problem and appropriate system configuration (OS, OS version, 32-bit or 64-bit, browser, browser version etc).

**Talkey works fine for me, but I've found a bug, now what?**  
Open an issue! Mention appropriate system configuration (OS, OS version, 32-bit or 64-bit, browser, browser version etc) and the bug you've encountered. If possible, mention the smallest (and simplest) amount of steps to reproduce the bug.
