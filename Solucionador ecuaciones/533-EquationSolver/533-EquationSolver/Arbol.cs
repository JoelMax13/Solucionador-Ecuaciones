using System;

namespace _533_EquationSolver
{
    class Arbol
    {
        private string elemento_sin_x; //Contenido sin x (números que no tienen x) 
        private string elemento_con_x; //Contenido con x (números que tienen x)
        private Arbol izq, der; //Referencia a los hijos izquierdo y derecho de cada nodo

        public string Elemento_sin_x { get => elemento_sin_x; set => elemento_sin_x = value; } //hacemos descriptores para trabajar en el main
        public string Elemento_con_x { get => elemento_con_x; set => elemento_con_x = value; } //hacemos descriptores para trabajar en el main
        public Arbol(string dato = null) //Constructor de Nodo vacío
        {
            elemento_sin_x = dato;
            elemento_con_x = dato;
            izq = der = null;
        }
        public void ConstruirCon3Datos(Arbol izquierdo, string raiz, Arbol derecho) //Método para generar los nodos de los árboles
        {
            izq = izquierdo;
            der = derecho;
            if (raiz.Contains("x"))
            {
                if (raiz == "x")
                    elemento_con_x = "1"; //En caso de que el string ingresado sea solo "x", se cambiará por 1 (Que existe una x)
                else
                    elemento_con_x = raiz.Replace("x", ""); //Cambiamos las x por vacío para trabajar con los números con x
                elemento_sin_x = "0"; //Si existe x en el número, entonces no existirá un número sin variable y será 0
            }
            else
            {
                elemento_sin_x = raiz; //En caso de existir un número sin variable se guarda en elementos sin x
                elemento_con_x = "0"; //Si no existe un número con variable será = 0
            }
        }
        //Verifica si el nodo en el que se trabaja es un signo o no
        public bool EsSigno(Arbol nodo)
        {
            bool respuesta = false; //variable para verificar si es un signo
            if (nodo.elemento_sin_x == "+" || nodo.elemento_sin_x == "-" || nodo.elemento_sin_x == "*")
                respuesta = true;
            return respuesta;
        }
        //Función que realiza el proceso algebráico de los nodos con sus hijos
        public Arbol Resolver()
        {
            Arbol respuesta = new Arbol(); //Variable donde se guardará la respuesta del proceso
            Arbol izquierda = new Arbol();
            Arbol derecha = new Arbol(); //Variables donde se guardarán los datos de los hijos
            
            if (EsSigno(izq))
                izquierda = izq.Resolver(); //Si el hijo izquierdo del nodo es un signo volverá a buscar
            else
            {
                izquierda.elemento_sin_x = izq.elemento_sin_x; //Cuando no sea un signo guardará en la variable izquierda para realizar el proceso
                izquierda.elemento_con_x = izq.elemento_con_x;
            }

            if (EsSigno(der))
                derecha = der.Resolver(); //Si el hijo derecho del nodo es un signo volverá a buscar
            else
            {
                derecha.elemento_sin_x = der.elemento_sin_x; //Cuando no sea un signo guardará en la variable izquierda para realizar el proceso
                derecha.elemento_con_x = der.elemento_con_x;
            }
            switch (elemento_sin_x) //dependiendo del operador realiza el proceso correspondiente
            {
                case "+":
                    {
                        //Resuelve las sumas de los números con x y sin x
                        respuesta.elemento_con_x = Convert.ToString(Convert.ToDouble(izquierda.elemento_con_x) + Convert.ToDouble(derecha.elemento_con_x));
                        respuesta.elemento_sin_x = Convert.ToString(Convert.ToDouble(izquierda.elemento_sin_x) + Convert.ToDouble(derecha.elemento_sin_x));
                        break;
                    }
                case "-":
                    {
                        //Resuelve las restas de los números con x y sin x
                        respuesta.elemento_con_x = Convert.ToString(Convert.ToDouble(izquierda.elemento_con_x) - Convert.ToDouble(derecha.elemento_con_x));
                        respuesta.elemento_sin_x = Convert.ToString(Convert.ToDouble(izquierda.elemento_sin_x) - Convert.ToDouble(derecha.elemento_sin_x));
                        break;
                    }
                case "*":
                    {
                        //Caso posible: Se realiza la multiplicación del nodo izquierdo sin variable y elemento derecho con variable 
                        if ((izquierda.elemento_sin_x != "0")&&(derecha.elemento_con_x != "0"))
                        {
                            respuesta.elemento_con_x = Convert.ToString(Convert.ToDouble(izquierda.elemento_sin_x) * Convert.ToDouble(derecha.elemento_con_x));
                            respuesta.elemento_sin_x = Convert.ToString(Convert.ToDouble(izquierda.elemento_sin_x) * Convert.ToDouble(derecha.elemento_sin_x));
                        }
                        //Caso posible: Se realiza la multiplicación del nodo derecho sin variable y el nodo izquiedo con variable
                        if ((derecha.elemento_sin_x != "0") && (izquierda.elemento_con_x != "0"))
                        {
                            respuesta.elemento_con_x = Convert.ToString(Convert.ToDouble(derecha.elemento_sin_x) * Convert.ToDouble(izquierda.elemento_con_x));
                            respuesta.elemento_sin_x = Convert.ToString(Convert.ToDouble(derecha.elemento_sin_x) * Convert.ToDouble(izquierda.elemento_sin_x));
                        }
                        else
                            respuesta.elemento_sin_x = Convert.ToString(Convert.ToDouble(derecha.elemento_sin_x) * Convert.ToDouble(izquierda.elemento_sin_x));

                        break;
                    }
            }
            return respuesta;
        }
    }
}
