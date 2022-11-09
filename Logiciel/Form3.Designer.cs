namespace Logiciel
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox_parametre = new System.Windows.Forms.GroupBox();
            this.radioButton_quart = new System.Windows.Forms.RadioButton();
            this.radioButton_reel = new System.Windows.Forms.RadioButton();
            this.textBox_Cubesup = new System.Windows.Forms.TextBox();
            this.label_Cubesup = new System.Windows.Forms.Label();
            this.label_Cubeintermediare = new System.Windows.Forms.Label();
            this.textBox_Cubeinf = new System.Windows.Forms.TextBox();
            this.label_Cubeinf = new System.Windows.Forms.Label();
            this.groupBox_commanditaire_destinataire = new System.Windows.Forms.GroupBox();
            this.label_destinataire = new System.Windows.Forms.Label();
            this.label_Info_propiétaire = new System.Windows.Forms.Label();
            this.button_Creation_commande = new System.Windows.Forms.Button();
            this.button_Annuler = new System.Windows.Forms.Button();
            this.groupBox_parametre.SuspendLayout();
            this.groupBox_commanditaire_destinataire.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_parametre
            // 
            this.groupBox_parametre.Controls.Add(this.radioButton_quart);
            this.groupBox_parametre.Controls.Add(this.radioButton_reel);
            this.groupBox_parametre.Controls.Add(this.textBox_Cubesup);
            this.groupBox_parametre.Controls.Add(this.label_Cubesup);
            this.groupBox_parametre.Controls.Add(this.label_Cubeintermediare);
            this.groupBox_parametre.Controls.Add(this.textBox_Cubeinf);
            this.groupBox_parametre.Controls.Add(this.label_Cubeinf);
            this.groupBox_parametre.Location = new System.Drawing.Point(26, 13);
            this.groupBox_parametre.Name = "groupBox_parametre";
            this.groupBox_parametre.Size = new System.Drawing.Size(442, 228);
            this.groupBox_parametre.TabIndex = 0;
            this.groupBox_parametre.TabStop = false;
            this.groupBox_parametre.Text = "Paramètre";
            // 
            // radioButton_quart
            // 
            this.radioButton_quart.AutoSize = true;
            this.radioButton_quart.Location = new System.Drawing.Point(11, 74);
            this.radioButton_quart.Name = "radioButton_quart";
            this.radioButton_quart.Size = new System.Drawing.Size(74, 24);
            this.radioButton_quart.TabIndex = 9;
            this.radioButton_quart.TabStop = true;
            this.radioButton_quart.Text = "Quart";
            this.radioButton_quart.UseVisualStyleBackColor = true;
            // 
            // radioButton_reel
            // 
            this.radioButton_reel.AutoSize = true;
            this.radioButton_reel.Location = new System.Drawing.Point(10, 44);
            this.radioButton_reel.Name = "radioButton_reel";
            this.radioButton_reel.Size = new System.Drawing.Size(67, 24);
            this.radioButton_reel.TabIndex = 8;
            this.radioButton_reel.TabStop = true;
            this.radioButton_reel.Text = "Réel";
            this.radioButton_reel.UseVisualStyleBackColor = true;
            this.radioButton_reel.CheckedChanged += new System.EventHandler(this.radioButton_reel_CheckedChanged);
            // 
            // textBox_Cubesup
            // 
            this.textBox_Cubesup.Location = new System.Drawing.Point(315, 170);
            this.textBox_Cubesup.Name = "textBox_Cubesup";
            this.textBox_Cubesup.Size = new System.Drawing.Size(87, 26);
            this.textBox_Cubesup.TabIndex = 7;
            // 
            // label_Cubesup
            // 
            this.label_Cubesup.AutoSize = true;
            this.label_Cubesup.Location = new System.Drawing.Point(311, 126);
            this.label_Cubesup.Name = "label_Cubesup";
            this.label_Cubesup.Size = new System.Drawing.Size(91, 40);
            this.label_Cubesup.TabIndex = 6;
            this.label_Cubesup.Text = "   Cube 3\r\nSupérieur à";
            // 
            // label_Cubeintermediare
            // 
            this.label_Cubeintermediare.AutoSize = true;
            this.label_Cubeintermediare.Location = new System.Drawing.Point(116, 138);
            this.label_Cubeintermediare.Name = "label_Cubeintermediare";
            this.label_Cubeintermediare.Size = new System.Drawing.Size(169, 60);
            this.label_Cubeintermediare.TabIndex = 5;
            this.label_Cubeintermediare.Text = "            Cube 2\r\n\r\nEntre ces deux valeurs";
            // 
            // textBox_Cubeinf
            // 
            this.textBox_Cubeinf.Location = new System.Drawing.Point(5, 170);
            this.textBox_Cubeinf.Name = "textBox_Cubeinf";
            this.textBox_Cubeinf.Size = new System.Drawing.Size(80, 26);
            this.textBox_Cubeinf.TabIndex = 4;
            // 
            // label_Cubeinf
            // 
            this.label_Cubeinf.AutoSize = true;
            this.label_Cubeinf.Location = new System.Drawing.Point(6, 126);
            this.label_Cubeinf.Name = "label_Cubeinf";
            this.label_Cubeinf.Size = new System.Drawing.Size(79, 40);
            this.label_Cubeinf.TabIndex = 3;
            this.label_Cubeinf.Text = "  Cube 1 \r\ninférieur à";
            // 
            // groupBox_commanditaire_destinataire
            // 
            this.groupBox_commanditaire_destinataire.Controls.Add(this.label_destinataire);
            this.groupBox_commanditaire_destinataire.Controls.Add(this.label_Info_propiétaire);
            this.groupBox_commanditaire_destinataire.Location = new System.Drawing.Point(474, 24);
            this.groupBox_commanditaire_destinataire.Name = "groupBox_commanditaire_destinataire";
            this.groupBox_commanditaire_destinataire.Size = new System.Drawing.Size(580, 217);
            this.groupBox_commanditaire_destinataire.TabIndex = 2;
            this.groupBox_commanditaire_destinataire.TabStop = false;
            this.groupBox_commanditaire_destinataire.Text = "Propriétaire/Destinataire";
            // 
            // label_destinataire
            // 
            this.label_destinataire.AutoSize = true;
            this.label_destinataire.Location = new System.Drawing.Point(6, 32);
            this.label_destinataire.Name = "label_destinataire";
            this.label_destinataire.Size = new System.Drawing.Size(169, 20);
            this.label_destinataire.TabIndex = 1;
            this.label_destinataire.Text = "label_info_destinataire";
            // 
            // label_Info_propiétaire
            // 
            this.label_Info_propiétaire.AutoSize = true;
            this.label_Info_propiétaire.Location = new System.Drawing.Point(6, 63);
            this.label_Info_propiétaire.Name = "label_Info_propiétaire";
            this.label_Info_propiétaire.Size = new System.Drawing.Size(166, 20);
            this.label_Info_propiétaire.TabIndex = 0;
            this.label_Info_propiétaire.Text = "label_info_proprietaire";
            // 
            // button_Creation_commande
            // 
            this.button_Creation_commande.Location = new System.Drawing.Point(26, 247);
            this.button_Creation_commande.Name = "button_Creation_commande";
            this.button_Creation_commande.Size = new System.Drawing.Size(156, 68);
            this.button_Creation_commande.TabIndex = 3;
            this.button_Creation_commande.Text = "Création commande";
            this.button_Creation_commande.UseVisualStyleBackColor = true;
            this.button_Creation_commande.Click += new System.EventHandler(this.button_Creation_commande_Click);
            // 
            // button_Annuler
            // 
            this.button_Annuler.Location = new System.Drawing.Point(898, 247);
            this.button_Annuler.Name = "button_Annuler";
            this.button_Annuler.Size = new System.Drawing.Size(156, 68);
            this.button_Annuler.TabIndex = 4;
            this.button_Annuler.Text = "Annuler";
            this.button_Annuler.UseVisualStyleBackColor = true;
            this.button_Annuler.Click += new System.EventHandler(this.button_Annuler_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 367);
            this.Controls.Add(this.button_Annuler);
            this.Controls.Add(this.button_Creation_commande);
            this.Controls.Add(this.groupBox_commanditaire_destinataire);
            this.Controls.Add(this.groupBox_parametre);
            this.Name = "Form3";
            this.Text = "Création commande";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.groupBox_parametre.ResumeLayout(false);
            this.groupBox_parametre.PerformLayout();
            this.groupBox_commanditaire_destinataire.ResumeLayout(false);
            this.groupBox_commanditaire_destinataire.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_parametre;
        private System.Windows.Forms.Label label_Cubeinf;
        private System.Windows.Forms.TextBox textBox_Cubeinf;
        private System.Windows.Forms.Label label_Cubeintermediare;
        private System.Windows.Forms.TextBox textBox_Cubesup;
        private System.Windows.Forms.Label label_Cubesup;
        private System.Windows.Forms.GroupBox groupBox_commanditaire_destinataire;
        private System.Windows.Forms.Label label_destinataire;
        private System.Windows.Forms.Label label_Info_propiétaire;
        private System.Windows.Forms.RadioButton radioButton_quart;
        private System.Windows.Forms.RadioButton radioButton_reel;
        private System.Windows.Forms.Button button_Creation_commande;
        private System.Windows.Forms.Button button_Annuler;
    }
}