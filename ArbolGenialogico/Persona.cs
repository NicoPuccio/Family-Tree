using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolGenialogico
{
    public class Persona
    {
        public string Nombre { get; set; }
        public int Dni { get; set; }
        public Persona Padre { get; set; }
        public Persona Madre { get; set; }
        public List<Persona> hijos { get; set; }
        
        public Persona()
        {
            hijos = new List<Persona>();
        }

        public void AgregarHijo(Persona p)
        {
            hijos.Add(p);
        }

        public override string ToString()
        {
            if(this.Nombre == null)
            {
                throw new Exception("El nombre del padre es nulo!");
            }
            return string.Format(this.Nombre);
        }


       
        /*public List<Persona> Hijos { get => hijos; set => hijos = value; }  /*???
        public string Nombre { get => nombre; set => nombre = value; }          ???*/
        /*public int Dni { get => dni; set => dni = value; }
        public Persona Padre { get => padre; set => padre = value; }
        public Persona Madre { get => madre; set => madre = value; }*/
    }
}
