echo off

REM File run during (Before, to be precise) the build process in the csproj file.
REM To change it, unload the project inside Visual Studio and edit the xml that shows up. You can find reference to this file there.
REM This script will generate a file that contains all the paths to the resources, assets, images, whatever that the app uses to function
REM Which is in turn used by the app to determine the integrity (Well, pressence, really) of each of these files to see if the app has to be re-downloaded.
REM It assumes that it's in the "Meta" folder inside the folder that holds the csproj file.
REM No error checks are made here. So... Make sure the project files have integrity! :D

REM Clear console
cls

REM Uses the parsecsproj project to generate an integrity file 
cd Meta\bin\Release\net6.0\
ParseCsproj.exe

REM Notify the compiler output that the file generated the txt successfully
echo Script - DONE
