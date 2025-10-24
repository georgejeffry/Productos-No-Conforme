
namespace PlaneamientoCDP
{
    partial class frmVisorImg
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
            this.btnCerrar = new GPNETv4.Windows.Ctrl.ButtonGP.btnButtonGP();
            this.pbVistoBueno = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbVistoBueno)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCerrar.Campo_Seguridad = "";
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCerrar.Image = null;
            this.btnCerrar.Location = new System.Drawing.Point(207, 385);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(93, 23);
            this.btnCerrar.TabIndex = 2;
            this.btnCerrar.Text = "&Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // pbVistoBueno
            // 
            this.pbVistoBueno.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbVistoBueno.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbVistoBueno.Location = new System.Drawing.Point(56, 70);
            this.pbVistoBueno.Name = "pbVistoBueno";
            this.pbVistoBueno.Size = new System.Drawing.Size(375, 299);
            this.pbVistoBueno.TabIndex = 3;
            this.pbVistoBueno.TabStop = false;
            // 
            // frmVisorImg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 447);
            this.Controls.Add(this.pbVistoBueno);
            this.Controls.Add(this.btnCerrar);
            this.Name = "frmVisorImg";
            this.Text = "frmVisorImg";
            this.Load += new System.EventHandler(this.frmVisorImg_Load);
            this.Controls.SetChildIndex(this.btnCerrar, 0);
            this.Controls.SetChildIndex(this.pbVistoBueno, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbVistoBueno)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbVistoBueno;
        private GPNETv4.Windows.Ctrl.ButtonGP.btnButtonGP btnCerrar;
    }
}