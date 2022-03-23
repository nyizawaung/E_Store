using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStore.Model;
using Newtonsoft.Json;

namespace EStore.Services
{
    public class LogService : ILogServices
    {
        private ESDBContext dbContext;
        public LogService(ESDBContext ctx)
        {
            dbContext = ctx;
        }
        
        public void Logging(dynamic request, dynamic response, string RequestPath, int UserID)
        {
            try
            {
                var log = new log
                {
                    RequestData = JsonConvert.SerializeObject(request),
                    RespData = JsonConvert.SerializeObject(response),
                    RequestPath = RequestPath,
                    UserID = UserID,
                    CreateDate = DateTime.Now
                };
                dbContext.Logs.Add(log);
                dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            
        }
    }
}
