﻿using JustAnotherVoiceChat.Server.Wrapper.Delegates;
using JustAnotherVoiceChat.Server.Wrapper.Elements.Models;
using JustAnotherVoiceChat.Server.Wrapper.Interfaces;
using JustAnotherVoiceChat.Server.Wrapper.Math;

namespace JustAnotherVoiceChat.Server.Wrapper.Tests.Fakes
{
    public class VoiceWrapperEventInvoker : IVoiceWrapper
    {

        private NativeDelegates.ClientConnectingCallback _clientConnecting;
        private NativeDelegates.ClientCallback _clientConnected;
        private NativeDelegates.ClientRejectedCallback _clientRejected;
        private NativeDelegates.ClientCallback _clientDisconnected;
        private NativeDelegates.ClientStatusCallback _clientTalkingChanged;
        private NativeDelegates.ClientStatusCallback _clientSpeakersMuteChanged;
        private NativeDelegates.ClientStatusCallback _clientMicrophoneMuteChanged;
        private NativeDelegates.LogMessageCallback _logMessage;

        public void InvokeClientConnectingCallback(ushort handle, string teamspeakId)
        {
            _clientConnecting(handle, teamspeakId);
        }

        public void InvokeClientConnectedCallback(ushort handle)
        {
            _clientConnected(handle);
        }

        public void InvokeClientRejectedCallback(ushort handle, int statusCode)
        {
            _clientRejected(handle, statusCode);
        }

        public void InvokeClientDisconnectedCallback(ushort handle)
        {
            _clientDisconnected(handle);
        }

        public void InvokeClientTalkingChangedCallback(ushort handle, bool newStatus)
        {
            _clientTalkingChanged(handle, newStatus);
        }

        public void InvokeClientSpeakersMuteChangedCallback(ushort handle, bool newStatus)
        {
            _clientSpeakersMuteChanged(handle, newStatus);
        }

        public void InvokeClientMicrophoneMuteChangedCallback(ushort handle, bool newStatus)
        {
            _clientMicrophoneMuteChanged(handle, newStatus);
        }

        public void InvokeLogMessageCallback(string message, int logLevel)
        {
            _logMessage(message, logLevel);
        }
        
        public void RegisterClientConnectingCallback(NativeDelegates.ClientConnectingCallback callback)
        {
            _clientConnecting = callback;
        }

        public void RegisterClientConnectedCallback(NativeDelegates.ClientCallback callback)
        {
            _clientConnected = callback;
        }

        public void RegisterClientRejectedCallback(NativeDelegates.ClientRejectedCallback callback)
        {
            _clientRejected = callback;
        }

        public void RegisterClientDisconnectedCallback(NativeDelegates.ClientCallback callback)
        {
            _clientDisconnected = callback;
        }

        public void RegisterClientTalkingChangedCallback(NativeDelegates.ClientStatusCallback callback)
        {
            _clientTalkingChanged = callback;
        }

        public void RegisterClientSpeakersMuteChangedCallback(NativeDelegates.ClientStatusCallback callback)
        {
            _clientSpeakersMuteChanged = callback;
        }

        public void RegisterClientMicrophoneMuteChangedCallback(NativeDelegates.ClientStatusCallback callback)
        {
            _clientMicrophoneMuteChanged = callback;
        }

        public void RegisterLogMessageCallback(NativeDelegates.LogMessageCallback callback)
        {
            _logMessage = callback;
        }
        
        public void UnregisterClientConnectedCallback()
        {
            _clientConnected = null;
        }

        public void UnregisterClientConnectingCallback()
        {
            _clientConnecting = null;
        }

        public void UnregisterClientDisconnectedCallback()
        {
            _clientDisconnected = null;
        }

        public void UnregisterClientRejectedCallback()
        {
            _clientRejected = null;
        }

        public void UnregisterClientTalkingChangedCallback()
        {
            _clientTalkingChanged = null;
        }

        public void UnregisterClientSpeakersMuteChangedCallback()
        {
            _clientSpeakersMuteChanged = null;
        }

        public void UnregisterClientMicrophoneMuteChangedCallback()
        {
            _clientMicrophoneMuteChanged = null;
        }

        public void UnregisterLogMessageCallback()
        {
            _logMessage = null;
        }

        public void SetClientVoiceRange(IVoiceClient client, float voiceRange)
        {
            
        }

        public bool SetListenerPosition(IVoiceClient listener, Vector3 position, float rotation)
        {
            return true;
        }

        public bool SetRelativeSpeakerPositionForListener(IVoiceClient listener, IVoiceClient speaker, Vector3 position)
        {
            return true;
        }

        public bool ResetRelativeSpeakerPositionForListener(IVoiceClient listener, IVoiceClient speaker)
        {
            return true;
        }

        public bool ResetAllRelativePositionsForListener(IVoiceClient listener)
        {
            return true;
        }
        
        public void CreateNativeServer(VoiceServerConfiguration configuration)
        {
            
        }

        public void DestroyNativeServer()
        {
            
        }

        public bool StartNativeServer()
        {
            return true;
        }

        public void StopNativeServer()
        {
            
        }

        public bool RemoveClient(IVoiceClient client)
        {
            return true;
        }

        public bool SetClientNickname(IVoiceClient client, string nickname)
        {
            return true;
        }

        public void Set3DSettings(float distanceFactor, float rolloffFactor)
        {
            
        }
    }
}