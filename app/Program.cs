using System;
using Protocol.Mumble;

namespace Mumble.net.app
{
    class Program
    {
        static MumbleClient client = new MumbleClient("Mumble.net", "localhost", "web");
        static void Main(string[] args)
        {

            client.Connect();
            client.OnConnected += client_OnConnected;
            client.OnTextMessage += client_OnTextMessage;
            client.OnPacketReceived += client_OnPacketReceived;

            while (true)
            {
                var command = Console.ReadLine();
                if (command == "exit")
                    break;
                if (command[0] == 'm' && command[1] == ' ')
                {
                    client.SendTextMessageToChannel(command.TrimStart('m', ' '), client.RootChannel, false);
                    client.SendTextMessage(command.TrimStart('m', ' '), client.Channels.Values, client.Channels.Values, client.Users.Values);
                }
            }
            client.Disconnect();

        }

        static void client_OnPacketReceived(object sender, MumblePacketEventArgs e)
        {

        }
        static void msg(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void client_OnTextMessage(object sender, MumblePacketEventArgs e)
        {


        }

        static void client_OnConnected(object sender, MumblePacketEventArgs e)
        {
            foreach (var usr in client.Users)
            {
                msg(usr.Value.Name);
            }
            client.SendQueryUsers();
        }
    }
}
