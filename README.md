# Calculator
Calculator web server, client and unit tests.

- JSON Web Server using WebApi
- Console Client
- Visual Studio 2015
- .NET Framework 4.5.2

# Usage
- CalculatorClient -s host -p port, example: "CalculatorClient -s localhost -p 8080"
- To interact with the program, simply type the operation and press enter, the client will request the operation based on the input syntax.

# Build
- From Visual Studio open "Calculator.sln", select the appropriate Solution Configuration and build the solution. NuGet dependencies will be automatically satisfied.
- To debug the projects, select the solution properties and, under the Common Properties -> Startup Project menu check "Multiple startup projects" and set to "Start" both Calculator and CalculatorClient Actions. Set debugging parameters for the client and click on Start to run both neccessary projects.

# Deployment
- **Client**: Build the release configuration and distribute the following files: CalculatorClient.exe, CalculatorClient.exe.config, CalculatorCommon.dll, log4net.dll, NDesk.Options.dll and Newtonsoft.Json.dll.
- **Server**: From the Visual Studio solution, right click the Calculator project and select "Publish". Under profile, create a Custom Profile and select the "File System" Publish Method. Click on the Browse button and, under the "Local IIS" section select an available IIS Express Site, it may be neccessary to create one from IIS Express.

# Testing
- From the Visual Studio solution open the Test Explorer, build the Calculator.Tests projects and Run All tests.

# Considerations
- Server log location can be changed at the log4net section of the Web.config under the "file" tag.
- IISExpress is automatically launched and hosts the server when debugging the application. The default configuration for IISExpress listens only to "localhost" requests, requests to "127.0.0.1" will return HTTP 400.

# External Libraries (Thanks!)
- log4net: [https://logging.apache.org/log4net/](https://logging.apache.org/log4net/)
- NDesk.Options: [http://www.ndesk.org/Options](http://www.ndesk.org/Options)
- Newtonsoft.Json: [http://www.newtonsoft.com/json](http://www.newtonsoft.com/json)