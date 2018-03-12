using System;
using System.Drawing;
using System.Windows.Forms;
using UBSAlarmModule.Clases;

namespace UBSAlarmModule
{
    public partial class AlarmForm : Form
    {
        private UBSLib.UBSVisualModule parent;

        ///////////////////////////////////
        private DescripcionAlarmas m_alarmas;

        ///////////////////////////////////
        public AlarmForm(UBSLib.UBSVisualModule _parent)
        {
            this.parent = _parent;
            InitializeComponent();

            Init();
            lstAlarms.Size = new Size(this.splSeparador.Panel1.Width - 100, this.splSeparador.Panel1.Height - 50);
            lstAlarms.Location = new Point(25, 25);
            lstAlarms.Font = new Font("Arial", 16, FontStyle.Bold);
            this.splSeparador.SplitterDistance = this.splSeparador.Height / 2;
        }

        ///////////////////////////////////
        private void Init()
        {
            m_alarmas = new DescripcionAlarmas();
        }

        ///////////////////////////////////
        delegate bool AddList(DescripcionAlarma da);
        public bool AñadirALista(DescripcionAlarma da)
        {
            if (lstAlarms.InvokeRequired)
            {
                AddList d = new AddList(AñadirALista);
                if (parent.Status != UBSLib.UBSModuleStatus.Closing && parent.Status != UBSLib.UBSModuleStatus.Closed)
                    return (bool)this.Invoke(new Func<bool>(() => d(da)));
                else
                    return true;
            }
            else
            {
                if (parent.Status != UBSLib.UBSModuleStatus.Closing && parent.Status != UBSLib.UBSModuleStatus.Closed)
                {
                    if (!lstAlarms.Items.Contains(da))
                    {
                        lstAlarms.Items.Add(da);
                        lstAlarms.DisplayMember = "Titulo";
                        return true;
                    }
                    else
                        return false;
                }
                return true;
            }
        }

        ///////////////////////////////////
        delegate bool RemoveList(DescripcionAlarma da);
        public bool BorrarDeLista(DescripcionAlarma da)
        {
            if (lstAlarms.InvokeRequired)
            {
                RemoveList d = new RemoveList(BorrarDeLista);
                if (parent.Status != UBSLib.UBSModuleStatus.Closing && parent.Status != UBSLib.UBSModuleStatus.Closed)
                    return (bool)this.Invoke(new Func<bool>(() => d(da)));
                else
                    return true;
            }
            else
            {
                try
                {
                    if (parent.Status != UBSLib.UBSModuleStatus.Closing && parent.Status != UBSLib.UBSModuleStatus.Closed)
                    {
                        if (lstAlarms.Items.Contains(da))
                        {
                            lstAlarms.Items.Remove(da);
                            return true;
                        }
                        return false;
                    }
                    return true;
                }
                catch (Exception e)
                {
                    parent.Error("Error al borrar la alarma de la lista. " + e.Message, true, false);
                    return false;
                }
            }
        }

        ///////////////////////////////////
        public bool ListaVacia()
        {
            return lstAlarms.Items.Count == 0;
        }

        ///////////////////////////////////
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAlarms.SelectedItem != null)
                txtDescripcion.Text = ((DescripcionAlarma)lstAlarms.SelectedItem).Titulo + ": " + ((DescripcionAlarma)lstAlarms.SelectedItem).Descripcion;
            else
                txtDescripcion.Text = "";
        }

        ///////////////////////////////////
        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (lstAlarms.SelectedIndex > 0)
                lstAlarms.SelectedIndex--;
            else
            {
                if (lstAlarms.SelectedIndex == 0)
                    lstAlarms.SelectedIndex = 0;
            }

        }

        ///////////////////////////////////
        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (lstAlarms.SelectedIndex < (lstAlarms.Items.Count - 1))
                lstAlarms.SelectedIndex++;
        }
    }
}
