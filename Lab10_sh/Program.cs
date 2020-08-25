using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10_sh
{
    class Program
    {
        static void Main(string[] args)
        {
            LFSR lfsr = new LFSR();
            lfsr.encrypt("register.txt", "plane.txt", "encrypted.txt");
            lfsr.decrypt("register.txt", "encrypted.txt", "decrypted.txt");
        }
    }
}
