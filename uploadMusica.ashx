<%@ WebHandler Language="C#" Class="uploadMusica" %>
 
using System.IO;
using System.Web;
using System.Web.Configuration;

public class uploadMusica : IHttpHandler
{
    public void ProcessRequest(HttpContext _context)
    {
        string uploadDir = HttpContext.Current.Server.MapPath("~/Musicas/");
        //string uploadDir = "Z:\\Desenvolvimento\\Treinamento\\Music";
        if (_context.Request.Files.Count == 0)
        {
            _context.Response.Write("<result><status>Error</status><message>No files selected</message></result>");
            return;
        }

        if (!Directory.Exists(uploadDir))
        {
            Directory.CreateDirectory(uploadDir);
        }
        foreach (string fileKey in _context.Request.Files)
        {
            HttpPostedFile file = _context.Request.Files[fileKey];
            file.SaveAs(Path.Combine(uploadDir, file.FileName));
        }
 
        _context.Response.Write("<result><status>Success</status><message>Upload completed</message></result>");
 
    }
 
    public bool IsReusable
    {
        get { return true; }
    }
}