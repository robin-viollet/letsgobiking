package com.soc.testwsclient;

import com.soap.ws.client.generated.Location;
import com.soap.ws.client.generated.ObjectFactory;
import javafx.collections.ObservableList;
import javafx.scene.control.ComboBox;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.scene.layout.GridPane;

public class AddressPane extends GridPane {
    private final Label label;
    private final Label streetLabel;
    private final TextField street;
    private final Label cityLabel;
    private final ComboBox<String> city;

    public AddressPane(String labelText, ObservableList<String> cities){
        this.label = new Label(labelText);
        this.street = new TextField();
        this.city = new ComboBox<>(cities);

        this.city.getSelectionModel().selectFirst();

        add(label, 0, 0, 1, 1);

        streetLabel = new Label("Street:");
        add(streetLabel, 0, 1);

        add(street, 1, 1, 2, 1);

        cityLabel = new Label("City:");
        add(cityLabel, 0, 2);
        add(city, 1, 2);

        setVgap(Columbus.GAP);
        setHgap(Columbus.GAP);
    }

    public Location toLocation(){
        ObjectFactory objectFactory = new  ObjectFactory();

        Location location = objectFactory.createLocation();

        location.setStreet(objectFactory.createLocationStreet(street.getText()));
        location.setCity(objectFactory.createLocationCity(city.getSelectionModel().getSelectedItem()));

        return location;
    }
}
