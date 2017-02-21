# <img src="https://raw.githubusercontent.com/mrousavy/WebUntisSharp/master/Images/Logo.png" width="50"> WebUntisSharp
A Wrapper of the [WebUntis](http://www.untis.at/Downloads/int/Manuals/de/WebUntis.pdf) JSON API for .NET

<img src="http://sankt-ansgar-schule.de/wp-content/uploads/2016/08/WebUntis.png">

# How to use

###1. Create WebUntis Object
```C#
WebUntis untis = new WebUntis("mrousavy", "password1234", schoolUrl, "WebUntisSharp API");
```

```VB
Dim untis As WebUntis = new WebUntis("mrousavy", "password1234", schoolUrl, "WebUntisSharp API");
```

###2. Send Requests
```C#
var timegrid = await _untis.GetTimegrid();
```

```VB
Dim timegrid As Timegrid = await _untis.GetTimegrid();
```
