<p align="center">
  <img src="https://raw.githubusercontent.com/mrousavy/WebUntisSharp/master/Images/Logo.png" width="50" />
  <br/>
  <img src="http://sankt-ansgar-schule.de/wp-content/uploads/2016/08/WebUntis.png" height="100" />
  <h3 align="center">WebUntisSharp</h3>
  <p align="center">A Wrapper of the <a href="http://www.untis.at/Downloads/int/Manuals/de/WebUntis.pdf">WebUntis</a> JSON API for .NET for sending and receiving Untis Informations</p>
  <p align="center">
    <a href="https://github.com/mrousavy/WebUntisSharp/wiki"><img src="https://img.shields.io/badge/API-Documentation-green.svg" alt="Documentation"></a>
  </p>
</p>


# How to use

## Full guide
Take a look at the [wiki](https://github.com/mrousavy/WebUntisSharp/wiki) to see the full documentation for this API.

## Quickstart
### 1. Add Binaries
   + NuGet
      * [WebUntisSharp is also available on NuGet!](https://www.nuget.org/packages/WebUntisSharp)   Install by typing `Install-Package WebUntisSharp` in NuGet Package Manager Console. (Or search for `WebUntisSharp` on NuGet)

   + Manually
      1. [Download the latest Library (.dll)](https://github.com/mrousavy/WebUntisSharp/releases/download/1.0.0.3/WebUntisSharp.dll)
      2. Add the .dll to your Project   (Right click `References` in the Project Tree View, click `Add References` and `Browse` to the `.dll` File)

### 2. Create WebUntis Object
* C#:
```C#
WebUntis untis = new WebUntis("mrousavy", "password1234", schoolUrl, "WebUntisSharp API");
```

* VB:
```VB
Dim untis As new WebUntis("mrousavy", "password1234", schoolUrl, "WebUntisSharp API")
```

### 3. Send Requests
* C#:
```C#
var timegrid = await untis.GetTimegrid();
```

* VB:
```VB
Dim timegrid As Timegrid = Await untis.GetTimegrid()
```

### 4. Logout
* Logout Method
   * C#:
   ```C#
    untis.Logout();
    ```

   * VB:
    ```VB
   untis.Logout()
   ```
* using Statement
   * C#:
   ```C#
   using(WebUntis untis = new WebUntis("mrousavy", "password1234", schoolUrl, "WebUntisSharp API"))
      // our Requests to the WebUntis API go here
   }
   // WebUntis Object is now disposed and Logged out (Session has ended)
   ```

   * VB:
   ```VB
   Using untis As New WebUntis("mrousavy", "password1234", schoolUrl, "WebUntisSharp API")
      ' Your Requests to the WebUntis API go here
   End Using
   ' WebUntis Object is now disposed and Logged out (Session has ended)
   ```
