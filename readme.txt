Requirement:
=============================
- Windows 7 or later (Vista might be able to run, but no promise).
- .Net Framework 4.5
- log4net 1.2.11 (newkey)
- HtmlAgilityPack 

For linux/Mac, you can try to compile the source code using:
- Mono (http://www.mono-project.com/Main_Page)
- SharpDevelop (http://www.icsharpcode.net/OpenSource/SD/Download/)

The latest source code can be pulled from https://github.com/Nandaka/DanbooruDownloader.
The compiled binary can be downloaded at http://nandaka.devnull.zone/tag/danbooru-batch-download/
The previous version of compiled binary can be downloaded at https://github.com/Nandaka/DanbooruDownloader/releases

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
- HardLimit   : Hard limit defined by server for each request.
- PasswordSalt: choujin-steiner--%PASSWORD%-- for danbooru.donmai.us.
- UserName    : Your username.
- Password    : Your password, in plain text.
- LoginType   : Anonymous/UserPass/Cookie. If set to cookie, paste your full cookie information to UserName field.


Filename Format
=============================
- %provider% 	= provider Name
- %id% 		= Image ID
- %tags% 	= Image Tags
- %rating% 	= Image Rating
- %md5% 	= MD5 Hash
- %artist% 	= Artist Tag
- %copyright% 	= Copyright Tag
- %character% 	= Character Tag
- %circle% 	= Circle Tag, yande.re extension
- %faults% 	= Faults Tag, yande.re extension
- %originalFilename% = Original Filename
- %searchtag% 	= Search tag

If the given tags is not available, it will replaced with empty string.

Settings
=============================
- Application Settings:
“Minimize to System Tray” => Minimize to system tray :D
“Auto Focus Currrently Downloaded” => Move the selected row to the currently downloaded image in Download List tab.
“Enable Logging”   => Enable general logging.
“Use Colored Tags” => Enable colored tags in Main Tabs.
"Use Global Tags.xml" => Only use tags.xml as the tags source.
                      If disabled, it will try to load tags-{provider_name}.xml as the source tags.xml.

- Tagging:
“Artist” [5]     => Limit number of Artist tags to be used in the filename format for %artist%.
“Copyright” [5]  => Limit number of Copyright tags to be used in the filename format for %copyright%.
“Character” [5]  => Limit number of Character tags to be used in the filename format for %character%.
“Circle” [5]     => Limit number of Circle tags to be used in the filename format for %cirle%.
“Faults” [5]     => Limit number of Faults tags to be used in the filename format for %faults%.
                    Double click to change the tags color.

“Blacklisted tags” => Tags to be blacklisted in the Main Tab/Batch Job. 
                      It will shown with grey background in Main Tab.
                      It will be skipped in Batch Job.
                      Separate each tag with new line.
“Ignored Tags”     => Tags to be ignored in the filename.
                      Separate each tag with new line.
“Use Regex” => Enable regular expression for filter Blacklisted/Ignored tags.

“Use Tags Auto Complete #” [200] => Enable Autocomplete in Tag searchbox in Main tab.
                                    Up to 200 tags will be retrieved. This is based on your tags.xml.
“Empty Tag Repl.” [____] => For filename formatting, for example you use %artist% meta for filename format 
                            but the image doesn't have this information, then it will replaced based on this value.

file:included_tags.txt => If this file exists in the application folder, the tags defined in the file will be prioritized for filename.
                       Separate each tags with new line. Regular expression is supported.
                       Please note that it still depends on the filename format for ordering.

How to Use
=============================
A. Getting tags.xml:
1. Download tags.xml from Settings tab => Click update tags.xml
2. Select the provider from the URL combo box then press download, I recommend to use yande.re.
3. Wait untik the download complete.

B. To search:
1. Go to main tab, select one of the provider.
2. Key in the search term like you do in the website on the Tag text box, the other text boxes are just a shortcut for specific search term (e.g. rating, order, etc). Check the Search Help for syntax.
3. Make sure to tick Load Preview  in the Danbooru Listing group.
4. Click the Get button and wait until it finished download the list.
5. Click the '<' or '>' button to move between page.

C. To download individual image
1. Load the image list following the steps from section B.
2. Select the images by ticking the checkbox.
3. Click the Add button, it will show the Download List tab.
4. Set the Save Folder by pressing the browse button.
5. Click the Download button to start.
6. You can also add image by right clicking and select Add Selected Rows.

D. To do Batch Download
1. Go to Full Batch Mode.
2. Click Add Batch Job.
3. Key in the tag query, this follow the same rule like in the main tab.
4. Select the provider.
5. Press OK, the job will be added to the list.
6. Press Start Batch job to start the download.


FAQ
=============================
Q1: I cannot download from Danbooru (403 Forbidden)!
A1: Please read http://danbooru.donmai.us/forum/show/72300.
    You need to supply login information in the DanbooruProvider.xml and set UseAuth to true.

Q2: I cannot download/can only download 1 image from 3DBooru!
A2: On Settings tab, check Pad User Agent.

Q3: I cannot use space in batch download, the program always converting it to underscores!
A3: Use '+' for space.

Q4: I got 503 error from Danbooru!
A4: See A1. If still have the error, see this: http://danbooru.donmai.us/forum/show/24011.

Q5: I got 'ERROR_MESSAGE_HERE'!
A5: Sent me a message in the comment with the details, such as:
    - The Provider you are using.
    - The query.
    - The settings (screen shot is fine, please upload it to http://imgur.com/)
    - The error message (screen shot also fine)

Q6: I got a lot of skipped files when do batch download!
A6: It is caused of the target filename is already exists. Add %md5% in your filename format.

Q7: How to login to Gelbooru/Sankaku/get cookie value?
A7: Follow this step:
    1. Press F12 on your Chrome browser and select Network tab.
    2. Go to the booru site and login.
    3. Click one of the entry and copy the Cookie value from the Request Header. 
       For gelbooru, it should like this: user_id=<number>; pass_hash=<long string>
       For sankaku, login=<username>; pass_hash=<long string>;
    4. Paste the Cookie value to the Username field.
    5. Set Login Type to Cookie. Refer to http://i.imgur.com/rCCjnPs.png

Q8: Downloading tags.xml from Sankaku is too slow, what I can do?
A8: Sankaku doesn't allow to download the tags json/xml directly, so the application need to
    parse each individual tag pages, which over 16k pages. To avoid this, the application can
    reparse the tags again so it can get the actual tag type from the post page itself.
    To enable it:
    1. Disable "Use Global Tags.xml" in the settings page.
    2. The actual tags type will be displayed after the post is added to the download list 
       (together with image url resolution).
    3. For batch download, the tags will be reparsed just before the actual download.
    The disadvantage of this feature:
    1. Tags autocomplete might not working or show incorrect value, as it depends on local 
       tags.xml.
    2. Wrong tags type will be displayed before the image url is resolved, as it depends on 
       local tags.xml.

Q8: Sankaku only download maximum 500 images?
A8: You need to supply the login information, see Q7.


Supported Board
=============================
Currently only supporting Danbooru-type, Gelbooru-type with Danbooru API,
and Shimmie2 with RSS enabled.
For Sankaku-related board, using HTML parser.

- danbooru (http://danbooru.donmai.us)						NSFW
- Sankaku Complex (http://chan.sankakucomplex.com)			NSFW
- Sankaku Complex (Idol) (http://idol.sankakucomplex.com)	NSFW
- Konachan (http://konachan.com)							NSFW
- oreno.imouto.org (http://yande.re)						NSFW
- gelbooru.com (http://gelbooru.com)						NSFW	Cookie Login Required
- safebooru (http://safebooru.org)
- TheDoujin.com (http://thedoujin.com)						NSFW
- 3dbooru (http://behoimi.org)								NSFW	Cookie Login Required
- vectorbooru (ichijou.org)											Cookie Login Required
- e621 (http://e621.net)									NSFW, Furry
- rule34 (http://rule34.xxx)								NSFW
- dollbooru (http://www.dollbooru.org)
- 4chanhouse (http://shimmie.4chanhouse.org)
- fairygarden (http://www.tfgmush.org/shimmie2)
- megabooru (http://megabooru.com)
- hypnohub (http://hypnohub.net/)
- rule34.paheal.net (http://rule34.paheal.net)				NSFW
- rule34hentai.net (http://rule34hentai.net)				NSFW	Cookie Login Required for some image type.

License Agreement
=============================
Copyright (c) 2012, Nandaka
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

  - Redistributions of source code must retain the above copyright notice, this 
    list of conditions and the following disclaimer.
  - Redistributions in binary form must reproduce the above copyright notice, 
    this list of conditions and the following disclaimer in the documentation 
    and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR 
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS 
OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN 
IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
