# D&D 5E Equipment Database Builder
This console app loads and organizes D&D 5th Edition equipment data into an SQL database. It pulls item info from JSON files provided by the D&D 5E API, transforming it into a format that's easy to manage for other applications.

## Features
- Parses equipment data from the [DND 5E API](https://www.dnd5eapi.co/)
- Organizes data into a SQL database using Entity Framework Core
- Handles nested data like magic item variants and weapon properties

## Getting Started

### Prerequisites
- .NET 6.0 SDK
- SQL Server (or any other database supported by EF Core)

### Installation on Windows
1. Install [Visual Studio Code](https://code.visualstudio.com/download)

2. Install [Microsoft SQL Server](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)


### Installation on Linux
1. Clone the repository:
```
git clone https://github.com/Dtlvoigt/Equipment-Database-Populator-5E.git
```

2. Navigate to the project directory and update appsettings.json with your database connection:
```
cd Equipment-Database-Populator-5E
```

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_connection_string_here"
  }
}
```

3. Install the dotnet sdk:
https://learn.microsoft.com/en-us/dotnet/core/install/linux

4. Run the console app:
```
dotnet run
```

## Schema
![Schema](https://github.com/Dtlvoigt/Equipment-Database-Populator-5E/blob/master/Schema.png "Database Schema")

## Usage
This app will pull data from the API, process the JSON files, and populate the database. Each item is transformed into a structured format, with relationships between items, weapon properties, and magic item variants managed automatically.

## Querying equipment data
### Searching for weapons with weapon properties
```
SELECT
	w.Name as WeaponName,
	wp.Name as WeaponProperty,
	wp.Description as Description
FROM 
	Equipment w
INNER JOIN
	EquipmentWeaponProperties ewp ON w.Id = ewp.EquipmentId
INNER JOIN
	WeaponProperties wp ON ewp.WeaponPropertyId = wp.Id
ORDER BY
	w.Id, wp.Id
```
![Weapon property query results](https://github.com/Dtlvoigt/Equipment-Database-Populator-5E/blob/master/WeaponPropertiesQuery.png "Weapon Query")

### Searching for magic items with variants
```
SELECT
	e.Name as ParentItem,
	v.Name as VariantItem
FROM 
	Equipment e
INNER JOIN
	Equipment v ON e.Id = v.ParentEquipmentId
ORDER BY
	e.Id, v.Id
```
![Variant items query results](https://github.com/Dtlvoigt/Equipment-Database-Populator-5E/blob/master/VariantItemsQuery.png "Variants Query")

### Searching for pack items and the equipment they contain
```
SELECT
	p.Name as PackItem,
	c.Name as Contents,
	pc.Amount
FROM 
	Equipment p
INNER JOIN
	PackContents pc ON p.Id = pc.PackId
INNER JOIN
	Equipment c ON c.Id = pc.ContentId
ORDER BY
	p.Id, c.Id
```
![Pack items query results](https://github.com/Dtlvoigt/Equipment-Database-Populator-5E/blob/master/PackContentsQuery.png "Packs Query")

## Differences from the API Database
- Magic and non-magic items are combined into a single table
- Magic items and their variants are now represented as 'Parent' and 'Child' items 

## Contributing
Feel free to submit pull requests or open issues if you find any bugs or want to suggest features.

## License
This project is licensed under the MIT License.
