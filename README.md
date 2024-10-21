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

### Installation
1. Clone the repository:
```
git clone https://github.com/Dtlvoigt/Equipment-Database-Populator-5E.git
```

2. Navigate to the project directory:
```
cd Equipment-Database-Populator-5E
```

3. Set up your database connection in appsettings.json.
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_connection_string_here"
  }
}
```

4. Run the console app:
```
dotnet run
```

## Schema
![Schema](https://github.com/Dtlvoigt/Equipment-Database-Populator-5E/blob/master/Schema.png "Database Schema")

## Usage
This app will pull data from the API, process the JSON files, and populate the database. Each item is transformed into a structured format, with relationships between items, weapon properties, and magic item variants managed automatically.

## Querying objects in C#

## Differences from the API Database
- Magic and non-magic items are combined into a single table
- Magic items and their variants are now represented as 'Parent' and 'Child' items 

## Contributing
Feel free to submit pull requests or open issues if you find any bugs or want to suggest features.

## License
This project is licensed under the MIT License.
