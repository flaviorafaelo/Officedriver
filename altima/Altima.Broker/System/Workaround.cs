using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Altima.Broker.System
{
    public static class Workaround
    {
        public static List<string> GetFiles (string searchPattern)
        {
          return Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, searchPattern).ToList<string>();
        }
    }
}
