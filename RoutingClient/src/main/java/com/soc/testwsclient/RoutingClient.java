/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Project/Maven2/JavaApp/src/main/java/${packagePath}/${mainClassName}.java to edit this template
 */

package com.soc.testwsclient;

import javax.xml.bind.JAXBElement;
import javax.xml.namespace.QName;

import com.soap.ws.client.generated.IServices;
import com.soap.ws.client.generated.Location;
import com.soap.ws.client.generated.ObjectFactory;
import com.soap.ws.client.generated.Server;

/**
 *
 * @author franc
 */
public class RoutingClient {

    public static void main(String[] args) {
        System.out.println("Hello World! we are going to test a SOAP client written in Java");
        Server server = new Server();
        IServices serverServices = server.getBasicHttpBindingIServices();
        System.out.println(serverServices.getBestPath(new Location(), new Location()));
        ObjectFactory test = new ObjectFactory();
        
        new Location().setCity(test.createString("Pute"));
    }
}
