
using ChallengeBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;


namespace ChallengeBlog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            challengePostContext db = new challengePostContext();
            List<Posts> list = db.Posts.Where(a => a.Borrado==0).ToList();
            return View(list);
        }

        public ActionResult Agregar_Post()
        {
                return View();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar_Post(Posts s, HttpPostedFileBase upload)
        {

            if (!ModelState.IsValid)
                return View();
            try
            {
                using (challengePostContext db = new challengePostContext())
                {

                    //agregar imagen

                    if (upload != null && upload.ContentLength > 0)
                    {
                        
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            s.Imagen = reader.ReadBytes(upload.ContentLength);
                        }
                       
                    }
                    //

                    s.FechaCreacion = DateTime.Now;
                    s.Borrado = 0;
                    db.Posts.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error when the Teacher is input", ex);
                return View();
            }

        }

        public ActionResult Detalles_Post(int id)
        {
            using (var db = new challengePostContext())
            {
                Posts pos = db.Posts.Where(a => a.ID == id).FirstOrDefault();
               
                return View(pos);
            }
        }


        public ActionResult Editar_Post(int id)
        {
            using (var db = new challengePostContext())
            {
                Posts pos = db.Posts.Where(a => a.ID == id).FirstOrDefault();
                return View(pos);
            }

        }
        [HttpPost, ActionName("Editar_Post")]
        [ValidateAntiForgeryToken]
        public ActionResult Editar_Post(Posts s, HttpPostedFileBase upload)
        {
            if (!ModelState.IsValid)
                return View();


            try
            {
                using (var db = new challengePostContext())
                {
                    //agregar imagen

                    if (upload != null && upload.ContentLength > 0)
                    {

                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            s.Imagen = reader.ReadBytes(upload.ContentLength);
                        }

                    }
                    //

                    Posts subj = db.Posts.Find(s.ID);
                    subj.Titulo = s.Titulo;
                    subj.Contenido = s.Contenido;
                    subj.Imagen = s.Imagen;
                    subj.Categoria = s.Categoria;
                    
                    subj.FechaCreacion = s.FechaCreacion;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error when the Post is input", ex);
                return View();

            }


        }
        public ActionResult SoftDelete(int id)
        {
            using (var db = new challengePostContext())
            {
                Posts pst = db.Posts.Where(a => a.ID == id).FirstOrDefault();
                pst.Borrado=1;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
      


    }
