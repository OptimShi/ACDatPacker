using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACDatPacker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Prompt for folder
            // OpenDialog browser
            string path = "D:\\ACE\\Portal\\";

            // Search for *.bin in folder
            var files = GetAllFiles(path);


            // PACK!
        }

        private List<string> GetAllFiles(string path)
        {
            List<string> files = new List<string>();
            DirectoryInfo d = new DirectoryInfo(path);

            foreach (var file in d.GetFiles("*.bin", SearchOption.AllDirectories))
            {
                files.Add(file.FullName);
            }
            return files;
        }
    }
}
