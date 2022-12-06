cd .\RoutingClient\
rd -r .\src\main\java\com\soap\
call mvn clean jaxws:wsimport
call mvn package
call mvn exec:java