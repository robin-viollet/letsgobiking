package com.soc.testwsclient;

import com.soap.ws.client.generated.IServices;
import com.soap.ws.client.generated.Location;
import com.soap.ws.client.generated.ServicesProvider;

import javax.swing.*;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;

public class Window extends JFrame {
    protected JPanel root = new JPanel();
    protected AddressPanel start;
    protected AddressPanel end;
    protected JButton calculate;
    
    public Window(){
        this.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
        this.addKeyListener(new KeyAdapter() {
            @Override
            public void keyTyped(KeyEvent e){
                super.keyTyped(e);

                if (e.getKeyChar() == KeyEvent.VK_ESCAPE){
                    System.exit(0);
                }
            }
        });

        this.setContentPane(root);

        start = new AddressPanel("1 Place de la Comédie", "Lyon", "France", "69001");
        end = new AddressPanel("228 Avenue du Plateau", "Lyon", "France", "69009");

        start.setLabel("Departure address");
        end.setLabel("Arrival address");

        calculate = new JButton("Calculate!");

        calculate.addActionListener(actionEvent -> {
            Location departure = start.toLocation();
            Location arrival = end.toLocation();

            System.out.println(departure);
            System.out.println(arrival);

            ServicesProvider server = new ServicesProvider();
            IServices serverServices = server.getBasicHttpBindingIServices();

            System.out.println(serverServices.getBestPath(departure, arrival).toString());
        });
        
        root.add(start);
        root.add(end);
        root.add(calculate);
    }
}