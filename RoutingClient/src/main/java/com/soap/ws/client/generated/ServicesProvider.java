
package com.soap.ws.client.generated;

import java.net.MalformedURLException;
import java.net.URL;
import javax.xml.namespace.QName;
import javax.xml.ws.Service;
import javax.xml.ws.WebEndpoint;
import javax.xml.ws.WebServiceClient;
import javax.xml.ws.WebServiceException;
import javax.xml.ws.WebServiceFeature;


/**
 * This class was generated by the JAX-WS RI.
 * JAX-WS RI 2.3.2
 * Generated source version: 2.2
 * 
 */
@WebServiceClient(name = "ServicesProvider", targetNamespace = "http://tempuri.org/", wsdlLocation = "http://localhost:8733/Design_Time_Addresses/RoutingServer/ServicesProvider?wsdl")
public class ServicesProvider
    extends Service
{

    private final static URL SERVICESPROVIDER_WSDL_LOCATION;
    private final static WebServiceException SERVICESPROVIDER_EXCEPTION;
    private final static QName SERVICESPROVIDER_QNAME = new QName("http://tempuri.org/", "ServicesProvider");

    static {
        URL url = null;
        WebServiceException e = null;
        try {
            url = new URL("http://localhost:8733/Design_Time_Addresses/RoutingServer/ServicesProvider?wsdl");
        } catch (MalformedURLException ex) {
            e = new WebServiceException(ex);
        }
        SERVICESPROVIDER_WSDL_LOCATION = url;
        SERVICESPROVIDER_EXCEPTION = e;
    }

    public ServicesProvider() {
        super(__getWsdlLocation(), SERVICESPROVIDER_QNAME);
    }

    public ServicesProvider(WebServiceFeature... features) {
        super(__getWsdlLocation(), SERVICESPROVIDER_QNAME, features);
    }

    public ServicesProvider(URL wsdlLocation) {
        super(wsdlLocation, SERVICESPROVIDER_QNAME);
    }

    public ServicesProvider(URL wsdlLocation, WebServiceFeature... features) {
        super(wsdlLocation, SERVICESPROVIDER_QNAME, features);
    }

    public ServicesProvider(URL wsdlLocation, QName serviceName) {
        super(wsdlLocation, serviceName);
    }

    public ServicesProvider(URL wsdlLocation, QName serviceName, WebServiceFeature... features) {
        super(wsdlLocation, serviceName, features);
    }

    /**
     * 
     * @return
     *     returns IServices
     */
    @WebEndpoint(name = "BasicHttpBinding_IServices")
    public IServices getBasicHttpBindingIServices() {
        return super.getPort(new QName("http://tempuri.org/", "BasicHttpBinding_IServices"), IServices.class);
    }

    /**
     * 
     * @param features
     *     A list of {@link javax.xml.ws.WebServiceFeature} to configure on the proxy.  Supported features not in the <code>features</code> parameter will have their default values.
     * @return
     *     returns IServices
     */
    @WebEndpoint(name = "BasicHttpBinding_IServices")
    public IServices getBasicHttpBindingIServices(WebServiceFeature... features) {
        return super.getPort(new QName("http://tempuri.org/", "BasicHttpBinding_IServices"), IServices.class, features);
    }

    private static URL __getWsdlLocation() {
        if (SERVICESPROVIDER_EXCEPTION!= null) {
            throw SERVICESPROVIDER_EXCEPTION;
        }
        return SERVICESPROVIDER_WSDL_LOCATION;
    }

}
