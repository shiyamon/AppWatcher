# AppWatcher

AppWatcher launches other application and check the process runnnig.
If the process of launched application is exited, starts new process again.

Currentyly AppWatcher made with MonoMac, therefore only runs on OSX but someone can easily port to other .NET platforms.

Developped with Xamarin 5.5.4

# How to use
1. Place a text file named "awConfig.cfg " besides AppWatcher.app
2. Write a path to application that you want to launch inside "awConfig.cfg" as below
  appToLaunch=FULLPATH_TO_APP
