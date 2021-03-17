using System;
using System.Collections.Generic;
using System.Text;

namespace ImageSharePassword.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime TimeUploaded { get; set; }
        public string Password { get; set; }
        public int Views { get; set; }

        public object ToIenumerable()
        {
            throw new NotImplementedException();
        }
    }
}
