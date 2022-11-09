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
using System.IO;

namespace Logiciel
{
    public partial class Form1 : Form
    {

        public Form1(string chemin = @"data source = C:\Logiciel_Cubage\Base_de_donne\Base_de_donnee.db")
        {
            InitializeComponent();
            Donneespubliques.chemin_bdd = chemin;
            ouverture_connexion();
            Actualiser_Liste_Destinataire();
            Actualiser_Liste_Proprietaire();
            fermeture_connexion();
            this.ControlBox = false;
            


        }
        
        //Déclaration des outils
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private bool test = false;
        private string mdp = "0000";

        public static void CopyDir(string sourceDir, string destDir)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir); if (dir.Exists)
            {
                string realDestDir;
                if (dir.Root.Name != dir.Name)
                {
                    realDestDir = Path.Combine(destDir, dir.Name);
                    if (!Directory.Exists(realDestDir))
                        Directory.CreateDirectory(realDestDir);
                }
                else realDestDir = destDir;
                foreach (string d in Directory.GetDirectories(sourceDir))
                    CopyDir(d, realDestDir);
                foreach (string file in Directory.GetFiles(sourceDir))
                {

                    string fileNameDest = Path.Combine(realDestDir, Path.GetFileName(file));
                    

                    File.Copy(file, fileNameDest, true);
                }
            }
        }

        private void textBox1_Nom_proprietaire_TextChanged(object sender, EventArgs e)
        {

        }
        //Fonction qui connecte notre application a la BDD SQLite
        private void ouverture_connexion()
        {
            
            sql_con = new SQLiteConnection(Donneespubliques.chemin_bdd+";foreign keys = true;");
            sql_con.Open();

        }
        //Fonction qui ferme la connexion a la BDD SQLite
        private void fermeture_connexion()
        {
            sql_con.Close();
        }
        //Fonction qui va executer la Query (Faire la requete SQL)
        public Task<bool> FaislaRequeteAsync(string txtRequete) //Permet d'effectuer un thread a la fois
        {
            return Task.Run(() =>
            {
                using (SQLiteConnection myConn = new SQLiteConnection(Donneespubliques.chemin_bdd+";foreign keys = true;"))
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
        //Utilisation de  Task pour s'assurer que la requete est finis avant de fermer la connexion
        private async Task ExecuteRequete(string txtRequete)//Utilise la fontion au dessus
        {
            ouverture_connexion();//Neccessaire sinon possible déconnexion avant l'execution de la fonction
            bool L = await FaislaRequeteAsync(txtRequete);
            fermeture_connexion();

        }
        //Permet de vider les textboxs
        private void Vider_champs()
        {
            textBox_Nom_Proprietaire.Text = "";
            textBox_Prenom_Proprietaire.Text = "";
            textBox_Date_ajout.Text = "";

            textBox_Typedebois.Text = "";
            textBox_Lieu.Text = "";

            textBox_Recherche_destinataire.Text = "";

            label_Datefichier.Text = "";


        }
        //Va chercher les données dans la BDD sur les destinataires et les insère dans la listbox destinataire
        private void Actualiser_Liste_Destinataire()
        {
            for (int i = 0; i < 1000000; i++) { }//Attend que la requete Sql soit effectuer avant de mettre a jour la liste
            ouverture_connexion();
            listBox_Destinataire.Items.Clear();
            sql_cmd = new SQLiteCommand("SELECT Nom FROM Destinataire", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                listBox_Destinataire.Items.Add(query.GetString(0));
            }
            query.Close();
        }
        //Va chercher les données dans la BDD sur les proprietaire et les insère dans la listbox proprietaire
        private void Actualiser_Liste_Proprietaire()
        {
            for (int i = 0; i < 1000000; i++) { }//Attend que la requete Sql soit effectuer avant de mettre a jour la liste
            ouverture_connexion();
            listBox_Nom_Proprietaire.Items.Clear();
            listBox_Prenom_Proprietaire.Items.Clear();
            listBox_Date_ajout.Items.Clear();
            sql_cmd = new SQLiteCommand("SELECT Nom,Prenom,Date_ajout FROM Proprietaire", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                listBox_Nom_Proprietaire.Items.Add(query.GetString(0));
                listBox_Prenom_Proprietaire.Items.Add(query.GetString(1));
                listBox_Date_ajout.Items.Add(query.GetString(2));
            }
            query.Close();


        }
        //Va chercher les données dans la BDD sur les parcelles et les insère dans la listbox parcelle
        private void Actualiser_Liste_Parcelle()
        {
            for (int i = 0; i < 1000000; i++) { }//Attent que la requete Sql soit effectuer avant de mettre a jour la liste
            ouverture_connexion();
            listBox_Typedebois.Items.Clear();
            listBox_Lieu.Items.Clear();
            sql_cmd = new SQLiteCommand("SELECT Typedebois,Lieu FROM Parcelle", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                listBox_Typedebois.Items.Add(query.GetString(0));
                listBox_Lieu.Items.Add(query.GetString(1));
            }
            query.Close();
        }
        //Va chercher les données dans la BDD sur les fichiers et les insère dans la listbox fichier
        private void Actualiser_Liste_Fichier()
        {
            if ((textBox_Recherche_destinataire.Text != "") && (textBox_Nom_Proprietaire.Text != "") && (textBox_Prenom_Proprietaire.Text != "") &&
                           (textBox_Typedebois.Text != "") && (textBox_Lieu.Text != "") && (textBox_Date_ajout.Text != ""))
            {
                ouverture_connexion();
                listBox_Datefichier.Items.Clear();
                sql_cmd = new SQLiteCommand("SELECT Date FROM Fichier WHERE Id_Destinataire LIKE (SELECT Id FROM Destinataire WHERE Nom LIKE '" + textBox_Recherche_destinataire.Text + "') AND Id_Parcelle LIKE (SELECT Id FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "' AND Lieu LIKE '" + textBox_Lieu.Text + "')", sql_con);//Nouvelle commande
                SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                while (query.Read())//Dure le temps de la requete
                {
                    listBox_Datefichier.Items.Add(query.GetString(0));
                }
                query.Close();
                fermeture_connexion();
            }
        }
        //Renvoie la date du jour avec un format défini
        private string Date_jour()
        {
            DateTime ajd = DateTime.Now;
            string ajd_text = "";
            if (ajd.Month < 10)
            {
                if (ajd.Day < 10)
                {
                    ajd_text = "0" + ajd.Day + "-0" + ajd.Month + "-" + ajd.Year + "";
                }
                else
                {
                    ajd_text = "" + ajd.Day + "-0" + ajd.Month + "-" + ajd.Year + "";
                }
            }
            else
            {
                if (ajd.Day < 10)
                {
                    ajd_text = "0" + ajd.Day + "-" + ajd.Month + "-" + ajd.Year + "";
                }
                else
                {
                    ajd_text = "" + ajd.Day + "-" + ajd.Month + "-" + ajd.Year + "";
                }
            }
            return ajd_text;
        }

        private void button_Validation_creation_destinataire_Click(object sender, EventArgs e)
        {
            if (textBox_Creation_nouveau_destinataire.Text != "")
            {
                ouverture_connexion();
                test = false;//Variable pour les tests
                sql_cmd = new SQLiteCommand("SELECT Nom FROM Destinataire", sql_con);//Nouvelle commande
                SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                while (query.Read())//Dure le temps de la requete
                {
                    if (query.GetString(0) == textBox_Creation_nouveau_destinataire.Text)//Compare aux éléments existants
                    {
                        test = true;
                    }

                }
                query.Close();
                //Si test est vrai alors ce destinataire existe deja sinon on l'ajoute
                if (test == true) { MessageBox.Show("Ce destinataire existe déja vous pouvez le selectionner"); }
                else
                {
                    Task x = ExecuteRequete("INSERT INTO Destinataire (Nom) VALUES('" + textBox_Creation_nouveau_destinataire.Text + "')");
                    for (int i = 0; i < 99999999; i++)
                    {

                    }
                    Actualiser_Liste_Destinataire();
                    Actualiser_Liste_Destinataire();
                    textBox_Recherche_destinataire.Text = "";
                    label_Datefichier.Text = "";
                    listBox_Datefichier.Items.Clear();

                }
                fermeture_connexion();
            }
            else { MessageBox.Show("Rentrer un destinataire"); }
            textBox_Creation_nouveau_destinataire.Text = "";
        }



        private void button_Suppression_destinataire_Click(object sender, EventArgs e)
        {
            ouverture_connexion();
            if (textBox_mdp.Text == mdp)
            {

                if (textBox_Recherche_destinataire.Text != "")
                {
                    Form2 Suppression_Destinataire = new Form2("Etes-vous sûr de supprimer ce destinataire ?");
                    if (Suppression_Destinataire.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
                    {
                        if (Suppression_Destinataire.Valeur == "Oui")
                        {
                            Task x = ExecuteRequete("DELETE FROM Destinataire WHERE Nom LIKE'" + textBox_Recherche_destinataire.Text + "';");
                            MessageBox.Show("Le destinataire a été supprimé");
                        }
                    }
                    textBox_Recherche_destinataire.Text = "";
                    Actualiser_Liste_Destinataire();//On effectue 2 fois les fonctions sinon des fois les éléments ne se mettent pas a jour (valable dans tout le programme)
                    Actualiser_Liste_Destinataire();
                }
                else { MessageBox.Show("Selectionner un destinataire dans la liste"); }
                fermeture_connexion();
            }
            else { MessageBox.Show("Veuillez saisir le mot de passe"); }//Verification par mot de passe défini tout en haut
            textBox_mdp.Text = "";//Vide la textbox mot de passe
        }
        //Lorsqu'on écrit dans cette textBox on va rechercher les destinataires correpondant
        private void textBox_Recherche_destinataire_TextChanged(object sender, EventArgs e)
        {
            ouverture_connexion();
            listBox_Destinataire.Items.Clear();
            sql_cmd = new SQLiteCommand("SELECT Nom FROM Destinataire WHERE Nom LIKE '" + textBox_Recherche_destinataire.Text + "%';", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                listBox_Destinataire.Items.Add(query.GetString(0));
            }
            query.Close();
            fermeture_connexion();
            Actualiser_Liste_Fichier();

        }
        //Bouton pour créer une nouvelle commande
        private void button_Nouvelle_commande_Click(object sender, EventArgs e)
        {
            if ((textBox_Recherche_destinataire.Text != "") && (textBox_Nom_Proprietaire.Text != "") && (textBox_Prenom_Proprietaire.Text != "") &&
               (textBox_Typedebois.Text != "") && (textBox_Lieu.Text != "") && (textBox_Date_ajout.Text != ""))
            {
                test = false;
                string date_format = "";
                ouverture_connexion();

                sql_cmd = new SQLiteCommand("SELECT Date FROM Fichier WHERE Id_Destinataire LIKE (SELECT Id FROM Destinataire WHERE Nom LIKE '" + textBox_Recherche_destinataire.Text + "') AND Id_Parcelle LIKE (SELECT Id FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "' AND Lieu LIKE '" + textBox_Lieu.Text + "')", sql_con);//Nouvelle commande
                SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                while (query.Read())//Dure le temps de la requete
                {
                    if (dateTimePicker_Creation_commande.Value.Month < 10)//Mettre au bon format la date choisie sur le dateTimePicker
                    {
                        if (dateTimePicker_Creation_commande.Value.Day < 10)
                        {
                            date_format = "0" + dateTimePicker_Creation_commande.Value.Day + "-0" + dateTimePicker_Creation_commande.Value.Month + "-" + dateTimePicker_Creation_commande.Value.Year + "";
                        }
                        else
                        {
                            date_format = "" + dateTimePicker_Creation_commande.Value.Day + "-0" + dateTimePicker_Creation_commande.Value.Month + "-" + dateTimePicker_Creation_commande.Value.Year + "";
                        }
                    }
                    else
                    {
                        if (dateTimePicker_Creation_commande.Value.Day < 10)
                        {
                            date_format = "0" + dateTimePicker_Creation_commande.Value.Day + "-" + dateTimePicker_Creation_commande.Value.Month + "-" + dateTimePicker_Creation_commande.Value.Year + "";
                        }
                        else
                        {
                            date_format = "" + dateTimePicker_Creation_commande.Value.Day + "-" + dateTimePicker_Creation_commande.Value.Month + "-" + dateTimePicker_Creation_commande.Value.Year + "";
                        }
                    }
                    if (query.GetString(0) == date_format) { test = true; }//Si la commande avec les mêmes élements (destinataire,proprietaire,parcelle) existe déja à la même date test vaut vrai
                }
                query.Close();
                fermeture_connexion();
                if (test == false)
                {//Si la commande n'existe pas déja
                    if (dateTimePicker_Creation_commande.Value.Date != DateTime.Now.Date)//Si l'utilisateur sélectionne un jour
                    {
                        Form3 Creation_Fichier = new Form3(textBox_Recherche_destinataire.Text, textBox_Nom_Proprietaire.Text, textBox_Prenom_Proprietaire.Text, textBox_Date_ajout.Text, textBox_Typedebois.Text, textBox_Lieu.Text, dateTimePicker_Creation_commande.Value);
                        Creation_Fichier.Show();
                        this.Close();
                    }
                    else//Si l'utilisateur selectionne la date actuelle (Potentiellement un oublie de selectionner la bonne date)
                    {
                        Form2 Validation_nouvelle_commande = new Form2("Etes-vous sûr de dater la commande à aujourd'hui ?");
                        if (Validation_nouvelle_commande.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
                        {
                            if (Validation_nouvelle_commande.Valeur == "Oui")
                            {
                                Form3 Creation_Fichier = new Form3(textBox_Recherche_destinataire.Text, textBox_Nom_Proprietaire.Text, textBox_Prenom_Proprietaire.Text, textBox_Date_ajout.Text, textBox_Typedebois.Text, textBox_Lieu.Text, dateTimePicker_Creation_commande.Value);
                                Creation_Fichier.Show();
                                this.Close();
                            }
                        }
                    }
                }
                else { MessageBox.Show("Cette commande existe déja"); }
            }


            else { MessageBox.Show("Selectionner un propriétaire, une parcelle et un destinataire"); }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBox_creation_nouveau_destinataire_Enter(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {


        }



        private void button_Quitter_Click(object sender, EventArgs e)
        {
            Application.Exit();//Permet de tout fermer
        }
        //Se déclanche lors de la selection d'un objet dans la listbox
        private void listBox_Destinataire_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Recherche_destinataire.Text = listBox_Destinataire.Text;
        }
        //Création d'un proprietaire
        private void button_creation_proprietaire_Click(object sender, EventArgs e)
        {
            if ((textBox_Creation_prenom_proprietaire.Text != "") && (textBox_Creation_prenom_proprietaire.Text != ""))//Vérification des champs renseigné
            {
                ouverture_connexion();
                string ajd_text = Date_jour();
                test = false;//Variable pour les tests
                sql_cmd = new SQLiteCommand("SELECT Nom,Prenom,Date_ajout FROM Proprietaire", sql_con);//Nouvelle commande
                SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                while (query.Read())//Dure le temps de la requete
                {
                    if ((query.GetString(0) == textBox_Creation_nom__proprietaire.Text) && (query.GetString(1) == textBox_Creation_prenom_proprietaire.Text) && (query.GetString(2) == ajd_text))//Compare aux éléments existants
                    {
                        test = true;
                    }
                }
                query.Close();
                fermeture_connexion();
                if (test == true) { MessageBox.Show("Ce propriétaire existe déja vous pouvez le selectionner"); }
                else
                {
                    Task x = ExecuteRequete("INSERT INTO Proprietaire (Nom,Prenom,Date_ajout) " +
                   "VALUES('" + textBox_Creation_nom__proprietaire.Text + "','" + textBox_Creation_prenom_proprietaire.Text + "','" + ajd_text + "')");
                    for (int i = 0; i < 99999999; i++)
                    {

                    }
                    Actualiser_Liste_Proprietaire();
                    Actualiser_Liste_Proprietaire();

                }
                textBox_Creation_nom__proprietaire.Text = "";
                textBox_Creation_prenom_proprietaire.Text = "";
                Vider_champs();

            }
            else
            { MessageBox.Show("Remplir les cases pour la création du propriétaire"); }
        }
        //Supression propriétaire
        private void button_suppression_proprietaire_Click(object sender, EventArgs e)
        {
            ouverture_connexion();
            if (textBox_mdp.Text == mdp)//Neccessite un mdp
            {
                if ((textBox_Nom_Proprietaire.Text != "") && (textBox_Prenom_Proprietaire.Text != "") && (textBox_Date_ajout.Text != ""))//Champs non vide
                {
                    Form2 Suppression_Proprietaire = new Form2("Etes-vous sûr de supprimer ce proprétaire ?");// On vérifie que l'action est voulue
                    if (Suppression_Proprietaire.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
                    {
                        if (Suppression_Proprietaire.Valeur == "Oui")
                        {
                            Task x = ExecuteRequete("DELETE FROM Proprietaire WHERE Nom LIKE '" + textBox_Nom_Proprietaire.Text + "' AND Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "' AND Date_ajout LIKE '" + textBox_Date_ajout.Text + "'");
                            MessageBox.Show("Le propriétaire a été supprimé");
                        }
                    }
                    Vider_champs();
                    Actualiser_Liste_Proprietaire();
                    Actualiser_Liste_Proprietaire();
                }
                else { MessageBox.Show("Selectionner un propriétaire dans la liste"); }
            }
            else { MessageBox.Show("Veuillez saisir le mot de passe"); }
            fermeture_connexion();
            textBox_mdp.Text = "";
        }
        //Se déclanche lors de la selection d'un objet dans la listbox
        private void listBox_Nom_Proprietaire_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Nom_Proprietaire.Text = listBox_Nom_Proprietaire.Text;
        }
        //Se déclanche lors de la selection d'un objet dans la listbox
        private void listBox_Prenom_Proprietaire_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Prenom_Proprietaire.Text = listBox_Prenom_Proprietaire.Text;
        }
        //Se déclanche lorsque le contenu de la textbox est modifié   on va faire des recherches dans la BDD pour aider a la saisie     
        private void textBox_Nom_Proprietaire_TextChanged(object sender, EventArgs e)
        {
            ouverture_connexion();
            if (textBox_Nom_Proprietaire.Text != "")//Si nom renseigné
            {
                if (textBox_Prenom_Proprietaire.Text == "")//Si prenom non renseigné
                {
                    listBox_Nom_Proprietaire.Items.Clear();
                    listBox_Prenom_Proprietaire.Items.Clear();
                    listBox_Date_ajout.Items.Clear();
                    sql_cmd = new SQLiteCommand("SELECT Nom,Prenom,Date_ajout FROM Proprietaire WHERE Nom LIKE '" + textBox_Nom_Proprietaire.Text + "%';", sql_con);//Nouvelle commande
                    SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        listBox_Nom_Proprietaire.Items.Add(query.GetString(0));
                        listBox_Prenom_Proprietaire.Items.Add(query.GetString(1));
                        listBox_Date_ajout.Items.Add(query.GetString(2));
                    }
                    query.Close();
                    listBox_Typedebois.Items.Clear();
                    listBox_Lieu.Items.Clear();
                    sql_cmd = new SQLiteCommand("SELECT Typedebois,Lieu FROM Parcelle WHERE Id_Proprietaire LIKE (SELECT Id FROM Proprietaire WHERE Nom LIKE '" + textBox_Nom_Proprietaire.Text + "%');", sql_con);//Nouvelle commande
                    query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        listBox_Typedebois.Items.Add(query.GetString(0));
                        listBox_Lieu.Items.Add(query.GetString(1));

                    }
                    query.Close();
                }
                else//Si prenom rensigné et donc le nom aussi
                {
                    listBox_Nom_Proprietaire.Items.Clear();
                    listBox_Prenom_Proprietaire.Items.Clear();
                    listBox_Date_ajout.Items.Clear();
                    sql_cmd = new SQLiteCommand("SELECT Nom,Prenom,Date_ajout FROM Proprietaire WHERE Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "%' AND Nom LIKE '" + textBox_Nom_Proprietaire.Text + "%';", sql_con);//Nouvelle commande
                    SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        listBox_Nom_Proprietaire.Items.Add(query.GetString(0));
                        listBox_Prenom_Proprietaire.Items.Add(query.GetString(1));
                        listBox_Date_ajout.Items.Add(query.GetString(2));
                    }
                    query.Close();
                    listBox_Typedebois.Items.Clear();
                    listBox_Lieu.Items.Clear();
                    sql_cmd = new SQLiteCommand("SELECT Typedebois,Lieu FROM Parcelle WHERE Id_Proprietaire LIKE (SELECT Id FROM Proprietaire WHERE Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "%' AND Nom LIKE '" + textBox_Nom_Proprietaire.Text + "%');", sql_con);//Nouvelle commande
                    query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        listBox_Typedebois.Items.Add(query.GetString(0));
                        listBox_Lieu.Items.Add(query.GetString(1));

                    }
                    query.Close();
                }
                fermeture_connexion();
            }
            else//Si Nom vide soit l'utilisateur éfface
            {
                if (textBox_Prenom_Proprietaire.Text == "" && textBox_Typedebois.Text == "")
                {
                    Actualiser_Liste_Proprietaire();
                    listBox_Typedebois.Items.Clear();
                    listBox_Lieu.Items.Clear();
                }
            }

        }
        //Se déclanche lorsque le contenu de la textbox est modifié on va faire des recherches dans la BDD pour aider a la saisie
        private void textBox_Prenom_Proprietaire_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Prenom_Proprietaire.Text != "")//Si prenom renseigné
            {
                ouverture_connexion();
                if (textBox_Nom_Proprietaire.Text == "")//Si nom vide
                {
                    listBox_Nom_Proprietaire.Items.Clear();
                    listBox_Prenom_Proprietaire.Items.Clear();
                    listBox_Date_ajout.Items.Clear();
                    sql_cmd = new SQLiteCommand("SELECT Nom,Prenom,Date_ajout FROM Proprietaire WHERE Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "%';", sql_con);//Nouvelle commande
                    SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        listBox_Nom_Proprietaire.Items.Add(query.GetString(0));
                        listBox_Prenom_Proprietaire.Items.Add(query.GetString(1));
                        listBox_Date_ajout.Items.Add(query.GetString(2));
                    }
                    query.Close();
                    listBox_Typedebois.Items.Clear();
                    listBox_Lieu.Items.Clear();
                    sql_cmd = new SQLiteCommand("SELECT Typedebois,Lieu FROM Parcelle WHERE Id_Proprietaire LIKE (SELECT Id FROM Proprietaire WHERE Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "%');", sql_con);//Nouvelle commande
                    query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        listBox_Typedebois.Items.Add(query.GetString(0));
                        listBox_Lieu.Items.Add(query.GetString(1));

                    }
                    query.Close();
                }
                else//Si Prenom et Nom renseigné
                {
                    listBox_Nom_Proprietaire.Items.Clear();
                    listBox_Prenom_Proprietaire.Items.Clear();
                    listBox_Date_ajout.Items.Clear();
                    sql_cmd = new SQLiteCommand("SELECT Nom,Prenom,Date_ajout FROM Proprietaire WHERE Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "%' AND Nom LIKE '" + textBox_Nom_Proprietaire.Text + "%';", sql_con);//Nouvelle commande
                    SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        listBox_Nom_Proprietaire.Items.Add(query.GetString(0));
                        listBox_Prenom_Proprietaire.Items.Add(query.GetString(1));
                        listBox_Date_ajout.Items.Add(query.GetString(2));
                    }
                    query.Close();
                    listBox_Typedebois.Items.Clear();
                    listBox_Lieu.Items.Clear();
                    sql_cmd = new SQLiteCommand("SELECT Typedebois,Lieu FROM Parcelle WHERE Id_Proprietaire LIKE (SELECT Id FROM Proprietaire WHERE Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "%' AND Nom LIKE '" + textBox_Nom_Proprietaire.Text + "%');", sql_con);//Nouvelle commande
                    query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        listBox_Typedebois.Items.Add(query.GetString(0));
                        listBox_Lieu.Items.Add(query.GetString(1));

                    }
                    query.Close();
                }
                fermeture_connexion();
            }
            else
            {
                if (textBox_Nom_Proprietaire.Text == "" && textBox_Typedebois.Text == "")
                {
                    Actualiser_Liste_Proprietaire();
                    listBox_Typedebois.Items.Clear();
                    listBox_Lieu.Items.Clear();
                }
            }

        }

        private void textBox__Creation_nouveau_destinataire_TextChanged(object sender, EventArgs e)
        {

        }
        //Bouton pour vider les champs des propriétaires
        private void button_Vider_Click(object sender, EventArgs e)
        {
            textBox_Nom_Proprietaire.Text = "";
            textBox_Prenom_Proprietaire.Text = "";
            textBox_Date_ajout.Text = "";
            textBox_Typedebois.Text = "";
            textBox_Lieu.Text = "";
            label_Datefichier.Text = "";
            listBox_Datefichier.Items.Clear();
            listBox_Typedebois.Items.Clear();
            listBox_Lieu.Items.Clear();
            Actualiser_Liste_Proprietaire();

        }
        //Création d'une nouvelle parcelle
        private void button_Creation_parcelle_Click(object sender, EventArgs e)
        {
            if ((label_Creation_Typedebois_liste.Text != "") && (textBox_Creation_lieu.Text != ""))//Si les champs de création son renseigné
            {
                if ((textBox_Nom_Proprietaire.Text != "") && (textBox_Prenom_Proprietaire.Text != "") && (textBox_Date_ajout.Text != ""))//Si on a seletionner un propriétaire
                {
                    ouverture_connexion();
                    test = false;//Variable pour les tests
                    sql_cmd = new SQLiteCommand("SELECT Typedebois,Lieu FROM Parcelle", sql_con);//Nouvelle commande
                    SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        if ((query.GetString(0) == label_Creation_Typedebois_liste.Text) && (query.GetString(1) == (textBox_Creation_lieu.Text)))//Compare aux éléments existants
                        {
                            test = true;
                        }
                    }
                    query.Close();
                    fermeture_connexion();
                    if (test == true) { MessageBox.Show("Cette parcelle existe déja vous pouvez la selectionner"); }//Vérifie si la parcelle existe déja
                    else
                    {
                        Task x = ExecuteRequete("INSERT INTO Parcelle (Typedebois,Lieu,Id_proprietaire) VALUES('" + label_Creation_Typedebois_liste.Text + "','" + textBox_Creation_lieu.Text.Replace("'", "`") + "',(SELECT Id FROM Proprietaire WHERE Nom LIKE '" + textBox_Nom_Proprietaire.Text + "' AND Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "' AND Date_ajout LIKE '" + textBox_Date_ajout.Text + "'))");

                        textBox_Typedebois.Text = "";
                        textBox_Lieu.Text = "";
                        label_Datefichier.Text = "";
                        listBox_Datefichier.Items.Clear();
                        for (int i = 0; i < 99999999; i++)
                        {

                        }
                        textBox_Nom_Proprietaire_TextChanged(sender, e);
                        textBox_Prenom_Proprietaire_TextChanged(sender, e);

                    }
                }
                else { MessageBox.Show("Veuillez selectionner le propriétaire qui possède cette parcelle"); }
            }
            else
            { MessageBox.Show("Remplir les cases pour la création d'une parcelle"); }
            textBox_Creation_lieu.Text = "";
            label_Creation_Typedebois_liste.Text = "";
        }
        //Lors de la sélection d'un objet dans la listbox
        private void listBox_Typedebois_Proprietaire_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Typedebois.Text = listBox_Typedebois.Text;
        }
        //Lors de la sélection d'un objet dans la listbox
        private void listBox_Lieu_Proprietaire_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Lieu.Text = listBox_Lieu.Text;
        }
        //Lors de la modification du contenu de la textbox
        private void textBox_Typedebois_Proprietaire_TextChanged(object sender, EventArgs e)
        {
            ouverture_connexion();
            if (textBox_Typedebois.Text != "")//Si Typedebois renseigné
            {
                if ((textBox_Nom_Proprietaire.Text == "") && (textBox_Prenom_Proprietaire.Text == ""))//Si le nom et le prenom rensigné
                {
                    listBox_Typedebois.Items.Clear();
                    listBox_Lieu.Items.Clear();
                    sql_cmd = new SQLiteCommand("SELECT Typedebois,Lieu FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "%';", sql_con);//Nouvelle commande
                    SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        listBox_Typedebois.Items.Add(query.GetString(0));
                        listBox_Lieu.Items.Add(query.GetString(1));
                    }
                    query.Close();

                    listBox_Nom_Proprietaire.Items.Clear();
                    listBox_Prenom_Proprietaire.Items.Clear();
                    listBox_Date_ajout.Items.Clear();
                    sql_cmd = new SQLiteCommand("SELECT Id_Proprietaire FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "%';", sql_con);//Nouvelle commande
                    query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        string id_proprietaire = query.GetValue(0).ToString();
                        sql_cmd = new SQLiteCommand("SELECT Nom,Prenom,Date_ajout FROM Proprietaire WHERE Id LIKE '" + id_proprietaire + "';", sql_con);//Nouvelle commande
                        SQLiteDataReader query1 = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                        while (query1.Read())//Dure le temps de la requete
                        {
                            listBox_Nom_Proprietaire.Items.Add(query1.GetString(0));
                            listBox_Prenom_Proprietaire.Items.Add(query1.GetString(1));
                            listBox_Date_ajout.Items.Add(query1.GetString(2));

                        }
                        query1.Close();

                    }
                    query.Close();


                }
                else
                {
                    if ((textBox_Nom_Proprietaire.Text == "") && (textBox_Prenom_Proprietaire.Text != ""))//Si Nom vide
                    {
                        listBox_Typedebois.Items.Clear();
                        listBox_Lieu.Items.Clear();
                        sql_cmd = new SQLiteCommand("SELECT Typedebois,Lieu FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "%' AND Id_Proprietaire LIKE (SELECT Id FROM Proprietaire WHERE Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "%');", sql_con);//Nouvelle commande
                        SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                        while (query.Read())//Dure le temps de la requete
                        {
                            listBox_Typedebois.Items.Add(query.GetString(0));
                            listBox_Lieu.Items.Add(query.GetString(1));
                        }
                        query.Close();

                        listBox_Nom_Proprietaire.Items.Clear();
                        listBox_Prenom_Proprietaire.Items.Clear();
                        listBox_Date_ajout.Items.Clear();

                        sql_cmd = new SQLiteCommand("SELECT Id_Proprietaire FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "%';", sql_con);//Nouvelle commande
                        query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                        while (query.Read())//Dure le temps de la requete
                        {
                            string id_proprietaire = query.GetValue(0).ToString();
                            sql_cmd = new SQLiteCommand("SELECT Nom,Prenom,Date_ajout FROM Proprietaire WHERE Id LIKE '" + id_proprietaire + "' AND Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "%';", sql_con);//Nouvelle commande
                            SQLiteDataReader query1 = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                            while (query1.Read())//Dure le temps de la requete
                            {
                                listBox_Nom_Proprietaire.Items.Add(query1.GetString(0));
                                listBox_Prenom_Proprietaire.Items.Add(query1.GetString(1));
                                listBox_Date_ajout.Items.Add(query1.GetString(2));

                            }
                            query1.Close();

                        }
                        query.Close();


                    }
                    if ((textBox_Nom_Proprietaire.Text != "") && (textBox_Prenom_Proprietaire.Text == ""))//Si prenom vide
                    {
                        listBox_Typedebois.Items.Clear();
                        listBox_Lieu.Items.Clear();
                        sql_cmd = new SQLiteCommand("SELECT Typedebois,Lieu FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "%' AND Id_Proprietaire LIKE (SELECT Id FROM Proprietaire WHERE Nom LIKE '" + textBox_Nom_Proprietaire.Text + "%');", sql_con);//Nouvelle commande
                        SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                        while (query.Read())//Dure le temps de la requete
                        {
                            listBox_Typedebois.Items.Add(query.GetString(0));
                            listBox_Lieu.Items.Add(query.GetString(1));
                        }
                        query.Close();

                        listBox_Nom_Proprietaire.Items.Clear();
                        listBox_Prenom_Proprietaire.Items.Clear();
                        listBox_Date_ajout.Items.Clear();
                        sql_cmd = new SQLiteCommand("SELECT Id_Proprietaire FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "%';", sql_con);//Nouvelle commande
                        query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                        while (query.Read())//Dure le temps de la requete
                        {
                            string id_proprietaire = query.GetValue(0).ToString();
                            sql_cmd = new SQLiteCommand("SELECT Nom,Prenom,Date_ajout FROM Proprietaire WHERE Id LIKE '" + id_proprietaire + "' AND Nom LIKE '" + textBox_Nom_Proprietaire.Text + "%';", sql_con);//Nouvelle commande
                            SQLiteDataReader query1 = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                            while (query1.Read())//Dure le temps de la requete
                            {
                                listBox_Nom_Proprietaire.Items.Add(query1.GetString(0));
                                listBox_Prenom_Proprietaire.Items.Add(query1.GetString(1));
                                listBox_Date_ajout.Items.Add(query1.GetString(2));

                            }
                            query1.Close();

                        }
                        query.Close();
                    }
                    if ((textBox_Nom_Proprietaire.Text != "") && (textBox_Prenom_Proprietaire.Text != ""))//Si nom et prénom ne sont pas renseigné
                    {
                        listBox_Typedebois.Items.Clear();
                        listBox_Lieu.Items.Clear();
                        sql_cmd = new SQLiteCommand("SELECT Typedebois,Lieu FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "%' AND Id_Proprietaire LIKE (SELECT Id FROM Proprietaire WHERE Nom LIKE '" + textBox_Nom_Proprietaire.Text + "%' AND Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "%');", sql_con);//Nouvelle commande
                        SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                        while (query.Read())//Dure le temps de la requete
                        {
                            listBox_Typedebois.Items.Add(query.GetString(0));
                            listBox_Lieu.Items.Add(query.GetString(1));
                        }
                        query.Close();

                        listBox_Nom_Proprietaire.Items.Clear();
                        listBox_Prenom_Proprietaire.Items.Clear();
                        listBox_Date_ajout.Items.Clear();
                        sql_cmd = new SQLiteCommand("SELECT Id_Proprietaire FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "%';", sql_con);//Nouvelle commande
                        query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                        while (query.Read())//Dure le temps de la requete
                        {
                            string id_proprietaire = query.GetValue(0).ToString();
                            sql_cmd = new SQLiteCommand("SELECT Nom,Prenom,Date_ajout FROM Proprietaire WHERE Id LIKE '" + id_proprietaire + "' AND Nom LIKE '" + textBox_Nom_Proprietaire.Text + "%' AND Prenom LIKE '" + textBox_Prenom_Proprietaire.Text + "%';", sql_con);//Nouvelle commande
                            SQLiteDataReader query1 = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                            while (query1.Read())//Dure le temps de la requete
                            {
                                listBox_Nom_Proprietaire.Items.Add(query1.GetString(0));
                                listBox_Prenom_Proprietaire.Items.Add(query1.GetString(1));
                                listBox_Date_ajout.Items.Add(query1.GetString(2));

                            }
                            query1.Close();

                        }
                        query.Close();

                    }
                }

            }
            else { listBox_Typedebois.Items.Clear(); listBox_Lieu.Items.Clear(); }

            fermeture_connexion();
            Actualiser_Liste_Fichier();
        }
        //Lorsque on change le texte dans la textbox
        private void textBox_Lieu_Proprietaire_TextChanged(object sender, EventArgs e)
        {
            Actualiser_Liste_Fichier();
        }
        //Vide les éléments en lien avec les parcelles
        private void button_Vider_Parcelle_Click(object sender, EventArgs e)
        {
            textBox_Typedebois.Text = "";
            textBox_Lieu.Text = "";
            listBox_Typedebois.Items.Clear();
            listBox_Lieu.Items.Clear();
            listBox_Datefichier.Items.Clear();
            listBox_Typedebois.Items.Clear();
            textBox1_Nom_proprietaire_TextChanged(textBox_Nom_Proprietaire.Text, e);
            textBox_Prenom_Proprietaire_TextChanged(textBox_Prenom_Proprietaire.Text, e);

        }
        //Lors de la sélection d'un objet dans la listbox
        private void listBox_Date_ajout_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Date_ajout.Text = listBox_Date_ajout.Text;
        }

        private void textBox_Creation_typedebois_TextChanged(object sender, EventArgs e)
        { }
        //Lors de l'appuie du bouton 
        private void button_Suppression_parcelle_Click(object sender, EventArgs e)
        {
            ouverture_connexion();
            if ((textBox_Typedebois.Text != "") && (textBox_Lieu.Text != ""))//Si une parcelle est sélectionnée
            {
                Form2 Suppression_Parcelle = new Form2("Etes-vous sûr de supprimer cette parcelle ?");//Vérification
                if (Suppression_Parcelle.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
                {
                    if (Suppression_Parcelle.Valeur == "Oui")
                    {
                        Task x = ExecuteRequete("DELETE FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "' AND Lieu LIKE '" + textBox_Lieu.Text + "'");
                        MessageBox.Show("La parcelle a été supprimée");
                    }
                }
                textBox_Lieu.Text = "";
                textBox_Typedebois.Text = "";
                textBox_Nom_Proprietaire_TextChanged(sender, e);
                textBox_Prenom_Proprietaire_TextChanged(sender, e);
            }
            else { MessageBox.Show("Selectionner une parcelle dans les listes"); }
            fermeture_connexion();
        }
        //Lors du chargement de la Form
        private void Form1_Load(object sender, EventArgs e)
        {
            button_Suppression_destinataire.BackColor = Color.Red;//Couleur
            button_Suppression_destinataire.FlatStyle = FlatStyle.Flat;
            button_Suppression_destinataire.FlatAppearance.BorderColor = Color.Red;
            button_Suppression_destinataire.FlatAppearance.BorderSize = 1;
            //
            button_suppression_proprietaire.BackColor = Color.Red;//Couleur
            button_suppression_proprietaire.FlatStyle = FlatStyle.Flat;
            button_suppression_proprietaire.FlatAppearance.BorderColor = Color.Red;
            button_suppression_proprietaire.FlatAppearance.BorderSize = 1;
            //
            button_Suppression_parcelle.BackColor = Color.Red;//Couleur
            button_Suppression_parcelle.FlatStyle = FlatStyle.Flat;
            button_Suppression_parcelle.FlatAppearance.BorderColor = Color.Red;
            button_Suppression_parcelle.FlatAppearance.BorderSize = 1;
            //
            button_Supprimer_une_commande_existante.BackColor = Color.Red;//Couleur
            button_Supprimer_une_commande_existante.FlatStyle = FlatStyle.Flat;
            button_Supprimer_une_commande_existante.FlatAppearance.BorderColor = Color.Red;
            button_Supprimer_une_commande_existante.FlatAppearance.BorderSize = 1;
            //
            //
            button_Creation_parcelle.BackColor = Color.LimeGreen;//Couleur
            button_Creation_parcelle.FlatStyle = FlatStyle.Flat;
            button_Creation_parcelle.FlatAppearance.BorderColor = Color.LimeGreen;
            button_Creation_parcelle.FlatAppearance.BorderSize = 1;
            //
            button_creation_proprietaire.BackColor = Color.LimeGreen;//Couleur
            button_creation_proprietaire.FlatStyle = FlatStyle.Flat;
            button_creation_proprietaire.FlatAppearance.BorderColor = Color.LimeGreen;
            button_creation_proprietaire.FlatAppearance.BorderSize = 1;
            //
            button_Nouvelle_commande.BackColor = Color.LimeGreen;//Couleur
            button_Nouvelle_commande.FlatStyle = FlatStyle.Flat;
            button_Nouvelle_commande.FlatAppearance.BorderColor = Color.LimeGreen;
            button_Nouvelle_commande.FlatAppearance.BorderSize = 1;
            //
            button_Validation_creation_destinataire.BackColor = Color.LimeGreen;//Couleur
            button_Validation_creation_destinataire.FlatStyle = FlatStyle.Flat;
            button_Validation_creation_destinataire.FlatAppearance.BorderColor = Color.LimeGreen;
            button_Validation_creation_destinataire.FlatAppearance.BorderSize = 1;
            //
            //
            button_Quitter.BackColor = Color.DarkOrange;//Couleur
            button_Quitter.FlatStyle = FlatStyle.Flat;
            button_Quitter.FlatAppearance.BorderColor = Color.DarkOrange;
            button_Quitter.FlatAppearance.BorderSize = 1;


            this.Size = new Size(1250, 525);    //Taille
            this.Location = new Point(0, 0);//En haut a gaucge
            groupBox_Modification_Code.Hide();//Cache la groupbox modification
            button_Choix_action.Text = "Modifier/Supprimer";
            //Initialisation de la liste avec les types de bois
            listBox_Creation_Typedebois.Items.Add("Pin");
            listBox_Creation_Typedebois.Items.Add("Sapin/Epicéa");
            listBox_Creation_Typedebois.Items.Add("Douglas");
            listBox_Creation_Typedebois.Items.Add("Feuillus");
            ouverture_connexion();
            sql_cmd = new SQLiteCommand("SELECT Chemin_ou_sauvegarde_BDD FROM Sauvegarde_BDD;", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                textBox_Sauvegarde_BDD.Text = query.GetValue(0).ToString();//Va chercher le chemin déja remplis si il a déja été saisi
            }
            query.Close();
            fermeture_connexion();
            var files = Directory.EnumerateFiles(@"C:\Logiciel_Cubage\Archives", "*.db");
            foreach (string fichier in files)
            {                
                string date_fichier = "";
                for (int i = 28; i < 38; i++)
                {
                    date_fichier = date_fichier + fichier[i];
                }
               listBox_archive.Items.Add(date_fichier);
            }
            
        }
        //Lors de la sélection d'un objet dans la listbox
        private void listBox_Creation_Typedebois_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_Creation_Typedebois_liste.Text = listBox_Creation_Typedebois.Text;
        }

        private void textBox_Creation_nom__proprietaire_TextChanged(object sender, EventArgs e)
        { }

        private void button_Ouvrir_une_commande_existante_Click(object sender, EventArgs e)
        {
            try
            {
                if ((textBox_Recherche_destinataire.Text != "") && (textBox_Nom_Proprietaire.Text != "") && (textBox_Prenom_Proprietaire.Text != "") &&
                  (textBox_Typedebois.Text != "") && (textBox_Lieu.Text != "") && (textBox_Date_ajout.Text != "") && (label_Datefichier.Text != ""))//On verifie que tout les champs sont saisi
                {
                    string id_fichier_text = "";
                    ouverture_connexion();
                    sql_cmd = new SQLiteCommand("SELECT Id FROM Fichier WHERE Id_Destinataire LIKE (SELECT Id FROM Destinataire WHERE Nom LIKE '" + textBox_Recherche_destinataire.Text + "') AND Id_Parcelle LIKE (SELECT Id FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "' AND Lieu LIKE '" + textBox_Lieu.Text + "') AND Date LIKE '" + label_Datefichier.Text + "'", sql_con);//Nouvelle commande
                    SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                    while (query.Read())//Dure le temps de la requete
                    {
                        id_fichier_text = query.GetValue(0).ToString();
                    }
                    query.Close();
                    Donneespubliques.id_fichier = Convert.ToUInt32(id_fichier_text);
                    fermeture_connexion();
                    Form4 Modification_Fichier = new Form4();
                    Modification_Fichier.Show();
                    this.Close();
                }
                else { MessageBox.Show("Selectionner un propriétaire, une parcelle , un destinataire et une date de création du fichier"); }
            }
            catch
            {
                MessageBox.Show("Cette commande n'existe pas"); Vider_champs(); listBox_Datefichier.Items.Clear();
            }
        }

        private void label_Datefichier_Click(object sender, EventArgs e)
        { }
        //Lorsque on change le texte dans la textbox
        private void textBox_Date_ajout_TextChanged(object sender, EventArgs e)
        {
            Actualiser_Liste_Fichier();

        }

        private void button_Supprimer_une_commande_existante_Click(object sender, EventArgs e)
        {
            try
            {
                if ((textBox_Recherche_destinataire.Text != "") && (textBox_Nom_Proprietaire.Text != "") && (textBox_Prenom_Proprietaire.Text != "") &&
                    (textBox_Typedebois.Text != "") && (textBox_Lieu.Text != "") && (textBox_Date_ajout.Text != "") && (label_Datefichier.Text != ""))//Si les champs sont tous remplis 
                {
                    Form2 Suppression_Fichier = new Form2("Etes-vous sûr de supprimer cette commande ?");
                    if (Suppression_Fichier.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
                    {
                        if (Suppression_Fichier.Valeur == "Oui")
                        {
                            Task x = ExecuteRequete("DELETE FROM Fichier WHERE Id_Destinataire LIKE (SELECT Id FROM Destinataire WHERE Nom LIKE '" + textBox_Recherche_destinataire.Text + "') AND Id_Parcelle LIKE (SELECT Id FROM Parcelle WHERE Typedebois LIKE '" + textBox_Typedebois.Text + "' AND Lieu LIKE '" + textBox_Lieu.Text + "') AND Date LIKE '" + label_Datefichier.Text + "'");
                            MessageBox.Show("Le fichier a été supprimer");
                            label_Datefichier.Text = "";
                            listBox_Datefichier.Items.Clear();
                        }
                    }
                }
                else { MessageBox.Show("Selectionner un propriétaire, une parcelle , un destinataire et une date de création du fichier"); }
            }
            catch
            {
                MessageBox.Show("Cette commande n'existe pas"); Vider_champs(); listBox_Datefichier.Items.Clear();

            }
        }


        private void listBox_Datefichier_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_Datefichier.Text = listBox_Datefichier.Text;
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            textBox_Recherche_destinataire.Text = "";
            listBox_Datefichier.Items.Clear();
            listBox_Typedebois.Items.Clear();
        }

        private void groupBox_Proprietaire_Enter(object sender, EventArgs e)
        { }

        private void button_Enregistrer_BDD_Click(object sender, EventArgs e)
        {
            // Simple synchronous file copy operations with no user interface.
            // To run this sample, first create the following directories and files:
            // C:\Users\Public\TestFolder
            // C:\Users\Public\TestFolder\test.txt
            // C:\Users\Public\TestFolder\SubDir\test.txt
            ouverture_connexion();
            string chemin = "";
            sql_cmd = new SQLiteCommand("SELECT Chemin_ou_sauvegarde_BDD FROM Sauvegarde_BDD ;", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                chemin = query.GetString(0);
            }
            query.Close();
            fermeture_connexion();
            string fileName = "Base_de_donnee.db";
            string sourcePath = @"C:\Logiciel_Cubage\Base_de_donne";
            string targetPath = @"" + chemin + "";

            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);


            // To copy a file to another location and
            // overwrite the destination file if it already exists.

            try
            {
                System.IO.File.Copy(sourceFile, destFile, true);
                MessageBox.Show("La base de donnée a été sauvegardée");
            }
            catch
            {
                MessageBox.Show("Le chemin de sauvegarde indiqué n'est pas valable (N'oubliez pas de brancher votre disque dur)");
            }
            // Keep console window open in debug mode.


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        { }

        private void button_Enregistrer_modification_Click(object sender, EventArgs e)
        {
            if (textBox_mdp.Text == mdp)//Neccessite un mdp
            {
                if ((textBox_Sauvegarde_BDD.Text != ""))//Le chemin ne soit pas vide
                {
                    Form2 Modification_savegarde_BDD = new Form2("Etes-vous sûr de modifier la localisation de sauvegarde de la base de donnée ? ATTENTION la sauvegarde précédente ne sera pas effacer ");//Vérification
                    if (Modification_savegarde_BDD.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
                    {
                        if (Modification_savegarde_BDD.Valeur == "Oui")
                        {
                            ouverture_connexion();
                            string chemin = "";
                            sql_cmd = new SQLiteCommand("SELECT Chemin_ou_sauvegarde_BDD FROM Sauvegarde_BDD;", sql_con);//Nouvelle commande
                            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                            while (query.Read())//Dure le temps de la requete
                            {
                                chemin = query.GetString(0);
                            }
                            query.Close();
                            fermeture_connexion();
                            if (chemin == "")
                            {
                   
                                 Task x = ExecuteRequete("INSERT INTO Sauvegarde_BDD(Chemin_ou_sauvegarde_BDD) VALUES('" + textBox_Sauvegarde_BDD.Text + "');");
                                MessageBox.Show("La localisation de sauvegarde de la base de donnée à été modifié");
                            }
                            else
                            {
                                Task x = ExecuteRequete("UPDATE Sauvegarde_BDD SET Chemin_ou_sauvegarde_BDD = '" + textBox_Sauvegarde_BDD.Text + "'");
                                MessageBox.Show("La localisation de sauvegarde de la base de donnée à été modifié");
                            }
                            
                        }
                    }
                }
                else { MessageBox.Show("Veuillez écrire le chemin de l'endroit où vous souhaitez sauvegarder la BDD"); }
            }
            else { MessageBox.Show("Veuillez saisir le mot de passe"); }
            textBox_mdp.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        { }
        //Bouton modifier/suprimer et Ajout
        private void button_Choix_action_Click(object sender, EventArgs e)
        {
            if (button_Choix_action.Text == "Modifier/Supprimer")//Si modifier/supprimer afficher
            {
                button_Choix_action.Text = "Ajout";
                groupBox_Modification_Code.Show();
                groupBox_Creation_proprietaire.Hide();
                groupBox_Creation_parcelle.Hide();
                groupBox_creation_nouveau_destinataire.Hide();
            }
            else//Si ajout afficher
            {
                groupBox_Modification_Code.Hide();
                groupBox_Creation_proprietaire.Show();
                groupBox_Creation_parcelle.Show();
                groupBox_creation_nouveau_destinataire.Show();
                button_Choix_action.Text = "Modifier/Supprimer";

            }
        }

        private void label_Recherche_destinataire_Click(object sender, EventArgs e)
        {

        }

        private void button_Archiver_Click(object sender, EventArgs e)
        {
            if (textBox_mdp.Text == mdp)
            {
                if (dateTimePicker_Archivage.Value.Date == DateTime.Now.Date)//Mettre au bon format la date choisie sur le dateTimePicker
                {
                    Form2 Archivage_Date = new Form2("Etes-vous sûr de dater l'archive à aujourd'hui ?");
                    if (Archivage_Date.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
                    {
                        if (Archivage_Date.Valeur == "Oui")
                        {
                            Form2 Archivage = new Form2("Etes-vous sûr d'archiver cette opération archivera toute les commandes/proprietaire/destinataire/parcelle ?");
                            if (Archivage.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
                            {
                                if (Archivage.Valeur == "Oui")
                                {
                                    string date_commande_text = "";
                                    if (dateTimePicker_Archivage.Value.Date.Month < 10)
                                    {
                                        if (dateTimePicker_Archivage.Value.Date.Day < 10)
                                        {
                                            date_commande_text = "0" + dateTimePicker_Archivage.Value.Date.Day + "-0" + dateTimePicker_Archivage.Value.Date.Month + "-" + dateTimePicker_Archivage.Value.Date.Year + "";
                                        }
                                        else
                                        {
                                            date_commande_text = "" + dateTimePicker_Archivage.Value.Date.Day + "-0" + dateTimePicker_Archivage.Value.Date.Month + "-" + dateTimePicker_Archivage.Value.Date.Year + "";
                                        }
                                    }
                                    else
                                    {
                                        if (dateTimePicker_Archivage.Value.Date.Day < 10)
                                        {
                                            date_commande_text = "0" + dateTimePicker_Archivage.Value.Date.Day + "-" + dateTimePicker_Archivage.Value.Date.Month + "-" + dateTimePicker_Archivage.Value.Date.Year + "";
                                        }
                                        else
                                        {
                                            date_commande_text = "" + dateTimePicker_Archivage.Value.Date.Day + "-" + dateTimePicker_Archivage.Value.Date.Month + "-" + dateTimePicker_Archivage.Value.Date.Year + "";
                                        }
                                    }
                                    fermeture_connexion();
                                    //Déplace et renomme
                                    
                                    string chemin_nouveau_nom = date_commande_text+".db";
                                    string sourcePath = @"C:\Logiciel_Cubage\Base_de_donne\Base_de_donnee.db";
                                    string targetPath = @"C:\Logiciel_Cubage\Archives\" + chemin_nouveau_nom;
                                    this.Close();
                                    System.IO.File.Copy(sourcePath, targetPath,true);
                                    
                                    //Copie une BDD vierge
                                    string nom_fichier = "Base_de_donnee.db";
                                    string Copie = @"C:\Logiciel_Cubage\Base_de_donne\Base_de_donne_vierge";
                                    string Colle = @"C:\Logiciel_Cubage\Base_de_donne";
                                    // Use Path class to manipulate file and directory paths.
                                    string sourceFile = System.IO.Path.Combine(Copie, nom_fichier);
                                    string destFile = System.IO.Path.Combine(Colle, nom_fichier);

                                    // To copy a file to another location and                           
                                    System.IO.File.Copy(sourceFile, destFile, true);
                                    Form1 Archivage_finis = new Form1();
                                    Archivage_finis.Show();

                                }
                            }
                        }
                    }

                }
                else
                {
                    Form2 Archivage = new Form2("Etes-vous sûr d'archiver cette opération archivera toute les commandes/proprietaire/destinataire/parcelle ?");
                    if (Archivage.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
                    {
                        if (Archivage.Valeur == "Oui")
                        {
                            string date_commande_text = "";
                            if (dateTimePicker_Archivage.Value.Date.Month < 10)
                            {
                                if (dateTimePicker_Archivage.Value.Date.Day < 10)
                                {
                                    date_commande_text = dateTimePicker_Archivage.Value.Date.Year + "-0" + dateTimePicker_Archivage.Value.Date.Month + "-0" + dateTimePicker_Archivage.Value.Date.Day + "" ;
                                }
                                else
                                {
                                    date_commande_text = dateTimePicker_Archivage.Value.Date.Year + "-0" + dateTimePicker_Archivage.Value.Date.Month + "-" + dateTimePicker_Archivage.Value.Date.Day + "";
                                }
                            }
                            else
                            {
                                if (dateTimePicker_Archivage.Value.Date.Day < 10)
                                {
                                    date_commande_text = dateTimePicker_Archivage.Value.Date.Year + "-" + dateTimePicker_Archivage.Value.Date.Month + "-0" + dateTimePicker_Archivage.Value.Date.Day + "";
                                }
                                else
                                {
                                    date_commande_text = dateTimePicker_Archivage.Value.Date.Year + "-" + dateTimePicker_Archivage.Value.Date.Month + "-" + dateTimePicker_Archivage.Value.Date.Day + "";
                                }
                            }
                            //Deplace et renomme
                            string chemin_nouveau_nom = date_commande_text+".db";
                            string sourcePath = @"C:\Logiciel_Cubage\Base_de_donne\Base_de_donnee.db";
                            string targetPath = @"C:\Logiciel_Cubage\Archives\" + chemin_nouveau_nom;
                            this.Close();
                            System.IO.File.Copy(sourcePath, targetPath, true);

                            //Copie une BDD vierge
                            string nom_fichier = "Base_de_donnee.db";
                            string Copie = @"C:\Logiciel_Cubage\Base_de_donne\Base_de_donne_vierge";
                            string Colle = @"C:\Logiciel_Cubage\Base_de_donne";
                            // Use Path class to manipulate file and directory paths.
                            string sourceFile = System.IO.Path.Combine(Copie, nom_fichier);
                            string destFile = System.IO.Path.Combine(Colle, nom_fichier);

                            // To copy a file to another location and                           
                            System.IO.File.Copy(sourceFile, destFile, true);
                            Form1 Archivage_finis = new Form1();
                            Archivage_finis.Show();

                        }
                    }
                }
            }
            else { MessageBox.Show("Veuillez saisir le mot de passe"); }

        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void listBox_archive_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_archive.Text = listBox_archive.Text;
        }

        private void button_Ouvrir_archive_Click(object sender, EventArgs e)
        {
            if (label_archive.Text != "")
            {
                Form2 Ouverture_archive = new Form2("Etes-vous sûr d'ouvrir l'achive du : "+label_archive.Text);//Vérification
                if (Ouverture_archive.ShowDialog() == DialogResult.OK)//Permet d'attendre qu'on ai cliqué sur un bouton
                {
                    if (Ouverture_archive.Valeur == "Oui")
                    {
                        Form1 Archive = new Form1(@"data source = C:\Logiciel_Cubage\Archives\" + label_archive.Text+".db");
                        this.Close();
                        Archive.Show();
                    }
                }
            }
            else { MessageBox.Show("Selectionner la date de l'archive"); }
        }

        private void button_Retour_actuel_Click(object sender, EventArgs e)
        {
            Form1 Actuel = new Form1();
            this.Close();
            Actuel.Show();
        }

        private void button_Sauvegarder_Archives_Click(object sender, EventArgs e)
        {
            ouverture_connexion();
            string chemin = "";
            sql_cmd = new SQLiteCommand("SELECT Chemin_ou_sauvegarde_BDD FROM Sauvegarde_BDD;", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                chemin = query.GetString(0);
            }
            query.Close();
            fermeture_connexion();

            CopyDir(@"C:\Logiciel_Cubage\Archives", @"" + chemin);
            MessageBox.Show("Les archives on été sauvegardée");
        }
    }
}
    

