﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.Entities;

namespace EMC.SPaaS.Repository
{
    public class DesignRepository : IDesignRepository
    {
        private readonly SPaaSDbContext _context;
        public DesignRepository(SPaaSDbContext context)
        {
            _context = context;
        }

        //static Dictionary<string, DesignItem> designItems = new Dictionary<string, DesignItem>();
        public void Add(DesignEntity nItem)
        {
            
            //Save to DB
            _context.Designs.Add(nItem);
            
        }

        public DesignEntity Find(int designID)
        {
            return _context.Designs.FirstOrDefault(s => s.DId == designID);
        }

        public IEnumerable<DesignEntity> GetAll(int userID)
        {
            //return designItems.Values;
           
            return _context.Designs.Where(d => d.UserId == userID);
                
        }

        public void Remove(int designID,int userID)
        {
           var design = _context.Designs.FirstOrDefault(s => s.DId == designID && s.UserId == userID);
            if(design !=null)
            {
                _context.Designs.Remove(design);
            }
            

        }
    }
}

