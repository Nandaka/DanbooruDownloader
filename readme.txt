Changelog:
=============================
- DanbooruDownloader201203xx
  - Fix batch download limit detection as reported by Q.

- DanbooruDownloader20120229
  - Add abort on error option for batch download as requested by Xemnarth.
  - Fix batch download stop button.
  - Add Pause/Resume batch download button.
  - Change Target Framework to .NET Framework 4.
  - Update DanbooruProviderList.xml.

- DanbooruDownloader20120203
  - Change rating to expanded mode for compatibility with gelbooru.
  - Fix bug when add batch job but no provider is selected.
  - Update batch job message.
  - Fix batch job limit detection.

- DanbooruDownloader20120120
  - Add danbooru login information using the DanbooruProviderList.xml.
  - fix preview image parsing if using relative path.
  - fix Gelbooru auto forward page hang (20120122).
  - Update DanbooruProviderList.xml to add board type (20120122).

- DanbooruDownloader20120108
  - Add proxy username/password support. 
  - Add select all provider button in add batch job dialog.
  - Updating progress message for batch download.
  - Fix proxy login support (20120110).

DanbooruProvider.xml
=============================
This file will be parsed when you run the applications. You can modify the xml to add new provider.
The contents structure are:
- Name	: The name of the *booru.
- Url	: The root path of the *booru url, ie:http://danbooru.donmai.us
- QueryStringJson : For danbooru based, use:/post/index.json?%_query%
		    For gelbooru, not supported/no API provided.
- QueryStringXml  : For danbooru based, use:/post/index.xml?%_query%
             	    For gelbooru based, use:/index.php?page=dapi&amp;s=post&amp;q=index&amp;%_query%
- Preferred   : Xml/Json.
- DefaultLimit: Number of limit if not given.
- HardLimit   : Hard limit from server for each request.
- PasswordSalt: choujin-steiner--%PASSWORD%-- for danbooru.donmai.us.
- UserName    : Your username.
- Password    : Your password, in plain text.
- UseAuth     : true/false. Use authentication/login.
