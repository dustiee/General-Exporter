# GeneralExporter

## Description
A Block Story BepInEx utility to export some assets as text on game launch.

The following are exported into ```/path/to/BlockStory/BepInEx/GeneralExporterOutput/``` as text files.

0. Item Info
0. Quests
0. Achievements
0. Recipes 

If Recipe Loader is available, exported recipes will be converted to Recipe Loader recipes if possible.

**NOTE:** GeneralExporter automatically clears the subdirectories in ```GeneralExporterOutput``` before populating them.
Please move the files you want to modify out of ```GeneralExporterOutput``` before making changes to them.

### Requirements

0. BepInEx properly installed in the BlockStory directory. Installation guide for BepInEx is available <u>[here](https://docs.bepinex.dev/articles/user_guide/installation/index.html).</u>

### Optional Requirements 

0. <u>Recipe Loader</u> for Block Story properly installed.

## Installation 

Download the latest release and move ```GeneralExporter.dll``` into ```/path/to/BlockStory/BepInEx/plugins/```

## Building prerequisites

You'll need the game's assemblies, so you'll need to paste Assembly-CSharp.dll from the game's ```Managed``` folder into ./lib 

## Disclaimer

This utility is publicly available in the hope that it will be useful. I do not take responsibility for maintaining or 
improving it in the future.
