## **Web API Project**

This project is a demonstration of a .NET Core 8.0 Web API implementation that includes several essential features for robust API management and extensibility.

## **Features**

### 1. Log All API Requests and Responses
- Logs the request and response bodies for all API calls, including external API requests.
- Implementation leverages `ILogger` to provide detailed and structured logs.
- Useful for debugging and monitoring API interactions.

### 2. Error Handling
- Centralized error-handling mechanism to capture and handle exceptions gracefully.
- Ensures consistent API responses with detailed error messages.
- Includes logging of errors for further investigation.

### 3. Swagger-UI Integration
- Provides interactive API documentation for developers to test endpoints directly.
- Swagger-UI setup follows OpenAPI specifications.
- Simplifies onboarding for new developers and API consumers.

### 4. Multi-Language Support
- Implements internationalization (i18n) to support multiple languages in the API responses.
- Utilizes resource files (`.resx`) for managing translations.
- Automatically detects the preferred language from the request headers.

## **Getting Started**

### **Prerequisites**
- .NET 8.0 SDK

### Database Setup

1. Open your preferred SQL client (e.g., SQL Server Management Studio, Azure Data Studio).
2. Run the SQL script located at `Scripts/DatabaseSchema.sql` to set up the database schema and seed data.

```bash
sqlcmd -S <ServerName> -d <DatabaseName> -i Scripts/DatabaseSchema.sql

### **Swagger-UI**
- Navigate to `http://localhost:<port>/swagger` to access Swagger-UI.

### **Logging**
- Logs are written to the console and a file (if configured) using `ILogger`.
- Configure the logging settings in `appsettings.json`.

### **Error Handling**
- Global error handling middleware catches unhandled exceptions.
- Errors are logged, and the API responds with a standardized error structure.

### **Multi-Language Support**
- Languages supported: English (default), Mandarin Chinese, and more.
- To test, include an `Accept-Language` header in the API request:
   ```http
   Accept-Language: zh-CN
   ```

## **Running in Docker**
*Note: Docker support is not yet implemented for this project. This section is a placeholder for future updates.*

Once Docker support is added, you will be able to build and run the project using the following commands:
1. Build the Docker image:
   ```bash
   docker build -t webapi-project .
   ```
2. Run the container:
   ```bash
   docker run -p 5000:5000 webapi-project
   ```

## **Future Enhancements**
- Add rate-limiting to prevent abuse.
- Extend multi-language support with additional translations.

## **License**
This project is licensed under the MIT License.
