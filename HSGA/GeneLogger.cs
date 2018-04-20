using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace HSGA
{
    public class GeneLogger
    {

        string logPath = "C:\\Users\\Elliott\\Documents\\Visual Studio 2017\\Projects\\HSGA\\Assets";
        ListBox log;
        int maxLogLines = 200;

        public GeneLogger(ListBox l)
        {
            log = l;
        }

        public GeneLogger()
        {

        }


        public void AddToLog(string text)
        {
            log.Items.Add(text);
            AddLogToFile(text);
            if(log.Items.Count > maxLogLines)
            {
                log.Items.RemoveAt(0);
            }
        }

        public void AddLogToFile(string text)
        {
            using (StreamWriter sw = File.AppendText(logPath + "\\geneLog.txt"))
            {
                sw.WriteLine(text + "\n");
                sw.Flush();
            }


        }

        
    }
}
