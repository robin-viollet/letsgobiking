module com.soc.testwsclient {
    requires javafx.controls;
    requires javafx.web;
    requires java.xml.bind;
    requires java.xml;
    requires java.jws;
    requires java.xml.ws;
    requires java.desktop;
    requires java.naming;
    requires gmbal;
    requires activemq.all;

    exports com.soap.ws.client.generated to com.sun.xml.ws;

    opens com.soc.testwsclient to javafx.graphics;
    opens com.soap.ws.client.generated to com.sun.xml.bind;
}