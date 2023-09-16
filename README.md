# Carboy 🔥
A Restful API for a carboy mobile application Using C# and SQL

:star: Star me on GitHub — it helps!

[![Ask Me Anything !](https://img.shields.io/badge/ask%20me-linkedin-1abc9c.svg)](https://www.linkedin.com/in/SoheilaSadeghian/)
[![Maintenance](https://img.shields.io/badge/maintained-yes-green.svg)](https://github.com/SoheilaSadeghian/SoheilaSadeghian.github.io)
[![Ask Me Anything !](https://img.shields.io/badge/production%20year-2019-1abc9c.svg)]()

## Overview

✔️ This project is a web service made with Restful API Technology using C#, SQL, .Net Framework 4.5\
    This web service can be used for any type of client(web app, android, IOS)\
    The database file is also attached to the repository.

 ### *This project is part of the web service used in Carboy system

In the following, I will explain the big project of Carboy.

## Carboy system Modules:
* carboy web service: *(this repository is this project)
webservice for repairmans applications(client of api)
* custumer web service:
web service for Customers(who place orders) application(client of api) 
* web panel for admin to manage system
* android app for customers
* ios app for customers
* web app for customers
* android app for repairmans
* ios app for repairmans

## System features
* zarinpal payment gateway 
* Connecting to Mellat Bank customer club web service
* Give GiftCard for Customer
* Implementation of an algorithm to prevent synchronization of services

## Tools Used 🛠️
*  Visual studio app,Sql server app
*  Restful API, C#, SQL

## Installation Steps 📦 
1. Restore DB in SQL Server from the DB file in root of repository (CarBoyDBLive.bak)<br/>
2. Open Web Service Solution in Visual Studio and build the project.<br/>
3. Execute (F5) to run. Browser will throw error page which is fine as this is only WEB-API implementation.<br/>
4. You can test the API using a tool such as Unit Test Project in repository.

### Implementation description:
 in [CoreController](https://github.com/soheilasadeghian/Carboy/blob/main/CarboyWebService/Controllers/CoreController.cs) call API method

### List of [web services](https://github.com/soheilasadeghian/Carboy/blob/main/CarboyWebService/Engine.cs) :
 getUserProductListConstructor\
 getCarboyPathConstructor\
 carboyMoveToCustomerConstructor\
 carboyStartServiceConstructor\
 carboyServiceProductDeliveredConstructor\
 ...

### There is some Database Diagrams:
![alt text](https://github.com/soheilasadeghian/Carboy/blob/main/DB%20Diagrams/CarDiagram.PNG)

![alt text](https://github.com/soheilasadeghian/Carboy/blob/main/DB%20Diagrams/CustomerDiagram.PNG)

![alt text](https://github.com/soheilasadeghian/Carboy/blob/main/DB%20Diagrams/ServiceDiagram.PNG)

## License
[MIT](https://github.com/soheilasadeghian/Carboy/blob/main/LICENSE)

## Support
For support, [click here](https://github.com/soheilasadeghian).

## Give a star ⭐️ !!!
If you liked the project, please give a star :)



