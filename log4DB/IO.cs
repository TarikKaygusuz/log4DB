using System;
using System.IO;
using System.Text;

namespace log4DB
{
    internal class IO
    {
        StreamReader sReader = null;
        StreamWriter sWriter = null;

        /// <summary>
        /// Tek bir bilgi yazmak için
        /// </summary>
        /// <param name="path">direk dosya ismi verirsen exe ile aynı yere yaratır dosyayı.path de verebilirsin.Tekrar çalıştırıldıgında dosyanın üzerine yazar,eklemez.</param>
        /// <param name="metin">yazılacak bilgi</param>
        public void WritetoTextFile(string path, string metin)
        {
            FileInfo finfo = new FileInfo(path);
            sWriter = finfo.CreateText();

            sWriter.Write(metin);
            sWriter.Close();

        }

        /// <summary>
        /// Dosya yoksa oluşturur,varsa üzerine yazar.
        /// </summary>
        public void AppendtoText(string path, string metin)
        {
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (sWriter = File.CreateText(path))
                {
                    sWriter.WriteLine(metin);
                }
            }
            else
            {
                using (sWriter = File.AppendText(path))
                {
                    sWriter.WriteLine(metin);
                }
            }
        }

        public string ReadfromTextFile(string path)
        {
            sReader = File.OpenText(path);
            string output = null;
            output = sReader.ReadToEnd();
            sReader.Close();
            return output;
        }

        /// <param name="path">Okunacak ve geriye string olarak dönülecek dosyanın yolu</param>
        public string ReadFileAsstring(string path)
        {

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] abyt = new byte[Convert.ToInt32(fs.Length)];
            fs.Read(abyt, 0, abyt.Length);
            fs.Close();
            return Encoding.UTF8.GetString(abyt);

        }

        /// <param name="path">Okunacak ve geriye string olarak dönülecek dosyanın yolu</param>
        public string ReadFromStream(string path)
        {
            string returnValue = string.Empty;
            sReader = new StreamReader(path);
            returnValue = sReader.ReadToEnd();
            sReader.Close();
            return returnValue;
        }

    }
}
