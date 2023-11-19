namespace max_api.Models
{
    public class SmtpNoti
    {
        public string Smtp { get; set; }
        public int Puerto { get; set; }
        public int EnableSsl { get; set; }//para no entrar en dilema usamos 1 true,0false
        public string Pwd { get; set; }
        public string CorreoRemitente { get; set; }
    }
}
