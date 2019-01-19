# Data Model Generator

The **Data Model Generator** is a set of code generation tools that augments the **Entity Framework** ORM.  The *ServerDataModelGenerator* creates an in-memory cache from an XSD description.  The
*RESTGenerator* creates a *RESTful API* Web Application (MVC) for accessing and managing your data from the web.  ORM products are great for reading or writing, but fail catostrophically when asked to do
both.  A write-through, in-memory cache solves this problem.  In just minutes, you can have a cached database that's fast enough to handle any task you have and a robust RESTful web interface to get at your
data.

Creating and maintaining a complex data model is a snap with **Data Model Generator**.  You describe your tables and relations with an XSD Schema file (very similar to how **Visual Studio** *DataSets*
are managed).  The tools will automatically generate a multi-user, in-memory database for you backed by an Entity Framework store.  The **REST Generator** automatically creates a full-featured RESTful API
for .NET Core MVC or MVC6 to access your data model from the web.
 
Advanced engineers will find a full-featured set of primitives for building *smart transactions* which can significantly reduce the stress on your database applications.  The in-memory data model will act like a
traffic cop to your relational database back-end and allows you to program around scenarios that would deadlock an ordinary ORM.  With the proper design, you can expect to reach the maximum TPS rating on
your database servers even with heavy real-time reads on your front end.
 
 The **Data Model Generator** is the perfect compliment to **Entity Framework**.  New and existing designs will benefit from the power of automatically generated Entities, DbSets and their relations.  You'll
 no longer have to maintain your RESTful API by hand and you get the power and speed of a multi-user, in-memory database.

## Project Wiki

More details about our project, like [how to build an API](https://github.com/GammaFour/data-model-generator/wiki/Step-by-Step-Guide-to-Building-an-API), are located in our our
[project wiki](https://github.com/GammaFour/data-model-generator/wiki/).

## Keeping it Real

The current version is the result of several years of research into the subject of augmenting ORM efficiency.  It has been designed with speed as the highest priority, funcionality second.  We're happy to
consider your functional requirements as we go foreward and we'd love hear your feedback and use cases.

## Contact Info
You can contact the author(s) at info@gammafour.com.