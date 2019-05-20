using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Sparepart.Models;
namespace Sparepart.Models
{
    public class ProductService : IDisposable
    {
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        private static bool UpdateDatabase = false;
        private dbsparepartEntities entities;

        public ProductService(dbsparepartEntities entities)
        {
            this.entities = entities;
        }

        public IList<SJCreateViewModel> GetAll()
        {
            var result = HttpContext.Current.Session["SJHeader"] as IList<SJCreateViewModel>;

            //if (result == null || UpdateDatabase)
            //{
            result = entities.sjheaders.Select(product => new SJCreateViewModel
                {
                    FPSID = product.FPSID,
                    SJID = product.SJID,
                    JenisSJ = product.JenisSJ,
                    Keterangan = product.Keterangan,
                    FPS = new FPSViewModel()
                    {
                        FPSID = product.FPSID,
                    },
                }).ToList();

            //    HttpContext.Current.Session["SJHeader"] = result;
            //}

            return result;
        }

        public IEnumerable<SJCreateViewModel> Read()
        {
            return GetAll();
        }

        public void Create(SJCreateViewModel model)
        {
            //if (!UpdateDatabase)
            //{
            //    fpsheader fps = Dbcontext.fpsheaders.Where(x => x.FPSID == model.FPS.FPSID).FirstOrDefault();
            //    var first = GetAll().OrderByDescending(e => e.SJID).FirstOrDefault();
            //    var id = (first != null) ? first.SJID : 0;
            //    model.SJID = id + 1;

            //    var entity = new sjheader();
            //    entity.SJID = model.SJID;
            //    entity.JenisSJ = fps.JenisFPS;
            //    entity.Keterangan = model.Keterangan;
            //    if (model.FPSID == null)
            //    {
            //        entity.FPSID = model.FPS.FPSID;
            //    }
                
            //    entities.sjheaders.Add(entity);
            //    entities.SaveChanges();
            //    GetAll().Insert(0, model);
            //}
            //else
            //{

            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
            fpsheader fps = Dbcontext.fpsheaders.Where(x => x.FPSID == model.FPS.FPSID).FirstOrDefault();
            var entity = new sjheader();

                entity.JenisSJ = fps.JenisFPS;
                entity.Keterangan = model.Keterangan;
                entity.TanggalInput = DateTime.Now;
                entity.UserInput = CurrentUser.NamaUser;
                if (entity.FPSID == null)
                {
                    entity.FPSID = model.FPS.FPSID;
                }

                if (model.FPS != null)
                {
                    entity.FPSID = model.FPS.FPSID;
                }

                entities.sjheaders.Add(entity);
                entities.SaveChanges();

                model.SJID = entity.SJID;
        //}
    }

        public SJCreateViewModel One(Func<SJCreateViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}