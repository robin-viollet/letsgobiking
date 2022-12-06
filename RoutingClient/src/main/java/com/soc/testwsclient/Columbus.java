package com.soc.testwsclient;

import com.soap.ws.client.generated.Contract;
import com.soap.ws.client.generated.IServices;
import com.soap.ws.client.generated.ServicesProvider;
import com.soc.testwsclient.actions.ContractsListener;
import javafx.application.Application;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.geometry.Insets;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.control.Label;
import javafx.scene.input.KeyCode;
import javafx.scene.input.KeyEvent;
import javafx.scene.layout.GridPane;
import javafx.scene.paint.Color;
import javafx.scene.web.WebEngine;
import javafx.scene.web.WebView;
import javafx.stage.Stage;

import javax.jms.JMSException;
import java.util.List;

public class Columbus extends Application {
    public static final int GAP = 10;

    private static final String INDEX_PATH = "/map.html";

    private static final int MAP_W_START = 0;
    private static final int MAP_H_START = 0;
    private static final int MAP_WIDTH = 9;
    private static final int MAP_HEIGHT = 2;
    private static final int DEPARTURE_W_START = 0;
    private static final int DEPARTURE_H_START = MAP_H_START + MAP_HEIGHT;
    private static final int DEPARTURE_WIDTH = 1;
    private static final int DEPARTURE_HEIGHT = 1;
    private static final int ARRIVAL_W_START = DEPARTURE_W_START + DEPARTURE_WIDTH;
    private static final int ARRIVAL_H_START = DEPARTURE_H_START;
    private static final int ARRIVAL_WIDTH = DEPARTURE_WIDTH;
    private static final int ARRIVAL_HEIGHT = DEPARTURE_HEIGHT;
    private static final int CONTRACTS_W_START = ARRIVAL_W_START + ARRIVAL_WIDTH;
    private static final int CONTRACTS_H_START = DEPARTURE_H_START;
    private static final int CONTRACTS_WIDTH = 2;
    private static final int CONTRACTS_HEIGHT = 1;
    private static final int CALCULATE_W_START = CONTRACTS_W_START + CONTRACTS_WIDTH;
    private static final int CALCULATE_H_START = DEPARTURE_H_START;

    private ActiveMQConsumer activeMQConsumer;

    private GridPane root;
    private AddressPane departure;
    private AddressPane arrival;
    private ComboBox<Contract> contracts;
    private Button calculate;
    private Label info;
    private Label nextStep;
    private Button nextStepButton;

    public static void main(String[] args) {
        launch(args);
    }

    @Override
    public void start(Stage stage) throws Exception {
        ServicesProvider server = new ServicesProvider();
        IServices serverServices = server.getBasicHttpBindingIServices();
        ObservableList<String> cities = FXCollections.observableArrayList();

        List<Contract> contractsList = serverServices.getAllContracts().getContract();

        WebView webView = new WebView();
        final WebEngine webEngine = webView.getEngine();

        var mainPage = getClass().getResource(INDEX_PATH);

        if (mainPage == null){
            webEngine.loadContent("<h1>Error</h1><p>Could not load map. Please try to reinstall the app.</p>");
        } else {
            webEngine.load(mainPage.toURI().toURL().toString());
        }

        root = new GridPane();

        root.setVgap(GAP);
        root.setHgap(GAP);

        departure = new AddressPane("Departure", cities);
        arrival = new AddressPane("Arrival", cities);
        contracts = new ComboBox<>(FXCollections.observableArrayList(contractsList));
        calculate = new Button("Calculate!");
        info = new Label("");
        nextStep = new Label("No next step.");
        nextStepButton = new Button("Next Step");

        info.setTextFill(Color.RED);

        nextStepButton.setDisable(true);
        nextStepButton.setOnAction(actionEvent -> {
            try {
                nextStep.setText(activeMQConsumer.consumeMessage());
            } catch (JMSException e) {
                throw new RuntimeException(e);
            }
        });

        contracts.setConverter(new ContractConverter(contractsList));
        contracts.valueProperty().addListener(new ContractsListener(cities, departure, arrival));
        contracts.getSelectionModel().selectFirst();

        RouteHandler routeHandler = new RouteHandler(webEngine);

        calculate.setOnAction(actionEvent -> {
            info.setText("");

            System.out.println("Retrieving path...");

            var path = serverServices.getBestPath(departure.toLocation(), arrival.toLocation());

            System.out.println("Path retrieved.");

            if (path == null || !routeHandler.displayPath(path)){
                info.setText("Could not find path.");
            }


            /*try {
                    activeMQConsumer = new ActiveMQConsumer(path.getInstructionQueueId().getValue());
                    nextStepButton.setDisable(false);
                } catch (JMSException e) {
                    throw new RuntimeException(e);
                }*/


                /*var route = path.getItineraries().getValue().getItinerary().get(0).getRoutes().getValue().getRoute().get(0).getGeometry().getValue();
                System.out.println(route);
                webEngine.executeScript("setRoute(\"" + route.replaceAll("\\\\", "\\\\\\\\") + "\");");*/
        });

        root.add(webView, MAP_W_START, MAP_H_START, MAP_WIDTH, MAP_HEIGHT);
        root.add(departure, DEPARTURE_W_START, DEPARTURE_H_START, DEPARTURE_WIDTH, DEPARTURE_HEIGHT);
        root.add(arrival, ARRIVAL_W_START, ARRIVAL_H_START, ARRIVAL_WIDTH, ARRIVAL_HEIGHT);
        root.add(contracts, CONTRACTS_W_START, CONTRACTS_H_START, CONTRACTS_WIDTH, CONTRACTS_HEIGHT);
        root.add(calculate, CALCULATE_W_START, CALCULATE_H_START);
        root.add(info, CALCULATE_W_START + 1, CALCULATE_H_START);
        root.add(nextStep, MAP_W_START, CALCULATE_H_START + 1);
        root.add(nextStepButton, CALCULATE_W_START + 2, CALCULATE_H_START + 1);

        Insets margin = new Insets(GAP);

        GridPane.setMargin(departure, margin);
        GridPane.setMargin(arrival, margin);
        GridPane.setMargin(nextStep, margin);
        GridPane.setMargin(nextStepButton, margin);

        Scene scene = new Scene(root);
        scene.addEventHandler(KeyEvent.KEY_PRESSED, t -> {
            if(t.getCode()== KeyCode.ESCAPE){
                System.exit(0);
            }
        });
        stage.setTitle("Let'sGoBiking - Client");
        stage.setScene(scene);
        stage.show();
    }
}