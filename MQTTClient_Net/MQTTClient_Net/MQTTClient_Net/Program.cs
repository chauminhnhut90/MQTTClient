using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTTClient_Net
{
    class Program
    {
        static MqttClient client;
        static string MQTT_BROKER_ADDRESS = "192.168.86.233";// BROKER IP ADDRESS

        static void Main(string[] args)
        {
            Console.WriteLine("--------START PROGRAM--------");

            connect();

            client.MqttMsgPublished += Client_MqttMsgPublished;
            client.MqttMsgSubscribed += Client_MqttMsgSubscribed;
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.MqttMsgUnsubscribed += Client_MqttMsgUnsubscribed;

            string topic = "my_topic";
            Subscribe(topic);

        }

        private static void connect()
        {
            client = new MqttClient(MQTT_BROKER_ADDRESS);
            client.ProtocolVersion = MqttProtocolVersion.Version_3_1;
            client.Connect(Guid.NewGuid().ToString());
        }

        private static void Publish(string topic, string message)
        {
            client.Publish(topic, // topic
            Encoding.UTF8.GetBytes(message), // message body
                              MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                              false); // retained
        }

        private static void Subscribe(string topic)
        {
           client.Subscribe(new string[] { topic },
                                new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        private static void Unsubscribe(string topic)
        {
            client.Unsubscribe(new string[] { topic });
        }

        private static void Client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Console.WriteLine("Unsubscribed for id = " + e.MessageId);
        }

        private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine(Encoding.UTF8.GetString(e.Message));
        }

        private static void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("Subscribed for id = " + e.MessageId);
        }

        private static void Client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Console.WriteLine("MessageId = " + e.MessageId + " Published = " + e.IsPublished);
        }
    }
}
