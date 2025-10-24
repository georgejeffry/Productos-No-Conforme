using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



using Janus.Windows.GridEX;
using GPNETv4.Sistema.Util.Frm;
using GPNETv4.Windows.Frm;
using GPNETv4.Sistema.Config;
using GPNETv4.DataString;
using GPNETv4.EntidadesNegocio;
using System.Data.SqlClient;
using GPNETv4.Sistema.Data;


namespace PlaneamientoCDP
{
    public partial class frmVisorImg : frmMaster
    {

        #region Propiedades

        public string sPathImagen { get; set; }

        #endregion

        #region Constructor
        public frmVisorImg()
        {
            InitializeComponent();
        }

        public frmVisorImg(string[] args):base(args)
        {
            InitializeComponent();
        }

        #endregion

        #region Eventos

        private void frmVisorImg_Load(object sender, EventArgs e)
        {
            Asignar_Titulo_Ventana("Imagen");


            try
            {

                pbVistoBueno.Image = new Bitmap(sPathImagen);
                pbVistoBueno.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch(Exception ex)
            {
                Mensaje_Proceso(ex.Message,Properties.Resources.Error,null,true,TipoMessageBoxGPNET.Error);
            }

        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
