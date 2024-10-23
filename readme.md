# Degree Planner
A project made for 31927 Application Development with .NET

## Setup
This project uses an MSSQL localDB that needs to be configured prior to use by setting the connection string in appsettings.json. For the Canvas submission, a preconfigured localDB file is provided as db.mdf inside the db directory. To set the connection string, please follow these instructions:
1. Open Visual Studio
2. Open the Server Explorer either using either ctrl + alt + s or from the view menu
3. Click the connect to database button in the Server Explorer window.
4. In the Add Connection window, change the data source to "Microsoft SQL Server Database File (SqlClient)
5. Click the browse button next to Database file name and locate the db.mdf file inside the project's db directory
6. Click OK and the database should be added to the server explorer
7. Right click on the database in the server explorer and click properties
8. Copy the "Connection String" value from the properties window
9. Open appsettings.json and paste the connection string into the SQLDBConnection property

After configuring the database, its values can be reset at any time using the database reset page found at https://localhost:7231/database.