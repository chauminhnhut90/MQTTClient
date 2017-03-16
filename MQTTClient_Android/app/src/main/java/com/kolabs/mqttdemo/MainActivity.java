package com.kolabs.mqttdemo;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import org.eclipse.paho.android.service.MqttAndroidClient;
import org.eclipse.paho.client.mqttv3.IMqttActionListener;
import org.eclipse.paho.client.mqttv3.IMqttDeliveryToken;
import org.eclipse.paho.client.mqttv3.IMqttToken;
import org.eclipse.paho.client.mqttv3.MqttCallback;
import org.eclipse.paho.client.mqttv3.MqttConnectOptions;
import org.eclipse.paho.client.mqttv3.MqttException;
import org.eclipse.paho.client.mqttv3.MqttMessage;

public class MainActivity extends AppCompatActivity {

    MqttAndroidClient mqttAndroidClient;
    final String serverUri = "tcp://192.168.86.218:1883";// broker ip address
    final String clientId = "AndroidMQTTClient";
    String subscriptionTopic = "";

    private EditText etTopic;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        setUp();
        etTopic = (EditText) findViewById(R.id.etTopic);

        findViewById(R.id.btSubscribe).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                subscriptionTopic = etTopic.getText().toString();

                // Subscripe to listen to  message from server (broker)
                subscribeToTopic();
            }
        });
    }

    // Connect to broker
    private void setUp() {
        mqttAndroidClient = new MqttAndroidClient(getApplicationContext(), serverUri, clientId);
        mqttAndroidClient.setCallback(new MqttCallback() {
            @Override
            public void connectionLost(Throwable throwable) {
                Log.e("", "");
            }

            @Override
            public void messageArrived(String s, final MqttMessage mqttMessage) throws Exception {

                // Receive message
                runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        showToast(mqttMessage.toString());
                    }
                });
            }

            @Override
            public void deliveryComplete(IMqttDeliveryToken iMqttDeliveryToken) {
                Log.e("", "");
            }
        });

        MqttConnectOptions mqttConnectOptions = new MqttConnectOptions();
        mqttConnectOptions.setCleanSession(false);

        try {
            mqttAndroidClient.connect(mqttConnectOptions, null, new IMqttActionListener() {
                @Override
                public void onSuccess(IMqttToken asyncActionToken) {
                    showToast("Successed to connect to: " + serverUri);
                }

                @Override
                public void onFailure(IMqttToken asyncActionToken, Throwable exception) {
                    showToast("Failed to connect to: " + serverUri);
                }
            });


        } catch (MqttException ex) {
            ex.printStackTrace();
        }
    }

    public void subscribeToTopic() {
        try {
            mqttAndroidClient.subscribe(subscriptionTopic, 0, null, new IMqttActionListener() {
                @Override
                public void onSuccess(IMqttToken asyncActionToken) {
                    showToast("Subscribed!");
                }

                @Override
                public void onFailure(IMqttToken asyncActionToken, Throwable exception) {
                    showToast("Failed to subscribe");
                }
            });

        } catch (MqttException ex) {
            System.err.println("Exception whilst subscribing");
            ex.printStackTrace();
        }
    }

    @SuppressWarnings("unused")
    public void publishMessage(String yourText) {
        try {
            MqttMessage message = new MqttMessage();
            message.setPayload(yourText.getBytes());
            mqttAndroidClient.publish(subscriptionTopic, message);
        } catch (MqttException e) {
            System.err.println("Error Publishing: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private void showToast(String mainText) {
        Toast.makeText(this, mainText, Toast.LENGTH_SHORT).show();
    }
}
