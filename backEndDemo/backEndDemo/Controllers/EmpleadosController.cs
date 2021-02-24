using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLayer;
using System.Data.Entity;
using System.Web;
using System.Threading.Tasks;
using System.Configuration;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace backEndDemo.Controllers
{
    public class EmpleadosController : ApiController
    {
        // GET: api/Empleados
        public IEnumerable<Empleado> Get()
        {
            using (GafetesEntities db = new GafetesEntities())
            {

                return db.Empleados.ToList();
                
                /*
                switch (img) {
                    case "no":
                        return db.Empleados.Select(x => new Empleado {
                            Id = x.Id,
                            Nombre = x.Nombre,
                            Direccion = x.Direccion,
                            Telefono = x.Telefono,
                            Curp = x.Telefono,
                            Imagen = (x.Imagen != null) ? "1" : "0"
                        });
                    case "thumbs":
                        return db.Empleados.Select(x => new Empleado
                        {
                            Id = x.Id,
                            Nombre = x.Nombre,
                            Direccion = x.Direccion,
                            Telefono = x.Telefono,
                            Curp = x.Telefono,
                            Imagen = x.Imagen //convert to Thumbnails
                        });
                    default:
                        return db.Empleados.Select(x => new Empleado
                        {
                            Id = x.Id,
                            Nombre = x.Nombre,
                            Direccion = x.Direccion,
                            Telefono = x.Telefono,
                            Curp = x.Telefono,
                            Imagen = (x.Imagen != null) ? "1" : "0"
                        });
                }*/
            }
            //return new string[] { "value1", "value2" };
        }

        // GET: api/Empleados/5
        public Empleado Get(int id)
        {
            using (GafetesEntities db = new GafetesEntities())
            {
                return db.Empleados.FirstOrDefault(d => d.Id == id);
            }
        }

        // POST: api/Empleados
        public HttpResponseMessage Post([FromBody]Empleado emp)
        {
            int resp = 0;
            HttpResponseMessage msg = null;
            try
            {
                using (GafetesEntities db = new GafetesEntities())
                {
                    db.Entry(emp).State = EntityState.Added;
                    resp = db.SaveChanges();
                    msg = Request.CreateErrorResponse(HttpStatusCode.OK, "Agregado con Exito");
                }
            }
            catch (Exception ex)
            {
                msg = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return msg;
        }

        // PUT: api/Empleados/5
        public HttpResponseMessage Put([FromBody]Empleado emp)
        {
            int resp = 0;
            HttpResponseMessage msg = null;
            try
            {
                using (GafetesEntities db = new GafetesEntities())
                {
                    db.Entry(emp).State = EntityState.Modified;
                    resp = db.SaveChanges();
                    msg = Request.CreateErrorResponse(HttpStatusCode.OK, "Actualizado con exito");
                }
            }
            catch (Exception ex)
            {
                msg = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return msg;
        }

        // DELETE: api/Empleados/5
        public HttpResponseMessage Delete(int id)
        {
            int resp = 0;
            HttpResponseMessage msg = null;
            try
            {
                using (GafetesEntities db = new GafetesEntities())
                {
                    Empleado emp = db.Empleados.FirstOrDefault(x => x.Id == id);
                    db.Entry(emp).State = EntityState.Deleted;
                    resp = db.SaveChanges();
                    msg = Request.CreateErrorResponse(HttpStatusCode.OK, "Eliminado con exito");
                }
            }
            catch (Exception ex)
            {
                msg = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return msg;
        }
        [Route("api/Empleados/SaveFile")]
        //[Route("Empleados/PostUserImage")]
        [AllowAnonymous]
        public string SaveFile()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;

                var physicalPath = HttpContext.Current.Server.MapPath("~/Photos/" + filename);
                postedFile.SaveAs(physicalPath);

                return filename;
            }
            catch (Exception)
            {
                return "none.png";
            }
        }
        [Route("api/Empleados/Gafete")]
        //[Route("Empleados/PostUserImage")]
        [AllowAnonymous]
        public string GenerateGafete(int id)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;

                var physicalPath = HttpContext.Current.Server.MapPath("~/Photos/" + filename);
                postedFile.SaveAs(physicalPath);

                return filename;
            }
            catch (Exception)
            {
                return "none.png";
            }
        }

        private void BarcodeGenerator(string id)
        {
            string prodCode = id;
            //context.Response.ContentType = "image/gif";
            if (prodCode.Length > 0)
            {
                Barcode128 code128 = new Barcode128();
                code128.CodeType = Barcode.CODE128;
                code128.ChecksumText = true;
                code128.GenerateChecksum = true;
                code128.Code = prodCode;
                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                bm.Save(HttpContext.Current.Server.MapPath("~/PDF/" + id + ".gif"), System.Drawing.Imaging.ImageFormat.Gif);
            }
        }
    }
}
