> # Deprecation Notice
> This version of Elite Observatory is ***deprecated*** and no longer supported.
> For information about the latest version please go to the [Observatory documentation site](https://observatory.xjph.net/), or the [Observatory Core repo](https://github.com/Xjph/ObservatoryCore).
 
# Elite Observatory
Tool for reading/monitoring Elite Dangerous journals for interesting objects.

## How To Use
Simply run the exe anywhere on a machine with Elite Dangerous installed. It should automatically locate your journal files, but if for some reason it cannot it will ask for a folder to use.

Once running, you can scan your previously existing journals for interesting objects, or begin monitoring for new events and add new discoveries on the fly.

From the settings window you have the option to disable checks for different types of discoveries, to turn on toast or text-to-speech notifications, and are given a very basic interface to edit the format of data copied out of the main list window.

## How to create custom criteria
This is now documented in the sample criteria file that is created when custom criteria are enabled but no criteria file is present. This information is also available here: https://github.com/Xjph/EliteObservatory/wiki/Custom-Criteria

## Prerequisites for use
.NET 4.5, and by extension Windows Vista or later.

## Prerequisites for building
C# 7.0, Newtonsoft.Json, System.ValueTuple

An application specific client ID and shared key from Frontier Developments is required for cAPI functionality, you can obtain one from the "Developer Zone" in the Frontier user portal.
A successful build requires ClientID and ClientSecret values stored in a resource file named OAuthIDs.resx, it may be necessary to create this file yourself to build.
If you're not interested in cAPI functionality they can be arbitrary strings instead of your actual client ID and shared key from Frontier.

Optional: ILMerge
