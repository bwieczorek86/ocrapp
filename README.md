# Application 

The main task of the application is to assign the OCR scan of the letter to the client's system. The letter is searched based on previously prepared templates in which we can mark permanent fields in which we can find unique data that can be associated with a specific record in the client's system.

## Libraries and Cloud Integrations

The application is based on Azure cloud solutions:

- App Service
- Azure Functions
- Blob Storage
- Queue Storage
- Cosmo DB
- Bing Spell Checker

Authorization and authentication was done using JWT Bearer Token

The commercial IronOCR library was used to OCR documents

## Instalation

npm -i -g azure-functions-core-tools@3

# OcrPlugin 

All commands should be run on project: OcrPlugin.App.Blazor

dotnet ef migrations add update-db-7 --project ..\\OcrPlugin.App.Db\\OcrPlugin.App.Db.csproj \
dotnet ef database update

# Run
az login
func azure functionapp fetch-app-settings ocr-plugin

# Deploy Azure Functions

1. Deploy to folder -> use portable setting
1. Copy polish tessdata from anywhere in the disc to: `ocrplugin\DotNetCode\OcrPlugin.App.Functions\bin\Release\net7.0\publish\runtimes\win-x64\native\tessdata`
1. while there, you can remove all runtimes from the `runtimes` folder except for `win-x64`
1. Pack the whole folder:
    - go to the `publish` folder in your 'release' folder
    - select all files
    - pack them to .zip file
1. Go to https://appName.scm.azurewebsites.net/ZipDeployUI
1. Drop the zip file to the UI window
1. Wait for it to finish, done


# TODO

- create setting to use database to mark files that has been ocred or use folders 
- log functionality used by user so we can know what they use
- group 
- types templates
- OcrPlugin.App.Integrations
- Delete temp files from wwwroot/files/temp folder when editing template it gets path with template file and copy to load in view

// https://docs.microsoft.com/en-us/rest/api/storageservices/understanding-the-table-service-data-model

# How to add a new template type 
- create the type in the namespace OcrPlugin.App.Core.Templates.TemplateTypes
- create typed sanitizer in the namespace OcrPlugin.App.Core.TextSanitizing