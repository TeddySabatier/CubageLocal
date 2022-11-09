namespace Logiciel
{
    partial class Form2
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
            this.label_Texte = new System.Windows.Forms.Label();
            this.button_Oui = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button_Non = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_Texte
            // 
            this.label_Texte.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Texte.AutoSize = true;
            this.label_Texte.Location = new System.Drawing.Point(8, 9);
            this.label_Texte.MaximumSize = new System.Drawing.Size(429, 144);
            this.label_Texte.Name = "label_Texte";
            this.label_Texte.Size = new System.Drawing.Size(182, 20);
            this.label_Texte.TabIndex = 0;
            this.label_Texte.Text = "Supprimer cet élément ?";
            this.label_Texte.Click += new System.EventHandler(this.label_Suprimercedestinataire_Click);
            // 
            // button_Oui
            // 
            this.button_Oui.Location = new System.Drawing.Point(361, 126);
            this.button_Oui.Name = "button_Oui";
            this.button_Oui.Size = new System.Drawing.Size(113, 47);
            this.button_Oui.TabIndex = 1;
            this.button_Oui.Text = "Oui";
            this.button_Oui.UseVisualStyleBackColor = true;
            this.button_Oui.Click += new System.EventHandler(this.button_Oui_Click);
            // 
            // button_Non
            // 
            this.button_Non.Location = new System.Drawing.Point(12, 126);
            this.button_Non.Name = "button_Non";
            this.button_Non.Size = new System.Drawing.Size(113, 47);
            this.button_Non.TabIndex = 2;
            this.button_Non.Text = "Non";
            this.button_Non.UseVisualStyleBackColor = true;
            this.button_Non.Click += new System.EventHandler(this.button_Non_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 181);
            this.Controls.Add(this.button_Non);
            this.Controls.Add(this.button_Oui);
            this.Controls.Add(this.label_Texte);
            this.Name = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Texte;
        private System.Windows.Forms.Button button_Oui;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button button_Non;
    }
}