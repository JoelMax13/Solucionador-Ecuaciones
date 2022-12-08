//Daniel Arcos, Fernando Maldonado, Joel Oña, Diego Pin
using System;
using System.Collections.Generic;

namespace _533_EquationSolver
{
    class Program
    {
        static string ConvertirNotPolInv(string operacion) //Función que Convierte a Notación Polaca Inversa
        {
            Stack<char> signos = new Stack<char>(); //Pila donde se guardan los signos
            string resultado = ""; //variable donde se guarda la notación
            char op; //variable que guardará caracter por caracter
            for (int i = 0; i < operacion.Length; i++)
            {
                op = Convert.ToChar(operacion.Substring(i, 1)); //Recorremos caracter por caracter
                if (op == '+' || op == '-' || op == '*')
                    signos.Push(op); //Si es un operador guardará al signo en una pila
                else if (op == ')')
                    resultado += signos.Pop(); //Si existe un ')' sacará un signo y lo pondrá en el resultado
                else if (op != '(')
                    resultado += op; //añade el caracter al resultado en caso de que exista '('
            }
            while (signos.Count > 0)
                resultado += signos.Pop(); //despues de recorrer toda la cadena desapila todos los signos
            return resultado;
        }
        static Arbol ConvertirArbol(string cad, string[] numeros) //Función para realizar arbol de las operaciones mediante notación polaca inversa
        {
            Arbol resultado = new Arbol(); //Árbol que será creado
            Stack<Arbol> p = new Stack<Arbol>(); //Ceamos una pila de los nodos (en este caso llamada la clase Arbol)
            string op, dato; //variable op para ir caracter por caracter y dato para tomar los números (no signos)
            int j = 0; //Variable para recorrer el arreglo de los números
            for (int i = 0; i < cad.Length; i++)
            {
                op = cad.Substring(i, 1); //Recorremos caracter por caracter
                if (op == "+" || op == "-" || op == "*") //verifica si es un operador y lo guarda en un nodo operador y a los números antes y después los guarda en los der e izq
                {
                    Arbol operando1 = new Arbol();
                    Arbol operando2 = new Arbol();
                    Arbol operador = new Arbol();
                    operando1 = p.Pop();
                    operando2 = p.Pop();
                    operador.ConstruirCon3Datos(operando2, op, operando1);
                    p.Push(operador);
                }
                else
                {
                    while(numeros[j] == "")
                        j++;
                    dato = numeros[j];
                    i += dato.Length - 1; //i avanzará en la cadena el número de subíndices que tenga un número
                    Arbol operando = new Arbol();
                    operando.ConstruirCon3Datos(null, dato, null);
                    p.Push(operando);
                    j++;
                }
            }
            resultado = p.Pop();
            return resultado;
        }
        static bool EsEcuacionLineal(string op_ing) //Función para verificar si es una ecuación lineal (utilizamos un cotntains)
        {
            bool respuesta = true; //Retorna falso si no es una ecuación lineal
            if (op_ing.Contains("x*x") || op_ing.Contains("x^"))
                respuesta = false;
            return respuesta;
        }
        static string Proceso(string op_ing) //Método para realizar el proceso de la ecuación lineal
        {
            Arbol arbol1 = new Arbol(); //(arbol1: Antes del igual)  
            Arbol arbol2 = new Arbol(); //(arbol2: Después del igual)
            Arbol arbolfin1 = new Arbol(); //(arbolfin1: antes de igualar a 0)
            Arbol arbolfin2 = new Arbol(); //(arbolfin2: despues de igualar a 0)
            char[] separador = { '+', '-', '(', ')', '*' }; //Matriz de tipo char para separar los números
            double sinX; //Respuesta del numerador (números sin x)
            double conX; //Respuesta del denominador (números con x)
            double resp; //X despejada
            string[] ecuacion = op_ing.Split('='); //Separa a la ecuación por el igual
            string antes_igual = ConvertirNotPolInv(ecuacion[0]); //Utiliza la notación inversa para 1ra parte
            string[] ec_antes = ecuacion[0].Split(separador); //Guarda a los números en arreglo
            string desp_igual = ConvertirNotPolInv(ecuacion[1]); //Utiliza la notación inversa para 2da parte
            string[] ec_desp = ecuacion[1].Split(separador); //Guarda a los números en arreglo
            string respuesta; //respuesta que retorna la función
            if(ec_antes.Length == 1) //Verificamos si antes del igual existe un solo número 
            {
                if (ec_antes[0].Contains("x")) //Si después del igual existe una x
                {
                    if (ec_antes[0] == "x") 
                        arbolfin1.Elemento_con_x = "1"; //Cambia al elemento con x por 1
                    else
                        arbolfin1.Elemento_con_x = ec_antes[0].Replace("x", ""); //si existe coeficiente remplaza la x por ""
                    arbolfin1.Elemento_sin_x = "0"; //Cambia al elemento sin x por 0
                }
                else
                {
                    arbolfin1.Elemento_sin_x = ec_antes[0]; //si no existe x despues del igual
                    arbolfin1.Elemento_con_x = "0"; //Verificamos si antes del igual existe un solo número
                }
            }
            else
            {
                arbol1 = ConvertirArbol(antes_igual, ec_antes); //Generamos un árbol con los primeros datos
                arbolfin1 = arbol1.Resolver();
            }

            if(ec_desp.Length == 1) //Verificamos si despues del igual existe un solo número
            {
                if (ec_desp[0].Contains("x")) //Si después del igual existe una x
                {
                    if (ec_desp[0] == "x")
                        arbolfin2.Elemento_con_x = "1"; //Cambia al elemento con x por 1
                    else
                        arbolfin2.Elemento_con_x = ec_desp[0].Replace("x", ""); //si existe coeficiente remplaza la x por ""
                    arbolfin2.Elemento_sin_x = "0"; //Cambia al elemento sin x por 0
                }
                else
                {
                    arbolfin2.Elemento_sin_x = ec_desp[0]; //si no existe x despues del igual
                    arbolfin2.Elemento_con_x = "0"; //Verificamos si antes del igual existe un solo número
                }
            }
            else
            {
                arbol2 = ConvertirArbol(desp_igual, ec_desp); //Generamos un árbol con los primeros datos
                arbolfin2 = arbol2.Resolver();
            }
            sinX = (Convert.ToDouble(arbolfin2.Elemento_sin_x) - Convert.ToDouble(arbolfin1.Elemento_sin_x)); //Dejamos la mínima expresión de los números sin variables
            conX = (Convert.ToDouble(arbolfin1.Elemento_con_x) - Convert.ToDouble(arbolfin2.Elemento_con_x)); //Dejamos la mínima expresión de los números con variables
            if ((sinX == 0) && (conX == 0))
                respuesta = "Infinitas soluciones."; //Caso en el que existan infinitas soluciones
            else if ((sinX != 0) && (conX == 0))
                respuesta = "No tiene solución."; //Caso en el que no exista solución
            else
            {
                resp = (sinX / conX); //Caso en el que exista una solución
                respuesta = "x = " + Convert.ToString(Math.Round(resp,6)); //utilizamos 6 variables
            }
            return respuesta;
        }
        static void ImprimirResp(Queue<string> Cola) //Imprimimos los resultados con el número de ecuación
        {
            int num_resp = Cola.Count; //Guardamos la longitud de la cola
            for(int i = 0; i < num_resp; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ecuación #{i+1}");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(Cola.Dequeue());
                Console.ResetColor();
                Console.WriteLine();
            }
        }
        static void Instrucciones()
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("*******************************************************************************");
            Console.WriteLine("*    S O L U C I O N A D O R   D E   E C U A C I O N E S   L I N E A L E S    *");
            Console.WriteLine("*******************************************************************************");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\nInstrucciones:\n");
            Console.ResetColor();
            Console.WriteLine("1. Ingresar ecuaciones de una sola variable y lineal, es decir, sin grados en las variables.");
            Console.WriteLine("2. Utilizar unicamente paréntesis cuando se necesite hacer subprocesos.");
            Console.WriteLine("3. Tomar en cuenta la jerarquía de signos, es decir, primero se resuelven paréntesis, multiplicación, sumas y restas.");
            Console.WriteLine("4. No puede ingresar '/' (divisiones).");
            Console.WriteLine("5. No puede ingresar números negativos al principio de la ecuación.");
            Console.WriteLine("6. Cuando ingrese un 'enter' el programa mostrará por pantalla sus resultados.");
            Console.WriteLine("7. Recordar que puede existir una solución, varias soluciones y ninguna solución.");
            Console.WriteLine("8. Utilizar x como variable, además escribir expresiones como: 4x y no 4*x. *Usar x minúsculas");
            Console.WriteLine("9. Cuando ingrese una multiplicación antes de un paréntesis, colocar un asterísco. Ej: 5*(4x)");
            Console.WriteLine("10. Si su ecuación no tiene igualación utilizar '=0'.");
            Console.WriteLine("11. No ingresar espacios en la ecuación.");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n\tPresione cualquier tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }
        static bool Repetir
        {
            get
            {
                bool repetir = false;
                Console.WriteLine("¿Desea repetir el programa?(si/no)");
                string resp = Console.ReadLine().ToUpper();
                if (resp == "SI")
                    repetir = true;
                return repetir;
            }
        }
        static void Main(string[] args)
        {
            Queue<string> cola_respuesta = new Queue<string>();//Lista para guardar los resultados que se presentaran por pantalla
            string op_ing; //Variable ecuación ingresada
            do
            {
                Console.Clear();
                Instrucciones(); //Muestra por pantalla las respuestas
                do
                {
                    Console.WriteLine("Ingrese su ecuación lineal...");
                    op_ing = Console.ReadLine().Trim();
                    if (op_ing != "") //Se verifica que no sea un enter ingresado
                    {
                        if (EsEcuacionLineal(op_ing)) //Se verifica si es una ecuación lineal 
                            cola_respuesta.Enqueue(Proceso(op_ing)); //Se realiza el proceso para sacar la respuesta y enconlar las respuestas
                        else
                            Console.WriteLine("Su ecuación no es lineal.");
                    }
                } while (op_ing != "");
                if (cola_respuesta.Count != 0)
                    ImprimirResp(cola_respuesta); //Imprimimos respuestas
                else
                    Console.WriteLine("No ingresó ecuaciones a resolver.");
            } while (Repetir);
            Console.Clear();
            Console.WriteLine("\n\tGracias por utilizar este programa =)!!. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
