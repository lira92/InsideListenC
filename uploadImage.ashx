<%@ WebHandler Language="C#" Class="uploadImage" %>
 
using System.IO;
using System.Web;
using System.Web.Configuration;
 
public class uploadImage : IHttpHandler
{
    public void ProcessRequest(HttpContext _context)
    {
        string uploadDir = HttpContext.Current.Server.MapPath("~/Imagens/");
        if (_context.Request.Files.Count == 0)
        {
            _context.Response.Write("<result><status>Error</status><message>No files selected</message></result>");
            return;
        }
 
        foreach (string fileKey in _context.Request.Files)
        {
            HttpPostedFile file = _context.Request.Files[fileKey];
            file.SaveAs(Path.Combine(uploadDir, file.FileName));

            Image.Resize(Path.Combine(uploadDir, file.FileName), Path.Combine(uploadDir, file.FileName), 50, 64);
        }
 
        _context.Response.Write("<result><status>Success</status><message>Upload completed</message></result>");
 
    }
 
    public bool IsReusable
    {
        get { return true; }
    }
}