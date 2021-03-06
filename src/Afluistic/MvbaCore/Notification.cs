//  * **************************************************************************
//  * Copyright (c) McCreary, Veselka, Bragg & Allen, P.C.
//  * This source code is subject to terms and conditions of the MIT License.
//  * A copy of the license can be found in the License.txt file
//  * at the root of this distribution. 
//  * By using this source code in any fashion, you are agreeing to be bound by 
//  * the terms of the MIT License.
//  * You must not remove this notice from this software.
//  * **************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace Afluistic.MvbaCore
{
    public class Notification<T> : Notification
    {
        public Notification()
        {
        }

        public Notification(NotificationMessage notificationMessage)
            : base(notificationMessage)
        {
        }

        public Notification(Notification notification)
            : this(notification, default(T))
        {
        }

        public Notification(Notification notification, T item)
        {
            Item = item;
            Add(notification);
        }

        public new static Notification<T> Empty
        {
            get { return new Notification<T>(); }
        }

        public T Item { get; set; }

        public static implicit operator T(Notification<T> notification)
        {
            if (notification.HasErrors)
            {
                throw new ArgumentNullException(string.Format("Cannot implicitly cast Notification<{0}> to {0} when there are errors.", typeof(T).Name));
            }
            return notification.Item;
        }

        public static implicit operator Notification<T>(T item)
        {
            return new Notification<T>
            {
                Item = item
            };
        }
    }

    public abstract class NotificationBase
    {
        protected NotificationBase()
        {
            Messages = new List<NotificationMessage>();
        }

        protected NotificationBase(NotificationMessage notificationMessage)
            : this()
        {
            AddMessage(notificationMessage);
        }

        public string Errors
        {
            get { return !HasErrors ? "" : GetMessages(x => x.Severity == NotificationSeverity.Error); }
        }

        public string ErrorsAndWarnings
        {
            get { return !(HasErrors || HasWarnings) ? "" : GetMessages(x => x.Severity == NotificationSeverity.Error || x.Severity == NotificationSeverity.Warning); }
        }

        public bool HasErrors { get; private set; }

        public bool HasWarnings { get; private set; }

        public string Infos
        {
            get { return GetMessages(x => x.Severity == NotificationSeverity.Info); }
        }

        public bool IsValid
        {
            get { return !(HasErrors || HasWarnings); }
        }

        public List<NotificationMessage> Messages { get; private set; }
        public string Warnings
        {
            get { return !HasWarnings ? "" : GetMessages(x => x.Severity == NotificationSeverity.Warning); }
        }

        public void Add(Notification notification)
        {
            foreach (var message in notification.Messages)
            {
                AddMessage(message);
            }
        }

        public void Add(NotificationMessage message)
        {
            AddMessage(message);
        }

        private void AddMessage(NotificationMessage message)
        {
            if (!(Messages.Any(x => x.Severity == message.Severity && x.Message == message.Message)))
            {
                switch (message.Severity)
                {
                    case NotificationSeverity.Error:
                        HasErrors = true;
                        break;
                    case NotificationSeverity.Warning:
                        HasWarnings = true;
                        break;
                }
                Messages.Add(message);
            }
        }

        private string GetMessages(Func<NotificationMessage, bool> predicate)
        {
            return Messages.Where(predicate).Select(x => x.Message).Join(Environment.NewLine);
        }

        public override string ToString()
        {
            return ErrorsAndWarnings;
        }
    }

    public class Notification : NotificationBase
    {
        public Notification()
        {
        }

        public Notification(NotificationMessage notificationMessage)
            : base(notificationMessage)
        {
        }

        public static Notification Empty
        {
            get { return new Notification(); }
        }

        public static Notification ErrorFor(string messageText)
        {
            return For(NotificationSeverity.Error, messageText);
        }

        public static Notification ErrorFor(string messageFormatString, params object[] messageParameters)
        {
            return For(NotificationSeverity.Error, messageFormatString, messageParameters);
        }

        public static Notification For(NotificationSeverity severity, string messageText)
        {
            return new Notification(new NotificationMessage(severity, messageText));
        }

        private static Notification For(NotificationSeverity severity, string messageFormatString, params object[] messageParameters)
        {
            return new Notification(new NotificationMessage(severity, messageFormatString, messageParameters));
        }

        public static Notification InfoFor(string messageText)
        {
            return For(NotificationSeverity.Info, messageText);
        }

        public static Notification InfoFor(string messageFormatString, params object[] messageParameters)
        {
            return For(NotificationSeverity.Info, messageFormatString, messageParameters);
        }

        public static Notification WarningFor(string messageText)
        {
            return For(NotificationSeverity.Warning, messageText);
        }

        public static Notification WarningFor(string messageFormatString, params object[] messageParameters)
        {
            return For(NotificationSeverity.Warning, messageFormatString, messageParameters);
        }
    }

    public static class NotificationExtensions
    {
        public static Notification<T> ToNotification<T>(this Notification notification)
        {
            // because we can't implicitly cast up from a base class
            return new Notification<T>(notification);
        }

        public static Notification<T> ToNotification<T>(this Notification notification, T item)
        {
            return new Notification<T>(notification, item);
        }
    }
}