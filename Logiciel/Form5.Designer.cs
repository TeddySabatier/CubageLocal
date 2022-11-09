namespace Logiciel
{
    partial class Form5
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
            this.label_Impression_texte = new System.Windows.Forms.Label();
            this.radioButton_Selection = new System.Windows.Forms.RadioButton();
            this.radioButton_Totalite = new System.Windows.Forms.RadioButton();
            this.button_Imprimer = new System.Windows.Forms.Button();
            this.groupBox_Feuille = new System.Windows.Forms.GroupBox();
            this.comboBox_Max = new System.Windows.Forms.ComboBox();
            this.comboBox_Min = new System.Windows.Forms.ComboBox();
            this.label_A = new System.Windows.Forms.Label();
            this.label_De = new System.Windows.Forms.Label();
            this.checkBox_Feuilles = new System.Windows.Forms.CheckBox();
            this.checkBox_Recapitulatif = new System.Windows.Forms.CheckBox();
            this.checkBox_Lesdeux = new System.Windows.Forms.CheckBox();
            this.button_annuler = new System.Windows.Forms.Button();
            this.progressBar_Impression = new System.Windows.Forms.ProgressBar();
            this.groupBox_Feuille.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_Impression_texte
            // 
            this.label_Impression_texte.AutoSize = true;
            this.label_Impression_texte.Location = new System.Drawing.Point(12, 9);
            this.label_Impression_texte.Name = "label_Impression_texte";
            this.label_Impression_texte.Size = new System.Drawing.Size(226, 20);
            this.label_Impression_texte.TabIndex = 0;
            this.label_Impression_texte.Text = "Que souhaitez vous imprimer ?";
            // 
            // radioButton_Selection
            // 
            this.radioButton_Selection.AutoSize = true;
            this.radioButton_Selection.Location = new System.Drawing.Point(41, 49);
            this.radioButton_Selection.Name = "radioButton_Selection";
            this.radioButton_Selection.Size = new System.Drawing.Size(100, 24);
            this.radioButton_Selection.TabIndex = 1;
            this.radioButton_Selection.TabStop = true;
            this.radioButton_Selection.Text = "Sélection";
            this.radioButton_Selection.UseVisualStyleBackColor = true;
            // 
            // radioButton_Totalite
            // 
            this.radioButton_Totalite.AutoSize = true;
            this.radioButton_Totalite.Location = new System.Drawing.Point(41, 175);
            this.radioButton_Totalite.Name = "radioButton_Totalite";
            this.radioButton_Totalite.Size = new System.Drawing.Size(86, 24);
            this.radioButton_Totalite.TabIndex = 2;
            this.radioButton_Totalite.TabStop = true;
            this.radioButton_Totalite.Text = "Totalité";
            this.radioButton_Totalite.UseVisualStyleBackColor = true;
            // 
            // button_Imprimer
            // 
            this.button_Imprimer.Location = new System.Drawing.Point(12, 193);
            this.button_Imprimer.Name = "button_Imprimer";
            this.button_Imprimer.Size = new System.Drawing.Size(123, 48);
            this.button_Imprimer.TabIndex = 16;
            this.button_Imprimer.Text = "Imprimer";
            this.button_Imprimer.UseVisualStyleBackColor = true;
            this.button_Imprimer.Click += new System.EventHandler(this.button_Imprimer_Click_1);
            // 
            // groupBox_Feuille
            // 
            this.groupBox_Feuille.Controls.Add(this.comboBox_Max);
            this.groupBox_Feuille.Controls.Add(this.comboBox_Min);
            this.groupBox_Feuille.Controls.Add(this.label_A);
            this.groupBox_Feuille.Controls.Add(this.radioButton_Totalite);
            this.groupBox_Feuille.Controls.Add(this.label_De);
            this.groupBox_Feuille.Controls.Add(this.radioButton_Selection);
            this.groupBox_Feuille.Location = new System.Drawing.Point(256, 12);
            this.groupBox_Feuille.Name = "groupBox_Feuille";
            this.groupBox_Feuille.Size = new System.Drawing.Size(182, 223);
            this.groupBox_Feuille.TabIndex = 19;
            this.groupBox_Feuille.TabStop = false;
            this.groupBox_Feuille.Text = "Impression feuilles";
            // 
            // comboBox_Max
            // 
            this.comboBox_Max.FormattingEnabled = true;
            this.comboBox_Max.Location = new System.Drawing.Point(103, 116);
            this.comboBox_Max.Name = "comboBox_Max";
            this.comboBox_Max.Size = new System.Drawing.Size(62, 28);
            this.comboBox_Max.TabIndex = 22;
            // 
            // comboBox_Min
            // 
            this.comboBox_Min.FormattingEnabled = true;
            this.comboBox_Min.Location = new System.Drawing.Point(6, 116);
            this.comboBox_Min.Name = "comboBox_Min";
            this.comboBox_Min.Size = new System.Drawing.Size(62, 28);
            this.comboBox_Min.TabIndex = 20;
            // 
            // label_A
            // 
            this.label_A.AutoSize = true;
            this.label_A.Location = new System.Drawing.Point(125, 93);
            this.label_A.Name = "label_A";
            this.label_A.Size = new System.Drawing.Size(20, 20);
            this.label_A.TabIndex = 21;
            this.label_A.Text = "A";
            // 
            // label_De
            // 
            this.label_De.AutoSize = true;
            this.label_De.Location = new System.Drawing.Point(22, 93);
            this.label_De.Name = "label_De";
            this.label_De.Size = new System.Drawing.Size(30, 20);
            this.label_De.TabIndex = 20;
            this.label_De.Text = "De";
            this.label_De.Click += new System.EventHandler(this.label1_Click);
            // 
            // checkBox_Feuilles
            // 
            this.checkBox_Feuilles.AutoSize = true;
            this.checkBox_Feuilles.Location = new System.Drawing.Point(26, 50);
            this.checkBox_Feuilles.Name = "checkBox_Feuilles";
            this.checkBox_Feuilles.Size = new System.Drawing.Size(89, 24);
            this.checkBox_Feuilles.TabIndex = 20;
            this.checkBox_Feuilles.Text = "Feuilles";
            this.checkBox_Feuilles.UseVisualStyleBackColor = true;
            this.checkBox_Feuilles.CheckedChanged += new System.EventHandler(this.checkBox_Feuilles_CheckedChanged);
            // 
            // checkBox_Recapitulatif
            // 
            this.checkBox_Recapitulatif.AutoSize = true;
            this.checkBox_Recapitulatif.Location = new System.Drawing.Point(26, 91);
            this.checkBox_Recapitulatif.Name = "checkBox_Recapitulatif";
            this.checkBox_Recapitulatif.Size = new System.Drawing.Size(124, 24);
            this.checkBox_Recapitulatif.TabIndex = 21;
            this.checkBox_Recapitulatif.Text = "Récapitulatif";
            this.checkBox_Recapitulatif.UseVisualStyleBackColor = true;
            this.checkBox_Recapitulatif.CheckedChanged += new System.EventHandler(this.checkBox_Recapitulatif_CheckedChanged);
            // 
            // checkBox_Lesdeux
            // 
            this.checkBox_Lesdeux.AutoSize = true;
            this.checkBox_Lesdeux.Location = new System.Drawing.Point(26, 130);
            this.checkBox_Lesdeux.Name = "checkBox_Lesdeux";
            this.checkBox_Lesdeux.Size = new System.Drawing.Size(99, 24);
            this.checkBox_Lesdeux.TabIndex = 22;
            this.checkBox_Lesdeux.Text = "Les deux";
            this.checkBox_Lesdeux.UseVisualStyleBackColor = true;
            this.checkBox_Lesdeux.CheckedChanged += new System.EventHandler(this.checkBox_Lesdeux_CheckedChanged);
            // 
            // button_annuler
            // 
            this.button_annuler.Location = new System.Drawing.Point(143, 193);
            this.button_annuler.Name = "button_annuler";
            this.button_annuler.Size = new System.Drawing.Size(107, 48);
            this.button_annuler.TabIndex = 23;
            this.button_annuler.Text = "Annuler";
            this.button_annuler.UseVisualStyleBackColor = true;
            this.button_annuler.Click += new System.EventHandler(this.button_annuler_Click);
            // 
            // progressBar_Impression
            // 
            this.progressBar_Impression.Location = new System.Drawing.Point(206, 89);
            this.progressBar_Impression.Name = "progressBar_Impression";
            this.progressBar_Impression.Size = new System.Drawing.Size(169, 33);
            this.progressBar_Impression.TabIndex = 24;
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 253);
            this.Controls.Add(this.progressBar_Impression);
            this.Controls.Add(this.button_annuler);
            this.Controls.Add(this.checkBox_Lesdeux);
            this.Controls.Add(this.checkBox_Recapitulatif);
            this.Controls.Add(this.checkBox_Feuilles);
            this.Controls.Add(this.groupBox_Feuille);
            this.Controls.Add(this.button_Imprimer);
            this.Controls.Add(this.label_Impression_texte);
            this.Name = "Form5";
            this.Text = "Impression";
            this.Load += new System.EventHandler(this.Form5_Load);
            this.groupBox_Feuille.ResumeLayout(false);
            this.groupBox_Feuille.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Impression_texte;
        private System.Windows.Forms.RadioButton radioButton_Selection;
        private System.Windows.Forms.RadioButton radioButton_Totalite;
        private System.Windows.Forms.Button button_Imprimer;
        private System.Windows.Forms.GroupBox groupBox_Feuille;
        private System.Windows.Forms.Label label_De;
        private System.Windows.Forms.ComboBox comboBox_Max;
        private System.Windows.Forms.ComboBox comboBox_Min;
        private System.Windows.Forms.Label label_A;
        private System.Windows.Forms.CheckBox checkBox_Feuilles;
        private System.Windows.Forms.CheckBox checkBox_Recapitulatif;
        private System.Windows.Forms.CheckBox checkBox_Lesdeux;
        private System.Windows.Forms.Button button_annuler;
        private System.Windows.Forms.ProgressBar progressBar_Impression;
    }
}