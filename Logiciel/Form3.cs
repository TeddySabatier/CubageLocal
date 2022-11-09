using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
namespace Logiciel
{
    public partial class Form3 : Form
    {
        public string nom_destinataire;
        public string nom_proprietaire;
        public string prenom_proprietaire;
        public string date_ajout;
        public string typedebois;
        public string lieu;
        public DateTime date_commande;
        public Form3(string nom_destinataire_don,string nom_proprietaire_don, string prenom_proprietaire_don, string date_ajout_don, string typedebois_don, string lieu_don,DateTime date_commande_don)
        {
            InitializeComponent();
            nom_destinataire = nom_destinataire_don;
            nom_proprietaire = nom_proprietaire_don;
            prenom_proprietaire = prenom_proprietaire_don;
            date_ajout = date_ajout_don;
            typedebois = typedebois_don;
            lieu = lieu_don;
            date_commande = date_commande_don;
            this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private void ouverture_connexion()
        {
            sql_con = new SQLiteConnection(Donneespubliques.chemin_bdd);
            sql_con.Open();
        }
        private void fermeture_connexion()
        {
            sql_con.Close();
        }
        public Task<bool> FaislaRequeteAsync(string txtRequete) //Permet d'effectuer un thraed a las fois
        {
            return Task.Run(() =>
            {
                using (SQLiteConnection myConn = new SQLiteConnection(Donneespubliques.chemin_bdd))
                {
                    try
                    {
                        myConn.Open();
                        using (SQLiteCommand sql_commande = new SQLiteCommand(txtRequete, myConn))
                        {
                            sql_commande.CommandText = txtRequete;
                            sql_commande.ExecuteNonQuery();
                            return true;
                        }
                    }
                    catch (Exception )
                    {
                        // do exception handling
                    }
                }


                return true;
            });
        }
        private async Task ExecuteRequete(string txtRequete)//Utilise la fontion au dessus
        {
            ouverture_connexion();//Neccessaire sinon possible déconnexion avant l'execution de la fonction
            bool L = await FaislaRequeteAsync(txtRequete);
            fermeture_connexion();
        }
        private string Date_Commande()//Renvoie la date du jour passé en paramètre avec un format défini
        {           
            string date_commande_text = "";
            if (date_commande.Month < 10)
            {
                if (date_commande.Day < 10)
                {
                    date_commande_text = "0" + date_commande.Day + "-0" + date_commande.Month + "-" + date_commande.Year + "";
                }
                else
                {
                    date_commande_text = "" + date_commande.Day + "-0" + date_commande.Month + "-" + date_commande.Year + "";
                }
            }
            else
            {
                if (date_commande.Day < 10)
                {
                    date_commande_text = "0" + date_commande.Day + "-" + date_commande.Month + "-" + date_commande.Year + "";
                }
                else
                {
                    date_commande_text = "" + date_commande.Day + "-" + date_commande.Month + "-" + date_commande.Year + "";
                }
            }
            return date_commande_text;
        }
        private void Form3_Load(object sender, EventArgs e)
        {//Récapitulatif et initialisation
            label_Info_propiétaire.Text = "Propriétaire : "+nom_proprietaire + " " + prenom_proprietaire + " \n" + typedebois + " à " + lieu;
            label_destinataire.Text = "Destinataire : " + nom_destinataire;
            textBox_Cubeinf.Text = "400";
            textBox_Cubesup.Text = "700";

            button_Annuler.BackColor = Color.DarkOrange;//Couleur
            button_Annuler.FlatStyle = FlatStyle.Flat;
            button_Annuler.FlatAppearance.BorderColor = Color.DarkOrange;
            button_Annuler.FlatAppearance.BorderSize = 1;
            //
            //
            button_Creation_commande.BackColor = Color.LimeGreen;//Couleur
            button_Creation_commande.FlatStyle = FlatStyle.Flat;
            button_Creation_commande.FlatAppearance.BorderColor = Color.LimeGreen;
            button_Creation_commande.FlatAppearance.BorderSize = 1;
        }

        private void label1_Click(object sender, EventArgs e)
        {        }

        private void radioButton_reel_CheckedChanged(object sender, EventArgs e)
        {        }

        private void button_Annuler_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 Ecran_acceuil = new Form1(Donneespubliques.chemin_bdd);
            Ecran_acceuil.Show();
        }

        private void button_Creation_commande_Click(object sender, EventArgs e)
        {
            string mesure="";
            if (radioButton_quart.Checked)
            {
                mesure = "quart";
            }
            else//Sinon aucun coché ou réel
            {
                if(radioButton_reel.Checked)//Si reel
                {
                    mesure = "reel";
                }
                
            }
            float cubesup;float cubeinf;string date=Date_Commande();
            //Création commande
            if ((mesure != ""))
            {
                if ((textBox_Cubeinf.Text != "") && (textBox_Cubesup.Text != "") && (float.TryParse(textBox_Cubeinf.Text, out cubeinf)) && (float.TryParse(textBox_Cubesup.Text, out cubesup)))//Verification que les cases ne sont pas vide et que les cubages saisi sont des float
                {
                    //Création de la nouvelle commande
                    Task x = ExecuteRequete("INSERT INTO Fichier (CubeInf,CubeSup,Id_Destinataire,Id_Parcelle,Date,Mesure) " +
                       "VALUES('" + cubeinf + "','" + cubesup + "',(SELECT Id FROM Destinataire WHERE Nom LIKE '" + nom_destinataire + "') ,(SELECT Id FROM Parcelle WHERE Typedebois LIKE '" + typedebois + "' AND Lieu LIKE '" + lieu +"'),'" + date + "','" + mesure + "')");

                    ouverture_connexion();
                    bool test = false;
                    while (test == false)//Récuperation de l'id fichier soit l'id de la commande
                    {
                        sql_cmd = new SQLiteCommand("SELECT Id FROM Fichier WHERE Id_Destinataire LIKE (SELECT Id FROM Destinataire WHERE Nom LIKE '" + nom_destinataire + "') AND Id_Parcelle LIKE (SELECT Id FROM Parcelle WHERE Typedebois LIKE '" + typedebois + "' AND Lieu LIKE '" + lieu + "') AND Date LIKE '" + date + "' ;", sql_con);//Nouvelle commande
                        SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                        while (query.Read())//Dure le temps de la requete
                        {
                            Donneespubliques.id_fichier = Convert.ToUInt32(query.GetValue(0).ToString());
                            test = true;
                        }
                        query.Close();
                    }
                    fermeture_connexion();
                    
                    Form4 Saisie_Arbres = new Form4();
                    Saisie_Arbres.Show();
                    this.Close();

                }
                else { MessageBox.Show("Il faut rentrer des chiffres dans les cases Cube inférieur et Cube Inférieur. Si vous voulez mettre une virgule mettez une << , >>"); }
            }
            else { MessageBox.Show("Selectionner Quart ou Réel"); }
            
        }
    }
}
