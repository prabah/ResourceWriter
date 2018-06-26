using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICISResourceWriter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var resourceFilepath = "..\\..\\ICIS.Resource.test.resx";
            Hashtable data = new Hashtable();
            data.Add("Item1", "测试");
            data.Add("Item2", "Value3");

            var resx = new List<DictionaryEntry>();
            using (var reader = new ResXResourceReader(resourceFilepath))
            {
                resx = reader.Cast<DictionaryEntry>().ToList();
                foreach (var key in data.Keys)
                {
                    var existingResource = resx.Where(r => r.Key.ToString() == key.ToString()).FirstOrDefault();
                    if (existingResource.Key == null && existingResource.Value == null) // NEW!
                    {
                        resx.Add(new DictionaryEntry() { Key = key, Value = data[key] });
                    }
                    else // MODIFIED RESOURCE!
                    {
                        var modifiedResx = new DictionaryEntry() { Key = existingResource.Key, Value = data[key] };
                        resx.Remove(existingResource);  // REMOVING RESOURCE!
                        resx.Add(modifiedResx);  // AND THEN ADDING RESOURCE!
                    }
                }
            }
            using (var writer = new ResXResourceWriter(resourceFilepath))
            {
                resx.ForEach(r =>
                {
                    // Again Adding all resource to generate with final items
                    writer.AddResource(r.Key.ToString(), r.Value.ToString());
                });
                writer.Generate();
            }

        }
    }
}
