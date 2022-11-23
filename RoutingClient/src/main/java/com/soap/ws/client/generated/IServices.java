
package com.soap.ws.client.generated;

import javax.jws.WebMethod;
import javax.jws.WebParam;
import javax.jws.WebResult;
import javax.jws.WebService;
import javax.xml.bind.annotation.XmlSeeAlso;
import javax.xml.ws.RequestWrapper;
import javax.xml.ws.ResponseWrapper;


/**
 * This class was generated by the JAX-WS RI.
 * JAX-WS RI 2.3.2
 * Generated source version: 2.2
 * 
 */
@WebService(name = "IServices", targetNamespace = "http://tempuri.org/")
@XmlSeeAlso({
    ObjectFactory.class
})
public interface IServices {


    /**
     * 
     * @param startLocation
     * @param endLocation
     * @return
     *     returns com.soap.ws.client.generated.Itinerary
     */
    @WebMethod(operationName = "GetBestPath", action = "http://tempuri.org/IServices/GetBestPath")
    @WebResult(name = "GetBestPathResult", targetNamespace = "http://tempuri.org/")
    @RequestWrapper(localName = "GetBestPath", targetNamespace = "http://tempuri.org/", className = "com.soap.ws.client.generated.GetBestPath")
    @ResponseWrapper(localName = "GetBestPathResponse", targetNamespace = "http://tempuri.org/", className = "com.soap.ws.client.generated.GetBestPathResponse")
    public Itinerary getBestPath(
        @WebParam(name = "startLocation", targetNamespace = "http://tempuri.org/")
        Location startLocation,
        @WebParam(name = "endLocation", targetNamespace = "http://tempuri.org/")
        Location endLocation);

}