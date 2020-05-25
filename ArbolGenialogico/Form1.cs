using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;


namespace ArbolGenialogico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged; //como es esto posible?
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //FuncionQueCarga = CargarXML;
            // personas = FuncionQueCarga();if
            personas = CargarXML();
            dataGridView1.DataSource = personas;
        }

        List<Persona> personas = new List<Persona>();
        Persona personaSeleccionada = null;

        private delegate List<Persona> Cargar();
       

        private void buttonAgregar_Click(object sender, EventArgs e) //boton agregar
        {
            var persona = new Persona();
            persona.Nombre = textNom.Text;
            persona.Dni = int.Parse(textDNI.Text);

            personas.Add(persona);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = personas;
            GuardarXML();
        }

        private void button1_Click(object sender, EventArgs e) // boton borrar
        {
            personas.Remove(personaSeleccionada);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = personas;
            GuardarXML();

        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                personaSeleccionada = (Persona)dataGridView1.SelectedRows[0].DataBoundItem;
                
                textNom.Text = personaSeleccionada.Nombre;
                textDNI.Text = personaSeleccionada.Dni.ToString();
            }
            else
            {
                personaSeleccionada = null;
            }
        }

        private void button2_Click(object sender, EventArgs e) // modificar personas
        {
            personaSeleccionada.Nombre = textNom.Text;
            personaSeleccionada.Dni = int.Parse(textDNI.Text);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = personas;
            GuardarXML();
        }

        private void button3_Click(object sender, EventArgs e) //setear padre
        {
            Persona nuevoPadre = (Persona)dataGridViewParent.SelectedRows[0].DataBoundItem;
            personaSeleccionada.Padre = nuevoPadre; //habria que poner una condicion para que no se pueda poner de parde a uno mismo
            personaSeleccionada.Padre.AgregarHijo(personaSeleccionada);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = personas;

            dataGridViewParent.DataSource = null;
            GuardarXML();
        }

        private void button4_Click(object sender, EventArgs e) //guardar el padre/madre para despues asignarselo al hijo
        {

            List<Persona> pariente = new List<Persona>();
            if (pariente.Count > 0)
            {
                pariente.RemoveAt(0);
            }
            pariente.Add(personaSeleccionada);
            dataGridViewParent.DataSource = null;
            dataGridViewParent.DataSource = pariente;


           
        }

        private void button5_Click(object sender, EventArgs e) //setear madre
        {
            personaSeleccionada.Madre = (Persona)dataGridViewParent.SelectedRows[0].DataBoundItem; //habria que poner una condicion para que no se pueda poner de madre a uno mismo
            personaSeleccionada.Madre.AgregarHijo(personaSeleccionada);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = personas;
            GuardarXML();
        }

        private void GuardarXML()
        {
            XmlDocument doc = new XmlDocument();
            var raiz = doc.CreateElement("Personas");
            doc.AppendChild(raiz);

            foreach (var p in personas)
            {
                var nodo = doc.CreateElement("Persona");
                var nombre = doc.CreateElement("Nombre");
                nombre.InnerText = p.Nombre;
                var dni = doc.CreateElement("DNI");
                dni.InnerText = p.Dni.ToString();
                var nodo2 = doc.CreateElement("Padre");
                var nombrePadre = doc.CreateElement("NombrePadre");

                if (p.Padre != null)
                {
                    nombrePadre.InnerText = p.Padre.Nombre;
                }
                var dniPadre = doc.CreateElement("DNIPadre");
                if (p.Padre != null)
                {
                    dniPadre.InnerText = p.Padre.Dni.ToString();
                }

                nodo.AppendChild(nombre);
                nodo.AppendChild(dni);
                nodo2.AppendChild(nombrePadre);
                nodo2.AppendChild(dniPadre);
                nodo.AppendChild(nodo2);
                raiz.AppendChild(nodo);
            }
            doc.Save(Environment.CurrentDirectory + "\\arbol.txt");
        }

        public List<Persona> CargarXML()
        {
            List<Persona> resultado = new List<Persona>();
            
                XmlDocument doc = new XmlDocument();
                doc.Load(Environment.CurrentDirectory + "\\arbol.txt");
                var raiz = doc.LastChild; // [Personas] ?
                foreach (XmlNode hijo in raiz.ChildNodes)
                {
                    var p = new Persona();
                    p.Nombre = hijo["Nombre"].InnerText;
                    p.Dni = int.Parse(hijo["DNI"].InnerText);
                    if (p.Padre != null)
                    {
                        p.Padre.Nombre = hijo["NombrePadre"].InnerText;
                        p.Padre.Dni = int.Parse(hijo["DNIPadre"].InnerText);
                    }
                    resultado.Add(p);
                }
            return resultado;
        }

        public List<Persona> MostrarHijos(Persona p)
        {
            return p.hijos;
        }

        private void GuardarXML2()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Persona>));
            var writer = XmlWriter.Create(Environment.CurrentDirectory + "\\arbol.txt");
            serializer.Serialize(writer, personas);
            writer.Close();
        }

        private List<Persona> CargarXML2()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Persona>));
            var reader = XmlReader.Create(Environment.CurrentDirectory + "\\arbol.txt");
            var result = (List<Persona>)serializer.Deserialize(reader);
            reader.Close();
            return result;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            personas = CargarXML();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = personas;
        }

        private Persona MostrarAbueloPaterno(Persona persona)
        {
            if (persona.Padre != null)
            {
                Persona abueloPaterno = (persona.Padre).Padre;
                return abueloPaterno;
            }
            else
                return null;
        }

        private List<Persona> MostrarHermano(Persona persona)
        {
            List<Persona> hermanos = new List<Persona>();
            foreach(Persona h in persona.Madre.hijos)
            {
                if(h != personaSeleccionada)
                {
                    hermanos.Add(h);
                }
            }
            return hermanos;
        }

        public List<Persona> MostrarTios(Persona p)
        {
            List<Persona> resultado = new List<Persona>();
            Persona abuelo = (p.Padre).Padre;
            foreach (Persona t in personas)
            {
                if((abuelo == t.Padre) && t != p && t!= p.Padre)
                {
                    resultado.Add(t); //los tios
                }
            }
            return resultado;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if ((personaSeleccionada.Padre).Padre != null)  //se puede manejar mejor este error
                {
                    Persona p = MostrarAbueloPaterno(personaSeleccionada);
                    textAbueloPaterno.Text = p.Nombre;
                }
            }
            catch(Exception)
            {
                textAbueloPaterno.Text = "Sin Padre";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MostrarHermano(personaSeleccionada);
            dataGridViewParent.DataSource = null;
            dataGridViewParent.DataSource = MostrarHermano(personaSeleccionada);
        }

        private void button9_Click(object sender, EventArgs e) //mostrar tios
        {
            dataGridViewParent.DataSource = null;
            dataGridViewParent.DataSource = MostrarTios(personaSeleccionada);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dataGridViewParent.DataSource = null;
            dataGridViewParent.DataSource = MostrarHijos(personaSeleccionada);
        }
    }
}
