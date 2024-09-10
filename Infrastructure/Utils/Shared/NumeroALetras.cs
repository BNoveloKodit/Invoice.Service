namespace Invoice.Service.Infrastructure.Utils.Shared
{
    using System.Globalization;

    public static class NumeroALetras
    {
        public static string Convertir(decimal monto)
        {
            string decimales = ((monto - Math.Truncate(monto)) * 100).ToString("00"); // Obtiene los decimales
            string montoEnLetras = NumeroEnLetras(Convert.ToInt64(Math.Truncate(monto))) + " pesos " + decimales + "/100 M.N.";
            return montoEnLetras;
        }

        private static string NumeroEnLetras(long numero)
        {
            if (numero == 0) return "cero";
            if (numero == 1) return "uno";
            if (numero == 2) return "dos";
            if (numero == 3) return "tres";
            if (numero == 4) return "cuatro";
            if (numero == 5) return "cinco";
            if (numero == 6) return "seis";
            if (numero == 7) return "siete";
            if (numero == 8) return "ocho";
            if (numero == 9) return "nueve";
            if (numero == 10) return "diez";
            if (numero == 11) return "once";
            if (numero == 12) return "doce";
            if (numero == 13) return "trece";
            if (numero == 14) return "catorce";
            if (numero == 15) return "quince";
            if (numero < 20) return "dieci" + NumeroEnLetras(numero - 10);
            if (numero == 20) return "veinte";
            if (numero < 30) return "veinti" + NumeroEnLetras(numero - 20);
            if (numero == 30) return "treinta";
            if (numero == 40) return "cuarenta";
            if (numero == 50) return "cincuenta";
            if (numero == 60) return "sesenta";
            if (numero == 70) return "setenta";
            if (numero == 80) return "ochenta";
            if (numero == 90) return "noventa";
            if (numero < 100) return NumeroEnLetras((numero / 10) * 10) + " y " + NumeroEnLetras(numero % 10);
            if (numero == 100) return "cien";
            if (numero < 200) return "ciento " + NumeroEnLetras(numero - 100);
            if (numero == 200) return "doscientos";
            if (numero == 300) return "trescientos";
            if (numero == 400) return "cuatrocientos";
            if (numero == 500) return "quinientos";
            if (numero == 600) return "seiscientos";
            if (numero == 700) return "setecientos";
            if (numero == 800) return "ochocientos";
            if (numero == 900) return "novecientos";
            if (numero < 1000) return NumeroEnLetras((numero / 100) * 100) + " " + NumeroEnLetras(numero % 100);
            if (numero == 1000) return "mil";
            if (numero < 2000) return "mil " + NumeroEnLetras(numero % 1000);
            if (numero < 1000000) return NumeroEnLetras(numero / 1000) + " mil " + NumeroEnLetras(numero % 1000);
            if (numero == 1000000) return "un millón";
            if (numero < 2000000) return "un millón " + NumeroEnLetras(numero % 1000000);
            return NumeroEnLetras(numero / 1000000) + " millones " + NumeroEnLetras(numero % 1000000);
        }
    }

}
