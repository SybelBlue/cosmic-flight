# cosmic-flight

A simple space game that gets harder as you go. We challenge you to terraform all the planets without getting sucked into black hole or running out of oxygen. How far can you go?

## Setup
 - Install [Unity](https://unity3d.com/get-unity/download)
   - Allow Unity to install Visual Studio OR
   - Install/use your favorite C# capable text editor
 - Using Unity's Asset Store, install [Github for Unity](https://unity.github.com/)
   - Configure Github for Unity with the proper user info
 - Get [Github Desktop](https://desktop.github.com/)
 - Start a new Unity Project with the Github for Unity package
 - Fork and/or clone [cosmic-flight](https://github.com/SybelBlue/cosmic-flight) using Github Desktop into the directory of the new Unity project, and refresh the project
 - Install the build tools for Android/iOS through the Build Settings menu
 - Change Game view to have the appropriate resolution 
 - Experiment!

## Game
 - The goal is to terraform all the asteroids into planets and return to a livable planet in as few flights as possible
 - Terraform an asteroid by landing, then going to a planet
   - Once an asteroid has a flag on it, it will be terraformed the next time the rocket lands on a planet
 - Aim by dragging and rotating the rocket when it's landed -- the farther away you drag, the more power (represented by fire)
 - Peek around space by dragging when the rocket is flying!

## Known Bugs
 - Level buttons don't load the levels they say they do
 - Camera does not focus on death animations
 - Rocket occasionally launches itself after death

## TODO
 - Add mute option for audio and launch haptics
 - Don't explode on purposeful relaunch?
