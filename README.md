GlassGetBitcoinPrice
====================

This is a Xamarin.Android port of GCBP at https://github.com/mr-sk/GlassGetBitcoinPrice

GlassGetBitcoinPrice (GGBP) is a simple Glass application to get the current Bitcoin trading price in dollars. 

![image](https://www.glass-community.com/t5/image/serverpage/image-id/2561i3F098FC35FF82AB9/image-size/large?v=mpbl-1&px=-1)

Introduction
------------

GGBP is a very simple Glass application that responds to a single voice trigger. It will fetch the
exchange rates from bitstamp.netusing a GET request and extract the "last" value. 

The request is executed using async/await and when complete executes a preset callback
which updates the prices and utilizes the Text-To-Speech (TTS) capabilies of Glass to read
the price to the user. 

Note that you will need ServiceStack.Text as a reference since it is used to parse the JSON.

Installation
------------

To build and install GlassGetBitcoinPrice, you will need Xamarin.Android.

Usage
-----

GGBP uses GDK voice triggers. Supported commands are:

* `get bitcoin price`

