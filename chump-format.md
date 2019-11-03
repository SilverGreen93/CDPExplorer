# CHUMP File Format Specification (*.chump, *.cdp)

A chump file or cdp file is a binary coded file for storing Trainz Assets of configuration files in a compact format, easy to process by the game.

A file with the chump extension is simply a packed config.txt file, whereas a cdp file can contain more assets and files beyond the config.txt. Note that there is no difference in format between the two.

The file format specification was determined by reverse-engineering and does not reflect any data provided by Auran/N3V.

All data is stored in little-endian (least significant byte first).

## File header

* 4 bytes: File ID: Always ACS$ (41 43 53 24)
* 4 bytes: Version: Always 01 00 00 00
* 4 bytes: Reserved: Always 00 00 00 00
* 4 bytes: File length (UInt32) - how many bytes are left to read until EOF is reached e.g. 32 00 00 00 (50 more bytes until EOF)

## File contents

After the header, the file contents begins:

* For Each Tag:  
  * 4 bytes: Tag length e.g. 0D 00 00 00 (13 bytes) - including sub-tags if it is a container  
  * 1 byte: Tag name length => e.g. 05 (5 bytes including null terminator)  
  * max 254 bytes: Tag name string => e.g. name
  * 1 byte: Null terminator => 00
  * 1 byte: Tag value type => e.g. 03 (string)
  *	If Tag value type = container
    * repeat For recursively, parse sub-tags
  * Else
    * n bytes: Tag value/data (see exmamples below)
  * End If
* End For
						
### Tag value types

There are 7 different types of data that a tag can store. This is indicated by the Tag type byte and can be one of the following:

* 00 = CONTAINER => contains more sub-tags
* 01 = INTEGER => 4 bytes or an array of 4 bytes until tag length reached
* 02 = FLOAT IEEE-754 => 4 bytes or an array of 4 bytes until tag length reached
* 03 = STRING => null terminated string
* 04 = BINARY => for embedding files in CDP (meshes, textures etc.)
* 05 = NULL => empty tag (no value)
* 0D = KUID => 8 bytes kuid in UUUVNNNN format (see [KUID Format Specification](kuid-format.md))

### Example of tags:

* 0D 00 00 00 05 n a m e 00 03 m i h a i 00
  * 0D 00 00 00 = total tag length is 13 bytes
  * 05 = the tag name is 5 bytes (including null terminator)
  * n a m e 00 = the tag name itself: name
  * 03 = tag value type: string
  * m i h a i 00 = the tag value: mihai
* 0F 00 00 00 05 k u i d 00 0D 53 3C 07 00 E9 86 01 00
  * 0F 00 00 00 = total tag length is 15 bytes
  * 05 = the tag name is 5 bytes (including null terminator)
  * k u i d 00 = the tag name itself: kuid
  * 0D = tag value type: kuid
  * 53 3C 07 00 E9 86 01 00 = kuid in UUUVNNNN format (\<kuid:474195:100073\>)
* 0A 00 00 00 04 i n t 00 01 22 1E D8 0A
  * 0A 00 00 00 = total tag length is 10 bytes
  * 04 = the tag name is 4 bytes (including null terminator)
  * i n t 00 = the tag name itself: int
  * 01 = tag value type: integer
  * 22 1E D8 0A = tab value: 181,935,650


## Standard CDP File Structure

This is an example of CDP file in user-readable format.

```
assets
{
	<kuid:474195:100073>
	{
		username        "My Asset"
		kuid            <kuid:474195:100073>
		mesh-table
		{
            default
            {
                mesh            "test.im"
                auto-create     1
            }
		}
		compression     "LZSS"
		files
		{
			test.im     #-# binary-data #-#
		}
		dsid            "7248:1375824714"
	}
}
contents-table
{
	0       <kuid:474195:100073>
}
kuid-table
{
}
obsolete-table
{
}
kind            "archive"
package-version 1
username        "unknown"
```

There are 4 containers:
* **assets** - has a subcontainer for each asset that is packed in the cdp file.
  * Each subcontainer is named as the kuid of the asset in clear text.
  * The subcontainer has an exact copy of the config.txt of the asset that is packed.
  * At the end of the config file there is a **file** container which dumps all the files in the asset in binary format, compressed with the LZSS algorithm (tag **compression** is always "LZSS")
  * Optionally if the asset is *Original* (i.e. downloaded from DLS), there will be a **dsid** tag added with an internal Download Station ID for Trainz to recognize and install the asset as not locally modified.
* **contents-table** - this will list all the kuids contained in the CDP file exactly like a kuid-table container from any config.txt file.
* **kuid-table**, **obsolete-table** - always present, can be ignored.

In the end, there are 3 required tags:
* **kind** - must be "archive"
* **package-version** - always 1
* **username** - always "unknown"
