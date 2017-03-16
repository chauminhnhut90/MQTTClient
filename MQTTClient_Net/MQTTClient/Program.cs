using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTTClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--------START PROGRAM--------");
            String MQTT_BROKER_ADDRESS = "192.168.86.233";

            MqttClient client = new MqttClient(MQTT_BROKER_ADDRESS);
            client.ProtocolVersion = MqttProtocolVersion.Version_3_1;

            //byte code = client.Connect(Guid.NewGuid().ToString(), null, null,
            //               false, // will retain flag
            //               MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // will QoS
            //               true, // will flag
            //               "autotrace", // will topic
            //               "will_message", // will message
            //               true,
            //               60);

            byte code = client.Connect(Guid.NewGuid().ToString());

            client.MqttMsgPublished += Client_MqttMsgPublished;
            client.MqttMsgSubscribed += Client_MqttMsgSubscribed;
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.MqttMsgUnsubscribed += Client_MqttMsgUnsubscribed;

            //ushort msgId = client.Publish("autotrace", // topic
            //Encoding.UTF8.GetBytes("MyMessageBody"), // message body
            //                  MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
            //                  false); // retained

            ushort msgId1 = client.Subscribe(new string[] { "autotrace", "/topic_2" },
                                 new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                                 MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            //ushort msgId2 = client.Unsubscribe(new string[] { "/autotrace", "/topic_2" });
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
