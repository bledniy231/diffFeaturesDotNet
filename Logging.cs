using System;
using System.IO;

namespace SecondLabNet
{
    public class Logger<T> where T : EventArgs
    {
        public event EventHandler<T> LoggerEvent;
        public virtual void OnLoggerEvent(T e)
        {
            EventHandler<T> eventHandler = LoggerEvent;
            eventHandler.Invoke(this, e);
        }
    }

    public class MyEventArgs : EventArgs
    {
        public string message;
        public MyEventArgs(string _message = "No message")
        {
            message = _message;
        }
    }

    public interface ILogger<T> where T : EventArgs
    {
        void DoEvent(object sender, T e);
    }

    public class ConsoleLogger : ILogger<MyEventArgs>
    {
        public void DoEvent(object sender, MyEventArgs e)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}: {e.message}");
        }

        public ConsoleLogger(Logger<MyEventArgs> doEvent)
        {
            doEvent.LoggerEvent += new EventHandler<MyEventArgs>(DoEvent);
        }
    }

    public class FileLogger : ILogger<MyEventArgs>, IDisposable
    {
        public StreamWriter file;
        public void DoEvent(object sender, MyEventArgs e)
        {
            file.WriteLineAsync($"{DateTime.Now.ToString()}: {e.message}");
            file.Flush();
        }

        public FileLogger(string path, Logger<MyEventArgs> doEvent)
        {
            file = new StreamWriter(path, append: true);
            doEvent.LoggerEvent += new EventHandler<MyEventArgs>(DoEvent);
        }

        public void Dispose()
        {
            file.Close();
        }
    }
}
