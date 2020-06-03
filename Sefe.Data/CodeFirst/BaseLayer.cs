using Sefe.ApplicationSettings;
using Sefe.Core;
using Sefe.Data.Enums;
using Sefe.Data.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefe.Data.CodeFirst
{
    public abstract class BaseLayer<TEntity> : IDisposable where TEntity : class
    {
        #region Properties
        /// <summary>
        /// Database object
        /// </summary>
        public DbContext Context;
        /// <summary>
        /// Data access layer
        /// </summary>
        private DataAccessLayer<TEntity> m_Repository;
        /// <summary>
        /// User
        /// </summary>
        protected Credential Credential;
        /// <summary>
        /// The variable name must be the same. If this value is true, the cache will be active.
        /// </summary>
        private readonly bool cacheEnabled = Settings.GetAppSetting("Project_CacheEnabled", false);
        private bool disposed = false;
        #endregion Properties

        #region Constructor
        public BaseLayer()
        {
        }
        public BaseLayer(Credential credential)
        {
            Credential = credential;
        }
        #endregion Constructor

        #region Repository
        private DataAccessLayer<TEntity> Repository
        {
            get
            {
                if (m_Repository == null)
                {
                    this.m_Repository = new DataAccessLayer<TEntity>(Context);
                }
                return this.m_Repository;
            }
        }
        #endregion Repository

        #region Operational Methods
        protected ProcessResult Find(object param)
        {
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Access);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            return Repository.Find(param);
        }
        protected ProcessResult ExecuteFirstOrDefault<T>(IQueryable<T> query)
        {
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Access);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            return Repository.ExecuteFirstOrDefault(query);
        }
        protected ProcessResult ExecuteList<T>(IQueryable<T> query)
        {
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Access);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            return Repository.ExecuteList(query);
        }
        protected PagedProcessResult ExecutePageList<T>(IQueryable<T> query, int pageNumber, int perPageCount)
        {
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Access);
                if (!result.IsSuccess())
                {
                    return new PagedProcessResult()
                    {
                        ErrorCode = result.ErrorCode,
                        ErrorList = result.ErrorList,
                        ErrorMessage = result.ErrorMessage,
                        PageCount = 0,
                        ResultType = result.ResultType,
                        ReturnObject = result.ReturnObject,
                        SystemError = result.SystemError,
                        TotalCount = 0
                    };
                }
            }
            return Repository.ExecutePageList(query, pageNumber, perPageCount);
        }
        protected ProcessResult ExecuteAny<T>(IQueryable<T> query)
        {
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Access);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            return Repository.ExecuteAny(query);
        }
        /// <summary>
        /// Runs Stored procedure
        /// The parameter names given and the parameter values must be in the same order.
        /// </summary>
        /// <param name="storedProcedureName">SP name</param>
        /// <param name="parameterNames">Parameter names (must start @).</param>
        /// <param name="parameterValues">parameter values</param>
        /// <returns>(int) SP result</returns>
        protected ProcessResult ExecuteStoredProcedure(string storedProcedureName, List<string> parameterNames, List<object> parameterValues)
        {
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Access);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            return Repository.ExecuteStoredProcedure(storedProcedureName, parameterNames, parameterValues);
        }
        protected ProcessResult Insert(TEntity entity)
        {
            // hak kontrolleri
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Insert);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            // Before insert operation (override)
            ProcessResult alterResult = ValidateAlter(entity);
            if (!alterResult.IsSuccess())
            {
                return alterResult;
            }
            // insert
            ProcessResult insertResult = Repository.Insert(entity);
            if (cacheEnabled && insertResult.IsSuccess())
            {
                ClearCache();
            }
            // history
            InsertHistory();

            return insertResult;
        }
        protected ProcessResult Update(TEntity entity)
        {
            // right validations
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Update);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            // Before update operation (override)
            ProcessResult alterResult = ValidateAlter(entity);
            if (!alterResult.IsSuccess())
            {
                return alterResult;
            }
            // update
            ProcessResult updateResult = Repository.Update(entity);
            if (cacheEnabled && updateResult.IsSuccess())
            {
                ClearCache();
            }
            // history
            UpdateHistory();

            return updateResult;
        }
        protected ProcessResult Delete(TEntity entity)
        {
            // Right validations
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Delete);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            // Before delete operation (override)
            ProcessResult alterResult = ValidateDelete(entity);
            if (!alterResult.IsSuccess())
            {
                return alterResult;
            }
            // delete
            ProcessResult deleteResult = Repository.Delete(entity);
            if (cacheEnabled && deleteResult.IsSuccess())
            {
                ClearCache();
            }
            // History
            DeleteHistory();

            return deleteResult;
        }

        // Methods with caching
        protected ProcessResult Find(object param, string cacheKey, int cacheTime)
        {
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Access);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            return Repository.Find(param, cacheKey, cacheTime);
        }
        protected ProcessResult ExecuteFirstOrDefault<T>(IQueryable<T> query, string cacheKey, int cacheTime)
        {
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Access);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            return Repository.ExecuteFirstOrDefault(query, cacheKey, cacheTime);
        }
        protected ProcessResult ExecuteList<T>(IQueryable<T> query, string cacheKey, int cacheTime)
        {
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Access);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            return Repository.ExecuteList(query, cacheKey, cacheTime);
        }
        protected ProcessResult ExecuteAny<T>(IQueryable<T> query, string cacheKey, int cacheTime)
        {
            if (Credential != null)
            {
                ProcessResult result = this.CheckRights(RoleTypes.Access);
                if (!result.IsSuccess())
                {
                    return result;
                }
            }
            return Repository.ExecuteAny(query, cacheKey, cacheTime);
        }
        #endregion Operational Methods

        #region Virtual Methods
        /// <summary>
        /// This is the method that ensures that the user who performs the operation has the right to do that.
        /// </summary>
        /// <param name="rightType">Yapılmak istenen işlem türü</param>
        /// <returns></returns>
        protected virtual ProcessResult CheckRights(RoleTypes roleType)
        {
            return new ProcessResult();
        }
        /// <summary>
        /// It runs before insert and update operations. It is used by overriding in managers (business layer)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual ProcessResult ValidateAlter(TEntity entity)
        {
            return new ProcessResult();
        }
        /// <summary>
        /// It runs before delete operations. It is used by overriding in managers (business layer)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual ProcessResult ValidateDelete(TEntity entity)
        {
            return new ProcessResult();
        }
        /// <summary>
        /// This method works after the insert, update and delete operations if the cache is activated and clears the cache.
        /// </summary>
        protected virtual void ClearCache()
        {

        }
        protected virtual void InsertHistory()
        {

        }
        protected virtual void UpdateHistory()
        {

        }
        protected virtual void DeleteHistory()
        {

        }
        #endregion Virtual Methods

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion Dispose
    }
}
