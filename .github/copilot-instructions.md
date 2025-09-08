# Project Overview

This project is a web API application that provides image randomization and gallery services to be used on other devices like kiosks, image frames, etc. The application handles autoscaling images based on request parameters and serves them through a RESTful interface. The application supports CORS, SSL, and does not require any authorization. It is intended to be deployed in a Docker container.

## Folder Structure
- `src/`: Contains the source code of the application.
- `tests/`: Contains unit tests for the application.
- The Visual Studio solution file is located at the root of the repository.
- The `Dockerfile` is located at `src/KwazyPixels/Dockerfile`.

## Libraries and Frameworks
- The .NET 9 SDK is used for building the application.
- The NUnit framework is used for unit testing.
- Swagger is used for API documentation.