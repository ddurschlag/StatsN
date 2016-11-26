﻿using StatsN.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StatsN
{
    public class StatsdOptions
    {
        public event EventHandler<System.Exception> OnExceptionGenerated;
        public event EventHandler<string> OnLogEventGenerated;

        public bool BufferMetrics { get; set; } = false;
        public int BufferSize { get; set; } = 512;
        public string HostOrIp { get; set; } = Constants.Localhost;
        public int Port { get; set; } = 8125;
        string prefix = string.Empty;
        public string Prefix
        {
            get
            {
                return prefix;
            }

            set
            {
                prefix = value.TrimEnd(Constants.dot);
            }
        }

        internal void LogException(System.Exception exception)
        {
            if(OnExceptionGenerated == null)
            {
                LogEvent(exception.Message, EventType.Error);
                return;
            }
            OnExceptionGenerated.Invoke(this, exception);

            
        }
        internal void LogEvent(string message, EventType evntType)
        {
            if(OnLogEventGenerated == null)
            {
                var traceMessage = $"{Constants.Statsd}: {message}";
                switch (evntType)
                {
                    case EventType.Info:
                        Trace.TraceInformation(traceMessage);
                        break;
                    case EventType.Warning:
                        Trace.TraceWarning(traceMessage);
                        break;
                    case EventType.Error:
                        Trace.TraceError(traceMessage);
                        break;
                }
            }
            else
            {
                OnLogEventGenerated?.Invoke(this, message);
            }
        }
    }
}