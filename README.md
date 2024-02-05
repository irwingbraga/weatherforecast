
# Weather Forecast API

This API allows users to input and retrieve weather forecasts.

## Features

-   Creates a daily forecast based on provided input.
-   Lists weekly forecasts in user-friendly language.
-   Validates input values and provides meaningful error messages.
-   Swagger UI for API documentation and testing.
-   Unit tests for ensuring functionality.
-   Onion architecture with MediatR, EF Core, and other best practices.

## Getting Started

## Prerequisites

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Docker](https://www.docker.com/products/docker-desktop)

## Running the API Locally

### Using .NET CLI

1.  Navigate to the project root directory:
    
    `cd path/to/weatherforecast` 
    
2.  Restore the required packages:
    
    `dotnet restore` 
    
3.  Build and run the project:
    
    `dotnet run` 
    
### Using Docker

1.  Navigate to the project root directory:
    
    `cd path/to/weatherforecast` 
2.  Execute docker compose command:
    
    `docker-compose up -d` 
    

After running the above commands, the API should be accessible at `http://localhost:8080/`.

## API Documentation

After launching the application, you can access the Swagger UI documentation at `http://localhost:8080/swagger`.

### Using .NET CLI

1.  Navigate to the tests directory:
    
    `cd path/to/WeatherForecast.UnitTests` 
    
2.  Run the tests:
    
    `dotnet test` 
    

## Built With

-   .NET 7
-   MediatR
-   xUnit, FluentAssertions, AutoFixture for testing

## Contributing

For contributing, please create a new branch, implement your feature or bug fix, and create a pull request to the `main` branch.

## License

This project is licensed under the MIT License.