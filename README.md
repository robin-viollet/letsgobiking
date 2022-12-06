# letsgobiking
## Start ActiveMQ
Download ActiveMQ [here](https://activemq.apache.org/components/classic/download/), decompress it, cd into the folder then start it with `.\bin\activemq start`

## Starting the server and the proxy
First you need to add MSBuild to your Path. It is usually located in the installation folder of VisualStudio).
For exemple : `C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin`

Then you need to run the file `RunServer.bat` as an administrator.

## Importing the WSDL
First start the server, then run `mvn clean jaxws:wsimport`

## Starting the client
To run the client just launch the `RunClient.bat`.

## Testing the client

Here are some addresses to test the client:
### Lyon contract
- 1 Place de la Comédie, Lyon
- 228 Avenue du Plateau, Lyon
- 15 Bd André Latarjet, Villeurbanne
