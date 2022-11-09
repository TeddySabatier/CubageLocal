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
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;
using System.Diagnostics;
using System.ComponentModel;

namespace Logiciel
{

    public partial class Form5 : Form
    {
        public Form5(DataGridView Donne_arbre, string Total_ecorce_ded, string Total_ecorce, string Cube1_ded, string Cube1, string Cube2_ded, string Cube2, string Cube3_ded, string Cube3)
        {
            InitializeComponent();
            Impression = Donne_arbre;
            button_Imprimer.DialogResult = DialogResult.OK;
            button_annuler.DialogResult = DialogResult.OK;
            Total_ecorce_deduite = Total_ecorce_ded;
            Total_ecorce_non = Total_ecorce;
            Cube1_non = Cube1;
            Cube1_deduit = Cube1_ded;
            Cube2_non = Cube2;
            Cube2_deduit = Cube2_ded;
            Cube3_non = Cube3;
            Cube3_deduit = Cube3_ded;
            this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        static bool IsOpened(string wbook)
        {
            bool isOpened = true;
            Excel.Application exApp;
            exApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            try
            {
                exApp.Workbooks.get_Item(wbook);
            }
            catch (Exception)
            {
                isOpened = false;
            }
            return isOpened;
        }
        DataGridView Impression;
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private UInt32 id_fichier = Donneespubliques.id_fichier;
        string Total_ecorce_deduite;
        string Total_ecorce_non;
        string Cube1_non;
        string Cube1_deduit;
        string Cube2_non;
        string Cube2_deduit;
        string Cube3_non;
        string Cube3_deduit;
        UInt32 numerotation = 0;
        private void ouverture_connexion()
        {
            sql_con = new SQLiteConnection(Donneespubliques.chemin_bdd);
            sql_con.Open();
        }
        private void fermeture_connexion()
        {
            sql_con.Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            button_annuler.BackColor = Color.DarkOrange;//Couleur
            button_annuler.FlatStyle = FlatStyle.Flat;
            button_annuler.FlatAppearance.BorderColor = Color.DarkOrange;
            button_annuler.FlatAppearance.BorderSize = 1;
            //
            //
            button_Imprimer.BackColor = Color.LimeGreen;//Couleur
            button_Imprimer.FlatStyle = FlatStyle.Flat;
            button_Imprimer.FlatAppearance.BorderColor = Color.LimeGreen;
            button_Imprimer.FlatAppearance.BorderSize = 1;

            UInt32 nb_pages = 0;
            ouverture_connexion();
            //On calcule le nombre de page de notre commande sachant qu'une page a 50 arbres
            sql_cmd = new SQLiteCommand("SELECT Numero FROM Arbre WHERE Id_Fichier LIKE '" + Convert.ToString(id_fichier) + "';", sql_con);//Nouvelle commande
            SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
            while (query.Read())//Dure le temps de la requete
            {
                numerotation = Convert.ToUInt32(query.GetValue(0).ToString());
            }
            query.Close();
            fermeture_connexion();
            double nb_page_reel = ((numerotation / 50.0) + 0.999999);
            nb_pages = Convert.ToUInt32(Math.Truncate(nb_page_reel));
            for (int i = 1; i <= nb_pages; i++)
            {
                comboBox_Max.Items.Add(i);
                comboBox_Min.Items.Add(i);
            }
            groupBox_Feuille.Hide();
            radioButton_Totalite.Checked = true;
            progressBar_Impression.Visible = false;
            progressBar_Impression.Maximum = 5;
            progressBar_Impression.Minimum = 0;


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //Gestion de la selections des options d'impression
        private void checkBox_Lesdeux_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Lesdeux.Checked)
            {
                checkBox_Feuilles.Checked = true;
                checkBox_Recapitulatif.Checked = true;
                checkBox_Feuilles.Checked = true;
                checkBox_Recapitulatif.Checked = true;

            }
            else
            {
                checkBox_Feuilles.Checked = false;
                checkBox_Recapitulatif.Checked = false;
            }
        }

