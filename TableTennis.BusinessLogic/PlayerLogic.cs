using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTennis.DataAccess;
using TableTennis.DataModels;

namespace TableTennis.BusinessLogic
{
    public class PlayerLogic : SPConnectionBase
    {
        public void Add(ref PLAYER dataObj)
        {
            PlayerAccess access = new PlayerAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                access.Add(ref dataObj);
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
                
        public PLAYER Get(int? id)
        {

            if (id == null)
                return null;
            PLAYER dataObj = new PLAYER();
            PlayerAccess access = new PlayerAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }

        public List<PLAYER> GridResult(ParamPlayer param)
        {
            if (param == null)
                return null;
            List<PLAYER> dataObj = new List<PLAYER>();
            PlayerAccess access = new PlayerAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GridResult(param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }
    }
}
