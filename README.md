# RatAssets
SCP: Labrat texture swapper

Thanks to https://github.com/KhronosGroup/UnityGLTF!
 
## Install

First install BepInEx 5: https://docs.bepinex.dev/v5.0/articles/user_guide/installation.html

Second, install the plugin into the plugins folder in the bepinex folder.

Done!

## Model Swapping

Export a `.glb` (glTF 2.0) file from Blender. 1m in blender is one Unity unit.

## Texture swapping

Diffuse/Color textures can be replaced by adding the file in the `GFX` folder in the `plugins` folder.

Normal textures can be replaced by adding `_Normal` to the file name.

Emission textures can be replaced by adding `_Emission` to the file name.

## Supported textures

Currently, only SCP-173 is supported.

Name the `.png` to `173.png` to change SCP-173's texture in game.
