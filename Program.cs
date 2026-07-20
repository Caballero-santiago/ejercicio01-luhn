using System;
using System.IO;

class Program
{
    //=========================================
    // Acumuladores del sistema
    //=========================================

    static int tarjetasAceptadas = 0;
    static int tarjetasRechazadas = 0;

    //=========================================
    // Estadísticas por compañía
    //=========================================

    static int cantidadVisa = 0;
    static int cantidadMaster = 0;
    static int cantidadAmex = 0;
    static int cantidadDiscover = 0;

    static void Main()
    {
        int opcionMenu = 0;
        
        Console.Title = "Validador de Tarjetas Bancarias";
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        do
        {
            MostrarMenuPrincipal();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nSeleccione una opción: ");
            Console.ResetColor();

            if (!int.TryParse(Console.ReadLine(), out opcionMenu))
            {
                opcionMenu = -1;
            }

            Console.WriteLine();

            switch (opcionMenu)
            {
                case 1:
                    ProcesarTarjeta();
                    break;

                case 2:
                    LeerArchivo();
                    break;

                case 3:

                    string numeroGenerado = CrearTarjetaValida();

                    Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine("╔════════════════════════════════════╗");
                    Console.WriteLine("║      TARJETA GENERADA              ║");
                    Console.WriteLine("╚════════════════════════════════════╝");

                    Console.ResetColor();

                    Console.WriteLine($"Número : {numeroGenerado}");
                    Console.WriteLine($"Marca  : {ObtenerMarca(numeroGenerado)}");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Estado : VALIDACIÓN EXITOSA");
                    Console.ResetColor();

                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para volver al menú...");
                    Console.ReadKey();

                    break;

                case 4:
                    VerEstadisticas();
                    break;

                case 5:

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Gracias por utilizar el sistema.");
                    Console.ResetColor();

                    break;

                default:

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("La opción seleccionada no existe.");
                    Console.ResetColor();

                    break;
            }

        } while (opcionMenu != 5);
    }

    static void MostrarMenuPrincipal()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("╔══════════════════════════════════════════════╗");
        Console.WriteLine("║      SISTEMA DE VALIDACIÓN DE TARJETAS      ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝");

        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("  [1] Validar una tarjeta");
        Console.WriteLine("  [2] Leer tarjetas desde archivo");
        Console.WriteLine("  [3] Generar tarjeta válida");
        Console.WriteLine("  [4] Mostrar estadísticas");
        Console.WriteLine("  [5] Salir");

