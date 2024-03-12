# OS2valghalla-3
Completely re-written version of OS2valghalla.

Documentation is available at: [https://os2valghalla.readthedocs.io/en/latest/](https://os2valghalla.readthedocs.io/en/latest/) or [OS2valghalla 3 documentation](https://github.com/OS2Valghalla/OS2valghalla-3-documentation) if you prefer to stay in GitHub.

See [OS2valghalla 3 documentation for contributing](https://github.com/OS2Valghalla/OS2valghalla-3-documentation/blob/main/docs/source/contribution/index.rst)

## Development guideline
### Installation
Recommended IDEs:
- Frontend development: [VSCode](https://code.visualstudio.com/)
- Backend development: [Visual Studio](https://visualstudio.microsoft.com/)

Other installations:
- [RabbitMQ](https://www.rabbitmq.com/)
- [PostgreSQL](https://www.postgresql.org/)

You need to install all NPM packages in Valghalla.Internal.Web folder and Valghalla.External.Web folder (use any package manager you prefer, for example using NPM command line: `npm install`). Nuget packages can be restored directly in Visual Studio.

### Application settings
There are 3 applications we need to configure app settings for development: Valghalla.Internal.API, Valghalla.External.API and Valghalla.Worker. Most of these files are similar to each other as they're configured for local hosting domain.
The main application settings file is in Environment folder at the top of source control, here you should clone `secrets.json` file to `secrets.development.json`.
The application authentication works based on SAML so you need to configure custom domain to your localhost.
For example in Windows OS, add custom domain to your host via `C:\Windows\System32\drivers\etc\hosts`.
#### secrets.development.json
- Queue: please configure like the one you setup in RabbitMQ
- Tenants: provide at least one local tenant for development, for example
```json
"Tenants": [
    {
      "Name": "Municipality (Dev)",
      "ConnectionString": "Host=localhost;port=5432;Database=DevDatabase;Username=dev;Password=dev",
      "InternalDomain": "localhost",
      "ExternalDomain": "localhost"
    },
  ]
  ```
- CPRService, Mail, SMS, DigitalPost: please contact providers to get the information and put in the correct places.
#### appsettings.development.json (Valghalla.Internal.API)
Please configure similar to these settings
```json
{
  "Urls": "https://<custom domain>:20002", // Valghalla.Internal.API port
  "Secrets": {
    "Path": "../../../../Environment/secrets.development.json" // Path to secrets.development.json file
  },
  "Authentication": {
    "IdPMetadataFile": "<path to metadata file>" // Path to metadata xml file
  },
  "AllowedOrigins": [
    "https://<custom domain>:4200", // Valghalla.Internal.Web port
    "https://<custom domain>:20002" // Valghalla.Internal.API port
  ],
  "AngularDevServer": "https://<custom domain>:4200", // Valghalla.Internal.Web port
  "HealthChecks-UI": {
    "HealthChecks": [
      {
        "Name": "Valghalla",
        "Uri": "https://localhost:7174/health"
      }
    ]
  }
}
```
#### appsettings.development.json (Valghalla.External.API)
Please configure similar to these settings
```json
{
  "Urls": "https://<custom domain>:20001", // Valghalla.External.API port
  "Secrets": {
    "Path": "../../../../Environment/secrets.development.json" // Path to secrets.development.json file
  },
  "Authentication": {
    "IdPMetadataFile": "<path to metadata file>" // Path to metadata xml file
  },
  "AllowedOrigins": [
    "https://<custom domain>:4600", // Valghalla.External.Web port
    "https://<custom domain>:20001", // Valghalla.External.API port
    "https://test-devtest4-nemlog-in.dk" // Test nem login domain
  ],
  "AngularDevServer": "https://<custom domain>:4600" // Valghalla.External.Web port
}
```
#### appsettings.development.json (Valghalla.Worker)
Please configure similar to these settings
```json
{
  "Secrets": {
    "Path": "../../../../Environment/secrets.development.json" // Path to secrets.development.json file
  },
  "Job": {
    "ConnectionString": "Host=localhost;port=5432;Database=DevJobDatabase;Username=dev;Password=dev" // Database for worker job
  }
}
```

### Start development
Make sure PostgreSQL server and RabbitMQ server are up and running then start the following applications in https:
- Valghalla.Internal.API
- Valghalla.External.API
- Valghalla.Worker

Then start following applications via command line `npm run start` (make sure to run the command in the correct folder):
- Valghalla.Internal.Web
- Valghalla.External.Web

You should see the addresses to access applications from console.