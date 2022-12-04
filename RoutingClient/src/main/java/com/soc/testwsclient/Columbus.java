package com.soc.testwsclient;

import com.soap.ws.client.generated.IServices;
import com.soap.ws.client.generated.Location;
import com.soap.ws.client.generated.ServicesProvider;
import javafx.application.Application;
import javafx.collections.FXCollections;
import javafx.geometry.Insets;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.input.KeyCode;
import javafx.scene.input.KeyEvent;
import javafx.scene.layout.GridPane;
import javafx.scene.web.WebEngine;
import javafx.scene.web.WebView;
import javafx.stage.Stage;

import java.util.stream.Collectors;
import java.util.List;

public class Columbus extends Application {
    public static final int GAP = 10;

    private static final String INDEX_PATH = "/map.html";

    private static final int MAP_W_START = 0;
    private static final int MAP_H_START = 0;
    private static final int MAP_WIDTH = 6;
    private static final int MAP_HEIGHT = 2;
    private static final int DEPARTURE_W_START = 0;
    private static final int DEPARTURE_H_START = MAP_H_START + MAP_HEIGHT;
    private static final int DEPARTURE_WIDTH = 1;
    private static final int DEPARTURE_HEIGHT = 1;
    private static final int ARRIVAL_W_START = DEPARTURE_W_START + DEPARTURE_WIDTH;
    private static final int ARRIVAL_H_START = DEPARTURE_H_START;
    private static final int ARRIVAL_WIDTH = DEPARTURE_WIDTH;
    private static final int ARRIVAL_HEIGHT = DEPARTURE_HEIGHT;
    private static final int CONTRACT_WIDTH = 2;
    private static final int CONTRACT_HEIGHT = 1;

    private AddressPane departure;
    private AddressPane arrival;
    private ComboBox<String> contracts;
    private Button calculate;

    public static void main(String[] args) {
        launch(args);
    }

    @Override
    public void start(Stage stage) throws Exception {
        ServicesProvider server = new ServicesProvider();
        IServices serverServices = server.getBasicHttpBindingIServices();
        var cities = FXCollections.observableArrayList("Lyon");

        List<String> contractsList = serverServices.getContracts().getContract().stream().map(c -> c.getCommercialName().getValue()).collect(Collectors.toList());

        WebView webView = new WebView();
        final WebEngine webEngine = webView.getEngine();

        var mainPage = getClass().getResource(INDEX_PATH);

        if (mainPage == null){
            webEngine.loadContent("<h1>Error</h1><p>Could not load map. Please try to reinstall the app.</p>");
        } else {
            webEngine.load(mainPage.toURI().toURL().toString());
        }

        GridPane root = new GridPane();

        root.setVgap(GAP);
        root.setHgap(GAP);

        departure = new AddressPane("Departure", cities);
        arrival = new AddressPane("Arrival", cities);
        contracts = new ComboBox<>(FXCollections.observableArrayList(contractsList));
        calculate = new Button("Calculate!");

        contracts.getSelectionModel().selectFirst();
        calculate.setOnAction(actionEvent -> {
            Location startLocation = departure.toLocation();
            Location endLocation = arrival.toLocation();

            System.out.println(startLocation);
            System.out.println(endLocation);
            var path = serverServices.getBestPath(startLocation, endLocation);
            webEngine.executeScript("setRoute(\"" + path.getRoutes().getValue().getRoute().get(0).getGeometry().getValue() + "\");");
            System.out.println(path);
        });

        root.add(webView, MAP_W_START, MAP_H_START, MAP_WIDTH, MAP_HEIGHT);
        root.add(departure, DEPARTURE_W_START, DEPARTURE_H_START, DEPARTURE_WIDTH, DEPARTURE_HEIGHT);
        root.add(arrival, ARRIVAL_W_START, ARRIVAL_H_START, ARRIVAL_WIDTH, ARRIVAL_HEIGHT);
        root.add(contracts, DEPARTURE_WIDTH + ARRIVAL_WIDTH, DEPARTURE_H_START, CONTRACT_WIDTH, CONTRACT_HEIGHT);
        root.add(calculate, DEPARTURE_WIDTH + ARRIVAL_WIDTH + CONTRACT_WIDTH, MAP_HEIGHT);

        GridPane.setMargin(departure, new Insets(GAP));
        GridPane.setMargin(arrival, new Insets(GAP));

        Scene scene = new Scene(root);
        scene.addEventHandler(KeyEvent.KEY_PRESSED, t -> {
            if(t.getCode()== KeyCode.ESCAPE){
                System.exit(0);
            }
        });
        stage.setTitle("Let'sGoDrinking - Client");
        stage.setScene(scene);
        stage.show();
    }
}