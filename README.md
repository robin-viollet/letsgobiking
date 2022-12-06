# letsgobiking
## Importing the WSDL
First start the server, then run `mvn clean jaxws:wsimport`

## Starting the client
Compile the client with `mvn package` then launch it with `java -jar LetsGoBiking-1.0-SNAPSHOT-jar-with-dependencies.jar`

## Starting the server and the proxy
First you need to add MSBuild to your Path. It is usually located in the installation folder of VisualStudio).
For exemple : `C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin`

Then you need to run the file `RunServer.bat` as an administrator.