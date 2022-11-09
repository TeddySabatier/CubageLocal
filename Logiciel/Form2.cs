using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logiciel
{
    public partial class Form2 : Form
    {
        public Form2(string element)
        {
            InitializeComponent();
            valeur = "Attente";
            button_Oui.DialogResult = DialogResult.OK;
            button_Non.DialogResult = DialogResult.OK;
            label_Texte.Text = element;
            this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private string valeur;
        public string Valeur
        {
            get { return valeur; }
            set { valeur = value; }
        }
        
        private void button_Non_Click(object sender, EventArgs e)
        {
            valeur = "Non";           
            this.Hide();
        }

        private void button_Oui_Click(object sender, EventArgs e)
        {
            valeur = "Oui";           
            this.Hide();
        }

        private void label_Suprimercedestinataire_Click(object sender, EventArgs e)
        {        }

        private void Form2_Load(object sender, EventArgs e)
        {        }
    }
}
