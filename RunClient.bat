cd RoutingClient\
rd -r .\src\main\java\com\soap\
mvn clean jaxws:wsimport
mvn package
mvn exec:java