package com.soc.testwsclient;

import com.soap.ws.client.generated.RequestResult;
import com.soap.ws.client.generated.Route;
import javafx.scene.web.WebEngine;

public class RouteHandler {

    private final WebEngine webEngine;

    public RouteHandler(WebEngine webEngine){
        this.webEngine = webEngine;
    }

    public boolean displayPath(RequestResult path){
        executeClear();

        var itinerary = path.getItineraries().getValue();

        if (itinerary == null){
            System.out.println("Itinerary null.");
            return false;
        }

        var it = itinerary.getItinerary().iterator();

        execute(it.next().getRoutes().getValue().getRoute().get(0), true, false);

        while (it.hasNext()){
            execute(it.next().getRoutes().getValue().getRoute().get(0), false, !it.hasNext());
        }

        return true;
    }

    private void execute(Route route, boolean start, boolean end){
        String jsFunction = ("addRoute(\"" + route.getGeometry().getValue() + "\", " + start + ", " + end + ");")
                .replaceAll("\\\\", "\\\\\\\\");

        System.out.println(jsFunction);
        webEngine.executeScript(jsFunction);
    }

    private void executeClear(){
        webEngine.executeScript("clearRoute();");
    }
}
