The concept is the following: 
Harmonix always uses the same type of image files in their Xbox 360/PS3 games (observed from Guitar Hero 2 to Rock Band: Blitz), with only the headers varying from game to game and based on the proportions of the image. All the images are DDS textures, in a combination of DXT1, DXT5, and 3Dc Normal Map formats with and without mip-maps enabled. Even though no DXT3 textures have been observed, this method will also support them if they are found.

Xbox 360 images are byte swapped, PS3 images are not. Advanced Art Converter takes care of that for you, just make sure you got the right extension for the file (.png_xbox or .png_ps3).

As of this writing, over 100 HMX headers are included, but there are undoubtedly more to be found.
Run your .png_xbox or .png_ps3 image through Advanced Art Converter, anything that isn't converted properly means it's got a header it doesn't (yet) recognize.

The header is always the first 32 bytes of the .png_xbox / .png_ps3 file, however, the second 16 bytes (so far observed) have always been 0x00, so we can ignore those. 

Copy the first 16 bytes of the header into a new binary file (I recommend using HxD, but any hex editor will work), and save it following the naming convention I came up with (more below). To speed up the process, the program only searches for .header files, so make sure to add that extension to it.

The file names are used by the code to efficiently determine the proportions and format of the image when it builds the DDS header we'll need.
Once Advanced Art Converter knows which file it is, it removes the 32-byte HMX header, does the byte-swapping if needed, and adds the appropriate DDS header.

For example: RB3_256x512_DXT5.header tells the code the image is 256 pixels wide, 512 pixels high, and uses DXT5 format. The 'RB3' at the beginning doesn't do anything, it's just there to distinguish the source of the header and because we usually have more than one header of any given proportion from different games. This naming convention is NOT case sensitive.

That's it. Once you got your 16 byte header file properly named and with the .header extension, drop it in the /bin/headers folder. Run Advanced Art Converter again and it will pick it up, and your file should be converted properly this time. Repeat for any other files that fail conversion.

---------------------------------------------------------------------------------

TIPS AND ADVANCED INFORMATION (I.E. OPTIONAL READING)

The current code will always generate mip-map-disabled DDS textures. If the original game texture contained mip-maps, the data is still contained in the file, but the header won't support it so if you open the file in Photoshop or Paint.NET you won't see the mip-maps. You can repair the header manually if you want to recover the mip-maps, for the purposes of this program the added complexity of getting this part of the header correctly is beyond my interest.

NORMAL FILES - Look at the name of the file. If the file name contains _norm or _normal, then make sure that header has NORMAL added to it for format. 3Dc Normal Map textures are used by Harmonix to create depth when combined with the corresponding texture. These are the minority of the textures you'll ever find, but just a heads up. If you incorrectly label it DXT5 or DXT1 the resulting image will be garbled. So getting the format correct is important.

DETERMINING THE FORMAT BASED ON THE HMX HEADER
This becomes easier based on how many files you've looked at. At this point I can pick it out instantly. This is a good way to start:
01 04 08, 02 04 08, or sometimes just 04 08 at the start of the header indicates DXT1
01 08 18, or sometimes just 08 18 at the start of the header indicates DXT5
01 08 20, or sometimes just 08 20 at the start of the header indicates 3Dc Normal Map (NORMAL)

DETERMINING THE DIMENSIONS BASED ON THE HMX HEADER
This changes location in the header based on the game, but it's always going to be width byte height byte or byte width byte height
Everything is in proportion to the number 256. So if the width byte is 0x01 = 256px, if it's 0x02 = 512px, 0x04 = 1024px, 0x08 = 2048px (the largest I've found)
Where the "byte" mentioned above before or after the width and height bytes come into play are if the width or height byte is 0x00.
Now we're working on the basis of 128. So if byte width is 0x80 0x00 = 128px, but if it's 0x40 0x00 = 64px, if it's 0x20 0x00 = 32px, 0x10 0x00 = 16px and 0x08 0x00 = 8px (the smallest I've found)
Knowing that, once you find where in the header the proportion bytes are, you should be able to quickly determine the proportions, if not...

DETERMINING THE DIMENSIONS BASED ON THE FILE SIZE
DDS textures are much like BITMAP images = file size is always the same for an image of the same proportions. Whether the image is transparent, pure white or full of varying colors, the resulting DDS file (and by extension, our .png_xbox or .png_ps3 file) will be a specific file size
Here are some file sizes to get you started:
256x256 DXT1 = 43KB
256x256 DXT5 = 86KB (always 2x the file size of the DXT1 counterpart)
512x512 DXT1 = 171KB
512x512 DXT5 = 342KB (see?)
1024x1024 DXT1 = 682KB
1024x1024 DXT5 = 1.33MB (see again - you get the point)

Using a combination of what you now know, figuring out the image format based on the HMX header suddenly becomes not so difficult :-)

ADVANCED TIP = Rock Band: Blitz and the Dance Central games were observed to vary the first 5 bytes of the header for otherwise identical files. You can still copy and use the 16 bytes as instructed above, but you will get greater coverage if you skip the first 5 bytes and only copy the other 11 to your header file. See the existing BLITZ_ and DC3_ headers for an example.

Hope this is helpful.