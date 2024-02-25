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
### Install Prerequisites
- .NET 8
  - Download the latest version of [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) SDK and install
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
