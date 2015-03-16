DotNetThumbor - C# Thumbor client by Mi9
========================================

DotNet C# client for the [Thumbor image service][1] which allows you to build URIs 
using a fluent API. This is loosly based on the Pollexor Java Library.

Download
--------

Download via Nuget:

https://www.nuget.org/packages/DotNetThumbor.dll/

Or build locally and import DLL into project.

Examples
--------

```C#
// Without key signing:
var thumbor = new Thumbor("http://example.com/");

// With key signing:
var thumbor = new Thumbor("http://example.com/", "key");
```

```C#
thumbor.BuildImage("http://example.com/image.png")
       .Resize(48, 48)
       .ToUrl()
// Produces: http://example.com/unsafe/48x48/example.com/image.png

thumbor.BuildImage("http://example.com/image.png")
       .Crop(10, 10, 90, 90)
       .Resize(40, 40)
       .Smart()
       .ToUrl()
// Produces: http://example.com/unsafe/10x10:90x90/smart/40x40/example.com/image.png

thumbor.BuildImage("http://example.com/image.png")
       .Crop(5, 5, 195, 195)
       .Resize(95, 95)
       .HorizontalAlign(Thumbor.ImageHorizontalAlign.Right)
       .VerticalAlign(Thumbor.ImageVerticalAlign.Bottom)
       .ToUrl()
// Produces: http://example.com/unsafe/5x5:195x195/right/bottom/95x95/example.com/image.png

thumbor.BuildImage("http://example.com/background.png")
       .Resize(200, 100)
       .RoundCorner(10),
       .Watermark("http://example.com/overlay1.png"), 0, 0, 25),
       .Watermark("http://example.com/overlay2.png"), 0, 100, 25),
       .Quality(85)
       .ToUrl()
// Produces: http://example.com/unsafe/200x100/filters:round_corner(10,255,255,255):watermark(http://example.com/overlay1.png,0,0,25):watermark(http://example.com/overlay2.png,0,100,25):quality(85)/http://example.com/background.png
```

*Note:* Thumbor 3.0 and older encryption/key signing is not supported.


License
=======

Copyright (c) 2015, Mi9
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of Mi9 nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


 [1]: https://github.com/globocom/thumbor
 