
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
            return View(db.Post.ToList());
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


                    db.Post.Add(s);
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
                Posts pos = db.Post.Where(a => a.ID == id).FirstOrDefault();
               
                return View(pos);
            }
        }


        public ActionResult Editar_Post(int id)
        {
            using (var db = new challengePostContext())
            {
                Posts pos = db.Post.Where(a => a.ID == id).FirstOrDefault();
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

                    Posts subj = db.Post.Find(s.ID);
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
        public ActionResult Delete_Subject(int id)
        {
            using (var db = new challengePostContext())
            {
                Posts pst = db.Post.Where(a => a.ID == id).FirstOrDefault();
                db.Post.Remove(pst);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }


    }
}