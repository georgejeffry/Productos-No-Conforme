using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GPNETv4.Windows.Frm;
using Janus.Windows.GridEX;
using GPNETv4.Sistema.Config;
using System.Diagnostics;
using System.Data.SqlClient;
using GPNETv4.EntidadesNegocio;
using GPNETv4.Sistema.Util.GridEx;
using GPNETv4.Sistema.Util.Frm;
using GPNETv4.Sistema.Data;

namespace PlaneamientoCDP
{
    public partial class frmMotivoAnulacion : frmMasterSnMenu
    {

        #region Variables

        private bool bEsAnulado;

        #endregion

        #region Propiedades

        public bool EsAnulado
        {
            get { return bEsAnulado; }
        }
        public string MotivoAnulacion
        {
            get { return txtMotivoAnulacion.Text; }
        }

        #endregion

        #region Constructor

        public frmMotivoAnulacion()
        {
            InitializeComponent();
        }

        public frmMotivoAnulacion(string[] args):base(args)
        {
            InitializeComponent();
            bEsAnulado = false;
        }



        #endregion

        #region Eventos

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bEsAnulado = false;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtMotivoAnulacion.Text.Trim().Length > 0)
            {
                if (txtMotivoAnulacion.Text.Trim().Length > 3)
                {
                    bEsAnulado = true;
                    this.Close();
                }
                else
                {
                    Uti_frm.MsjInformacion("Minimo requerido es una frase o palabra");
                    txtMotivoAnulacion.Select();
                    txtMotivoAnulacion.Focus();
                    bEsAnulado = false;
                }
            }
            else
            {
                Uti_frm.MsjInformacion("Debe de ingresar un motivo");
                txtMotivoAnulacion.Select();
                txtMotivoAnulacion.Focus();
                bEsAnulado = false;
            }
        }


        #endregion
    }
}
