# Imagine Cup Game

## Team composition

a) Adrian Soucup
b) Alexandru Pana
c) Andrei Grigoriu
d) Timotei Dolean

## Setup Instructions

1) Install VS 2012
2) Install VS 2010 (If you install the Express Edition . Install link for the Express Edition: http://go.microsoft.com/?linkid=9709939)
3) Install XNA Game Studio 4.0 (http://www.microsoft.com/en-us/download/details.aspx?id=23714)
4) Invoke the "xna-setup/copy_xna.cmd" file (tools repository), by right-clicking on it, and chosing 'Run as administrator'.
4b) If you have installed VS 2010 Express edition, add the following line (replace with your install path) at the beginning of the 'copy_xna.cmd' file:
	SET VS100COMNTOOLS=c:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\Tools\
5) Open the solution file "Equitrilium.sln"
6) Go to: "Tools", "Import and Export settings".
7) Choose "Import", "Next"
8) Choose "Yes, save my current settings", "Next"
9) Press "Browse" and select the "Utils/VS2012_settings.vssettings" file
10) Select "All settings" until there is an "OK" checkmark and everything is selected, "Finish"

Optional:

1) Install ReSharper7
1) Go to "Resharper", "Manage options"
2) Select "This Computer", and in the left corner: "Import/Export settings", "Import from file" and select the "Utils/ReSharper7.DotSettings" file.


## Build instructions
If building for Windows 8, execute the 'ContentBuild' script each time you modify the content project, or first time you do a build.
