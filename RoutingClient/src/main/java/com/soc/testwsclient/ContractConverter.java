package com.soc.testwsclient;

import com.soap.ws.client.generated.Contract;
import javafx.util.StringConverter;

import java.util.List;

public class ContractConverter extends StringConverter<Contract> {
    protected List<Contract> contracts;

    public ContractConverter(List<Contract> contractList){
        this.contracts = contractList;
    }
    @Override
    public String toString(Contract contract) {
        return contract == null ? "null" : contract.getCommercialName().getValue() + " (" + contract.getName().getValue() + ")";
    }

    @Override
    public Contract fromString(String s) {
        return null;
    }
}
