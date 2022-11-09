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
using System.Globalization;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
namespace Logiciel
{
    public partial class Form4 : Form
    {
        public UInt32 id_fichier = Donneespubliques.id_fichier;
        public Form4()
        {
            InitializeComponent();
            textBox_Diametre.KeyPress += new KeyPressEventHandler(keypressed_diametre);//Ce sont des évenements lors de l'appui sur une touche dans notre cas entré
            textBox_longueur.KeyPress += new KeyPressEventHandler(keypressed_longueur);
            textBox_diametremodification.KeyPress += new KeyPressEventHandler(keypressed_modif_diametre);
            textBox_longueurmodification.KeyPress += new KeyPressEventHandler(keypressed_modif_longueur);
            this.ControlBox = false;//Enleve la croix et reduire,agrandir
            

        }
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private string numero = "";
        private float longeur;
        private float diametre;
        private float cube;
        uint Id_arbre;

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
                    catch (Exception)
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
        private void Retriage()//Permet de tout retrier / actualiser avec les paramètre changé
        {

            string mesure = "";
            string id_arbre = "";
            if (radioButton_quart.Checked)
            {
                mesure = "quart";
            }
            else
            {
                if (radioButton_reel.Checked)
                {
                    mesure = "reel";
                }

            }
            
            float cubesup; float cubeinf;
            if ((mesure != ""))
            {
                if ((textBox_Cubeinf.Text != "") && (textBox_Cubesup.Text != "") && (float.TryParse(textBox_Cubeinf.Text, out cubeinf)) && (float.TryParse(textBox_Cubesup.Text, out cubesup)))
                {
                    Task x = ExecuteRequete("UPDATE Fichier SET CubeInf = '" + textBox_Cubeinf.Text + "', CubeSup ='" + textBox_Cubesup.Text + "', Mesure ='" + mesure + "' WHERE Id LIKE '" + Convert.ToString(id_fichier) + "';");
                }
                else { MessageBox.Show("Il faut rentrer des chiffres dans les cases Cube inférieur et Cube Inférieur. Si vous voulez mettre une virgule mettez une << , >>"); }
            }
            else { MessageBox.Show("Selectionner Quart ou Réel"); }
            ouverture_connexion();
            sql_cmd = new SQLiteCommand("SELECT Diametre,Longueur,Id FROM Arbre WHERE Id_Fichier LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            int i=0;
            while (query.Read())//Dure le temps de la requete
            {
                diametre = Convert.ToSingle(query.GetValue(0).ToString());
                longeur = Convert.ToSingle(query.GetValue(1).ToString());
                id_arbre = query.GetValue(2).ToString();

                if (mesure == "quart")
                {
                    cube = Convert.ToSingle((Math.Truncate((longeur * diametre * diametre * 0.007853931634) / 1.273) / 1000));
                }
                if (mesure == "reel")
                {
                    cube = Convert.ToSingle((Math.Truncate(longeur * diametre * diametre * 0.007853931634)) / 1000);
                }
                Task x = ExecuteRequete("UPDATE Arbre SET Cube = '" + cube.ToString("0.000", CultureInfo.InvariantCulture) + "' WHERE Id LIKE '" + id_arbre + "';");
                i++;
            }
            query.Close();
            for (int u = 0; u< i*1999999; u++)//Obligatoire sinon un appui sur le bouton ne suffit pas 
            {

            }
            Actualiser_total_cube();
            Actualiser_total_cube();//Deuxieme fois sinon bug
            SetUp_Tableau_Arbres();
            SetUp_Tableau_Arbres();//Deuxieme fois sinon bug
            fermeture_connexion();

        }
        private void SetUp_Tableau_Arbres()//Pour remplir la data gridview
        {
            ouverture_connexion();
            dataGridView_Arbres.Rows.Clear();
            float cubeinf = 0; float cubesup = 0;
            sql_cmd = new SQLiteCommand("SELECT CubeInf,CubeSup FROM Fichier WHERE Id LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                cubeinf = Convert.ToSingle(query.GetValue(0).ToString());
                cubesup = Convert.ToSingle(query.GetValue(1).ToString());
            }
            query.Close();
            sql_cmd = new SQLiteCommand("SELECT Numero,Longueur,Diametre,Cube FROM Arbre WHERE Id_Fichier LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
            query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                if (Convert.ToSingle(query.GetValue(3).ToString()) < (cubeinf / 1000))
                {
                    string[] ligne = { query.GetValue(0).ToString(), query.GetValue(1).ToString(), query.GetValue(2).ToString(), query.GetValue(3).ToString(), "", "" };
                    dataGridView_Arbres.Rows.Add(ligne);
                }
                else
                {
                    if (Convert.ToSingle(query.GetValue(3).ToString()) > (cubesup / 1000))
                    {
                        string[] ligne = { query.GetValue(0).ToString(), query.GetValue(1).ToString(), query.GetValue(2).ToString(), "", "", query.GetValue(3).ToString() };
                        dataGridView_Arbres.Rows.Add(ligne);
                    }
                }

                if ((cubeinf / 1000) <= Convert.ToSingle(query.GetValue(3).ToString()) && Convert.ToSingle(query.GetValue(3).ToString()) <= (cubesup / 1000))
                {
                    string[] ligne = { query.GetValue(0).ToString(), query.GetValue(1).ToString(), query.GetValue(2).ToString(), "", query.GetValue(3).ToString(), "" };
                    dataGridView_Arbres.Rows.Add(ligne);
                }
            }
            query.Close();


        }
        private void Form4_Load(object sender, EventArgs e)
        {
            button_Retour_acceuil.BackColor = Color.DarkOrange;//Couleur
            button_Retour_acceuil.FlatStyle = FlatStyle.Flat;
            button_Retour_acceuil.FlatAppearance.BorderColor = Color.DarkOrange;
            button_Retour_acceuil.FlatAppearance.BorderSize = 1;
            //
            //
            button_Enregister_commentaire.BackColor = Color.LimeGreen;//Couleur
            button_Enregister_commentaire.FlatStyle = FlatStyle.Flat;
            button_Enregister_commentaire.FlatAppearance.BorderColor = Color.LimeGreen;
            button_Enregister_commentaire.FlatAppearance.BorderSize = 1;
            //
            button_Enregistrer_arbre.BackColor = Color.LimeGreen;//Couleur
            button_Enregistrer_arbre.FlatStyle = FlatStyle.Flat;
            button_Enregistrer_arbre.FlatAppearance.BorderColor = Color.LimeGreen;
            button_Enregistrer_arbre.FlatAppearance.BorderSize = 1;
            //
            button_Imprimer.BackColor = Color.LimeGreen;//Couleur
            button_Imprimer.FlatStyle = FlatStyle.Flat;
            button_Imprimer.FlatAppearance.BorderColor = Color.LimeGreen;
            button_Imprimer.FlatAppearance.BorderSize = 1;
            


            this.Location = new Point(0, 0);//Form en haut a gauche
            id_fichier = Donneespubliques.id_fichier;
            dataGridView_Arbres.ColumnCount = 6;
            dataGridView_Arbres.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView_Arbres.Font, FontStyle.Bold);
            dataGridView_Arbres.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_Arbres.Columns[0].Name = "Pièce";
            dataGridView_Arbres.Columns[1].Name = "Longueur";
            dataGridView_Arbres.Columns[2].Name = "Diamètre";
            dataGridView_Arbres.Columns[3].Name = "Cube 1";
            dataGridView_Arbres.Columns[4].Name = "Cube 2";
            dataGridView_Arbres.Columns[5].Name = "Cube 3";
            dataGridView_Arbres.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_Arbres.MultiSelect = false;

            string mesure = "";
            string commentaire = "";
            //Va chercher les commentaire s'ils existent
            ouverture_connexion();
            sql_cmd = new SQLiteCommand("SELECT CubeInf,CubeSup,Mesure,Commentaire FROM Fichier WHERE Id LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                textBox_Cubeinf.Text = query.GetValue(0).ToString();
                textBox_Cubesup.Text = query.GetValue(1).ToString();
                mesure = query.GetValue(2).ToString();
                commentaire = query.GetValue(3).ToString();
            }
            query.Close();
            //On découpe les commentaire avec le marqueur ¤ ça permet d'avoir une seule colonne commentaire dans la BDD
            if (commentaire != "")
            {
                int i = 0;
                while (commentaire[i] != '¤')
                {
                    textBox_commentaire1.Text = textBox_commentaire1.Text + commentaire[i];
                    i++;
                }
                i++;
                while (commentaire[i] != '¤')
                {
                    textBox_commentaire2.Text = textBox_commentaire2.Text + commentaire[i];
                    i++;
                }
                i++;
                while (commentaire[i] != '¤')
                {
                    textBox_commentaire3.Text = textBox_commentaire3.Text + commentaire[i];
                    i++;
                }
            }
            //Initialisation des boutons 
            if (mesure == "reel")
            {
                radioButton_reel.Checked = true;
            }
            else
            {
                radioButton_quart.Checked = true;
            }
            fermeture_connexion();
            //Récupère le numéro de l'arbre où on s'était arreter
            ouverture_connexion();
            sql_cmd = new SQLiteCommand("SELECT Numero FROM Arbre WHERE Id_Fichier LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
            query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                numero = query.GetValue(0).ToString();
            }
            query.Close();
            if (numero == "")//Si on a rien saisi dans cette commande et qu'on l'ouvre
            {
                numero = "1";
            }
            else//On Incrémente de 1 le numéro on ne veut pas que les numéros se repetent
            {
                numero = Convert.ToString(Convert.ToUInt32(numero) + 1);
            }
            fermeture_connexion();
            label_Cube1_Ecorcededuite.Text = "0";
            label_Cube1_Ecorcenondeduite.Text = "0";
            label_Cube2_Ecorcededuite.Text = "0";
            label_Cube2_Ecorcenondeduite.Text = "0";
            label_Cube3_Ecorcededuite.Text = "0";
            label_Cube3_Ecorcenondeduite.Text = "0";
            label_Num.Text = numero;
            groupBox_Modificationarbre.Hide();
            Retriage();
            Retriage();

        }
        //Actualise la group box total cube
        private void Actualiser_total_cube()
        {
            label_Cube1_Ecorcededuite.Text = "0";
            label_Cube1_Ecorcenondeduite.Text = "0";
            label_Cube2_Ecorcededuite.Text = "0";
            label_Cube2_Ecorcenondeduite.Text = "0";
            label_Cube3_Ecorcededuite.Text = "0";
            label_Cube3_Ecorcenondeduite.Text = "0";
            string Typebois = "";
            string mesure = "";
            string Nb_arbre = "";
            ouverture_connexion();
            sql_cmd = new SQLiteCommand("SELECT Typedebois FROM Parcelle WHERE Id LIKE (SELECT Id_Parcelle FROM Fichier WHERE Id LIKE '" + Convert.ToString(id_fichier) + "' );", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                Typebois = query.GetString(0);
            }
            query.Close();
            fermeture_connexion();

            ouverture_connexion();

            float cubeinf = 0; float cubesup = 0;
            sql_cmd = new SQLiteCommand("SELECT CubeInf,CubeSup,Mesure FROM Fichier WHERE Id LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
            query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                cubeinf = Convert.ToSingle(query.GetValue(0).ToString());
                cubesup = Convert.ToSingle(query.GetValue(1).ToString());
                mesure = query.GetValue(2).ToString();
            }
            query.Close();
            fermeture_connexion();
            ouverture_connexion();
            for (int i = 0; i < 10000; i++)
            {

            }
            //en fonction du type de bois on a des pourcentages différents
            switch (Typebois)
            {
                case "Feuillus":
                    sql_cmd = new SQLiteCommand("SELECT Cube,Numero FROM Arbre WHERE Id_Fichier LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
                    query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        Nb_arbre = query.GetValue(1).ToString();
                        if (Convert.ToSingle(query.GetValue(0).ToString()) < (cubeinf / 1000))
                        {
                            label_Cube1_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube1_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                            label_Cube1_Ecorcededuite.Text = label_Cube1_Ecorcenondeduite.Text;//O% de perte sur les feuillus
                        }
                        else
                        {
                            if (Convert.ToSingle(query.GetValue(0).ToString()) > (cubesup / 1000))
                            {
                                label_Cube3_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube3_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                                label_Cube3_Ecorcededuite.Text = label_Cube3_Ecorcenondeduite.Text;//O% de perte sur les feuillus
                            }
                        }

                        if ((cubeinf / 1000) <= Convert.ToSingle(query.GetValue(0).ToString()) && Convert.ToSingle(query.GetValue(0).ToString()) <= (cubesup / 1000))
                        {
                            label_Cube2_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube2_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                            label_Cube2_Ecorcededuite.Text = label_Cube2_Ecorcenondeduite.Text;//O% de perte sur les feuillus
                        }
                    }
                    query.Close();
                    break;
                case "Pin":
                    sql_cmd = new SQLiteCommand("SELECT Cube,Numero FROM Arbre WHERE Id_Fichier LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
                    query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        Nb_arbre = query.GetValue(1).ToString();
                        if (Convert.ToSingle(query.GetValue(0).ToString()) < (cubeinf / 1000))
                        {
                            label_Cube1_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube1_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                            label_Cube1_Ecorcededuite.Text = Convert.ToString((Convert.ToSingle(label_Cube1_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) * 0.92).ToString("0.000", CultureInfo.InvariantCulture));//8% de perte 
                        }
                        else
                        {
                            if (Convert.ToSingle(query.GetValue(0).ToString()) > (cubesup / 1000))
                            {
                                label_Cube3_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube3_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                                label_Cube3_Ecorcededuite.Text = Convert.ToString((Convert.ToSingle(label_Cube3_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) * 0.92).ToString("0.000", CultureInfo.InvariantCulture));//8% de perte 
                            }
                        }

                        if ((cubeinf / 1000) <= Convert.ToSingle(query.GetValue(0).ToString()) && Convert.ToSingle(query.GetValue(0).ToString()) <= (cubesup / 1000))
                        {
                            label_Cube2_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube2_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                            label_Cube2_Ecorcededuite.Text = Convert.ToString((Convert.ToSingle(label_Cube2_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) * 0.92).ToString("0.000", CultureInfo.InvariantCulture));//8% de perte 
                        }
                    }
                    query.Close();
                    break;
                case "Sapin/Epicéa":
                    sql_cmd = new SQLiteCommand("SELECT Cube,Numero FROM Arbre WHERE Id_Fichier LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
                    query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        Nb_arbre = query.GetValue(1).ToString();
                        if (Convert.ToSingle(query.GetValue(0).ToString()) < (cubeinf / 1000))
                        {
                            label_Cube1_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube1_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                            label_Cube1_Ecorcededuite.Text = Convert.ToString((Convert.ToSingle(label_Cube1_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) * 0.90).ToString("0.000", CultureInfo.InvariantCulture));//10% de perte 
                        }
                        else
                        {
                            if (Convert.ToSingle(query.GetValue(0).ToString()) > (cubesup / 1000))
                            {
                                label_Cube3_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube3_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                                label_Cube3_Ecorcededuite.Text = Convert.ToString((Convert.ToSingle(label_Cube3_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) * 0.90).ToString("0.000", CultureInfo.InvariantCulture));//10% de perte 
                            }
                        }

                        if ((cubeinf / 1000) <= Convert.ToSingle(query.GetValue(0).ToString()) && Convert.ToSingle(query.GetValue(0).ToString()) <= (cubesup / 1000))
                        {
                            label_Cube2_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube2_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                            label_Cube2_Ecorcededuite.Text = Convert.ToString((Convert.ToSingle(label_Cube2_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) * 0.90).ToString("0.000", CultureInfo.InvariantCulture));//10% de perte 
                        }
                    }
                    query.Close();
                    break;
                case "Douglas":
                    sql_cmd = new SQLiteCommand("SELECT Cube,Numero FROM Arbre WHERE Id_Fichier LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
                    query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        Nb_arbre = query.GetValue(1).ToString();
                        if (Convert.ToSingle(query.GetValue(0).ToString()) < (cubeinf / 1000))
                        {
                            label_Cube1_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube1_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                            label_Cube1_Ecorcededuite.Text = Convert.ToString((Convert.ToSingle(label_Cube1_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) * 0.88).ToString("0.000", CultureInfo.InvariantCulture));//12% de perte 
                        }
                        else
                        {
                            if (Convert.ToSingle(query.GetValue(0).ToString()) > (cubesup / 1000))
                            {
                                label_Cube3_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube3_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                                label_Cube3_Ecorcededuite.Text = Convert.ToString((Convert.ToSingle(label_Cube3_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) * 0.88).ToString("0.000", CultureInfo.InvariantCulture));//12% de perte 
                            }
                        }

                        if ((cubeinf / 1000) <= Convert.ToSingle(query.GetValue(0).ToString()) && Convert.ToSingle(query.GetValue(0).ToString()) <= (cubesup / 1000))
                        {
                            label_Cube2_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube2_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(query.GetValue(0).ToString())).ToString("0.000", CultureInfo.InvariantCulture));
                            label_Cube2_Ecorcededuite.Text = Convert.ToString((Convert.ToSingle(label_Cube2_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) * 0.88).ToString("0.000", CultureInfo.InvariantCulture));//12% de perte 
                        }
                    }
                    break;

