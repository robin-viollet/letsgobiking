package com.soc.testwsclient;

import com.soap.ws.client.generated.Location;
import com.soap.ws.client.generated.ObjectFactory;
import javafx.collections.ObservableList;
import javafx.scene.control.ComboBox;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.scene.layout.GridPane;
import javafx.scene.text.Font;
import javafx.scene.text.FontWeight;

public class AddressPane extends GridPane {
    private final Label label;
    private final Label streetLabel;
    private final TextField street;
    private final Label cityLabel;
    private final ComboBox<String> city;

    public AddressPane(String labelText, String promptStreet, ObservableList<String> cities){
        this.label = new Label(labelText);
        this.streetLabel = new Label("Street:");
        this.street = new TextField();
        this.cityLabel = new Label("City:");
        this.city = new ComboBox<>(cities);

        Font defaultFont = Font.getDefault();

        this.label.setFont(Font.font(defaultFont.getName(), FontWeight.BOLD, defaultFont.getSize()));
        this.city.getSelectionModel().selectFirst();
        this.street.setPromptText(promptStreet);

        add(label, 0, 0, 1, 1);
        add(streetLabel, 0, 1);
        add(street, 1, 1, 2, 1);
        add(cityLabel, 0, 2);
        add(city, 1, 2);

        setVgap(Columbus.GAP);
        setHgap(Columbus.GAP);
    }

    public void selectFirstCity(){
        city.getSelectionModel().selectFirst();
    }

    public Location toLocation(){
        ObjectFactory objectFactory = new  ObjectFactory();

        Location location = objectFactory.createLocation();

        location.setStreet(objectFactory.createLocationStreet(street.getText()));
        location.setCity(objectFactory.createLocationCity(city.getSelectionModel().getSelectedItem()));

        return location;
    }
}
