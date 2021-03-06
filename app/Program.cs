﻿using System;
using Protocol.Mumble;

namespace Mumble.net.app
{
    class Program
    {
        static MumbleClient client = new MumbleClient("Mumble.net", "localhost", "user","password");
        static void Main(string[] args)
        {

            client.Connect();
            client.OnConnected += client_OnConnected;
            client.OnMessageRecived += client_OnMessageRecived;
            client.OnUserStatusEvent += client_OnUserStatusEvent;
            client.OnUserConnected += client_OnUserConnected;
            client.OnPacketReceived += client_OnPacketReceived;

            while (true)
            {
                var c = Console.ReadLine();
                if (c == "exit")
                    break;
                if (c.Length > 1)
                {
                    if (c[0] == 'm' && c[1] == ' ')
                    {
                        client.SendTextMessageToChannel(c.TrimStart('m', ' '), client.RootChannel, false);
                        //client.SendTextMessage(c.TrimStart('m', ' '), client.Channels.Values, client.Channels.Values, client.Users.Values);
                    }
                    if (c[0] == 'u')
                    {
                        client.SendUserList();

                    }
                }
            }
            client.Disconnect();

        }

        static void client_OnPacketReceived(object sender, MumblePacketEventArgs e)
        {

        }

        static void client_OnUserConnected(object sender, MumbleUserStatusEventArgs e)
        {
            msg("Connected : " + e.User.Name + " : " + e.User.Hash);
        }

        static void client_OnUserStatusEvent(object sender, MumbleUserStatusEventArgs e)
        {
            msg("user status : " + e.User.Name);
        }

        static void client_OnMessageRecived(object sender, MumbleMessageRecivedEventArgs e)
        {
            msg(e.ToString());
        }

        static void msg(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void client_OnConnected(object sender, MumblePacketEventArgs e)
        {
            client.SendUserList();
            foreach (var usr in client.Users)
            {
                msg(usr.Value.Name);
            }
            client.SendQueryUsers();
        }
    }
}
