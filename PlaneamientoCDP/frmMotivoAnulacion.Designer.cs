
namespace PlaneamientoCDP
{
    partial class frmMotivoAnulacion
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
            this.txtMotivoAnulacion = new GPNETv4.Windows.Ctrl.Texbox.txtTextBoxGP();
            this.btnCancelar = new GPNETv4.Windows.Ctrl.ButtonGP.btnButtonGP();
            this.btnAceptar = new GPNETv4.Windows.Ctrl.ButtonGP.btnButtonGP();
            this.SuspendLayout();
            // 
            // txtMotivoAnulacion
            // 
            this.txtMotivoAnulacion._MarcaAguaText = "Ingrese un Dato...";
            this.txtMotivoAnulacion.AgregarBotonIzquierda = false;
            this.txtMotivoAnulacion.ButtonImage = null;
            this.txtMotivoAnulacion.ButtonImageIzquierda = null;
            this.txtMotivoAnulacion.Campo_del_Seg_Valor_Dev = "";
            this.txtMotivoAnulacion.Campo_del_Valor_Buscar = "";
            this.txtMotivoAnulacion.Campo_del_Valor_Dev = "";
            this.txtMotivoAnulacion.Campo_Info_System = "";
            this.txtMotivoAnulacion.Campo_Scrib_Anulado = "";
            this.txtMotivoAnulacion.Campo_Seguridad = "";
            this.txtMotivoAnulacion.CamposColAgruparBq = null;
            this.txtMotivoAnulacion.CamposOcultosdelaBusq = null;
            this.txtMotivoAnulacion.CamposPintadoGrupo1 = null;
            this.txtMotivoAnulacion.ColorControlDesenfocado = System.Drawing.Color.White;
            this.txtMotivoAnulacion.ColorControlEnfocado = System.Drawing.Color.LightCyan;
            this.txtMotivoAnulacion.ColorPrimerGrupo = System.Drawing.Color.Empty;
            this.txtMotivoAnulacion.ControlSetFocus = null;
            this.txtMotivoAnulacion.DialogoDescripcion = null;
            this.txtMotivoAnulacion.DialogoFiltro = null;
            this.txtMotivoAnulacion.DialogoPathFull = null;
            this.txtMotivoAnulacion.DialogoTitulo = null;
            this.txtMotivoAnulacion.dtDatosDevMultipleBusq = null;
            this.txtMotivoAnulacion.EjecutarBusquedaalCargar = false;
            this.txtMotivoAnulacion.Entidad_SelectPersonalizado = null;
            this.txtMotivoAnulacion.ListaCamposAdicionales = null;
            this.txtMotivoAnulacion.ListaSQLParametros = null;
            this.txtMotivoAnulacion.Location = new System.Drawing.Point(12, 12);
            this.txtMotivoAnulacion.MaskTypeGPzCustom = "";
            this.txtMotivoAnulacion.MatchElement = null;
            this.txtMotivoAnulacion.MaxNumOfSuggestions = 0;
            this.txtMotivoAnulacion.Mostrar_Anulados = false;
            this.txtMotivoAnulacion.MostrarSubtotalesBusquedaAvanzada = false;
            this.txtMotivoAnulacion.MostrarSubtotalesPiePagBA = Janus.Windows.GridEX.InheritableBoolean.Default;
            this.txtMotivoAnulacion.Multiline = true;
            this.txtMotivoAnulacion.Name = "txtMotivoAnulacion";
            this.txtMotivoAnulacion.SegExportarExcel = false;
            this.txtMotivoAnulacion.SegundoValorDevuelto = "";
            this.txtMotivoAnulacion.SeleccionMultipleBusq = false;
            this.txtMotivoAnulacion.Size = new System.Drawing.Size(483, 253);
            this.txtMotivoAnulacion.sstEstadoControl = null;
            this.txtMotivoAnulacion.SuggestDataSource = null;
            this.txtMotivoAnulacion.TabIndex = 3;
            this.txtMotivoAnulacion.TextFromElement = null;
            this.txtMotivoAnulacion.TituloVentanaBusq = "";
            this.txtMotivoAnulacion.TypeRulParaFormBusqueda = null;
            this.txtMotivoAnulacion.VinoDelaVentanaBusqueda = false;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Campo_Seguridad = "";
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancelar.Image = global::PlaneamientoCDP.Properties.Resources.Cancelar;
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelar.Location = new System.Drawing.Point(402, 271);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(93, 23);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Campo_Seguridad = "";
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAceptar.Image = global::PlaneamientoCDP.Properties.Resources.Aceptar;
            this.btnAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAceptar.Location = new System.Drawing.Point(12, 271);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(93, 23);
            this.btnAceptar.TabIndex = 7;
            this.btnAceptar.Text = "&Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // frmMotivoAnulacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(507, 327);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.txtMotivoAnulacion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMotivoAnulacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Motivo de Anulación";
            this.Controls.SetChildIndex(this.txtMotivoAnulacion, 0);
            this.Controls.SetChildIndex(this.btnAceptar, 0);
            this.Controls.SetChildIndex(this.btnCancelar, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GPNETv4.Windows.Ctrl.Texbox.txtTextBoxGP txtMotivoAnulacion;
        private GPNETv4.Windows.Ctrl.ButtonGP.btnButtonGP btnCancelar;
        private GPNETv4.Windows.Ctrl.ButtonGP.btnButtonGP btnAceptar;
    }
}