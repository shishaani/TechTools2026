using System;
using System.Collections.Generic;
using System.Text;
using TechTools.Data;
using TechTools.Models;

namespace TechTools.Services
{
    public class GPUService(ApplicationDbContext db)
    {
        public List<GPU> ReadAll()
        {
            List<GPU> gpus = db.GPUs.ToList();
            return gpus;
        }

        public void Create(GPU g)
        {
            db.GPUs.Add(g);
            db.SaveChanges();
        }

        public GPU? Read(int PGUId)
        {
            GPU? g = db.GPUs.Find(PGUId);
            return g;
        }

        public bool Delete(int PGUId)
        {
            GPU? g = db.GPUs.Find(PGUId);

            if (g == null) return false;

            db.GPUs.Remove(g);
            db.SaveChanges();
            return true;
        }

        public bool Update(GPU g)
        {
            GPU? existingGPU = db.GPUs.Find(g.Id);
            if (existingGPU == null) return false;

            existingGPU.Brand = g.Brand;
            existingGPU.Model = g.Model;
            existingGPU.Price = g.Price;
            existingGPU.Picture = g.Picture;
            existingGPU.Description = g.Description;

            db.SaveChanges();
            return true;
        }

    }
}
