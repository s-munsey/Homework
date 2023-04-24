using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHomework.Services.Interfaces
{
    public interface IFileService
    {
        void AppendAllText(string contents);
        void WriteAllLines(string[] contents);
    }
}
