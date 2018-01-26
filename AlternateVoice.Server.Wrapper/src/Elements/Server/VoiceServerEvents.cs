﻿using AlternateVoice.Server.Wrapper.Interfaces;

namespace AlternateVoice.Server.Wrapper.Elements.Server
{
    internal partial class VoiceServer
    {
        public event Delegates.EmptyEvent OnServerStarted;
        public event Delegates.EmptyEvent OnServerStopping;
        
        public event Delegates.ClientEvent OnClientConnecting;
        public event Delegates.ClientDisconnected OnClientDisconnected;
        
        public event Delegates.ClientEvent OnClientAdded;
        public event Delegates.ClientEvent OnClientRemoved;

        public event Delegates.ClientGroupEvent OnClientJoinedGroup;
        public event Delegates.ClientGroupEvent OnClientLeftGroup;

        private void DisposeEvents()
        {
            OnServerStarted = null;
            OnServerStopping = null;
            
            OnClientConnecting = null;
            OnClientConnecting = null;
            
            OnClientAdded = null;
            OnClientRemoved = null;
            
            OnClientJoinedGroup = null;
            OnClientLeftGroup = null;
        }

        internal void FireClientJoinedGroup(IVoiceClient client, IVoiceGroup group)
        {
            OnClientJoinedGroup?.Invoke(client, group);
        }

        internal void FireClientLeftGroup(IVoiceClient client, IVoiceGroup group)
        {
            OnClientLeftGroup?.Invoke(client, group);
        }
        
    }
}