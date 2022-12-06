package com.soc.testwsclient.actions;

import com.soap.ws.client.generated.ArrayOfstring;
import com.soap.ws.client.generated.Contract;
import com.soc.testwsclient.AddressPane;
import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;
import javafx.collections.ObservableList;

public class ContractsListener implements ChangeListener<Contract> {
    private ObservableList<String> cities;
    private AddressPane departure;
    private AddressPane arrival;

    public ContractsListener(ObservableList<String> cities, AddressPane departure, AddressPane arrival) {
        this.cities = cities;
        this.departure = departure;
        this.arrival = arrival;
    }

    @Override
    public void changed(ObservableValue<? extends Contract> observableValue, Contract oldContract, Contract newContract) {
        cities.clear();
        ArrayOfstring contratCities = newContract.getCities().getValue();

        if (contratCities != null) {
            cities.addAll(contratCities.getString());
            departure.selectFirstCity();
            arrival.selectFirstCity();
        }
    }
}
