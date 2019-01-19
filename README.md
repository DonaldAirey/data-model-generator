# Data Model Generator

## What is Data Model Generator?

The **Data Model Generator** creates a fast, write-through cache that augments the **Entity Framework** ORM.  It also generate the *RESTful API* for accessing and managing your data on the web.  The product was
motivated by a need to access data quickly (on the order of microseconds) for real-time front end tasks while simultaneously enabling high volume transactions.  Existing ORM products are good at reading or writing,
but fail catostrophically when asked to do both.  In just minutes, you can have a fully-functional, high-performance cached REST API that's extensible enough to handle any task you have.

 You describe your tables and relations with an XSD Schema file (very similar to how **Visual Studio** *DataSets* are managed) and the tools will automatically generate a multi-user, multi-tasking, in-memory
 database for you.  Additionally, you can use the REST Genrator to automatically create a complete, full-featured API for .NET Core MVC or MVC6.  A major advantage of this tool is productivity: in just seconds, you
 can change columns or relations in your model and generate a complete REST API from that specification.  The tool creates partial classes, so it's easy to add custom verb handlers (Delete, Get, Post, Put) that work
 along-side the generated code.  This allows you to focus on the design of your data model and not on the implementation and maintenance.
 
 Advanced engineers will find a fully-featured set of primitives for building smart transactions which can remove the stress from your database applications.  The in-memory cache will act like a traffic cop to your
 relational database and allows you to program around dead-locking scenarios.  With the proper design, you can expect to reach the maximum TPS rating on your database servers even with real-time reads on your
 front end.
 
 The **Data Model Generator** is the perfect compliment to the robust ORM features of **Entity Framework**.  The ORM provinces the heavy lifting of managing the persistent data and relations, and the write-through
 in-memory cache eliminates the need to deal with disk-based storage for reading.

## Project Wiki

More details about our project, like [how to build an API](https://github.com/GammaFour/data-model-generator/wiki/Step-by-Step-Guide-to-Building-an-API), are located in our our [project wiki](https://github.com/GammaFour/data-model-generator/wiki/).

## Contact Info
You can contact the author(s) at info@gammafour.com.