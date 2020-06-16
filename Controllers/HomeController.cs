using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renci.SshNet;

namespace LearnSSHNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        string host = "", username="", password="";
        int port = 22;
        [HttpGet]
        public ActionResult Get()
        {
            string result = "";
            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                result = client.ConnectionInfo.ServerVersion;

                var filePath = Path.Combine(@"c:\Project\testing2\", "inbound.txt");
                Stream stream = System.IO.File.Create(filePath);
                client.DownloadFile("/folder/Test/inbounds/inbound.txt", stream);
                stream.Close();
            }
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            string result = "";
            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                result = client.ConnectionInfo.ServerVersion;

                var files = client.ListDirectory("/folder/Test/inbounds/");
                foreach (var file in files)
                {
                    if (!file.Name.StartsWith("."))
                    {
                        string remoteFileName = file.Name;
                        var filePath = Path.Combine(@"c:\Project\testing2\", remoteFileName);
                        Stream stream = System.IO.File.Create(filePath);

                        client.DownloadFile("/folder/Test/inbounds/"+remoteFileName, stream);
                        stream.Close();
                    }
                }
            }
            return Ok(result);
        }
        [HttpGet("Rename")]
        public ActionResult Rename()
        {
            string result = "";
            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                result = client.ConnectionInfo.ServerVersion;

                client.RenameFile("/folder/Test/inbounds/inbound.txt", "/folder/Test/inbounds/inbound21.txt");
            }
            return Ok(result);
        }
        [HttpGet("MoveFile")]
        public ActionResult MoveFile()
        {
            string result = "";
            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                result = client.ConnectionInfo.ServerVersion;

                client.RenameFile("/folder/Test/inbounds/inbound21.txt", "/folder/Test/errors/inbound21.txt");
            }
            return Ok(result);
        }
        [HttpGet("DeleteFile")]
        public ActionResult DeleteFile()
        {
            string result = "";
            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                result = client.ConnectionInfo.ServerVersion;

                client.DeleteFile("/folder/Test/errors/inbound21.txt");
            }
            return Ok(result);
        }
        [HttpGet("CheckExist")]
        public ActionResult CheckExist()
        {
            bool result = false;
            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();

                result = client.Exists("/folder/Test/inboundsasdads/satu123.txt");
            }
            return Ok(result);
        }


        [HttpGet("GetStream")] //Read on Memory Stream Masih Gagal
        public ActionResult GetStream()
        {
            string result = "";
            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();

                MemoryStream stream = new MemoryStream();
                client.DownloadFile("/folder/Test/inbounds/inbound.txt", stream);
                stream.Close();
            }
            return Ok(result);
        }
    }
}