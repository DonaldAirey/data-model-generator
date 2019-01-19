# Data Model Generator

## What is Data Model Generator?

The **Data Model Generator** creates a fast, write-through cache that augments the **Entity Framework** ORM.  It also generate a *RESTful API* for accessing and managing your data on the web.  ORM products
are great for reading or writing, but fail catostrophically when asked to do both.  In just minutes, you can have a cached, high-performance RESTful API that's fast enough to handle any database task you have and a
robust web interface. 

Maintaining a complex data model and the API is a snap.  You describe your tables and relations with an XSD Schema file (very similar to how **Visual Studio** *DataSets* are managed).  The tools will automatically
generate a multi-user, in-memory database for you backed by an Entity Framework store.  Additionally, you can use the **REST Generator** to automatically create a full-featured RESTful API for .NET Core MVC or
MVC6 to access your data model from the web.
 
Advanced engineers will find a fully-featured set of primitives for building smart transactions which can significantly reduce the stress on your database applications.  The in-memory data model will act like a traffic cop
to your relational database back-end and allows you to program around scenarios that would deadlock an ordinary ORM.  With the proper design, you can expect to reach the maximum TPS rating on your database
servers even with heavy real-time reads on your front end.
 
 The **Data Model Generator** is the perfect compliment to **Entity Framework**.  New and existing designs will benefit from the power of automatically generated DbSets and their relations.  You'll no longer
 have to maintain your RESTful API by hand and you get the power and speed of a multi-user, in memory database.

## Project Wiki

More details about our project, like [how to build an API](https://github.com/GammaFour/data-model-generator/wiki/Step-by-Step-Guide-to-Building-an-API), are located in our our [project wiki](https://github.com/GammaFour/data-model-generator/wiki/).

## Keeping it Real

The current version is the result of several years of research into the subject of augmenting ORM efficiency.

## Contact Info
You can contact the author(s) at info@gammafour.com.