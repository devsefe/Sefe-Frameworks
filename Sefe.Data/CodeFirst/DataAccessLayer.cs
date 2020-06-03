using Microsoft.Win32.SafeHandles;
using Sefe.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Sefe.Caching;
using Sefe.Extensions;
using Sefe.ApplicationSettings;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace Sefe.Data.CodeFirst
{
    /// <summary>
    /// It is the layer that communicates with the database.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DataAccessLayer<TEntity> where TEntity : class
    {
        #region Variables
        protected DbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;
        private bool cacheEnabled = Settings.GetAppSetting("Project_CacheEnabled", false);
        #endregion Variables

        #region Constructor
        public DataAccessLayer(DbContext context)
        {
            dbContext = context;
            dbSet = dbContext.Set<TEntity>();
        }
        #endregion Constructor

        #region Operational Methods
        public ProcessResult Find(object param)
        {
            ProcessResult result = new ProcessResult();
            TEntity data = null;
            try
            {
                data = dbSet.Find(param);
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }

            result.ReturnObject = data;
            return result;
        }
        public ProcessResult ExecuteFirstOrDefault<T>(IQueryable<T> query)
        {
            ProcessResult result = new ProcessResult();
            T data;
            try
            {
                data = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }

            result.ReturnObject = data;
            return result;
        }
        public ProcessResult ExecuteList<T>(IQueryable<T> query)
        {
            ProcessResult result = new ProcessResult();
            List<T> data = null;
            try
            {
                data = query.ToList();
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }

            result.ReturnObject = data;
            return result;
        }
        public PagedProcessResult ExecutePageList<T>(IQueryable<T> query, int pageNumber, int perPageCount)
        {
            PagedProcessResult result = new PagedProcessResult();
            List<T> data = null;
            int startRow = ((pageNumber - 1) * perPageCount);

            try
            {
                data = query.Skip(startRow).Take(perPageCount).ToList();
                result.TotalCount = query.Count();
                result.PageCount = Math.Ceiling((double)result.TotalCount / perPageCount).ToString().ToInt(0).Value;
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }

            result.ReturnObject = data;
            return result;
        }
        public ProcessResult ExecuteAny<T>(IQueryable<T> query)
        {
            ProcessResult result = new ProcessResult();
            bool? data = null;
            try
            {
                data = query.Any();
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }

            result.ReturnObject = data;
            return result;
        }
        public ProcessResult Insert(TEntity entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                dbSet.Add(entity);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }
            result.ReturnObject = entity;
            return result;
        }
        public ProcessResult Update(TEntity entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }
            result.ReturnObject = entity;
            return result;
        }
        public ProcessResult Delete(TEntity entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                dbSet.Remove(entity);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }
            return result;
        }
        /// <summary>
        /// Runs Stored procedure
        ///The parameter names given and the parameter values must be in the same order.
        /// </summary>
        /// <param name="storedProcedureName">SP name</param>
        /// <param name="parameterNames">Parameter names (must start @).</param>
        /// <param name="parameterValues">Parameter values</param>
        /// <returns>(int) SP resuşt</returns>
        public ProcessResult ExecuteStoredProcedure(string storedProcedureName, List<string> parameterNames, List<object> parameterValues)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                // define variables
                string paramNames = string.Empty;
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                // join parameter names and values
                for (int i = 0; i < parameterNames.Count(); i++)
                {
                    sqlParameters.Add(new SqlParameter(parameterNames[i], parameterValues[i]));
                    paramNames += string.Format(",{0}", parameterNames[i]);
                }
                // remove first comma
                paramNames = paramNames.Remove(0, 1);
                // run procedure
                result.ReturnObject = dbContext.Database.ExecuteSqlCommand(
                    string.Format("exec {0} {1}", storedProcedureName, paramNames), sqlParameters.ToArray());
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }
            return result;
        }
        public ProcessResult ExecuteView<TModel>(string viewName, List<string> parameterNames, List<object> parameterValues)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                // define variables
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                // join parameter names and values
                for (int i = 0; i < parameterNames.Count(); i++)
                {
                    sqlParameters.Add(new SqlParameter(parameterNames[i], parameterValues[i]));
                }

                // generate dynamic query
                StringBuilder whereClause = new StringBuilder(" where 1=1");
                foreach (string item in parameterNames)
                {
                    whereClause.AppendFormat(" and @{0}={0}", item);
                }

                result.ReturnObject = dbContext.Database.SqlQuery<TModel>(
                    string.Format("select * from {0}{1}", viewName, whereClause), sqlParameters.ToArray()
                ).ToList();
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }
            return result;
        }

        // Methods with caching
        public ProcessResult Find(object param, string cacheKey, int cacheTime)
        {
            ProcessResult result = new ProcessResult();
            // get data from cache
            TEntity data = null;
            if (cacheEnabled)
            {
                data = CacheManager.GetFromCache<TEntity>(cacheKey);
            }
            if (data == null)
            {
                // get data from db
                result = this.Find(param);
                if (!result.IsSuccess())
                {
                    return result;
                }
                if (cacheEnabled)
                {
                    // add data to cache
                    CacheManager.AddToCache(cacheKey, result.ReturnObject, cacheTime);
                }
            }
            else
            {
                result.ReturnObject = data;
            }
            return result;
        }
        public ProcessResult ExecuteFirstOrDefault<T>(IQueryable<T> query, string cacheKey, int cacheTime)
        {
            ProcessResult result = new ProcessResult();
            // get data from cache
            object data = null;
            if (cacheEnabled)
            {
                data = CacheManager.GetFromCache(cacheKey);
            }
            if (data == null)
            {
                // get data from db
                result = this.ExecuteFirstOrDefault(query);
                if (!result.IsSuccess())
                {
                    return result;
                }
                if (cacheEnabled)
                {
                    // add data to cache
                    CacheManager.AddToCache(cacheKey, result.ReturnObject, cacheTime);
                }
            }
            else
            {
                result.ReturnObject = data;
            }
            return result;
        }
        public ProcessResult ExecuteList<T>(IQueryable<T> query, string cacheKey, int cacheTime)
        {
            ProcessResult result = new ProcessResult();
            // get data from cache
            object data = null;
            if (cacheEnabled)
            {
                data = CacheManager.GetFromCache(cacheKey);
            }
            if (data == null)
            {
                // get data from db
                result = this.ExecuteList(query);
                if (!result.IsSuccess())
                {
                    return result;
                }
                if (cacheEnabled)
                {
                    // add data to cache
                    CacheManager.AddToCache(cacheKey, result.ReturnObject, cacheTime);
                }
            }
            else
            {
                result.ReturnObject = data;
            }
            return result;
        }
        public ProcessResult ExecuteAny<T>(IQueryable<T> query, string cacheKey, int cacheTime)
        {
            ProcessResult result = new ProcessResult();
            // get data from cache
            object data = null;
            if (cacheEnabled)
            {
                data = CacheManager.GetFromCache(cacheKey);
            }
            if (data == null)
            {
                // get data from db
                result = this.ExecuteAny(query);
                if (!result.IsSuccess())
                {
                    return result;
                }
                if (cacheEnabled)
                {
                    // add data to cache
                    CacheManager.AddToCache(cacheKey, result.ReturnObject, cacheTime);
                }
            }
            else
            {
                result.ReturnObject = (bool)data;
            }
            return result;
        }
        #endregion Operational Methods
    }
}
