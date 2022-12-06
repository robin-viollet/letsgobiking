package com.soc.testwsclient;

import org.apache.activemq.ActiveMQConnectionFactory;

import javax.jms.Connection;
import javax.jms.Destination;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageConsumer;
import javax.jms.Session;
import javax.jms.TextMessage;

public class ActiveMQConsumer implements AutoCloseable {
    private static final String URL = ActiveMQConnection.DEFAULT_BROKER_URL; // default broker URL is : tcp://localhost:61616"
    private static final ActiveMQConnectionFactory connectionFactory = new ActiveMQConnectionFactory(URL);

    private Connection consumerConnection;
    private Session consumerSession;
    private Destination queue;
    private MessageConsumer messageConsumer;

    public ActiveMQConsumer(String queueId) throws JMSException {
        consumerConnection = connectionFactory.createConnection();
        consumerConnection.start();

        consumerSession = consumerConnection.createSession(false, Session.AUTO_ACKNOWLEDGE);
        queue = consumerSession.createQueue(queueId);
        messageConsumer = consumerSession.createConsumer(queue);
    }

    public String consumeMessage() throws JMSException {
        Message consumerMessage = messageConsumer.receive(1000);
        TextMessage consumerTextMessage = (TextMessage) consumerMessage;

        if (consumerTextMessage == null){
            throw new JMSException("No message is available.");
        }

        System.out.println("Consumed message: " + consumerTextMessage.getText());

        return consumerTextMessage.getText();
    }

    @Override
    public void close() throws Exception {
        consumerConnection.close();
    }
}
