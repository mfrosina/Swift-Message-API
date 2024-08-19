# SwiftMessageAPI

This is a simple ASP.NET Core Web API that parses and stores Swift MT799 messages in an SQLite database. The API includes an endpoint for uploading Swift messages via a file upload.

## Features

- Upload Swift MT799 message files via Swagger UI.
- Parse Swift messages into structured data.
- Store parsed data in an SQLite database.

## Prerequisites

- [.NET SDK 6.0 or later](https://dotnet.microsoft.com/download/dotnet)
- [SQLite](https://www.sqlite.org/download.html)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

## Getting Started

Follow these steps to set up and run the project.

### 1. Clone the repository

```bash
git clone https://github.com/mfrosina/SwiftMessageAPI.git
cd SwiftMessageAPI
```

### 2. Install Dependencies

Ensure you have the .NET SDK installed on your machine. You can check this by running:

```bash
dotnet --version
```

If not installed, download and install it from [here](https://dotnet.microsoft.com/download).

### 3. Build the Project

Navigate to the project directory and run the following command to restore the dependencies and build the project:

```bash
dotnet build
```

### 4. Run the Application

You can run the application locally using the following command:

```bash
dotnet run
```

The application will start and should be accessible at `http://localhost:<port>`.

### 5. Access Swagger UI

Once the application is running, open a browser and navigate to:

```
http://localhost:<port>/swagger
```

Replace `<port>` with the actual port number shown in the console output if you are not redirected directly.

### 6. Upload a Swift Message File

In the Swagger UI, find the `POST /api/SwiftMessage/upload` endpoint. Use the `Try it out` button to upload a Swift MT799 message file. The file content will be parsed, logged, and stored in the SQLite database.

