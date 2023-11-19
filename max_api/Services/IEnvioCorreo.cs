using System.Collections.Generic;

namespace max_api.Services
{
    public interface IEnvioCorreo
    {
        public bool EnvioCorreo(string Smtp, int Puerto, string EmailRemiten, string pwd,int ssl, List<string> lista, string Titulo, string MensajeHTLM, string DirEnviarImg, string GuidId);

    }
}
