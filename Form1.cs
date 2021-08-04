using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1.claseObjeto_BoletaDeVenta
{    
    public partial class frmBoleta : Form
    {
        // Variable Globales
        static int n;
        ListViewItem item;

        // Objeto de la clase boleta
        Boleta objB = new Boleta();
        public frmBoleta()
        {
            InitializeComponent();
        }

        private void frmBoleta_Load(object sender, EventArgs e)
        {
            lblNumero.Text = generaNumero();
            txtFecha.Text = DateTime.Now.ToShortDateString();
        }

        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            objB.producto = cboProducto.Text;
            txtPrecio.Text = objB.determinaPrecio().ToString("C");
        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            if (valida() == "")
            {
                // Capturar los datos
                capturaDatos();

                // Determina los calculos de la aplicacion
                double precio = objB.determinaPrecio();
                double importe = objB.calculaImporte();

                // Imprimir detalle de la venta
                imprimirDetalle(precio, importe);

                // Imprimir el total acumulado
                lblTotal.Text = determinaTotal().ToString("C");
            }
            else
                MessageBox.Show("Humano el error se encuentra en " + valida());
        }

        private void btnRegistrarBoleta_Click(object sender, EventArgs e)
        {
            ListViewItem fila = new ListViewItem("2021 - " + (int.Parse(lblNumero.Text).ToString("0000")));
            fila.SubItems.Add(txtFecha.Text);
            fila.SubItems.Add(totalCantidad().ToString("0.00"));
            fila.SubItems.Add(acumuladoImportes().ToString("C"));
            lvEstadisticas.Items.Add(fila);
            limpiarControles();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Humano estas seguro de salir del formulario?", "Boleta", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (r == DialogResult.Yes)
                this.Close();
        }
        // Metodo quegenera un numero aleatorio usando lambda
        Func<string> generaNumero = () =>
        {
            n++;
            return n.ToString("0000");
        };

        // Capturar los datos del formulario
        void capturaDatos()
        {
            objB.numero = int.Parse(lblNumero.Text);
            objB.cliente = txtCliente.Text;
            objB.direccion = txtDireccion.Text;
            objB.fecha = DateTime.Parse(txtFecha.Text);
            objB.dni = txtDni.Text;
            objB.producto = cboProducto.Text;
            objB.cantidad = int.Parse(txtCantidad.Text);
        }
        void imprimirDetalle(double precio, double importe)
        {
            ListViewItem fila = new ListViewItem(objB.cantidad.ToString());
            fila.SubItems.Add(objB.producto);
            fila.SubItems.Add(precio.ToString("0.00"));
            fila.SubItems.Add(importe.ToString("0.00"));
            lvDetalle.Items.Add(fila);
        }
        // Metodo que calcula el monto acumulado de importes
        double determinaTotal()
        {
            double total = 0;
            for(int i=0; i<lvDetalle.Items.Count; i++)
            {
                total += double.Parse(lvDetalle.Items[i].SubItems[3].Text);
            }
            return total;
        }
        // total de productos por boleta
        int totalCantidad()
        {
            int total = 0;
            for(int i=0; i<lvDetalle.Items.Count; i++)
            {
                total += int.Parse(lvDetalle.Items[i].SubItems[0].Text);
            }
            return total;
        }
        // Monto acumulado de los importes por boleta
        double acumuladoImportes()
        {
            double acumulado = 0;
            for(int i=0; i<lvDetalle.Items.Count; i++)
            {
                acumulado += double.Parse(lvDetalle.Items[i].SubItems[3].Text);
            }
            return acumulado;
        }
        // Validar el ingreso de datos
        string valida()
        {
            if (txtCliente.Text.Trim().Length == 0)
            {
                txtCliente.Focus();
                return "Humano escriba el nombre del cliente";
            }
            else if (txtDireccion.Text.Trim().Length == 0)
            {
                txtDireccion.Focus();
                return "Humano escriba la direccio del cliente";
            }
            else if (txtDni.Text.Trim().Length == 0)
            {
                txtDni.Focus();
                return "Humano ingrese el DNI del cliente";
            }
            else if (cboProducto.SelectedIndex == -1)
            {
                cboProducto.Focus();
                return "Humano seleccione producto";
            }
            else if (txtCantidad.Text.Trim().Length == 0)
            {
                txtCantidad.Focus();
                return "Humano ingrese la cantidad comprada";
            }
            return "";
        }
        void limpiarControles()
        {
            lblNumero.Text = generaNumero();
            txtCliente.Clear();
            txtDireccion.Clear();
            txtDni.Clear();
            cboProducto.Text ="Humano seleccione producto";
            txtPrecio.Clear();
            txtCantidad.Clear();
            lvDetalle.Items.Clear();
        }

        private void lvDetalle_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            item = lvDetalle.GetItemAt(e.X, e.Y);
            string producto = lvDetalle.Items[item.Index].SubItems[1].Text;
            DialogResult r = MessageBox.Show("Humano estas seguro de eliminar el producto? " + producto, "Boleta", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (r == DialogResult.Yes)
            {
                lvDetalle.Items.Remove(item);
                lblTotal.Text = acumuladoImportes().ToString("C");
                MessageBox.Show("Humano el detalle fue eliminado carrectamente");
            }
        }
    }
}
