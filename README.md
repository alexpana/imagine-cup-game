# Imagine Cup Game

## Team composition

a) Adrian Soucup
b) Alexandru Pana
c) Andrei Grigoriu
d) Timotei Dolean

## Setup Instructions

1) Install VS 2012
2) Install VS 2010 (Express edition will be fine: http://go.microsoft.com/?linkid=9709939)
3) Install XNA Game Studio 4.0 (http://www.microsoft.com/en-us/download/details.aspx?id=23714)
4) Invoke the "Utils/copy_xna.cmd" file from ADMINISTRATOR command line, specifying the VS 2010 and VS 2012 install dirs:
	eg: copy_xna.cmd "C:\Program Files (x86)\Microsoft Visual Studio 10.0" "C:\Program Files (x86)\Microsoft Visual Studio 11.0"
	4a) Press the 'd' key when asked about: "(F = file, D = directory)?"
	4b) Wait for finishing
5) Open the solution file "ICGame.sln"
6) Go to: "Tools", "Import and Export settings".
7) Choose "Import", "Next"
8) Choose "Yes, save my current settings", "Next"
9) Press "Browse" and select the "Utils/VS2012_settings.vssettings" file
10) Select "All settings" until there is an "OK" checkmark and everything is selected, "Finish"

Optional:

1) Install ReSharper7
1) Go to "Resharper", "Manage options"
2) Select "This Computer", and in the left corner: "Import/Export settings", "Import from file" and select the "Utils/ReSharper7.DotSettings" file.

