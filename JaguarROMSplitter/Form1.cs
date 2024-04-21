namespace JaguarROMSplitter
{
    public partial class Form1 : Form
    {
        byte[] romImage = new byte[0];
        int romSize = 0;
        string fullpath = "";

        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "ROM files (*.jag)|*.jag"
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                romImage = File.ReadAllBytes(openFileDialog1.FileName);
                romSize = romImage.Length;
                textBox2.AppendText(openFileDialog1.SafeFileName+" loaded"+Environment.NewLine+"ROM Size: " + romSize.ToString()+" bytes"+Environment.NewLine);
                fullpath = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] romHi = new byte[romSize / 2];
            byte[] romLo = new byte[romSize / 2];
            for (int i=0; i<romSize/4; i++)
            {
                romHi[i * 2 + 1] = romImage[i * 4 + 0];
                romHi[i * 2 + 0] = romImage[i * 4 + 1];
                romLo[i * 2 + 1] = romImage[i * 4 + 2];
                romLo[i * 2 + 0] = romImage[i * 4 + 3];
            }
            string filename = Path.GetDirectoryName(fullpath) +"\\"+ Path.GetFileNameWithoutExtension(fullpath);
            if(checkBox1.Checked)
            {
                textBox2.AppendText("Split by 512k chunks:" + Environment.NewLine);
                int offset = 0;
                int index = 0;
                int chunk = 524288; //512k chunk
                while (offset < romSize/2)
                {
                    string hifile = filename + "_0" + index.ToString() + "_hi.hex";
                    string lofile = filename + "_0" + index.ToString() + "_lo.hex";
                    File.WriteAllBytes(hifile, romHi.Skip(offset).Take(chunk).ToArray());
                    textBox2.AppendText(hifile + " SAVED" + Environment.NewLine);
                    File.WriteAllBytes(lofile, romLo.Skip(offset).Take(chunk).ToArray());
                    textBox2.AppendText(lofile + " SAVED" + Environment.NewLine);
                    offset += chunk;
                    index++;
                }
            }
            else
            {
                File.WriteAllBytes(filename + "_hi.hex", romHi);
                textBox2.AppendText(filename + "_hi.hex SAVED" + Environment.NewLine);
                File.WriteAllBytes(filename + "_lo.hex", romLo);
                textBox2.AppendText(filename + "_lo.hex SAVED" + Environment.NewLine);
            }
            textBox2.AppendText("Done");
            //File.WriteAllBytes()

        }
    }
}