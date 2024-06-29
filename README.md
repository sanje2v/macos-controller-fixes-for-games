# Introduction
This repository contains fixes and patches for certain MacOS targeted games whose controller mappings don't work correctly.

The patches are only targeted for Xbox One Wireless Controller connected via Bluetooth.

[**>** Blog](https://sanje2v.wordpress.com/2024/06/25/patching-oxenfree-to-fix-xbox-one-bluetooth-controller-key-mappings-haptics-for-macos/)

# Tools used
* [dnSpy](https://github.com/dnSpy/dnSpy/releases) - for editing .NET assembly. This program is designed for Windows but works well with Wine 9.11+DXVK for DirectX (use Heroic Launcher for easy setup) on a Mac.

# Games
The files needed for specific games are in their specific folder in this repo.

## Oxenfree (2014)
This patch fixes button and axes mappings for Xbox One Controller. Adding controller rumble haptics is a little more involved, so please refer to the blog above.

1. Find `Oxenfree.app` in Finder (For Steam, you can right click on the game and select `Manage -> Browse local files`). Then, right click on it and select `Show Package Contents`.
2. Navigate to `Contents/Resources/Data/Managed`.
3. If you are using a Windows machine to run dnSpy, copy all the files in this folder to your Windows machine. If on MacOS, create a backup copy of only `Assembly-CSharp.dll`.
4. Open `CAssembly-Managed.dll` in dnSpy.
5. On the tree view at the left side of the UI, navigate to `Assembly-CSharp.dll -> InControl`.
6. Right click on `XboxOneMacProfile` class and select `Edit Class...`.
7. Replace all the contents of the textbox with contents of the file from [Oxenfree/InControl_XboxOneMacProfile.cs](Oxenfree/InControl_XboxOneMacProfile.cs).
8. Click on `Compile` and then save changes by going to `File -> Save Module...` on the menu bar.
9. If on a Windows machine, replace only `CAssembly-CSharp.dll` in its original location on your Mac.

## Subnautica (2018)
This patch fixes button and axes mappings for Xbox One Controller. The game has no support for controller rumble haptics.
1. Find `Subnautica.app` in Finder (For Steam, you can right click on the game and select `Manage -> Browse local files`). Then, right click on it and select `Show Package Contents`.
2. Navigate to `Contents/Resources/Data/Managed`.
3. If you are using a Windows machine to run dnSpy, copy all the files in this folder to your Windows machine. If on MacOS, create a backup copy of only `Assembly-CSharp.dll`.
4. Open `CAssembly-Managed.dll` in dnSpy.
5. On the tree view at the left side of the UI, navigate to `Assembly-CSharp.dll -> - -> GameInput`.
6. Find the class method `GetKeyCodeAsInputName` and select `Edit Method...`.
7. Replace all the contents of the textbox with contents of the file from [Subnautica/GameInput_GetKeyCodeAsInputName.cs](Subnautica/GameInput_GetKeyCodeAsInputName.cs).
8. Click on `Compile`.
9. Find the class method `UpdateAxisValues` and select `Edit Method...`.
10. Replace all the contents of the textbox with contents of the file from [Subnautica/GameInput_UpdateAxisValues.cs](Subnautica/GameInput_UpdateAxisValues.cs).
11. Click on `Compile` and then save changes by going to `File -> Save Module...` on the menu bar.
12. If on a Windows machine, replace only `CAssembly-CSharp.dll` in its original location on your Mac.

## Life Is Strange
1. Find `Life Is Strange.app` in Finder (For Steam, you can right click on the game and select `Manage -> Browse local files`). Then, right click on it and select `Show Package Contents`.
2. Navigate to `Contents/Resources/InputDevices`.
3. Paste the file [LifeIsStrange/XboxSeriesBluetooth.plist](LifeIsStrange/XboxSeriesBluetooth.plist) in this directory.
4. After your controller is paired via Bluetooth, add the game to the list by going to `System settings... -> Bluetooth -> Xbox Wireless Controller (i) -> Gaming Controller Settings... -> Xbox Wireless Controller -> +`.
5. With newly added game, enable `Increase controller compatibility` toggle button.

## Life Is Strange 2, SOMA, Estranged: The Departure
1. After your controller is paired via Bluetooth, add the game to the list by going to `System settings... -> Bluetooth -> Xbox Wireless Controller (i) -> Gaming Controller Settings... -> Xbox Wireless Controller -> +`.
2. With newly added game, enable `Increase controller compatibility` toggle button.