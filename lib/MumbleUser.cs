﻿using System;

namespace Protocol.Mumble
{
    public class MumbleUser
    {
        private readonly MumbleClient client;

        public MumbleChannel Channel { get; private set; }
        public string Name { get; private set; }
        public uint Session { get; private set; }
        public bool Deaf { get; private set; }
        public bool DeafSelf { get; private set; }
        public bool Mute { get; private set; }
        public bool MuteSelf { get; private set; }
        public string Hash { get; private set; }

        public MumbleUser(MumbleClient client, UserState message)
        {
            this.client = client;
            Name = message.name;
            Session = message.session;

            client.Users.Add(Session, this);

            Channel = client.Channels[message.channel_id];

            Channel.AddLocalUser(this);
        }

        public void Update(UserState message)
        {
            if (message.channel_idSpecified && message.channel_id != Channel.ID)
            {
                Channel.RemoveLocalUser(this);
                Channel = client.Channels[message.channel_id];
                Channel.AddLocalUser(this);
            }

            if (message.deafSpecified) { Deaf = message.deaf; }
            if (message.self_deafSpecified) { DeafSelf = message.self_deaf; }
            if (message.muteSpecified) { Mute = message.mute; }
            if (message.self_muteSpecified) { MuteSelf = message.self_mute; }
            Hash = message.hash; 
            if (message.hashSpecified) { Hash = message.hash; }
            
        }

        public void Update(UserRemove message)
        {
            client.Channels.Remove(Session);
            Channel.RemoveLocalUser(this);
        }

        public string Tree(int level)
        {
            return new String(' ', level) + "U " + Name + " (" + Session + ")" + Environment.NewLine;
        }

    }
}
