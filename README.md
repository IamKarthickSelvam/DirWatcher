# DirWacher
DirWatcher is a .NET Core application designed to monitor directories for changes and provide real-time updates via a REST API. It consists of a REST API server and a long-running background task that continuously monitors specified directories.

## Main Components
### 1. REST API Server
Exposes endpoints to configure directory monitoring settings and retrieve task run details.
### 2. Long Running Background Task
Monitors configured directories at scheduled intervals, tracks file changes, counts occurrences of a specified magic string, and persists results to the database.

## Features
### Directory Monitoring
Monitors directories for file additions, deletions, and changes.
### Magic String Detection
Counts occurrences of a specified magic string within monitored files.
### Dynamic Configuration
Allows configuration of monitored directories, monitoring intervals, and magic strings via REST API calls.
### Task Run Details
Provides detailed information about task runs, including start time, end time, total runtime, files added, files deleted, total magic string occurrences, and task status.

## Code Setup
### 1. Install Prerequisites
- .NET 8
  - Download the latest version of [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) SDK and install in your system
- MongoDB
  - MongoDB Atlas Cloud
    -  Open [Mongo DB Atlas](https://www.mongodb.com/cloud/atlas/register) on your browser
    -  Register/Login using your Gmail/Github account
    -  Deploy a free cluster from one of the three major cloud providers of your choice (GCP/AWS/Azure)
    -  Create user to access cluster
    -  Add your network's IP to your cluster's allowed IPs
    -  Connect to cluster
    -  Insert/View Documents
    -  [More detailed steps here](https://www.mongodb.com/docs/atlas/tutorial/create-atlas-account/)
  - Local DB Setup
    - Download MongoDB Community Edition installer from [here](https://www.mongodb.com/try/download/community)
    - Run the installer (.msi file)
    - Install MongoDB as a Windows Service
    - (Optional) Install [MongoDB Compass](https://www.mongodb.com/products/compass)- GUI to access DB
    - Install [Mongosh](https://www.mongodb.com/docs/mongodb-shell/) - MongoDB Shell
    - Add mongosh.exe binary to path enviroment after installation
    - Open Command Prompt or Terminal and enter 'mongosh.exe'
    - Insert/View Documents
- Code Editor
  - Install [Visual Studio 22 - Community Edition](https://visualstudio.microsoft.com/vs/)
  - Or Install [Visual Studio Code](https://code.visualstudio.com/)

### 2. Clone the repository
Clone the repository in your local using the clone URL inside **<> Code** button above or simply use the below command - `git clone https://github.com/IamKarthickSelvam/DirWatcher.git`

### 3. Navigate to directory
open terminal or cmd and run `cd DirWatcher` 

### 4. Restore Dependencies
Run `dotnet restore` command to install all nuget packages/dependencies

### 5. Setting up Connection String
The existing connection string in appsettings.json is configured to allow any IP addresses to connect to DB at the moment but I will be making the DB private in the near future so if you've tried the existing conn string and are failing at authenticating. Please follow the MongoDB part of prerequisites and setup up MongoDB on cloud or locally according to your preferences. 

If you plan to your own DB, use the following command and provide your DB credentials - `mongodb+srv://<collectionname>:<password>@<clustername>.ystwcxh.mongodb.net/`
- `<collectionname>` - Name of your collection, eg. TaskDatabase
- `<password>` - Password configured during creation of cluster
- `<clustername>` - Name of your cluster, eg. CourierTrackingCluster

### 6. Build the Application
That's all the setup, now build your application using `dotnet build`

### 7. Run the Application
Run the application using `dotnet run`

## API Endpoints
