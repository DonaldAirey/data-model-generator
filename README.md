# Data Model Generator

## What is Data Model Generator?

The Data Model Generator creates a fast, write-through cache that augments the Entity Framework ORM.  It also generate the RESTful API for accessing and managing your data on the web.  It was motivated by a need to
access data quickly (on the order of microseconds) for real-time front end tasks while also supporting high volume transactions.  Existing products are good at reading or writing, but without a cache it will fail when asked
to do both.  In just minutes, you can have a fully-functional, high-performance cached REST API that's extensible enough to handle any task you have.

 You describe your tables and relations with an XMD Schema documentation (very similar to how Visual Studio DataSet are managed) and the tools will automatically generate a multi-user, multi-tasking, in-memory
 database for you.  Additionally, you can use the REST Genrator to automatically create a complete, full-featured API for .NET Core MVC or MVC6.  A major advantage of this tool is productivity: in just seconds, you
 can change columns or relations in your model and generate a complete REST API from that specification.  The tool creates partial classes, so it's easy to add custom verb handlers (Delete, Get, Post, Put) that work
 along-side the generated code.  This allows you to focus on the design of your data model and not on the implementation and maintenance.

## Project Wiki

More details about our project, like [how to build an API](https://github.com/GammaFour/data-model-generator/wiki/Step-by-Step-Guide-to-Building-an-API), are located in our our [project wiki](https://github.com/GammaFour/data-model-generator/wiki/).

## Contact Info
If you wold like to contact the author(s), please use [info @ GammaFour](mail:info@gammafour.com)