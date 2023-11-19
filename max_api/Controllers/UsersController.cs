using max_api.DAL;
using max_api.Models;
using max_api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace max_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly DbmaxContext _context;
        private readonly IEnvioCorreo _oCorreo;
        private readonly SmtpNoti _smtp;
        public UsersController(IWebHostEnvironment env, DbmaxContext context, IEnvioCorreo oCorreo, IOptions<SmtpNoti> smtp)
        {
            _context = context;
            _oCorreo = oCorreo;
            _smtp = smtp.Value;
            _env = env;
        }


        [HttpGet("getEstados")]
        public async Task<ActionResult<IEnumerable<vmEstadoCdInfo>>> getEstados()
        {
            return await _context.EstadoCdInfoEntities.ToListAsync();
        }
        [HttpPost("addUser")]
        public async Task<ActionResult<int>> addUser(vmUserAdd model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserEntities.Add(model);
            int resok = await _context.SaveChangesAsync();
            if (resok > 0)
            {
                string DirEnviarImg = Path.Combine(_env.ContentRootPath, "Images", "logoEmpresa.png");
                string GuidIdImg = Guid.NewGuid().ToString();
                string miHtml = htmlBody(model.Nombre,model.Email,GuidIdImg);
                var listacorreo = new List<string>() { model.Email };

                _oCorreo.EnvioCorreo( _smtp.Smtp,_smtp.Puerto, _smtp.CorreoRemitente, _smtp.Pwd,_smtp.EnableSsl,
                    listacorreo,"Confirmacion de datos, recibidos.", miHtml, DirEnviarImg, GuidIdImg);
            }



            return Ok(resok);
        }
        #region HELPER
        public static string htmlBody(string nombre,string correo,string GuidIdImg)
        {
            string html = "";
            string temp = "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
                "<meta charset='utf-8' />" +
                "<title>cONFIRMACION DE RECEPCION DE DATOS</title>" +
            "</head>" +
            "<body>" +               
                $"<img src='cid:{GuidIdImg}'/><br /><br>" +
                $"<p><h1 style='color:darkblue'>MIE EMPRESA S.A DE C.V</h1></p><br />" +
                $"<p>Estimado: {nombre}</p><br />" +
                $"<p>Hemos recibido sus datos y nos pondremos en contacto con usted en la brevedad posible. Enviaremos un correo con información a su cuenta: {correo}</p><br />" +
                "<p style='color:gray;font-size:x-small;text-align:justify;'>" +
                     "Este mensaje y los archivos adjuntos son de la exclusiva responsabilidad del remitente y están destinados únicamente a los destinatarios. Asi mismo, son de carácter confidencial." +
                    "Se prohíbe cualquier divulgación, revelación o uso no autorizado. Si no es el destinatario de este mensaje, elimínelo y notifíquelo al remitente." +
                    "La integridad de este mensaje no se puede garantizar en internet, por lo tanto, Mi empresa no será responsable del mensaje si es modificado." +
                "</p><br />" +
            "</body>" +
            "</html>";



            html = temp;
            return html;
        }
        #endregion


    }
}
