package com.soc.testwsclient.actions;

import com.soap.ws.client.generated.IServices;
import com.soc.testwsclient.AddressPane;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.scene.control.Label;
import javafx.scene.web.WebEngine;

public class CalculateButtonAction implements EventHandler<ActionEvent> {
    protected IServices serverServices;
    protected WebEngine webEngine;
    protected Label info;
    protected AddressPane departure;
    protected AddressPane arrival;

    public CalculateButtonAction(IServices serverServices, WebEngine webEngine, Label info, AddressPane departure, AddressPane arrival) {
        this.serverServices = serverServices;
        this.webEngine = webEngine;
        this.info = info;
        this.departure = departure;
        this.arrival = arrival;
    }

    @Override
    public void handle(ActionEvent actionEvent) {
        info.setText("");

        var path = serverServices.getBestPath(departure.toLocation(), arrival.toLocation());

        if (path != null){
            var route = path.getItineraries().getValue().getItinerary().get(0).getRoutes().getValue().getRoute().get(0).getGeometry().getValue();
            System.out.println(route);
            webEngine.executeScript("setRoute(\"" + route.replaceAll("\\\\", "\\\\\\\\") + "\");");
        } else {
            info.setText("Could not find path.");
        }
    }
}
