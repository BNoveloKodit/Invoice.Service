namespace Infraction.Backend.Image.Service.Infrastructure.Utils.Enums
{
    public static class ExpressionsRegularsEnum
    {
        public static readonly string ValidateCURP = @"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$";
        
        public static readonly string ValidateNombre = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$";

        public static readonly string ValidatePlaca = @"^[a-zA-Z]{3}[0-9]{2}[a-zA-Z0-9]";

        public static readonly string ValidateRFC = @"^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))([A-Z\d]{3})?$";

        public static readonly string ValidateSoloNumeros = @"^([0-9])+$";
        
        
    }
}
