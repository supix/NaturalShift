//-----------------------------------------------------------------------
// <copyright file="Logging.cs" company="supix">
//
// NaturalShift is an AI based engine to compute workshifts.
// Copyright (C) 2016 - Marcello Esposito (esposito.marce@gmail.com)
//
// This file is part of NaturalShift.
// NaturalShift is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// NaturalShift is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NaturalShift.SolvingEnvironment.Utils
{
    public interface IInternalLogger
    {
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }

        void Error(object message);

        void Error(object message, Exception exception);

        void ErrorFormat(string format, params object[] args);

        void Fatal(object message);

        void Fatal(object message, Exception exception);

        void Debug(object message);

        void Debug(object message, Exception exception);

        void DebugFormat(string format, params object[] args);

        void Info(object message);

        void Info(object message, Exception exception);

        void InfoFormat(string format, params object[] args);

        void Warn(object message);

        void Warn(object message, Exception exception);

        void WarnFormat(string format, params object[] args);
    }

    public interface ILoggerFactory
    {
        IInternalLogger LoggerFor(string keyName);

        IInternalLogger LoggerFor(System.Type type);
    }

    public class LoggerProvider
    {
        private const string NhibernateLoggerConfKey = "naturalshift-logger";
        private readonly ILoggerFactory loggerFactory;
        private static LoggerProvider instance;

        static LoggerProvider()
        {
            string nhibernateLoggerClass = GetNhibernateLoggerClass();
            ILoggerFactory loggerFactory = string.IsNullOrEmpty(nhibernateLoggerClass) ? new NoLoggingLoggerFactory() : GetLoggerFactory(nhibernateLoggerClass);
            SetLoggersFactory(loggerFactory);
        }

        private static ILoggerFactory GetLoggerFactory(string nhibernateLoggerClass)
        {
            ILoggerFactory loggerFactory;
            var loggerFactoryType = System.Type.GetType(nhibernateLoggerClass);
            try
            {
                loggerFactory = (ILoggerFactory)Activator.CreateInstance(loggerFactoryType);
            }
            catch (MissingMethodException ex)
            {
                throw new ApplicationException("Public constructor was not found for " + loggerFactoryType, ex);
            }
            catch (InvalidCastException ex)
            {
                throw new ApplicationException(loggerFactoryType + "Type does not implement " + typeof(ILoggerFactory), ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to instantiate: " + loggerFactoryType, ex);
            }
            return loggerFactory;
        }

        private static string GetNhibernateLoggerClass()
        {
            // look for log4net.dll
            string nhibernateLoggerClass = null;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
            string binPath = relativeSearchPath == null ? baseDir : Path.Combine(baseDir, relativeSearchPath);
            string log4NetDllPath = binPath == null ? "log4net.dll" : Path.Combine(binPath, "log4net.dll");

            if (File.Exists(log4NetDllPath) || AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == "log4net"))
            {
                nhibernateLoggerClass = typeof(Log4NetLoggerFactory).AssemblyQualifiedName;
            }
            return nhibernateLoggerClass;
        }

        public static void SetLoggersFactory(ILoggerFactory loggerFactory)
        {
            instance = new LoggerProvider(loggerFactory);
        }

        private LoggerProvider(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        public static IInternalLogger LoggerFor(string keyName)
        {
            return instance.loggerFactory.LoggerFor(keyName);
        }

        public static IInternalLogger LoggerFor(System.Type type)
        {
            return instance.loggerFactory.LoggerFor(type);
        }
    }

    public class NoLoggingLoggerFactory : ILoggerFactory
    {
        private static readonly IInternalLogger Nologging = new NoLoggingInternalLogger();

        public IInternalLogger LoggerFor(string keyName)
        {
            return Nologging;
        }

        public IInternalLogger LoggerFor(System.Type type)
        {
            return Nologging;
        }
    }

    public class NoLoggingInternalLogger : IInternalLogger
    {
        public bool IsErrorEnabled
        {
            get { return false; }
        }

        public bool IsFatalEnabled
        {
            get { return false; }
        }

        public bool IsDebugEnabled
        {
            get { return false; }
        }

        public bool IsInfoEnabled
        {
            get { return false; }
        }

        public bool IsWarnEnabled
        {
            get { return false; }
        }

        public void Error(object message)
        {
        }

        public void Error(object message, Exception exception)
        {
        }

        public void ErrorFormat(string format, params object[] args)
        {
        }

        public void Fatal(object message)
        {
        }

        public void Fatal(object message, Exception exception)
        {
        }

        public void Debug(object message)
        {
        }

        public void Debug(object message, Exception exception)
        {
        }

        public void DebugFormat(string format, params object[] args)
        {
        }

        public void Info(object message)
        {
        }

        public void Info(object message, Exception exception)
        {
        }

        public void InfoFormat(string format, params object[] args)
        {
        }

        public void Warn(object message)
        {
        }

        public void Warn(object message, Exception exception)
        {
        }

        public void WarnFormat(string format, params object[] args)
        {
        }
    }

    public class Log4NetLoggerFactory : ILoggerFactory
    {
        private static readonly System.Type LogManagerType = System.Type.GetType("log4net.LogManager, log4net");
        private static readonly Func<string, object> GetLoggerByNameDelegate;
        private static readonly Func<System.Type, object> GetLoggerByTypeDelegate;

        static Log4NetLoggerFactory()
        {
            GetLoggerByNameDelegate = GetGetLoggerMethodCall<string>();
            GetLoggerByTypeDelegate = GetGetLoggerMethodCall<System.Type>();
        }

        public IInternalLogger LoggerFor(string keyName)
        {
            return new Log4NetLogger(GetLoggerByNameDelegate(keyName));
        }

        public IInternalLogger LoggerFor(System.Type type)
        {
            return new Log4NetLogger(GetLoggerByTypeDelegate(type));
        }

        private static Func<TParameter, object> GetGetLoggerMethodCall<TParameter>()
        {
            var method = LogManagerType.GetMethod("GetLogger", new[] { typeof(TParameter) });
            ParameterExpression resultValue;
            ParameterExpression keyParam = Expression.Parameter(typeof(TParameter), "key");
            MethodCallExpression methodCall = Expression.Call(null, method, resultValue = keyParam);
            return Expression.Lambda<Func<TParameter, object>>(methodCall, resultValue).Compile();
        }
    }

    public class Log4NetLogger : IInternalLogger
    {
        private static readonly System.Type ILogType = System.Type.GetType("log4net.ILog, log4net");
        private static readonly Func<object, bool> IsErrorEnabledDelegate;
        private static readonly Func<object, bool> IsFatalEnabledDelegate;
        private static readonly Func<object, bool> IsDebugEnabledDelegate;
        private static readonly Func<object, bool> IsInfoEnabledDelegate;
        private static readonly Func<object, bool> IsWarnEnabledDelegate;

        private static readonly Action<object, object> ErrorDelegate;
        private static readonly Action<object, object, Exception> ErrorExceptionDelegate;
        private static readonly Action<object, string, object[]> ErrorFormatDelegate;

        private static readonly Action<object, object> FatalDelegate;
        private static readonly Action<object, object, Exception> FatalExceptionDelegate;

        private static readonly Action<object, object> DebugDelegate;
        private static readonly Action<object, object, Exception> DebugExceptionDelegate;
        private static readonly Action<object, string, object[]> DebugFormatDelegate;

        private static readonly Action<object, object> InfoDelegate;
        private static readonly Action<object, object, Exception> InfoExceptionDelegate;
        private static readonly Action<object, string, object[]> InfoFormatDelegate;

        private static readonly Action<object, object> WarnDelegate;
        private static readonly Action<object, object, Exception> WarnExceptionDelegate;
        private static readonly Action<object, string, object[]> WarnFormatDelegate;

        private readonly object logger;

        static Log4NetLogger()
        {
            IsErrorEnabledDelegate = DelegateHelper.BuildPropertyGetter<bool>(ILogType, "IsErrorEnabled");
            IsFatalEnabledDelegate = DelegateHelper.BuildPropertyGetter<bool>(ILogType, "IsFatalEnabled");
            IsDebugEnabledDelegate = DelegateHelper.BuildPropertyGetter<bool>(ILogType, "IsDebugEnabled");
            IsInfoEnabledDelegate = DelegateHelper.BuildPropertyGetter<bool>(ILogType, "IsInfoEnabled");
            IsWarnEnabledDelegate = DelegateHelper.BuildPropertyGetter<bool>(ILogType, "IsWarnEnabled");
            ErrorDelegate = DelegateHelper.BuildAction<object>(ILogType, "Error");
            ErrorExceptionDelegate = DelegateHelper.BuildAction<object, Exception>(ILogType, "Error");
            ErrorFormatDelegate = DelegateHelper.BuildAction<string, object[]>(ILogType, "ErrorFormat");

            FatalDelegate = DelegateHelper.BuildAction<object>(ILogType, "Fatal");
            FatalExceptionDelegate = DelegateHelper.BuildAction<object, Exception>(ILogType, "Fatal");

            DebugDelegate = DelegateHelper.BuildAction<object>(ILogType, "Debug");
            DebugExceptionDelegate = DelegateHelper.BuildAction<object, Exception>(ILogType, "Debug");
            DebugFormatDelegate = DelegateHelper.BuildAction<string, object[]>(ILogType, "DebugFormat");

            InfoDelegate = DelegateHelper.BuildAction<object>(ILogType, "Info");
            InfoExceptionDelegate = DelegateHelper.BuildAction<object, Exception>(ILogType, "Info");
            InfoFormatDelegate = DelegateHelper.BuildAction<string, object[]>(ILogType, "InfoFormat");

            WarnDelegate = DelegateHelper.BuildAction<object>(ILogType, "Warn");
            WarnExceptionDelegate = DelegateHelper.BuildAction<object, Exception>(ILogType, "Warn");
            WarnFormatDelegate = DelegateHelper.BuildAction<string, object[]>(ILogType, "WarnFormat");
        }

        public Log4NetLogger(object logger)
        {
            this.logger = logger;
        }

        public bool IsErrorEnabled
        {
            get { return IsErrorEnabledDelegate(logger); }
        }

        public bool IsFatalEnabled
        {
            get { return IsFatalEnabledDelegate(logger); }
        }

        public bool IsDebugEnabled
        {
            get { return IsDebugEnabledDelegate(logger); }
        }

        public bool IsInfoEnabled
        {
            get { return IsInfoEnabledDelegate(logger); }
        }

        public bool IsWarnEnabled
        {
            get { return IsWarnEnabledDelegate(logger); }
        }

        public void Error(object message)
        {
            if (IsErrorEnabled)
                ErrorDelegate(logger, message);
        }

        public void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
                ErrorExceptionDelegate(logger, message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
                ErrorFormatDelegate(logger, format, args);
        }

        public void Fatal(object message)
        {
            if (IsFatalEnabled)
                FatalDelegate(logger, message);
        }

        public void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
                FatalExceptionDelegate(logger, message, exception);
        }

        public void Debug(object message)
        {
            if (IsDebugEnabled)
                DebugDelegate(logger, message);
        }

        public void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
                DebugExceptionDelegate(logger, message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
                DebugFormatDelegate(logger, format, args);
        }

        public void Info(object message)
        {
            if (IsInfoEnabled)
                InfoDelegate(logger, message);
        }

        public void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
                InfoExceptionDelegate(logger, message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
                InfoFormatDelegate(logger, format, args);
        }

        public void Warn(object message)
        {
            if (IsWarnEnabled)
                WarnDelegate(logger, message);
        }

        public void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
                WarnExceptionDelegate(logger, message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
                WarnFormatDelegate(logger, format, args);
        }
    }

    internal static class DelegateHelper
    {
        public static Func<object, T> BuildPropertyGetter<T>(System.Type type, string propertyName)
        {
            var parameter = Expression.Parameter(typeof(object), "x");
            var instance = Expression.Convert(parameter, type);
            var property = Expression.Property(instance, propertyName);
            return Expression.Lambda<Func<object, T>>(property, parameter).Compile();
        }

        public static Action<object, T> BuildPropertySetter<T>(System.Type type, string propertyName)
        {
            var parameter = Expression.Parameter(typeof(object), "x");
            var instance = Expression.Convert(parameter, type);
            var property = Expression.Property(instance, propertyName);
            var value = Expression.Parameter(typeof(T), "value");
            var assign = Expression.Assign(property, value);
            return Expression.Lambda<Action<object, T>>(assign, parameter, value).Compile();
        }

        public static Action<object> BuildAction(System.Type type, string methodName)
        {
            var parameter = Expression.Parameter(typeof(object));
            var instance = Expression.Convert(parameter, type);
            var methodCall = Expression.Call(
                instance,
                GetMethod(type, methodName));

            return Expression.Lambda<Action<object>>(methodCall, parameter).Compile();
        }

        public static Action<object, T> BuildAction<T>(System.Type type, string methodName)
        {
            var parameter = Expression.Parameter(typeof(object), "x");
            var instance = Expression.Convert(parameter, type);

            var arg0 = Expression.Parameter(typeof(T), "arg0");

            var methodCall = Expression.Call(
                instance,
                GetMethod(type, methodName, typeof(T)),
                arg0);

            return Expression.Lambda<Action<object, T>>(methodCall, parameter, arg0).Compile();
        }

        public static Action<object, T1, T2> BuildAction<T1, T2>(System.Type type, string methodName)
        {
            var parameter = Expression.Parameter(typeof(object), "x");
            var instance = Expression.Convert(parameter, type);

            var arg0 = Expression.Parameter(typeof(T1), "arg0");
            var arg1 = Expression.Parameter(typeof(T2), "arg1");

            var methodCall = Expression.Call(
                instance,
                GetMethod(type, methodName, typeof(T1), typeof(T2)),
                arg0,
                arg1);

            return Expression.Lambda<Action<object, T1, T2>>(methodCall, parameter, arg0, arg1).Compile();
        }

        public static Func<object, TReturn> BuildFunc<TReturn>(System.Type type, string methodName)
        {
            var parameter = Expression.Parameter(typeof(object));
            var instance = Expression.Convert(parameter, type);
            var methodCall = Expression.Call(
                instance,
                GetMethod(type, methodName));

            return Expression.Lambda<Func<object, TReturn>>(methodCall, parameter).Compile();
        }

        private static MethodInfo GetMethod(System.Type type, string methodName, params System.Type[] types)
        {
            return type.GetMethod(
                methodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                types,
                null);
        }
    }
}