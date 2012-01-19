Changelog:
=============================
- DanbooruDownloader20120120
  - Add danbooru login information using the DanbooruProviderList.xml.
  - fix preview image parsing if using relative path.

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
