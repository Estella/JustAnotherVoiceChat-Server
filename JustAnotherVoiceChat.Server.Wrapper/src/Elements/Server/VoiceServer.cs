﻿/*
 * File: VoiceServer.cs
 * Date: 21.2.2018,
 *
 * MIT License
 *
 * Copyright (c) 2018 JustAnotherVoiceChat
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Linq;
using JustAnotherVoiceChat.Server.Wrapper.Elements.Models;
using JustAnotherVoiceChat.Server.Wrapper.Enums;
using JustAnotherVoiceChat.Server.Wrapper.Exceptions;
using JustAnotherVoiceChat.Server.Wrapper.Interfaces;

namespace JustAnotherVoiceChat.Server.Wrapper.Elements.Server
{
    public partial class VoiceServer<TClient, TIdentifier> : IVoiceServer<TClient> where TClient : IVoiceClient
    {
        private readonly IVoiceClientFactory<TClient, TIdentifier> _factory;

        public VoiceServerConfiguration Configuration { get; }
        public IVoiceWrapper NativeWrapper { get; }
        
        public bool Started { get; private set; }

        private LogLevel _minimumLogLevel = LogLevel.Info;

        protected VoiceServer(IVoiceClientFactory<TClient, TIdentifier> factory, VoiceServerConfiguration configuration) : this(factory, configuration, JustAnotherVoiceChat.GetVoiceWrapper())
        {
            
        }
        
        internal VoiceServer(IVoiceClientFactory<TClient, TIdentifier> factory, VoiceServerConfiguration configuration, IVoiceWrapper voiceWrapper)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            NativeWrapper = voiceWrapper ?? throw new ArgumentNullException(nameof(voiceWrapper));
            
            Configuration = configuration;

            NativeWrapper.CreateNativeServer(configuration);

            AttachToNativeEvents();
            AttachTasksToStartAndStopEvent();
        }

        public void Start()
        {
            if (Started)
            {
                throw new VoiceServerAlreadyStartedException();
            }
            
            if (NativeWrapper.StartNativeServer() == false)
            {
                throw new VoiceServerNotStartedException();
            }
            
            Started = true;
            InvokeProtectedEvent(() => OnServerStarted?.Invoke());
        }

        public void Stop()
        {
            if (!Started)
            {
                throw new VoiceServerNotStartedException();
            }
            
            InvokeProtectedEvent(() => OnServerStopping?.Invoke());
            
            NativeWrapper.StopNativeServer();
            
            Started = false;
        }

        public void Log(LogLevel logLevel, string message)
        {
            try
            {
                if (logLevel > _minimumLogLevel)
                {
                    return;
                }
                
                OnLogMessage?.Invoke(message, logLevel);
            }
            catch
            {
                // We can't do anything if an exception occurs here...
            }
        }

        public void Log(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void SetLogLevel(LogLevel logLevel)
        {
            if (logLevel > LogLevel.Trace)
            {
                logLevel = LogLevel.Trace;
            } 
            else if (logLevel < LogLevel.Error)
            {
                logLevel = LogLevel.Error;
            }
            
            _minimumLogLevel = logLevel;
            NativeWrapper.SetLogLevel(logLevel);
        }

        public void Dispose()
        {
            var clients = _clients.Values.ToArray();
            _clients.Clear();
            
            foreach (var client in clients)
            {
                client.Dispose();
            }
            
            DisposeTasks();

            DisposeEvents();

            NativeWrapper.DestroyNativeServer();
        }
    }
}
