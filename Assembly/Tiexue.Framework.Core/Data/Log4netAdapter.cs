using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Data
{
    internal class Log4netAdapter : System.IO.TextWriter
    {
        private log4net.ILog _logger;
        public Log4netAdapter(log4net.ILog logger)
        {
            _logger = logger;
        }

        public override void Write(string value)
        {
            _logger.Debug(value);
        }
        private Encoding _encoding;
        public override Encoding Encoding
        {
            get
            {
                if (_encoding == null)
                {
                    _encoding = Encoding.UTF8;
                }
                return _encoding;
            }
        }
        public override void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer, index, count));
        }
    }
}