        private void checkBox_Recapitulatif_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Recapitulatif.Checked && checkBox_Feuilles.Checked)
            {
                checkBox_Lesdeux.Checked = true;
            }
            else
            {
                checkBox_Lesdeux.Checked = false;
            }
        }

        private void checkBox_Feuilles_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Feuilles.Checked)
            {
                groupBox_Feuille.Show();
            }
            else
            {
                groupBox_Feuille.Hide();

            }
            if (checkBox_Recapitulatif.Checked && checkBox_Feuilles.Checked)
            {
                checkBox_Lesdeux.Checked = true;
            }
            else
            {
                checkBox_Lesdeux.Checked = false;
            }
        }

        private void button_annuler_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void button_Imprimer_Click_1(object sender, EventArgs e)
        {
            try
            {
               Microsoft.Office.Interop.Excel.Application excelApp = null;
                try
                {
                    int page_min = 0;
                    int page_max = 0;
                    if (checkBox_Feuilles.Checked)//Selectionner feuilles
                    {
                        if (radioButton_Selection.Checked)//Selectionner juste la selection des pages a imprimer
                        {
                            if (comboBox_Min.Text != "" && comboBox_Max.Text != "" && int.TryParse(comboBox_Min.Text, out page_min) && int.TryParse(comboBox_Max.Text, out page_max) && (page_min <= page_max))
                            {
                                progressBar_Impression.Step = 1;//Barre de chargement
                                progressBar_Impression.Visible = true;
                                button_Imprimer.Visible = false;
                                button_annuler.Visible = false;
                                groupBox_Feuille.Hide();
                                //On commence par copier le fichier Excel original et on écrase l'ancienne copie

                                string fileName = "Impression_Feuille.xls";
                                string sourcePath = @"C:\Logiciel_Cubage\Fichier_Excel_Impression\Dossier_original";
                                string targetPath = @"C:\Logiciel_Cubage\Fichier_Excel_Impression\Dossier_copie";

                                // Use Path class to manipulate file and directory paths.
                                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                                string destFile = System.IO.Path.Combine(targetPath, fileName);


                                // To copy a file to another location and
                                // overwrite the destination file if it already exists.
                                System.IO.File.Copy(sourceFile, destFile, true);
                                //On remplis le fichier Excel
                                excelApp = new Excel.Application();
                                progressBar_Impression.PerformStep();
                                excelApp.Visible = false;

                                Excel.Worksheet workBooks = (Excel.Worksheet)excelApp.ActiveSheet;


                                //Ouverture du fichier Excel, à vous de choisir l'emplacement ou est situé le fichier excel ainsi que son nom!!

                                Microsoft.Office.Interop.Excel._Workbook workbook = excelApp.Workbooks.Open(@"C:\Logiciel_Cubage\Fichier_Excel_Impression\Dossier_copie\Impression_Feuille.xls");

                                workBooks = workbook.Sheets["Feuil1"]; // On sélectionne la Feuil1

                                Impression.RowHeadersVisible = false;

                                for (int Rows = 1; Rows < Impression.Columns.Count + 1; Rows++)
                                {
                                    workBooks.Cells[1, Rows] = Impression.Columns[Rows - 1].HeaderText;
                                }

                                // on recopie toutes les valeurs du DataGridView dans le fichier Excel
                                progressBar_Impression.PerformStep();
                                for (int Rows = 0; Rows < Impression.Rows.Count - 1; Rows++)
                                {
                                    for (int Columns = 0; Columns < Impression.Columns.Count; Columns++)
                                    {
                                        if (Convert.ToString(Impression.Rows[Rows].Cells[Columns].Value) != "") { workBooks.Cells[Rows + 2, Columns + 1] = Convert.ToSingle(Impression.Rows[Rows].Cells[Columns].Value); }
                                        else { workBooks.Cells[Rows + 2, Columns + 1] = Impression.Rows[Rows].Cells[Columns].Value; }
                                    }

                                }
                                progressBar_Impression.PerformStep();
                                workBooks.PrintOutEx(page_min, page_max, 1, false);
                                excelApp.DisplayAlerts = false;
                                excelApp.Quit();
                                progressBar_Impression.PerformStep();
                                progressBar_Impression.Visible = false;
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                            }
                            else { MessageBox.Show("Selectionner des pages parmis la liste et s'assurer que la page minimun est inférieure à la page maximum"); }

                        }
                        if (radioButton_Totalite.Checked)//Si on imprimer toute la commande
                        {
                            progressBar_Impression.Step = 1;
                            progressBar_Impression.Visible = true;
                            button_Imprimer.Visible = false;
                            button_annuler.Visible = false;
                            groupBox_Feuille.Hide();
                            //On commence par copier le fichier Excel original et on écrase l'ancienne copie

                            string fileName = "Impression_Feuille.xls";
                            string sourcePath = @"C:\Logiciel_Cubage\Fichier_Excel_Impression\Dossier_original";
                            string targetPath = @"C:\Logiciel_Cubage\Fichier_Excel_Impression\Dossier_copie";

                            
                            // Use Path class to manipulate file and directory paths.
                            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                            string destFile = System.IO.Path.Combine(targetPath, fileName);


                            // To copy a file to another location and
                            // overwrite the destination file if it already exists.
                            System.IO.File.Copy(sourceFile, destFile, true);
                            //On remplis le fichier Excel
                            excelApp = new Excel.Application();
                            progressBar_Impression.PerformStep();
                            excelApp.Visible = false;

                            Excel._Worksheet workBooks = (Excel.Worksheet)excelApp.ActiveSheet;

                            //Ouverture du fichier Excel, à vous de choisir l'emplacement ou est situé le fichier excel ainsi que son nom!!

                            Microsoft.Office.Interop.Excel._Workbook workbook = excelApp.Workbooks.Open(@"C:\Logiciel_Cubage\Fichier_Excel_Impression\Dossier_copie\Impression_Feuille.xls");

                            workBooks = workbook.Sheets["Feuil1"]; // On sélectionne la Feuil1
                            Impression.RowHeadersVisible = false;

                            for (int Rows = 1; Rows < Impression.Columns.Count + 1; Rows++)
                            {
                                workBooks.Cells[1, Rows] = Impression.Columns[Rows - 1].HeaderText;
                            }
                            progressBar_Impression.PerformStep();
                            // on recopie toutes les valeurs du DataGridView dans le fichier Excel

                            for (int Rows = 0; Rows < Impression.Rows.Count - 1; Rows++)
                            {
                                for (int Columns = 0; Columns < Impression.Columns.Count; Columns++)
                                {
                                    if (Convert.ToString(Impression.Rows[Rows].Cells[Columns].Value) != "") { workBooks.Cells[Rows + 2, Columns + 1] = Convert.ToSingle(Impression.Rows[Rows].Cells[Columns].Value); }
                                    else { workBooks.Cells[Rows + 2, Columns + 1] = Impression.Rows[Rows].Cells[Columns].Value; }
                                }
                            }
                            
                            progressBar_Impression.PerformStep();
                            
                            workBooks.PrintOutEx();
                            
                            excelApp.DisplayAlerts = false;
                            
                            excelApp.Quit();
                            
                            progressBar_Impression.PerformStep();
                            
                            progressBar_Impression.Visible = false;
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                    }
                    this.Close();
                }
                catch//En cas d'erreur on ferme les fichiers excel ouvert
                {
                    excelApp.DisplayAlerts = false;
                    excelApp.Quit();
                    MessageBox.Show("Erreur de la copie du fichier pour l'impression. Assurez-vous que votre Excel soit en mode virgule et non point.");
                }

                try
                {
                    if (checkBox_Recapitulatif.Checked)//Si imprimer le récapitulatif
                    {
                        progressBar_Impression.Step = 1;
                        progressBar_Impression.Visible = true;
                        button_Imprimer.Visible = false;
                        button_annuler.Visible = false;
                        groupBox_Feuille.Hide();
                        //On commence par copier le fichier Excel original et on écrase l'ancienne copie

                        string fileName = "Impression_Recapitulatif.xls";
                        string sourcePath = @"C:\Logiciel_Cubage\Fichier_Excel_Impression\Dossier_original";
                        string targetPath = @"C:\Logiciel_Cubage\Fichier_Excel_Impression\Dossier_copie";

                        // Use Path class to manipulate file and directory paths.
                        string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                        string destFile = System.IO.Path.Combine(targetPath, fileName);
                        
                        // To copy a file to another location and
                        // overwrite the destination file if it already exists.
                        System.IO.File.Copy(sourceFile, destFile, true);
                        //On remplis le fichier Excel
                        excelApp = new Excel.Application();
                        excelApp.Visible = false;
                        Excel._Worksheet workBooks = (Excel.Worksheet)excelApp.ActiveSheet;

                        //Ouverture du fichier Excel, à vous de choisir l'emplacement ou est situé le fichier excel ainsi que son nom!!

                        Microsoft.Office.Interop.Excel._Workbook workbook = excelApp.Workbooks.Open(@"C:\Logiciel_Cubage\Fichier_Excel_Impression\Dossier_copie\Impression_Recapitulatif.xls");

                        workBooks = workbook.Sheets["Feuil1"]; // On sélectionne la Feuil1

                        progressBar_Impression.PerformStep();
                        Impression.RowHeadersVisible = false;
                        ouverture_connexion();
                        string Typedebois = "";
                        sql_cmd = new SQLiteCommand("SELECT Typedebois FROM Parcelle WHERE Id LIKE (SELECT Id_Parcelle FROM Fichier WHERE Id LIKE " + Convert.ToString(id_fichier) + ");", sql_con);//Nouvelle commande
                        SQLiteDataReader query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                        while (query.Read())//Dure le temps de la requete
                        {
                            Typedebois = query.GetValue(0).ToString();
                        }
                        query.Close();
                        fermeture_connexion();
                         switch (Typedebois)
                        {
                            case "Feuillus":

                                workBooks.Cells[2, 3] = "0%";
                                break;
                            case "Pin":
                                workBooks.Cells[2, 3] = "8%";
                                break;
                            case "Sapin/Epicéa":
                                workBooks.Cells[2, 3] = "10%";
                                break;
                            case "Douglas":
                                workBooks.Cells[2, 3] = "12%";

                                break;

                            default:
                                MessageBox.Show("Erreur Type de bois non trouvé");
                                break;
                        }
                        ouverture_connexion();
                        string commentaire = "";
                        string commentaire1 = "";
                        string commentaire2 = "";
                        string commentaire3 = "";
                        string mesure = "";
                        string Type_cubage = "";
                        sql_cmd = new SQLiteCommand("SELECT Commentaire,Mesure FROM Fichier WHERE Id LIKE " + Convert.ToString(id_fichier) + ";", sql_con);//Nouvelle commande
                        query = sql_cmd.ExecuteReader();//Permet de lire ce que fait la commande
                        while (query.Read())//Dure le temps de la requete
                        {
                            commentaire = query.GetValue(0).ToString();
                            mesure = query.GetValue(1).ToString();
                        }
                        query.Close();
                        progressBar_Impression.PerformStep();
                        fermeture_connexion();
                        if (commentaire != "")
                        {
                            int i = 0;
                            while (commentaire[i] != '¤')
                            {
                                commentaire1 = commentaire1 + commentaire[i];
                                i++;
                            }
                            i++;
                            while (commentaire[i] != '¤')
                            {
                                commentaire2 = commentaire2 + commentaire[i];
                                i++;
                            }
                            i++;
                            while (commentaire[i] != '¤')
                            {
                                commentaire3 = commentaire3 + commentaire[i];
                                i++;
                            }
                        }
                        workBooks.Cells[8, 2] = commentaire1;
                        workBooks.Cells[9, 2] = commentaire2;
                        workBooks.Cells[10, 2] = commentaire3;
                        workBooks.Cells[13, 2] = numerotation;
                        progressBar_Impression.PerformStep();
                        if (mesure == "reel")
                        {
                            Type_cubage = "Au réel";
                        }
                        else
                        {
                            Type_cubage = "Au quart";
                        }
                        workBooks.Cells[15, 2] = Type_cubage;
                        progressBar_Impression.PerformStep();
                        workBooks.Cells[3, 2] = Convert.ToDecimal(Total_ecorce_non.Replace(".", ","));
                        workBooks.Cells[4, 2] = Convert.ToDecimal(Cube1_non.Replace(".", ","));
                        workBooks.Cells[5, 2] = Convert.ToDecimal(Cube2_non.Replace(".", ","));
                        workBooks.Cells[6, 2] = Convert.ToDecimal(Cube3_non.Replace(".", ","));

                        workBooks.Cells[3, 3] = Convert.ToDecimal(Total_ecorce_deduite.Replace(".", ","));
                        workBooks.Cells[4, 3] = Convert.ToDecimal(Cube1_deduit.Replace(".", ","));
                        workBooks.Cells[5, 3] = Convert.ToDecimal(Cube2_deduit.Replace(".", ","));
                        workBooks.Cells[6, 3] = Convert.ToDecimal(Cube3_deduit.Replace(".", ","));
                        workBooks.PrintOutEx();
                        excelApp.DisplayAlerts = false;
                        excelApp.Quit();
                        progressBar_Impression.PerformStep();
                        progressBar_Impression.Visible = false;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    foreach (Process clsProcess in Process.GetProcesses())
                        if (clsProcess.ProcessName.Equals("EXCEL"))  //Process Excel?
                            clsProcess.Kill();
                    // Fermeture de l'application Excel
                    this.Close();
                }
                catch//En cas d'erreur on ferme les fichiers excel ouvert
                {
                    foreach (Process clsProcess in Process.GetProcesses())
                        if (clsProcess.ProcessName.Equals("EXCEL"))  //Process Excel?
                            clsProcess.Kill();

                    excelApp.DisplayAlerts = false;
                    excelApp.Quit();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    MessageBox.Show("Erreur de la copie du fichier pour l'impression. Assurez-vous que votre Excel soit en mode virgule et non point.");
                }
                
            }
            catch
            {
                foreach (Process clsProcess in Process.GetProcesses())
                    if (clsProcess.ProcessName.Equals("EXCEL"))  //Process Excel?
                        clsProcess.Kill();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                MessageBox.Show("Verifiez que vous avez Excel et que la version est compatible avec 97-2003");
            }

        }
    }
}
