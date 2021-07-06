# TeamsLauncher
A simple command-line tool which allows you to run multiple instances of Microsoft Teams side-by-side.

Also includes a crude UI to configure your instances and easily access them from the traybar.

## How does it work?
Microsoft Teams relies on a User Profile to operate. 

This tool overrides the Environment Variable of the User Profile prior to starting an instance. This allows you to run an infinite number of Microsoft Teams instances side-by-side.

## What is included?
There are two executables included:
- TeamsLauncher.exe
- TeamsLauncher.UI.exe

### TeamsLauncher.exe
TeamsLauncher.exe is the command-line version and only takes a 0 or 1 arguments. 
- When 0 arguments are provided, it will read `instances.cfg` and use each line as an Microsoft Teams instance to start.
- When 1 argument is provided, it will start a Microsoft Teams instance using that alias.

### TeamsLauncher.UI.exe
TeamsLauncher.UI.exe is the UI version and runs continuously.
- The UI can be used to add/remove instances.
- A traybar icon will be enabled, allowing you to quickly switch between instances.

## Pro-tips
- If you set the Alias of an instance to `Default`, the tool will make no attempt to override your User Profile. This way you can use the tool to start your default instance of Microsoft Teams.
- If you set TeamsLauncher.exe or TeamsLauncher.UI.exe as a startup application, it will launch all the instances for you when your systems boots up. Be sure to disable to provided "Run at startup" option in Microsoft Teams.
- You can make shortcuts to TeamsLauncher.exe with 0 or 1 arguments provided. You can then use these shortcuts to launch or switch to the appropriate (running) instance.
- If you place these shortcuts in your Start Menu folder, you can search for them and even pin them to your Start Menu for easy access.