        Console.ResetColor();
    }
    static void ProcesarTarjeta()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Ingrese el número de la tarjeta: ");
        Console.ResetColor();

        string numeroDigitado = Console.ReadLine()?.Trim() ?? "";

        bool tarjetaCorrecta = EsTarjetaValida(numeroDigitado);

        string tipoTarjeta = ObtenerMarca(numeroDigitado);

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("══════════════════════════════════════");
        Console.WriteLine("        RESULTADO DE LA CONSULTA");
        Console.WriteLine("══════════════════════════════════════");
        Console.ResetColor();

        Console.WriteLine($"Número ingresado : {numeroDigitado}");
        Console.WriteLine($"Franquicia       : {tipoTarjeta}");

        if (tarjetaCorrecta)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Estado           : TARJETA VÁLIDA");
            Console.ResetColor();

            tarjetasAceptadas++;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Estado           : VALIDACIÓN FALLIDA");
            Console.ResetColor();

            tarjetasRechazadas++;
        }

        ActualizarEstadisticasMarca(tipoTarjeta);

        Console.WriteLine();
        Console.WriteLine("Presione una tecla para continuar...");
        Console.ReadKey();
    }

    static void VerEstadisticas()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;

        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine("║         REPORTE GENERAL            ║");
        Console.WriteLine("╚════════════════════════════════════╝");

        Console.ResetColor();

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Tarjetas válidas   : {tarjetasAceptadas}");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Tarjetas inválidas : {tarjetasRechazadas}");

        Console.ResetColor();

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Cantidad por franquicia");
        Console.ResetColor();

        Console.WriteLine("-------------------------------------");

        Console.WriteLine($"Visa               : {cantidadVisa}");
        Console.WriteLine($"Mastercard         : {cantidadMaster}");
        Console.WriteLine($"American Express   : {cantidadAmex}");
        Console.WriteLine($"Discover           : {cantidadDiscover}");

        Console.WriteLine("-------------------------------------");

        Console.WriteLine();
        Console.WriteLine("Presione una tecla para volver al menú...");
        Console.ReadKey();
    }

    static void LeerArchivo()
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Escriba la ruta del archivo: ");
            Console.ResetColor();

            string ubicacionArchivo = Console.ReadLine() ?? "";

            string[] tarjetas = File.ReadAllLines(ubicacionArchivo);

            int aceptadasArchivo = 0;
            int rechazadasArchivo = 0;

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========== RESULTADOS ===========");
            Console.ResetColor();

            foreach (string registro in tarjetas)
            {
                string numeroTarjeta = registro.Trim();

                bool esCorrecta = EsTarjetaValida(numeroTarjeta);

                string nombreMarca = ObtenerMarca(numeroTarjeta);

                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("--------------------------------------------");
                Console.ResetColor();

                Console.WriteLine($"Número : {numeroTarjeta}");
                Console.WriteLine($"Marca  : {nombreMarca}");

                if (esCorrecta)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Estado : TARJETA VÁLIDA");
                    Console.ResetColor();

                    tarjetasAceptadas++;
                    aceptadasArchivo++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Estado : VALIDACIÓN FALLIDA");
                    Console.ResetColor();

                    tarjetasRechazadas++;
                    rechazadasArchivo++;
                }
                ActualizarEstadisticasMarca(nombreMarca);
            }

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("============= RESUMEN =============");
            Console.ResetColor();

            Console.WriteLine($"Tarjetas leídas : {tarjetas.Length}");
            Console.WriteLine($"Válidas         : {aceptadasArchivo}");
            Console.WriteLine($"Inválidas       : {rechazadasArchivo}");

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine();
            Console.WriteLine("No fue posible abrir el archivo.");
            Console.WriteLine(ex.Message);

            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para regresar...");
            Console.ReadKey();
        }
    }

    static string CrearTarjetaValida()
    {
        Random aleatorio = new Random();

        string estructuraBase = "";
        int longitudTarjeta = 16;

        // Selecciona una franquicia al azar
        int marcaAleatoria = aleatorio.Next(4);

        switch (marcaAleatoria)
        {
            case 0: // Visa
                estructuraBase = "4";
                longitudTarjeta = 16;
                break;

            case 1: // Mastercard
                estructuraBase = aleatorio.Next(51, 56).ToString();
                longitudTarjeta = 16;
                break;

            case 2: // American Express
                estructuraBase = aleatorio.Next(2) == 0 ? "34" : "37";
                longitudTarjeta = 15;
                break;

            case 3: // Discover
                estructuraBase = "6011";
                longitudTarjeta = 16;
                break;
        }

        // Completa todos los dígitos excepto el último
        while (estructuraBase.Length < longitudTarjeta - 1)
        {
            estructuraBase += aleatorio.Next(10);
        }

        // Busca el dígito verificador correcto
        for (int ultimoDigito = 0; ultimoDigito <= 9; ultimoDigito++)
        {
            string numeroCompleto = estructuraBase + ultimoDigito;

            if (EsTarjetaValida(numeroCompleto))
            {
                return numeroCompleto;
            }
        }

        return "";
    }

    static bool EsTarjetaValida(string cadenaTarjeta)
    {
        if (string.IsNullOrWhiteSpace(cadenaTarjeta))
        {
            return false;
        }

        int sumaFinal = 0;
        bool multiplicarPorDos = false;

        // Se recorre la cadena desde el último carácter
        // aplicando el algoritmo de Luhn.

        for (int indice = cadenaTarjeta.Length - 1; indice >= 0; indice--)
        {
            if (!char.IsDigit(cadenaTarjeta[indice]))
            {
                return false;
            }

            int digitoActual = cadenaTarjeta[indice] - '0';

            if (multiplicarPorDos)
            {
                digitoActual *= 2;

                if (digitoActual > 9)
                {
                    digitoActual -= 9;
                }
            }

            sumaFinal += digitoActual;

            multiplicarPorDos = !multiplicarPorDos;
        }

        return sumaFinal % 10 == 0;
    }

    static string ObtenerMarca(string cadenaTarjeta)
    {
        if (string.IsNullOrWhiteSpace(cadenaTarjeta))
        {
            return "Desconocida";
        }

        if (!long.TryParse(cadenaTarjeta, out _))
        {
            return "Desconocida";
        }

        // Una tarjeta que no supera Luhn
        // no será tomada como perteneciente
        // a una franquicia reconocida.

        if (!EsTarjetaValida(cadenaTarjeta))
        {
            return "Desconocida";
        }

        //================ VISA =================

        if (cadenaTarjeta.StartsWith("4") &&
            (cadenaTarjeta.Length == 13 ||
            cadenaTarjeta.Length == 16))
        {
            return "Visa";
        }

        //============== MASTERCARD ==============

        if (cadenaTarjeta.Length == 16)
        {
            if (int.TryParse(cadenaTarjeta.Substring(0, 2), out int prefijoInicial))
            {
                if (prefijoInicial >= 51 &&
                    prefijoInicial <= 55)
                {
                    return "Mastercard";
                }
            }
        }

        //========== AMERICAN EXPRESS ============

        if (cadenaTarjeta.Length == 15 &&
            (cadenaTarjeta.StartsWith("34") ||
            cadenaTarjeta.StartsWith("37")))
        {
            return "American Express";
        }

        //=============== DISCOVER ===============

        if (cadenaTarjeta.Length >= 16 &&
            cadenaTarjeta.Length <= 19)
        {
            if (cadenaTarjeta.StartsWith("6011") ||
                cadenaTarjeta.StartsWith("65"))
            {
                return "Discover";
            }

            if (cadenaTarjeta.Length >= 3)
            {
                if (int.TryParse(cadenaTarjeta.Substring(0, 3), out int prefijoTresDigitos))
                {
                    if (prefijoTresDigitos >= 644 &&
                        prefijoTresDigitos <= 649)
                    {
                        return "Discover";
                    }
                }
            }

            if (cadenaTarjeta.Length >= 6)
            {
                if (int.TryParse(cadenaTarjeta.Substring(0, 6), out int prefijoSeisDigitos))
                {
                    if (prefijoSeisDigitos >= 622126 &&
                        prefijoSeisDigitos <= 622925)
                    {
                        return "Discover";
                    }
                }
            }
        }
        return "No identificada";
    }
    static void ActualizarEstadisticasMarca(string nombreMarca)
    {
        switch (nombreMarca)
        {
            case "Visa":
                cantidadVisa++;
                break;

            case "Mastercard":
                cantidadMaster++;
                break;

            case "American Express":
                cantidadAmex++;
                break;

            case "Discover":
                cantidadDiscover++;
                break;

            default:
                break;
        }
    }
}