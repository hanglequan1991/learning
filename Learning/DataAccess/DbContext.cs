using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.DataAccess
{
    public static class DbContext
    {
        private static DBreeze.DBreezeEngine _engine = default!
            ;
        public static DBreeze.DBreezeEngine Engine
        {
            get
            {
                if (_engine == null) _engine = new DBreeze.DBreezeEngine(Configs.DB_PATH); 
                return _engine;
            }
        }
    }
}
