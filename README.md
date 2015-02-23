#Wintumbra
####Windows Antumbra|Glow Client Software

##Quick Start

[Latest Release Download](https://github.com/TeamAntumbra/wintumbra/releases/latest)

####How to Install Glow Client and Driver

1) Plug in your Glow.

2) Run `setup.exe`

3) Press enter in console window that opens once prompted.

Run `Antumbra.exe` and enjoy!

##Please Note

The built-in settings of `stepSleep` and the weighted average checkbox will impact some
drivers in unexpected ways. For example, a HSVFade with a `stepSleep` of 0ms will look white and jittery.
It is recommended that after loading a driver extension you click the recommended settings button to the
right of the selection box. This will set the `stepSleep` variable to the recommended value. As for the weighted
average, you should disable this whenever you want the decorated driver output to be sent directly to Glow rather
have output colors be weighted against the past values. 
e.g. for a HSVFade, where averaging values together would warp the desired output

##Extension Notes

###Types

####GlowDriver

A GlowDriver announces NewColorAvailEvents when it has a new Color to 
send to Glow.

####GlowScreenGrabber

A GlowScreenGrabber captures Bitmaps of the specified screen area and announces 
a NewBitmapAvailEvent when it has a new Bitmap to be processed.

####GlowScreenProcessor

A GlowScreenProcessor processes the captured Bitmaps and announces a 
NewColorAvailEvent when it has a color to send to Glow.

####GlowDecorator

A GlowDecorator decorates colors before they are sent to Glow.

####GlowNotifier

A GlowNotifier alerts Glow devices when it detects a notification occurs, and plays a 
light sequence. This object has not been implemented fully yet.

-----------------------

###Some Built-ins

---------

###GlowDrivers


####AntumbraScreenDriverCoupler

This is actually not an extension, but an internal type that partners a GlowScreenGrabber and
GlowScreenProcessor to output colors in tandem.


####ManualColorSelector

When started opens a color selection window which will send the selected color whenever changed,
and once closed will stay on the last selected color.


####ColorClock

Outputs colors that are generated based off the time of day.


####Flux Companion

Outputs colors generated based off a kelvin temperature that is generated similarly to how F.lux generates
it's current filter color.

####HSV Fade

Fades through an HSV color spectrum from Hue value 0 to 360 continuously.

--------

###GlowScreenGrabbers

####AntumbraScreenGrabber

This extension will capture the specified screen area as long as the content of that area
is not being rendered by DirectX (see DirectXScreenCapture).

####DirectXScreenCapture

Captures pixel information for a Direct3D application. To use select it, apply, open its settings, and
enter the name of the exe you would like to capture (including the .exe). Then click start and start using the
specified exe. This extension is unstable and under heavy development.

------

###GlowScreenProcessors

####AntumbraFastScreenProcessor

This extension will take a straight average of the captured screen.

####AntumbraSmartScreenProcessor

This extension will process the screen using a 'smarter' algorithm based off the following 
configurable values. (To configure apply this extension and click the gear to the right of it)

To understand how these values play their roles in processing here is a rundown of how the smart
processor works. Four sums are set up, `blues`, `reds`, `greens`, and `all`. 

Before any pixels are analyzed the image is shrunk to a new size found by taking the current
size and dividing it by the **Scale Factor**. Raising this value can result in dramatically faster
screen processing.

As each pixel is analyzed it is checked for **Min Brightness**. If all components (`R`, `G`, & `B`) are
found to be under the **Min Brightness** then the pixel is ignored.

If a pixel is at least the minimum brightness then its components are added to two of the previously mentioned sums.

If the color's `R` component is greatest, it is added to the `reds` sum.

If the color's `G` component is greatest, it is added to the `greens` sum.

If the color's `B` component is greatest, it is added to the `blues` sum.

Each pixel color is always added to the `all` sum.

Once all pixels have been read if all of the differences between the number of colors in each of the 3 non-all (`reds`, 
`greens`, and `blues`) sums are less than the **Use All Tolerance** then the `all` sum is used.

Else, the sum (either `reds`, `greens`, or `blues`) is used as the base for color output.

After that, if `all` is not being used, and either of the other color sums are (in size) 
at least **Min Mix Percentage** of the size of the base color sum, then that sum is added 
to the base sum.

For example, if `reds` is greatest at a size of 500, and **Min Mix Percentage** is set to 10% then for the `greens` or `blues` sum
to be added into the `reds` sum their size must be at least 50 colors.

Finally the average of the final color sum is returned as the found color.

* **Min Brightness** - Changes the minimum value each component of a pixel must be to be included in the processing.

* **Use All Tolerance** - Tolerance to use the `all` sum (see explanation of algorithm).

* **Min Mix Percentage** - Minimum percentage a component sum must be of the total to be included in the processing.

* **Scale Factor** - Factor to scale the Bitmap down by before processing.

-----

###GlowDecorators

####Brightener

Increases the output colors lightness value (in an HSL color space) by the specified value in its settings.

####Saturator

Increases the output colors saturation value (in an HSL color space) by the specified value in its settings;

-----

###GlowNotifiers

Currently none.

------

More will be here soon.

See the `Licenses` dir for license information.

