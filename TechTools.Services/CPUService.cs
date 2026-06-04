using System;
using System.Collections.Generic;
using System.Text;
using TechTools.Data;
using TechTools.Models;

namespace TechTools.Services
{
    public class CPUService(ApplicationDbContext db)
    {
        public List<CPU> ReadAll()
        {
            List<CPU> cpus = db.CPUs.ToList();
            return cpus;
        }

        public void Create(CPU c)
        {
            db.CPUs.Add(c);
            db.SaveChanges();
        }

        public CPU? Read(int CPUId)
        {
            CPU? c = db.CPUs.Find(CPUId);
            return c;
        }

        public bool Delete(int CPUId)
        {
            CPU? c = db.CPUs.Find(CPUId);

            if (c == null) return false;

            db.CPUs.Remove(c);
            db.SaveChanges();
            return true;
        }

        public bool Update(CPU c)
        {
            CPU? existingCPU = db.CPUs.Find(c.Id);
            if (existingCPU == null) return false;

            existingCPU.Brand = c.Brand;
            existingCPU.Model = c.Model;
            existingCPU.Price = c.Price;
            existingCPU.Picture = c.Picture;
            existingCPU.Description = c.Description;

            db.SaveChanges();
            return true;
        }

    }
}
