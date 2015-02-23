#Wintumbra
####Windows Antumbra|Glow Client Software

##Quick Start

[Latest Release Download](https://github.com/TeamAntumbra/wintumbra/releases/latest)

####How to Install Glow Client and Driver

1) Plug in your Glow.

2) Run `setup.exe`

3) Press enter in console window that opens once prompted.

Run `Antumbra.exe` and enjoy!

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

###AntumbraScreenGrabber

This extension will capture the specified screen area as long as the content of that area
is not being rendered by DirectX (see DirectXScreenCapture).

###AntumbraFastScreenProcessor

This extension will take a straight average of the captured screen.

###AntumbraSmartScreenProcessor

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

More will be here soon.

See the `Licenses` dir for license information.

