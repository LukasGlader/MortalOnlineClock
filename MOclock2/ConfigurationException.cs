
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOclock2
{
    class ConfigurationException: Exception
    {
        private string p;

        public ConfigurationException(string message): base(message)
        {
        }
    }
}
