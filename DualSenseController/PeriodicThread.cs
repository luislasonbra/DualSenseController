using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DualSenseController
{
    public class PeriodicThread
    {
        Action callback;
        long milliseconds;
        Thread _thread;
        bool _running = false;
        bool _runAtStartup = false;
        bool _runAtStartupUser = false;

        public PeriodicThread(Action callback, long milliseconds) : this(callback, milliseconds, false) { }

        public PeriodicThread(Action callback, long milliseconds, bool runAtStartup)
        {
            this.callback = callback;
            this.milliseconds = milliseconds;
            _thread = new Thread(new ThreadStart(build));
            _runAtStartup = _runAtStartupUser = runAtStartup;
        }

        public PeriodicThread Start()
        {
            if (_thread == null) return this;
            _running = true;
            _thread.Start();
            return this;
        }

        public void Abort()
        {
            _running = false;
            if (_thread != null)
            {
                _thread.Interrupt();
                _thread = null;
            }
        }

        private void build()
        {
            // https://stackoverflow.com/questions/31840902/my-fps-counter-is-in-accurate-any-ideas-why
            DateTime _lastTime = DateTime.Now;
            while (_running)
            {
                if (!_running) return;
                if ((DateTime.Now - _lastTime).TotalMilliseconds >= milliseconds || _runAtStartup)
                {
                    _runAtStartup = false;
                    _lastTime = DateTime.Now;
                    if (callback != null)
                    {
                        callback.Invoke();
                    }
                }

                // fixed cpu usage
				try
                {
					Thread.Sleep(2);
				}
                catch { }
			}
        }
    }
}