                default:
                    MessageBox.Show("Erreur Type de bois non trouvé");
                    break;
            }
            label_Total_Ecorcenondeduite.Text = Convert.ToString((Convert.ToSingle(label_Cube1_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(label_Cube2_Ecorcenondeduite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(label_Cube3_Ecorcenondeduite.Text, CultureInfo.InvariantCulture)).ToString("0.000", CultureInfo.InvariantCulture));
            label_Total_Ecorcededuite.Text = Convert.ToString((Convert.ToSingle(label_Cube1_Ecorcededuite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(label_Cube2_Ecorcededuite.Text, CultureInfo.InvariantCulture) + Convert.ToSingle(label_Cube3_Ecorcededuite.Text, CultureInfo.InvariantCulture)).ToString("0.000", CultureInfo.InvariantCulture));
            label_nombre_arbre_valeur.Text = Nb_arbre;
            if (mesure == "reel") { label_Typecubage_valeur.Text = "Réel"; }
            if (mesure == "quart") { label_Typecubage_valeur.Text = "Quart"; }

            fermeture_connexion();
        }
        private void button_Enregistrer_arbre_Click(object sender, EventArgs e)
        {

            if ((textBox_longueur.Text != "") && (textBox_Diametre.Text != ""))
            {
                if ((float.TryParse(textBox_longueur.Text, out longeur)) && (float.TryParse(textBox_Diametre.Text, out diametre)))
                {
                    string mesure = "";
                    ouverture_connexion();
                    sql_cmd = new SQLiteCommand("SELECT Mesure FROM Fichier WHERE Id LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
                    SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        mesure = query.GetString(0);
                    }
                    query.Close();
                    fermeture_connexion();

                    if (mesure == "quart")
                    {
                        cube = Convert.ToSingle((Math.Truncate((longeur * diametre * diametre * 0.007853931634) / 1.273) / 1000));
                    }

                    if (mesure == "reel")
                    {
                        cube = Convert.ToSingle((Math.Truncate(longeur * diametre * diametre * 0.007853931634)) / 1000);
                    }

                    Task x = ExecuteRequete("INSERT INTO Arbre (Longueur,Diametre,Numero,Cube,Id_Fichier) VALUES('" + textBox_longueur.Text + "','" + textBox_Diametre.Text + "','" + numero + "','" + Convert.ToString(cube.ToString("0.000", CultureInfo.InvariantCulture)) + "','" + Convert.ToString(id_fichier) + "')");

                    float cubeinf = 0; float cubesup = 0;
                    sql_cmd = new SQLiteCommand("SELECT CubeInf,CubeSup FROM Fichier WHERE Id LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
                    query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        cubeinf = Convert.ToSingle(query.GetValue(0).ToString());
                        cubesup = Convert.ToSingle(query.GetValue(1).ToString());
                    }
                    query.Close();
                    
                    if (cube < (cubeinf / 1000))
                    {
                        string[] ligne = { numero, textBox_longueur.Text, textBox_Diametre.Text, Convert.ToString(cube.ToString("0.000", CultureInfo.InvariantCulture)), "", "" };
                        dataGridView_Arbres.Rows.Add(ligne);
                    }
                    else
                    {
                        if (cube > (cubesup / 1000))
                        {
                            string[] ligne = { numero.ToString(), textBox_longueur.Text, textBox_Diametre.Text, "", "", Convert.ToString(cube.ToString("0.000", CultureInfo.InvariantCulture)) };
                            dataGridView_Arbres.Rows.Add(ligne);
                        }
                    }

                    if (((cubeinf / 1000) <= cube) && (cube <= (cubesup / 1000)))
                    {
                        string[] ligne = { numero.ToString(), textBox_longueur.Text, textBox_Diametre.Text, "", Convert.ToString(cube.ToString("0.000", CultureInfo.InvariantCulture)), "" };
                        dataGridView_Arbres.Rows.Add(ligne);
                    }
                    numero = Convert.ToString(Convert.ToUInt32(numero) + 1);
                    label_Volume_affiche.Text = Convert.ToString(cube.ToString("0.000", CultureInfo.InvariantCulture));
                    label_Num.Text = numero;
                    textBox_longueur.Text = "";
                    textBox_Diametre.Text = "";
                    dataGridView_Arbres.FirstDisplayedScrollingRowIndex = dataGridView_Arbres.RowCount - 1;
                    for (int i = 0; i < 100000; i++)//Attendre que la requete au dessus se fasse
                    {                    }
                    Actualiser_total_cube();
                    Actualiser_total_cube();
                    textBox_longueur.Select();
                }
                else { MessageBox.Show("Il faut ecrire des chiffres dans Longueur et Diamètre"); textBox_Diametre.Text = ""; textBox_longueur.Text = ""; }

            }
            else { MessageBox.Show("Il faut renseigner les cases Longueur et Diamètre "); }
        }
        
        

        private void button_Retriage_Click(object sender, EventArgs e)
        {
            Retriage();//2 fois sinon il ne s'effectue pas tout le temps
            Retriage();
        }
        //Permet de selectionner l'arbre que l'on veut modifier et met ses valeurs dans les textbox de modification
        private void button_Ok_Click(object sender, EventArgs e)
        {
    
            ouverture_connexion();
            uint Num_arbre;
            if ((textBox_Revenirarbre.Text != "") && (uint.TryParse(textBox_Revenirarbre.Text, out Num_arbre)) && (Num_arbre <= uint.Parse(numero)-1)) 
            {
                groupBox_nouvel_arbre.Hide();
                groupBox_Modificationarbre.Show();
                label_Numaffichemodification.Text = Convert.ToString(Num_arbre);

                sql_cmd = new SQLiteCommand("SELECT Diametre,Longueur,Id FROM Arbre WHERE Id_Fichier LIKE '" + Convert.ToString(id_fichier) + "' AND Numero LIKE '" + Convert.ToString(Num_arbre) + "';", sql_con);//Nouvelle commande
                SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                while (query.Read())//Dure le temps de la requete
                {
                    diametre = Convert.ToSingle(query.GetValue(0).ToString());
                    longeur = Convert.ToSingle(query.GetValue(1).ToString());
                    Id_arbre = Convert.ToUInt32(query.GetValue(2).ToString());

                }
                query.Close();
                textBox_longueurmodification.Text = Convert.ToString(longeur);
                textBox_diametremodification.Text = Convert.ToString(diametre);
                textBox_Revenirarbre.Text = "";
            }
            else { MessageBox.Show("Rentrer un numéro d'arbre valide"); textBox_Revenirarbre.Text = ""; }
            fermeture_connexion();
        }
        //Valide la modification 
        private void button_Modifierarbre_Click(object sender, EventArgs e)
        {
            string mesure = "";
            ouverture_connexion();
            if ((label_Arbrenummodification.Text != "") && (float.TryParse(textBox_diametremodification.Text, out diametre)) && (float.TryParse(textBox_longueurmodification.Text, out longeur)))
            {
                sql_cmd = new SQLiteCommand("SELECT Mesure FROM Fichier WHERE Id LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
                SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                while (query.Read())//Dure le temps de la requete
                {
                    mesure = query.GetString(0);
                }
                query.Close();
                fermeture_connexion();

                if (mesure == "quart")
                {
                    cube = Convert.ToSingle((Math.Truncate((longeur * diametre * diametre * 0.007853931634) / 1.273) / 1000));
                }
                if (mesure == "reel")
                {
                    cube = Convert.ToSingle((Math.Truncate(longeur * diametre * diametre * 0.007853931634)) / 1000);
                }

                Task x = ExecuteRequete("UPDATE Arbre SET Longueur = '" + textBox_longueurmodification.Text + "', Diametre = '" + textBox_diametremodification.Text + "', Cube = '" + Convert.ToString(cube.ToString("0.000", CultureInfo.InvariantCulture)) + "' WHERE Id LIKE '" + Id_arbre + "' ;");
                label_Volume_modification.Text = Convert.ToString(cube.ToString("0.000", CultureInfo.InvariantCulture));

                for (int i = 0; i < 100000; i++)
                {

                }

                groupBox_Modificationarbre.Hide();
                groupBox_nouvel_arbre.Show();
                fermeture_connexion();
                Retriage();
                textBox_longueur.Select();
            }
            else { MessageBox.Show("Rentrer des chiffres dans les cases Longueur et Diamètre"); textBox_diametremodification.Text = "";textBox_longueurmodification.Text = ""; }

        }

        private void label_Cube2__Ecorcenondeduite_Click(object sender, EventArgs e)
        {        }

        private void button_Retour_acceuil_Click(object sender, EventArgs e)
        {
            Form1 Acceuil = new Form1(Donneespubliques.chemin_bdd);
            Acceuil.Show();
            this.Close();
        }
        //Sauvegarde les modfications faite dans l'espace commentaire
        private void button_Enregister_commentaire_Click(object sender, EventArgs e)
        {
            string commentaire = textBox_commentaire1.Text + "¤" + textBox_commentaire2.Text + "¤" + textBox_commentaire3.Text+"¤";
            if (commentaire == "¤¤¤" )
            {
                Form2 Efface_commentaire = new Form2("Etes-vous sûr de vouloir supprimer vos commentaires ?");
                if (Efface_commentaire.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
                {
                    if (Efface_commentaire.Valeur == "Oui")
                    {
                        Task x = ExecuteRequete("UPDATE Fichier SET Commentaire = '" + "" + "' WHERE Id LIKE '" + Convert.ToString(id_fichier) + "';");
                    }
                }
            }
            else
            {
                Task x = ExecuteRequete("UPDATE Fichier SET Commentaire = '" + commentaire + "' WHERE Id LIKE '" + Convert.ToString(id_fichier) + "';");
            }
            fermeture_connexion();
        }
        //Lance la form5 qui gère l'impression si on annule on reviens sur la form4 sinon si on imprime on revient sur la form1
        private void button_Imprimer_Click(object sender, EventArgs e)
        {
            Retriage();
            Retriage();
            bool Imprimer = false;
            string[] ligne = { "","","", "", "","" };
            dataGridView_Arbres.Rows.Add(ligne);
            Form5 Impression = new Form5(dataGridView_Arbres,label_Total_Ecorcededuite.Text,label_Total_Ecorcenondeduite.Text,label_Cube1_Ecorcededuite.Text,label_Cube1_Ecorcenondeduite.Text, label_Cube2_Ecorcededuite.Text, label_Cube2_Ecorcenondeduite.Text, label_Cube3_Ecorcededuite.Text, label_Cube3_Ecorcenondeduite.Text);
            if (Impression.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
            {
                Imprimer = true;
            }
            if (Imprimer == true)
            {
                dataGridView_Arbres.Rows.RemoveAt(dataGridView_Arbres.RowCount - 1);
            }
       
            
        }

        private void dataGridView_Arbres_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox_Arbres_Enter(object sender, EventArgs e)
        {

        }
        //Fonctions aggissant sur la touche entré qu'on lie au textbox a l'initialisation
        private void keypressed_diametre(Object o, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                button_Enregistrer_arbre_Click(o,e);
                textBox_longueur.Select();
                e.Handled = true;
            }  
        }
        private void keypressed_modif_diametre(Object o, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                button_Modifierarbre_Click(o, e);
                textBox_longueurmodification.Select();
                e.Handled = true;
            }           
        }
        private void keypressed_longueur(Object o, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                textBox_Diametre.Select();
                e.Handled = true;
            }
        }
        private void keypressed_modif_longueur(Object o, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                textBox_diametremodification.Select();
                e.Handled = true;
            }
        }
        private void textBox_longueurmodification_TextChanged(object sender, EventArgs e)
        {

        }

        private void GroupBox_Totalcubes_Enter(object sender, EventArgs e)
        {

        }

        private void textBox_longueur_TextChanged(object sender, EventArgs e)
        {
            if (textBox_longueur.TextLength == 3)
            {
                textBox_Diametre.Select();
            }
        }
    }
}
