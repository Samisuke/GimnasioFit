namespace GimnasioFit.Core.Common
{
    public class Resultado<T>
    {
        public bool EsCorrecto {get;}
        public string MensajeError {get;}
        public T? Valor {get;}

        public Resultado (bool esCorrecto, string mensajeError, T? valor)
        {
            EsCorrecto = esCorrecto;
            MensajeError = mensajeError;
            Valor = valor;
        }

        public static Resultado<T> Bien(T valor) 
            => new Resultado<T>(true, string.Empty, valor);

        public static Resultado<T> Mal(string mensajeError) 
            => new Resultado<T>(false, mensajeError, default);
    }
}