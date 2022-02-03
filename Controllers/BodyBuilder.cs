using System;

namespace AftebiClassroom.Controllers
{
    internal class BodyBuilder
    {
        public string HtmlBody { get; internal set; }

        internal object ToMessageBody()
        {
            throw new NotImplementedException();
        }
    }
}