# Appinno 🔥
News website with admin panel (Website & WebService(SOAP) With DB)

:star: Star me on GitHub — it helps!

[![Ask Me Anything !](https://img.shields.io/badge/ask%20me-linkedin-1abc9c.svg)](https://www.linkedin.com/in/SoheilaSadeghian/)
[![Maintenance](https://img.shields.io/badge/maintained-yes-green.svg)](https://github.com/SoheilaSadeghian/SoheilaSadeghian.github.io)
[![Ask Me Anything !](https://img.shields.io/badge/production%20year-2016-1abc9c.svg)]()

## Overview
This repository (Appinno) includes two projects:

✔️ Appinno Website project as a client in root of repository(appinno_website Folder)\
    website includes modules like News, Events, Publications, Downloads, Organizations and Charts.\
    "Appinno Website" is a website made with ASP.NET Web Forms Technology using C#, SQL, HTML, CSS, JavaScript, Bootstrap, JQuery, Ajax and EntityFramework.\
    this project use Web Service for read operation as client.

✔️ Web Service project with Admin Panel in root of repository(appinno_panel_webservice Folder)\
    Admin Panel use Web Service for CRUD operations.\
    this project is a Web Service(SOAP) shows how to implement Web Services with C#, .Net Framework 4.5.\
    website and panel use web service as client.

## Tools Used 🛠️
*  Visual studio app,Sql server app
*  ASP.NET Web Forms, WebService(SOAP), .Net Framework 4.5, C#, SQL, HTML, CSS, JavaScript, Bootstrap, JQuery, Ajax

## Installation Steps 📦 
1. Restore DB in SQL Server from the DB file in root of repository (AppInnonew.bak.sql)<br/>
2. Open Web Service Solution in Visual Studio and build the project <br/>
3. Execute (F5) to run. Browser will show Homepage of website panel.(the picture of homepage is end of this readme) user:'sa',pass:'sa'<br/>
4. Open Appinno Website Solution in Visual Studio and build the project <br/>
3. Execute (F5) to run. Browser will show Homepage of website.(the picture of homepage is end of this readme)<br/>


### How call a web service Method:
```c#
 public static string CallMethod( string method, Dictionary<string, string> Parameters)
{
    string webServiceURL = "http://localhost:21457/service/userservice.asmx/" + method;
    byte[] dataStream = CreateHttpRequestData(Parameters);

    WebRequest request = WebRequest.Create(webServiceURL);
    request.Method = "POST";
    request.ContentType = "application/x-www-form-urlencoded";
    Stream stream = request.GetRequestStream();
    stream.Write(dataStream, 0, dataStream.Length);
    stream.Close();
    WebResponse response = request.GetResponse();
    Stream respStream = response.GetResponseStream();
    StreamReader reader = new StreamReader(respStream);

    string json = reader.ReadToEnd();
    return json;
}
```
## List of some requests in webservice files:

 ### User service [asmx file](https://github.com/soheilasadeghian/Appinno/blob/main/appinno_panel_webservice/AppinnoNew/service/userservice.asmx.cs)
getAllEvents,getAllMessage,getBestIdeaCompetition,getChart,getChartCommentList,getCreativityCompetition,getDaysWithEvent,getDownload,getDownloadCommentList,getEventCommentList,getEvents,getEventsInMonthForList,getIcan,getIcanCommentList,getIdea,getIo,getIoCommentListgetLatestIoForList,getLatestIoForNotification,getLatestMyIranCompetitionForList,getLatestNewsForList,getLatestNewsForNotification,getLatestPubForList,getLatestPubForNotification,getNews,getNewsCommentList,getPartnerForList,getPolicy,getPoll,getPollList,getPollResult,getPub,getPublicationCommentList,getSingleDownload,getSingleEvents,getSingleIcan,getSingleIo,getSingleNews,getSinglePub,getTagList

deleteDownloadContent,deleteEventContent,deleteIOContent,deleteIcanContent,deleteIdea,deleteIdeaContent,deleteNewsContent,deletePublicationContent,deleteReportContent

editDownload,editDownloadContent,editEvent,editEventContent,editIO,editIOContent,editIcan,editIcanContent,editIdea,editIdeaContent,editNews,editNewsContent,editPublication,editPublicationContent,editReport,editReportContent

isExistUser,likeUnlikeDownload,likeUnlikeEvents,likeUnlikeIo,likeUnlikeNews,likeUnlikePub

registerAnswer,registerAnswerContent,registerDownloadContent,registerEvent,registerEventContent,registerIO,registerIOContent,registerIcan,registerIcanContent,registerIdea,registerIdeaContent,registerNews,registerNewsContent,registerPublication,registerPublicationContent,registerReport,registerReportContent,registerResponse,registerResponseContent,registerUser,registermessage,registertag

resetPassword,sendChartComment,sendDownloadComment,sendEventComment,sendIcanComment,sendIoComment,sendNewsComment,sendPubComment,sendValidationCode
 
 ### Upload service [asmx file](https://github.com/soheilasadeghian/Appinno/blob/main/appinno_panel_webservice/AppinnoNew/service/uploadService.asmx.cs)
UploadFile,uploadPacket,uploadRequest

 ### Push service [asmx file](https://github.com/soheilasadeghian/Appinno/blob/main/appinno_panel_webservice/AppinnoNew/service/pushservice.asmx.cs)
sendPush,sendPushTo,setPushInfo

## Appinno Website:
![alt text](https://github.com/soheilasadeghian/Appinno/blob/main/screenshot.png)

## Appinno panel:
![alt text](https://github.com/soheilasadeghian/Appinno/blob/main/panel_home.PNG)

## License
[MIT](https://github.com/soheilasadeghian/Appinno/blob/main/LICENSE)

## Support
For support, [click here](https://github.com/soheilasadeghian).

## Give a star ⭐️ !!!
If you liked the project, please give a star :)



