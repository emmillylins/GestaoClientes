using Domain.Comum;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public readonly record struct Cnpj
    {
        public string Numero { get; }


        public Cnpj(string valor)
        {
            var digitos = Regex.Replace(valor ?? string.Empty, @"\n|\r|\s|\.|-|/", string.Empty);
            if (digitos.Length != 14 || !Regex.IsMatch(digitos, "^\\d{14}$") || !EhCnpjValido(digitos))
                throw new DomainException("CNPJ inválido.");
            Numero = digitos;
        }


        public override string ToString() => Numero;


        public static implicit operator string(Cnpj cnpj) => cnpj.Numero;


        private static bool EhCnpjValido(string cnpj)
        {
            // Rejeita sequências repetidas
            if (new string(cnpj[0], cnpj.Length) == cnpj) return false;


            int[] multiplicador1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplicador2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];


            string temporario = cnpj[..12];
            int soma = 0;
            for (int i = 0; i < 12; i++) soma += (temporario[i] - '0') * multiplicador1[i];
            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;


            temporario += digitoVerificador1.ToString();
            soma = 0;
            for (int i = 0; i < 13; i++) soma += (temporario[i] - '0') * multiplicador2[i];
            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;


            return cnpj.EndsWith(digitoVerificador1.ToString() + digitoVerificador2.ToString());
        }
    }
}
