# Fog

## Unity 2017.4 Shader Demo - Study Project - WIP
### Vertical fog shader with textures and distortion.

### Location: Assets/Vertical_Fog_Distortion.shader

This is a demo project for a basic unity shader that fades/blends vertically based on the camera depth texture.
This part of the shader is based on [this tutorial](http://halisavakis.com/my-take-on-shaders-vertical-fog/)
by Harry Alisavakis. It may require a directional light in the scene.

This shader was written with the intent of just using it on a quad and will not work well with very different geometry.

## Texture Features
###Texture, Ivert, Tint Color, Alpha, Fog Transition
The shader can be used without textures to achieve the result visible in the above tutorial.
A default white texture is used as the main texture if none are assigned. This is tinted with the chosen color.

An Alpha control was added, which simply reduces the alpha based on the darkest parts of the image first.

Fog Transition corresponds to intersectionThresholdMax; from experimenting with values I set this on a slider
to be in a limited range. This controls how "deep" the blend is between the quad and the objects occluded by it.

###Use Cookie toggle, Cookie, Cookie Strength
A "cookie" texture slot was added and is simply multiplied onto the main texture. Tiling and Offset are not available
on this texture as it uses the same UV as the main texture. If using this feature, make sure wrapmode is set to
clamp in texture import settings. Otherwise, the texture will tile and you will see it in the corners if there is
a rotation applied.(Likewise if using a non-tiling main texture).

## Transform Features
###Rotation toggle, RotationSpeed, Origin X, Origin Y
Checking the Rotation toggle allows rotation of the textures. Since the cookie texture is on the same UV as the main
texture, it will always rotate with it. The origin of the rotation can be moved using Origin X and Origin Y. This can
be used to position the texture/cookie relative to the quad. If not using the cookie and using a tileable texture,
Origin can be set to values outside the quad which will mimic a translation. This range can easily be increased by
editing the Range in the shader properties. Of course, you can always use a tileable cookie texture but it would be
more efficient to bake that into a single texture and just use the main texture slot.

This toggle also allows the distortion texture to be rotated. Since this is on a separate UV, I have assigned
separate speed controls so that the distorion can rotate relative to the main visible image and either can be static.

## Distortion Features

This part of the shader is based on the "Animated materials: a water shader" paragraph and code snippet from
[this article](https://www.alanzucconi.com/2015/07/01/vertex-and-fragment-shaders-in-unity3d/) by Alan Zucconi.
###Texture
A dedicated distortion texture can be added. It will also default to white.

###Use Main Texture toggle
Since this was designed around the included noise texture(s), I added the option to use the main texture as the
distortion texture. This will override the assigned texture.

###Use Cookie toggle, Amount
Using the cookie in the distortion, again that's just a multiply operation so it has the effect of reducing the
amount of distortion in the dark parts of the cookie image; white being unaffected.

###Add Main Texture Distortion toggle, Amount
Add Main texture distortion does what it says; uses the main texture to add more variation to the distortion.

###Show Distortion Texture toggle
The show distortion texture toggle was mostly a debugging feature but I have left it in. It just allows you to check
what the distortion texture currently looks like, rotation and translation. Add main texture distortion is not visible
here as it is not applied to the texture directly, but to the UV coords via the sine function.

###Speed, Magnitude, Period, Period Offset
Speed; how quickly the waves move/oscillate.
Magnitude; Amplitude.
Period; Wavelength.
Period Offset; sin(Time) is used as the main animating input. Period offset is added to this so instead of the
function going positive to negative, it will vary relative to Period Offset. The effect is a standing wave-like
pattern through the shades of grey in the image. It is a similar effect to lowering the period but not exactly
the same.
TL;DR How the distortion works is, a 2D sine function is used to distort the UV that the main texture will
use. To any given position on that UV, we also add the value of the distortion texture. This just adds some variation
so that it is not perfectly smooth sine waves everywhere. 

##Distortion Transforms
###Rotation Speed
Rotation speed for the distortion.

###Translation toggle, X Speed, Y Speed
The toggle enables translation of the textures for a drifting motion. Currently, the shader does not allow translating
the distortion relative to the main texture.

###Apply Fog Before Main Texture
The shader supports using unity's default fog. This toggle lets you decide if the fog is applied to the output before
or after the main texture. Possibly useless.

##Limitations, Future Plans
Translating the distortion relative to the main texture.
Adding a sine function option to the translation X and Y Speed so you could make elipses.
Allowing translation and rotation of distortion and main texture relative to the cookie texture.
The cookie is very limited. Ideally I would like it on its own UV so I could move the other textures relative to it.
I would need to sacrifice unity fog to free up a set of UV coordinates, or build this in as a toggle option.

##SceneViewCamera.cs
I included this little exercise, posted as a question on reddit. The goal was to make camera controls similar to
how the scene view works: rightclick rotates and enables wasd movement, middlemouse moves on a plane perpendicular
to the forward transform, Alt+leftclick rotates around the selected object's origin.
keycode.LeftAlt does not seem to operate in the editor so I used the z key.

Having no selection mechanic I opted to use a raycast along tranform.forward; if nothing is hit it defaults to the
world origin. I found this not so bad, might actually be useful in a game since you can aim with
rightclick and then rotate around that point with z+leftclick. The ray's range is 50 so that should be adjusted
to the scene (not currently available in editor). I included a modifier to the speed so it slows down when close up.
This is also hardcoded to distance / 50 currently.

The Altclick has a bug in that if you move the mouse fast, the camera travels away from the target. This is because
I'm translating in a straight line each frame so it zig-zags away. Possible fix: save the initial distance and move
the camera towards it after every LookAt().





