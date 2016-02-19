using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using FluorineFx;
/// <summary>
/// Summary description for Email
/// </summary>
[RemotingService]
public class Email
{
    public string assunto = "";
    public string destinatario = "";
    public string mensagem = "";
    public Email()
    {
    }
    public Boolean enviarEmail(Email email)
    {
        try
        {
            MailMessage mailMessage = new MailMessage();
            //Endereço que irá aparecer no e-mail do usuário 
            mailMessage.From = new MailAddress("des.willian@insidesistemas.com.br", "Willian");
            //destinatarios do e-mail, para incluir mais de um basta separar por ponto e virgula  
            mailMessage.To.Add(email.destinatario);
            mailMessage.Subject = email.assunto;
            mailMessage.IsBodyHtml = true;
            //conteudo do corpo do e-mail 
            mailMessage.Body = email.mensagem;
            mailMessage.Priority = MailPriority.High;
            //smtp do e-mail que irá enviar 
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "mail.ita.locamail.com.br";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = false;
            //credenciais da conta que utilizará para enviar o e-mail 
            smtpClient.Credentials = new NetworkCredential("des.willian@insidesistemas.com.br", "romanos8,28");
            smtpClient.Send(mailMessage);
            return true;
        }
        catch
        {
            return false;
        }
    }

    

    
}
