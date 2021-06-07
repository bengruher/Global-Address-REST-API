# REST API for Countries and Country Address Fields

Make sure you have the [latest .NET core](https://dotnet.microsoft.com/download) and an [editor of choice](https://code.visualstudio.com/Download).

## To build and run the API locally

```bash
dotnet restore
dotnet build

ASPNETCORE_ENVIRONMENT=Development dotnet run false
```

The above commands build and run the API. The system relies on a configuration file to enforce field formats. This config file, named addressConfig.json contains the names of each country and their address fields. It also contains regex strings specifying the required format for those fields. 

If there are significant changes to this configuration file, the DB will need to be rebuilt. To do this, run the API in the rebuild mode using the argument "true" instead of false. The command will look like:
```dotnet run true```. 

## API

As of now, the following methods are supported;
  - GET / 
      - Returns a list of country names
  - GET /{countryName} 
      - Returns the fields for a country in the form of a a dictionary (key: field name, value: field type)
  - GET /search/{countryName} 
      - Searches for addresses matching a set of parameters
      - Provide query parameters in the form of key, value pairs in the URI
      - Exaxmple: /search/Canada?province=Quebec
  - POST /{countryName} 
      - Adds an address to the database with the values specified in the parameters
      - Provide query parameters in the form of key, value pairs in the URI
      - Example: /search/Canada?name=John Jones,street=10-123 Main Street,city=Montreal,province=QC,postal code=H3Z 2Y7

## Swagger
This application uses Swagger for API Documentation. Check out the following link to interact with the API via the Swagger interface.
http://localhost:59969/swagger/index.html