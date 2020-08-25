using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10_sh
{
    class LFSR
    {
        private StreamWriter registerOut;
        private StreamReader registerIn;

        private StreamWriter dataOut;
        private StreamReader dataIn;

        private int[] generateRegister()
        {
            int[] register = new int[64];
            Random secureRandom = new Random();
            for (int i = 0; i < register.Length; i++)
                register[i] = secureRandom.Next(2);
            return register;
        }

        private ulong generateGamma(int[] register)
        {
            StringBuilder g = new StringBuilder();
            int s = 0;
            for (int i = 0; i < 64; i++)
            {
                s = register[register.Length - 1];
                s = ((((s >> 15) ^ (s >> 4) ^ (s >> 2) ^ (s >> 1) ^ s) & 1) << 15) | (s >> 1);
                for (int k = register.Length - 1; k > 0; k--)
                    register[k] = register[k - 1];
                register[0] = s;
                if (s > 0)
                    g.Append(1);
                else
                    g.Append(0);
            }
            return Convert.ToUInt64(g.ToString(), 2);
        }

        private void overlayGamma(ulong gamma)
        {
            string line;
            while ((line = dataIn.ReadLine()) != null) {
                for (int i = 0; i < line.Length; i++)
                    dataOut.Write((char)(line[i] ^ gamma));
                dataOut.Write("\n");
            }
            dataIn.Close();
            dataOut.Close();
        }

        public void encrypt(String keyPath, String inputPath, String outputPath)
        {
            int[] key = generateRegister();
            registerOut = new StreamWriter(keyPath);
            dataIn = new StreamReader(inputPath);
            dataOut = new StreamWriter(outputPath);
            for (int i = 0; i < key.Length; i++)
                registerOut.Write(Convert.ToString(key[i]));
            registerOut.Close();
            overlayGamma(generateGamma(key));
            dataIn.Close();
            dataOut.Close();
        }

        public void decrypt(String keyPath, String inputPath, String outputPath)
        {
            int[] register = generateRegister();
            registerIn = new StreamReader(keyPath);
            dataIn = new StreamReader(inputPath);
            dataOut = new StreamWriter(outputPath);
            String k;
            if ((k = registerIn.ReadLine()) != null)
            {
                for (int i = 0; i < k.Length; i++)
                    register[i] = Convert.ToInt16(k[i].ToString(), 2);
                overlayGamma(generateGamma(register));
            }
            registerIn.Close();
            dataOut.Close();
            registerIn.Close();
        }
    }
}
