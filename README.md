# Crud Test

This is a CRUD test, it has basic creation, reading, updating, deletion controllers

# Installation

Required external software: Microsoft SQL Server, Redis

## Installing PostgreSQL
   1. Go to the [PostgreSQL download page](https://www.postgresql.org/download/windows/)
   2. Choose the version of PostgreSQL that you want to install, and click the corresponding "Download" link.
   3. Run the downloaded installer, and follow the prompts to install PostgreSQL on your machine.
   4. During the installation process, you will be prompted to set a password for the "postgres" user. Remember this password, as you will need it later.
   5. After the installation is complete, PostgreSQL should be up and running on your machine.

## Installing Redis

Redis is not officially supported on Windows.
However, you can install Redis on Windows for development by following the instructions below.

To install Redis on Windows, you'll first need to enable WSL2 (Windows Subsystem for Linux) which allows Linux binaries to run on Windows through a Linux VM.
For this method to work, you'll need to be running Windows 10 version 2004 and higher or Windows 11.

Once you're running Ubuntu on Windows, you can follow the steps detailed at Install on Ubuntu/Debian to install recent stable versions of Redis from the official packages.redis.io APT repository. Add the repository to the apt index, update it, and then install:

```
curl -fsSL https://packages.redis.io/gpg | sudo gpg --dearmor -o /usr/share/keyrings/redis-archive-keyring.gpg

echo "deb [signed-by=/usr/share/keyrings/redis-archive-keyring.gpg] https://packages.redis.io/deb $(lsb_release -cs) main" | sudo tee /etc/apt/sources.list.d/redis.list

sudo apt-get update
sudo apt-get install redis
```

Lastly, start the Redis server like so:
```
sudo service redis-server start
```
Connect to Redis
You can test that your Redis server is running by connecting with the Redis CLI:

```
redis-cli 
127.0.0.1:6379> ping
PONG
```


# Project Description

This project is built around ***asp.net core*** for backend and ***Vite*** for front-end and utilizes a number of technologies to provide a robust and scalable application.

# Known issues

## Back-end file layout
The file layout on the back-end contains 2 folders, CrudTest and CrudTestT, upon trying to merge these 2 folders I get an error which essentially breaks the entire application

### Swagger
Swagger is an open-source tool that simplifies the process of designing, building, and documenting APIs. It provides a user-friendly interface for developers to interact with APIs, and allows API providers to easily document their APIs.

### Caching and Ratelimiting
Redis is used for caching and ratelimiting. Redis is a popular, open-source, in-memory data store that provides fast, scalable, and flexible data caching and management capabilities.

### Job Scheduling
Hangfire is used for job scheduling, providing reliable and efficient background job processing capabilities.

### Logging
Serilog, is used for logging, providing comprehensive and customizable logging capabilities to monitor application performance and identify issues.


## Front-end technicaly specifications
 * Uses vite + react with typescript

### Front-end Apis

### GET /
 - Displays all items by sending a request to /api/items

### GET /page2/{itemCode}
 - Fetches all item details by item code and displays them

## Back-end technical specifications
 * Nearly all values come from the cache rather than database
 * Cache refreshes every 30m via hangfire
 * Any sort of errors or relative information are logged

## Back-end Apis

### GET - /api/items
 - Fetches all items we have on site from cache and returns a json array of their basic object

### POST - /api/create
 - Creates a new item, stores in database then caches it

 - Parameters
```cs
public record CreateItemRequest
{
    public required string Description { get; set; }
    public bool Active { get; set; }
    public string? CustomerDescription { get; set; }
    
    public bool SalesItem { get; set; }
    public bool StockItem { get; set; }
    public bool PurchasedItem { get; set; }
    
    public required string Barcode { get; set; }
    public int ManageItemBy { get; set; }
    
    public decimal MinimumInventory { get; set; }
    public decimal MaximumInventory { get; set; }
    
    public string? Remarks { get; set; }
    public required string ImagePath { get; set; }
}
```

### GET - /api/search/{itemName}
 - Fetches item with the nearest name from cache, For example if the item name is **Item1** and we search **Ite** it will still return all results that contain **"Ite"**

### GET - /api/fetch/{itemCode}
 - Fetches item from database by itemCode 

### PATCH - /api/items/{itemCode}/update
 - Searches item if found it updates the object with the new entites\

 - Parameters 
```cs
public record ModifyItemRequest
{
    public string? Description { get; set; }
    public bool? Active { get; set; }
    public string? CustomerDescription { get; set; }
    public bool? SalesItem { get; set; }
    public bool? StockItem { get; set; }
    public bool? PurchasedItem { get; set; }
    public string? Barcode { get; set; }
    public int? ManageItemBy { get; set; }
    public decimal? MinimumInventory { get; set; }
    public decimal? MaximumInventory { get; set; }
    public string? Remarks { get; set; }
    public string? ImagePath { get; set; }
}
```

### DELETE - /api/items/{itemCode}/delete
 - Fetches item from database if found deletes it then clears it from cache

### DELETE - /api/items/refresh-cache
 - Clears entire items cache then fetches from database and caches all items

## Database specifcations

To create the table we run

```sql
USE Crud; 

CREATE TABLE ItemMaster
(
    ItemCode NVARCHAR(25) NOT NULL PRIMARY KEY,
    Description NVARCHAR(300) NOT NULL,
    Active BIT NOT NULL,
    CustomerDescription NVARCHAR(300),
    SalesItem BIT NOT NULL,
    StockItem BIT NOT NULL,
    PurchasedItem BIT NOT NULL,
    Barcode NVARCHAR(100) NOT NULL,
    ManageItemBy INT NOT NULL,
    MinimumInventory DECIMAL(18, 2) NOT NULL,
    MaximumInventory DECIMAL(18, 2) NOT NULL,
    Remarks NVARCHAR(MAX),
    ImagePath NVARCHAR(MAX) NOT NULL
);

CREATE UNIQUE INDEX IX_ItemCode ON ItemMaster (ItemCode);
```