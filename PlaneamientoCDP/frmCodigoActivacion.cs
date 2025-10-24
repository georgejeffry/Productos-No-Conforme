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
    public partial class frmCodigoActivacion : frmMasterSnMenu
    {
        #region Parametros

        private int m_nResultadoValidacion = 5;
        private string sUsuario = GlobalIdentity.Instance.P_Sys_Default_Usuario;

        public int ResultadoValidacion
        {
            get { return m_nResultadoValidacion; }
        }
        public string Id_Usuario_Aprueba
        {
            get { return sUsuario; }
        }

        #endregion
        #region constructor

        public frmCodigoActivacion()
        {
            InitializeComponent();
        }
        public frmCodigoActivacion(string[] args):base(args)
        {
            InitializeComponent();
        }


        #endregion

        #region Eventos

        private void frmCodigoActivacion_Load(object sender, EventArgs e)
        {


            txtUsuario.Text = sUsuario;
            txtPassword.Select();
        }

        #endregion

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            m_nResultadoValidacion = -1;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            daDatabase odaDatabase = new daDatabase();

            //Validar previa
            if (txtUsuario.Text.Trim().Length == 0)
            {
                Uti_frm.MsjAdvertencia("Debe de ingresar un usuario");
                txtUsuario.Select();
                return;
            }
            if (txtPassword.Text.Trim().Length == 0)
            {
                Uti_frm.MsjAdvertencia("Debe de ingresar una clave");
                txtPassword.Select();
                return;
            }

            sUsuario = txtUsuario.Text.Trim().ToUpper();


            string sProcediminetoAlm = "sp_Esta_Autorizado_VB_Art_no_conf";

            List<SqlParameter> loSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@Cia",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@Id_Usuario",SqlDbType.VarChar,20){Value=sUsuario},
                new SqlParameter("@Password",SqlDbType.VarChar,100){Value=GPNETv4.Sistema.Seguridad.GP.EncriptarXISONE(txtPassword.Text)}
            };



            try
            {



                beDatabaseResult obeDatabaseResult = odaDatabase.GetUnicoValor(sProcediminetoAlm, loSqlParameter);
                Control oControl = null;

                if (obeDatabaseResult != null && obeDatabaseResult.Data != null)
                {

                    m_nResultadoValidacion = (int)obeDatabaseResult.Data;

                    if (m_nResultadoValidacion == 1 )
                    {
                        this.Close();
                    }
                    else
                    {
                        string sMensaje = string.Empty;

                        switch (m_nResultadoValidacion)
                        {
                            case 0:
                                sMensaje = "Usuario no autorizado";
                                oControl = txtUsuario;
                                break;
                            case 2:
                                sMensaje = "Usuario inactivo";
                                oControl = txtUsuario;
                                break;
                            case 3:
                                sMensaje = "No existe el usuario " + txtUsuario.Text;
                                oControl = txtUsuario;
                                break;
                            case 4:
                                sMensaje = "Contraseña incorrecta ";
                                oControl = txtPassword;
                                break;
                        }

                        Uti_frm.MsjError(sMensaje);
                        oControl.Select();
                        ((GPNETv4.Windows.Ctrl.Texbox.txtTextBoxGP)oControl).SelectAll();
                    }




                }
                else if (obeDatabaseResult.Resultado != "")
                {
                    Uti_frm.MsjError(obeDatabaseResult.Resultado);
                }




            }
            catch (SqlException ex)
            {

            }
        }
    }
}
