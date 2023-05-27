# KUID Format Specification

A KUID is an ID used to identify a particular Asset in Trainz. Standard human-readable format is: \<kuid:474195:10010\> or \<kuid2:474195:10010:1\> => General format: \<kuid2:UserID:ContentID:Version\>.

If version is 0, Trainz displays the kuid in legacy kuid format (\<kuid:) otherwise it will display it in kuid2 format (\<kuid2:)

It is represented internally on 64 bits as follows:

* 25 bits signed for the UserID (24 bits + 1 sign bit)
* 32 bits signed for ContentID
* 7 bits for Version

The first 32 bits combines the UserID and version, the last 32 bits are dedicated to the ContentID. Below is the representation on bits:

1. First 32 bits:

```
3        2        1        0      0
1....... 3....... 5....... 7......0
VVVVVVVS UUUUUUUU UUUUUUUU UUUUUUUU
V = Version range: 0-127
S = UserID sign 
U = UserID range: 0-16777215 (total range: -16777216 -> 16777215)
```

2. Last 32 bits:

```
3        2        1        0      0
1....... 3....... 5....... 7......0
SNNNNNNN NNNNNNNN NNNNNNNN NNNNNNNN
S = ContentID sign
N = ContentID range: 0-2147483647 (total range: -2147483648 -> 2147483647)
```

The bytes are always stored in little-endian. i.e. 101085(dec) = 00018ADD(hex) -> store as: DD 8A 01 00

## Kuid examples

* \<kuid:474195:100073\> will be stored as: 53 3C 07 00 E9 86 01 00
* \<kuid2:474195:100073:8\> will be stored as: 53 3C 07 10 E9 86 01 00

## Hashing

Hasing is done to quickly find the Asset file on disk. There are 256 hash buckets available. Hashing the kuid is XOR between all bytes, but with Version bits being 0. This means that all versions of the asset will have the same hash.  Here are some examples for bucket 00 and bucket 01:

```
hash-00
 51358 101085 0 - 9E C8 00 00 DD 8A 01 00
 60238  38192 0 - 4E EB 00 00 30 95 00 00
 66724    416 0 - A4 04 01 00 A0 01 00 00
506034 202011 1 - B2 B8 07 02 1B 15 03 00
568300 100040 2 - EC AB 08 04 C8 86 01 00
413547 101288 3 - 6B 4F 06 06 A8 8B 01 00
413547 101033 4 - 6B 4F 06 08 A9 8A 01 00

hash-01
 76656   21001 0 - 70 2B 01 00 09 52 00 00
276733  100680 0 - FD 38 04 00 48 89 01 00
328583    2698 0 - 87 03 05 00 8A 0A 00 00
400260 4742027 2 - 84 1B 06 04 8B 5B 48 00
236443  103312 1 - 9B 9B 03 02 90 93 01 00
```

## Exceptions

If UserID is negative, the Version bits should be invalid because the sign of the UserID will be extended on 32 bits rendering all Assets with negative UserID having version 127.

There are some exceptions that will occur only on overflow. For example, if the UserID is greater than 16777215, say 16777216, it will overflow, but the version bits will remain 0. Such a KUID will be rendered by Trainz as \<kuid2:-16777216:12345:0\> although it should be \<kuid:-16777216:12345\>.

In general, kuids are stored in the UUUVNNNN format, but mapfiles (\*.trc, \*.trk, \*.gnd...) and assets.tdx store kuids in NNNNUUUV format, so the UserID+Version bytes are switched with the ContentID bytes (a-b-c-d:e-f-g-h -> e-f-g-h:a-b-c-d).

Example:

* In config.chump: \<kuid:474195:100073\> will be stored as: 53 3C 07 00 E9 86 01 00
* In profile.trk: \<kuid:474195:100073\> will be stored as: E9 86 01 00 53 3C 07 00
