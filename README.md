# RIPA
Racial and Identity Profiling Act (RIPA - AB953)

The San Diego County Sheriff's Department is openly developing this application to share and collaborate with California LE community at large.

The RIPA App is a mobile first web application for phones and MDCs to capture stop information directly in the field. It is completely stand-alone and has no third party system integrations.

## Feature Highlight

* Semi offline
  * Cached lookups
  * Stop in progress
* Templates
* Pull forward on multiples
* Reverse geocoding
* Instrumentation
* Dark Mode
* Regulation reference links
* JSON documents

## Requirements

The application is built using MSFT ASP.NET MVC/WEBAPI with Entity Foundation and ReactJS views. 

* IIS (SSL is recommended)
* SQL Server 2016
* Visual Studio 
* Windows Authentication

## How To Get Started

1. **Database Setup** - On the package manager console in VS, add a new migration `add-migration myMigration` and update the database `update-database`, which you will need to point to in the web.config file. Alternatively, if you don't want to mess with migrations, you can implement the SQL script in the SQL folder.

2. **IIS Setup** - Set up a new web application, turn on Windows Authentication and set up your port 443/https bindings, a dev cert will suffice, unless you are setting up production. Make sure the application pool identity has read/write access to the database you set up in step 1.
