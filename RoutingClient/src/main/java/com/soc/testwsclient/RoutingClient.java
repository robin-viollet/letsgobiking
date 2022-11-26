/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Project/Maven2/JavaApp/src/main/java/${packagePath}/${mainClassName}.java to edit this template
 */

package com.soc.testwsclient;

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
        
        ObjectFactory objectFactory = new  ObjectFactory();
        Location startLocation = new Location();
        Location endLocation = new Location();
        
        startLocation.setStreet(objectFactory.createString("22 Rue Jacques Preiss"));
        startLocation.setPostalCode(objectFactory.createString("68100"));
        startLocation.setCity(objectFactory.createString("Mulhouse"));
        startLocation.setCountry(objectFactory.createString("France"));
        
        endLocation.setStreet(objectFactory.createString("44 Av. Roger Salengro"));
        endLocation.setPostalCode(objectFactory.createString("68100"));
        endLocation.setCity(objectFactory.createString("Mulhouse"));
        endLocation.setCountry(objectFactory.createString("France"));
        
        System.out.println(serverServices.getBestPath(startLocation, endLocation).getType().getValue());
    }
}
